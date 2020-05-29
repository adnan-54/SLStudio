using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SlrrLib.Model
{
  public class CfgLine
  {
    public List<CfgLineToken> Tokens
    {
      get;
      set;
    } = new List<CfgLineToken>();

    public bool IsEmpty
    {
      get
      {
        return Tokens.Count == 0;
      }
    }
    public string TypeStr
    {
      get
      {
        if (!Tokens.Any(x => !x.IsComment))
          return "";
        return Tokens.Where(x => !x.IsComment).Select(x => x.TypeStr).Aggregate((x, y) => x + " " + y);
      }
    }
    public string NameStr
    {
      get
      {
        if (IsEmpty)
          return "";
        if (Tokens.First().IsComment)
          return "";
        return Tokens.First().Value.ToLower();
      }
    }

    public CfgLine()
    {

    }
    public CfgLine(string ln,bool eofReached = false)
    {
      uint indComment1 = (uint)ln.IndexOf("//");
      uint indComment2 = (uint)ln.IndexOf(";");
      uint indComment3 = (uint)ln.IndexOf("#");
      int commentInd = (int)Math.Min(Math.Min(indComment1, indComment2), indComment3);
      string noCommentPart = ln;
      string commentPart = "";
      if (commentInd != -1)
      {
        noCommentPart = ln.Remove(commentInd);
        commentPart = ln.Substring(commentInd);
      }
      if (eofReached)
      {
        noCommentPart = "";
        commentPart = ln;
        if (commentInd != 0 && commentPart != "")
          commentPart = ";" + commentPart;
      }
      Tokens.AddRange(noCommentPart.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => new CfgLineToken(x)));
      if (commentPart != "")
        Tokens.Add(new CfgLineToken(commentPart));
    }

    public bool IsFormatCorrect()
    {
      return MatchedFormat() != "";
    }
    public string MatchedFormat()
    {
      foreach(var potFormat in getFormatdescriptors())
      {
        string realTest = potFormat;
        if(potFormat.EndsWith("*"))
        {
          string toad = " "+potFormat[potFormat.Length - 2].ToString();
          while (realTest.Length < TypeStr.Length)
            realTest = realTest.Replace("*", toad + "*");
          realTest = realTest.Remove(realTest.Length - 1);
        }
        var testAgainst = realTest.Split(' ');
        var testThis = TypeStr.Split(' ');
        if (testAgainst.Length != testThis.Length)
          continue;
        bool match = true;
        for(int t_i = 0; t_i != testAgainst.Length; ++t_i)
        {
          if (!isFormatEQ(testAgainst[t_i], testThis[t_i]))
          {
            if (!((testAgainst[t_i] == "F" || testAgainst[t_i] == "H" || testAgainst[t_i] == "R") && Tokens[t_i].Value == "0")
                && !(testAgainst[t_i] == "F" && testThis[t_i] == "I"))
            {
              match = false;
              break;
            }
          }
        }
        if(match)
        {
          if (testAgainst.Length == 0)
            return "";
          return testAgainst.Aggregate((x, y) => x + " " + y);
        }
      }
      return "";
    }
    public string GetTypeOfIndexFromFormatdescriptor(int ind)
    {
      foreach(var f in getFormatdescriptors())
      {
        if (f.Length < ind * 2 && f[ind * 2] != 'Z')
          return f[ind * 2].ToString();
      }
      foreach (var f in getFormatdescriptors())
      {
        if (f.Length < ind * 2)
          return f[ind * 2].ToString();
      }
      foreach(var f in getFormatdescriptors())
      {
        if (f.Length >= ind * 2 && f.Last() == '*')
        {
          return f[f.Length - 2].ToString();
        }
      }
      return "";
    }
    public string GetDeafultValueFromType(string type)
    {
      switch(type)
      {
        case "Z":
        case "I":
          return "0";
        case "H":
        case "R":
          return "0x0";
        case "F":
          return "0.000000";
        case "S":
          return "DEFAULT_TEXT";
      }
      return "0";
    }
    public void FixCountFromFormatDescriptor(int desiredTokenCount)
    {
      while (Tokens.Count < desiredTokenCount)
      {
        Tokens.Add(new CfgLineToken(GetDeafultValueFromType(GetTypeOfIndexFromFormatdescriptor(Tokens.Count))));
      }
    }
    public virtual void SaveTo(StreamWriter sw)
    {
      bool first = true;
      for (int tkn_i = 0; tkn_i != Tokens.Count; ++tkn_i)
      {
        var tkn = Tokens[tkn_i];
        sw.Write(tkn.Value);
        if (tkn_i != Tokens.Count - 1)
        {
          if (first)
            sw.Write("\t\t");
          else
            sw.Write(" ");
        }
        first = false;
      }
      sw.WriteLine();
    }
    public override string ToString()
    {
      if(Tokens.Any())
        return Tokens.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y);
      return "";
    }

    protected string[] getFormatdescriptors()
    {
      if(formatDescriptors.ContainsKey(NameStr))
      {
        return formatDescriptors[NameStr];
      }
      return new string[] { };
    }
    private bool isFormatEQ(string f1,string f2)
    {
      if (f1 == f2)
        return true;
      if (f1 == "Z" && (f2 == "I" || f2 == "F"))
        return true;
      if (f2 == "Z" && (f1 == "I" || f1 == "F"))
        return true;
      if (f2 == "R" && f1 == "H")
        return true;
      if (f1 == "R" && f2 == "H")
        return true;
      return false;
    }

    #region FormatDescriptors
    protected Dictionary<string, string[]> formatDescriptors = new Dictionary<string, string[]>()
    {
      {
        "[STATE]",new string[] {"S"}
      },
      {
        "[ACTION]",new string[] {"S"}
      },
      {
        "shifter",new string[] {"S F F F"}
      },
      {
        "trigger",new string[]{"S Z Z Z Z Z Z S F"}
      },
      {
        "light",new string[]{"S H I Z Z"}
      },
      {
        "source_angle",new string[]{"S S F F F"}
      },
      {
        "dest_angle",new string[]{"S S F F F F F F"}
      },
      {
        "bounce",new string[]{"S S F F F S F F F F F F"}
      },
      {
        "notify",new string[]{"S"}
      },
      {
        "sfx_loop",new string[]{"S H","S H I I I","S H I"}
      },
      {
        "init",new string[]{"S I"}
      },
      {
        "velscale",new string[]{"S F"}
      },
      {
        "maxcount",new string[]{"S I"}
      },
      {
        "width",new string[]{"S F F"}
      },
      {
        "height",new string[]{"S F"}
      },
      {
        "uv_end",new string[]{"S F F F F"}
      },
      {
        "uv_normal",new string[]{"S F F F F"}
      },
      {
        "fade",new string[]{"S F"}
      },
      {
        "anim",new string[]{"S H F"}
      },
      {
        "sfx_horn",new string[]{"S H F I"}
      },
      {
        "gravity",new string[]{"S S F F F"}
      },
      {
        "source_size",new string[]{"S S F F F","S S F F F F F F"}
      },
      {
        "dest_size",new string[]{"S S F F F F F F","S S F F F"}
      },
      {
        "source_color",new string[]{"S S F F F","S S F F F F F F"}
      },
      {
        "dest_color",new string[]{"S S F F F F F F","S S F F F"}
      },
      {
        "source_alpha",new string[]{"S S F F F F F F","S S F F F"}
      },
      {
        "dest_alpha",new string[]{"S S F F F"}
      },
      {
        "lifetime",new string[]{"S S F F F F F F","S S F F F"}
      },
      {
        "move",new string[]{"S S I I"}
      },
      {
        "damping",new string[]{"S S F F F"}
      },
      {
        "source",new string[]{"S S I S F F F S F F F","S S I S F F F F F F S F F F F F","S S I S F F F S F F F F F","S S I S F F F F F F S F F F F F F"}
      },
      {
        "wheelpars",new string[]{"S F F F F F F F"}
      },
      {
        "sfx_random",new string[]{"S H I I I I I"}
      },
      {
        "rollbar",new string[]{"S I I I I"}
      },
      {
        "rendertypes",new string[]{"S I R*"}//I number of Rs
      },
      {
        "linked",new string[]{"S I I","S I"}
      },
      {
        "colors",new string[]{"S I H*"}//I is the number of Hs (which are rgb colors)
      },
      {
        "cockpit_rpm",new string[]{"S F F F F F F H F F F F"}
      },
      {
        "cockpit_speed",new string[]{"S F F F F F F H F F F F"}
      },
      {
        "buoy",new string[]{"S"}
      },
      {
        "wheelbones",new string[]{"S"}
      },
      {
        "bonepos",new string[]{"S F F F"}
      },
      {
        "slotdeform",new string[]{"S F"}
      },
      {
        "camera",new string[]{"S F F F F F F F"}
      },
      {
        "seat",new string[]{"S F F F F F F"}
      },
      {
        "pedals",new string[]{"S F F F","S F F F F F F"}
      },
      {
        "steering",new string[]{"S F F F F F F F S F"}
      },
      {
        "name",new string[]{"S S*"}
      },
      {
        "flap",new string[]{"S F F F F F F F F F F F","S F F F F F F F F F F F F"}
      },
      {
        "steerhelp",new string[]{"S F F F F F","S F F F F F F"}
      },
      {
        "deformable",new string[]{"S R"}
      },
      {
        "damagedef",new string[]{"S Z"}
      },
      {
        "ext_camera",new string[]{"S I S S S I"}
      },
      {
        "enginepower",new string[]{"S F F F F","S F F F F F F"}
      },
      {
        "brakepower",new string[]{"S F F"}
      },
      {
        "maxsteer",new string[]{"S F F","S F"}
      },
      {
        "ratios",new string[]{"S I F F F F F","S I F F F F F F","S I F F F F F F F"}
      },
      {
        "rearendratio",new string[]{"S F"}
      },
      {
        "controller",new string[]{"S I I","S I I I I I I I","S I I I I I","S I I I","S I"}
      },
      {
        "spring",new string[]{"S F F F"}
      },
      {
        "wheel",new string[]{"S F F F F F F F F F F"}
      },
      {
        "compound",new string[]{"S"}
      },
      {
        "chassis",new string[]{"S"}
      },
      {
        "endcompound",new string[]{"S S"}
      },
      {
        "bone",new string[]{"S"}
      },
      {
        "type",new string[]{"S F S F","S F"}
      },
      {
        "slottype",new string[]{"S I"}
      },
      {
        "bounds",new string[]{"S F F F F F F","S F F F F F F I"}
      },
      {
        "material",new string[]{"S R R R","S R R R I"}
      },
      {
        "stockpart",new string[]{"S R"}
      },
      {
        "flags",new string[]{"S I"}
      },
      {
        "slotdmgmode",new string[]{"S H"}//this is not a ref
      },
      {
        "intdamage",new string[]{"S F"}
      },
      {
        "compatible",new string[]{"S R I"}//not necessarily a defined R but someone should define a matching attach line
      },
      {
        "lod",new string[]{"S I I"}
      },
      {
        "lods",new string[]{"S I F*"}//there are I number of render-lod lines
      },
      {
        "nocollision",new string[]{"S"}
      },
      {
        "damage",new string[]{"S F"}
      },
      {
        "texture",new string[]{"S R"}
      },
      {
        "wing",new string[]{"S I F F F F F F F F F"}
      },
      {
        "noclick",new string[]{"S"}
      },
      {
        "attach",new string[]{"S R I"}//not necessarily a defined R but someone can define a compatible line matching and it will  work
      },
      {
        "body",new string[]{"S F F F F F F F S F F F","S F F F F F F F S","S F F F F F F F S F","S R"}
      },
      {
        "mesh",new string[]{"S R","S R F F F F F F"}
      },
      {
        "click",new string[]{"S R","S R F F F F F F","S R F F F"}
      },
      {
        "category",new string[]{"S I"}
      },
      {
        "slot",new string[]{"S F F F F F F I"}
      },
      {
        "render",new string[]{"S R F F F F F F","S R","S R F F F"}
      },
      {
        "eof",new string[]{"S"}
      }
    };
    #endregion
  }
}
