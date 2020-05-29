using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class ScriptJava
  {
    private string javaFnam;
    private string javaCache = "";
    private List<ScriptJavaToken> tokensCache = null;
    private List<ScriptJavaClassToken> classesDefinedCache = new List<ScriptJavaClassToken>();

    public ScriptJavaToken PackageNameToken
    {
      get;
      private set;
    } = null;
    public IEnumerable<ScriptJavaClassToken> ClassesDefined
    {
      get
      {
        return classesDefinedCache;
      }
    }
    public List<ScriptJavaToken> Tokens
    {
      get
      {
        if (tokensCache == null)
          tokensCache = ReLoadTokens();
        return tokensCache;
      }
      set
      {
        tokensCache = value;
      }
    }

    public ScriptJava(string fnam)//loads tokens, inits cache string
    {
      javaFnam = fnam;
      loadClasses();
    }

    public List<ScriptJavaToken> ReLoadTokens()
    {
      List<ScriptJavaToken> tokens = new List<ScriptJavaToken>();
      if(!File.Exists(javaFnam))
        return tokens;
      string allStr = File.ReadAllText(javaFnam);
      javaCache = allStr;
      bool inString = false;
      bool inComment = false;
      bool inLineComment = false;
      StringBuilder sb = new StringBuilder();
      char charBefore = '\0';
      int startInd = -1;
      for(int str_i = 0; str_i != allStr.Length; ++str_i)
      {
        char charAfter = '\0';
        if (str_i != allStr.Length - 1)
          charAfter = allStr[str_i + 1];
        if (!inComment && !inLineComment && !inString && (allStr[str_i] == '\"'))
        {
          inString = true;
        }
        else if (inString && ((allStr[str_i] == '\"' && (charBefore != '\\' || (charBefore == '\\' && allStr[str_i-2] == '\\')))))
        {
          inString = false;
          str_i++;
        }
        else if (!inComment && !inLineComment && !inString && (allStr[str_i] == '/' && charAfter == '/'))
        {
          inLineComment = true;
        }
        else if (inLineComment && allStr[str_i] == '\n')
        {
          inLineComment = false;
        }
        else if (!inComment && !inLineComment && !inString && (charAfter == '*' && allStr[str_i] == '/'))
        {
          inComment = true;
        }
        else if (inComment && (allStr[str_i] == '/' && charBefore == '*'))
        {
          inComment = false;
          str_i++;
        }

        if (!inComment && !inLineComment && !inString)
        {
          char lastCharWritten = '\0';
          if(sb.Length != 0)
            lastCharWritten = sb.ToString(sb.Length - 1, 1)[0];
          char curChar = allStr[str_i];
          if(isWhiteSpaceorLineBreake(lastCharWritten) && isWhiteSpaceorLineBreake(curChar))
          {
            //don't write
          }
          else if (isTokenBreak(curChar) && !isWhiteSpaceorLineBreake(curChar) && !isTokenBreak(lastCharWritten))
          {
            if(sb.Length != 0)
              tokens.Add(new ScriptJavaToken { StartInd = startInd, EndInd = str_i, Token = sb.ToString() });
            sb = new StringBuilder();
            startInd = -1;
            tokens.Add(new ScriptJavaToken { StartInd = str_i, EndInd = str_i + 1, Token = curChar.ToString() });
          }
          else if (isTokenBreak(curChar) && !isWhiteSpaceorLineBreake(curChar) && isTokenBreak(lastCharWritten))
          {
            tokens.Add(new ScriptJavaToken { StartInd = str_i, EndInd = str_i + 1, Token = curChar.ToString() });
          }
          else if(isWhiteSpaceorLineBreake(curChar) && !isWhiteSpaceorLineBreake(lastCharWritten) && sb.Length != 0)
          {
            tokens.Add(new ScriptJavaToken { StartInd = startInd, EndInd = str_i, Token = sb.ToString() });
            sb = new StringBuilder();
            startInd = -1;
          }
          else
          {
            if (startInd == -1 && !isWhiteSpaceorLineBreake(curChar))
              startInd = str_i;
            if (!isWhiteSpaceorLineBreake(curChar))
              sb.Append(allStr[str_i]);
          }
        }
        charBefore = allStr[str_i];
      }
      return tokens;
    }
    public void SaveChangesToFile(string fnam)//fnam can be = javaFnam
    {
      List<ScriptJavaToken> toUpdate = new List<ScriptJavaToken>();
      toUpdate.Add(PackageNameToken);
      foreach(var cls in classesDefinedCache)
      {
        if (cls.ClassExtendsName != null)
          toUpdate.Add(cls.ClassExtendsName);
        cls.ClassName.PropagateClassNameChange();
        foreach (var ctor in cls.ClassName.othersWithSameStringValue)
        {
          toUpdate.Add(ctor);
        }
        toUpdate.Add(cls.ClassName);
        foreach (var rpkref in cls.RPKRefs)
        {
          rpkref.GetBoundsAndTokenValueFromComponents();
          toUpdate.Add(rpkref);
        }
      }
      var updateOrder = toUpdate.OrderByDescending(x => x.StartInd);
      string cpyOfCache = new string(javaCache.ToArray());
      foreach(var upd in updateOrder)
      {
        cpyOfCache = cpyOfCache.Remove(upd.StartInd, upd.EndInd - upd.StartInd).Insert(upd.StartInd, upd.Token);
      }
      File.WriteAllText(fnam, cpyOfCache);
    }
    public void SaveChangesToOriginalFile(bool bak = true)
    {
      string javFnamLocal = FileEntry.GetWindowsPhysicalPath(javaFnam);
      if (bak)
      {
        int bakInd = 0;
        while (File.Exists(javFnamLocal + "_BAK_ClassJavaPair_" + bakInd.ToString()))
          bakInd++;
        File.Copy(javFnamLocal, javFnamLocal + "_BAK_ClassJavaPair_" + bakInd.ToString());
      }
      SaveChangesToFile(javFnamLocal);
    }

    private bool isWhiteSpaceorLineBreake(char c)
    {
      string possibleChars = " \t\r\n";
      return (possibleChars.Contains(c));
    }
    private bool isTokenBreak(char c)
    {
      string possibleChars = " \t\r\n{}()[]:.?!=/*-+&|~;<>,";
      return (possibleChars.Contains(c));
    }
    private void loadClasses()
    {
      int curlyDepth = 0;
      Stack<ScriptJavaClassToken> classStack = new Stack<ScriptJavaClassToken>();
      for(int token_i = 0; token_i != Tokens.Count; ++token_i)
      {
        var curT = Tokens[token_i];
        if(curT.Token == "{")
        {
          curlyDepth++;
          continue;
        }
        if (curT.Token == "}")
        {
          curlyDepth--;
          if(curlyDepth < 0)
          {
            throw new Exception("Bad scopes (trying to decode java "+javaFnam+")");
          }
          if (classStack.Any() && classStack.Peek().Depth == curlyDepth)
            classStack.Pop();
          continue;
        }
        if(curT.Token.ToLower() == "package" && PackageNameToken == null)
        {
          PackageNameToken = new ScriptJavaToken();
          token_i++;
          PackageNameToken.StartInd = Tokens[token_i].StartInd;
          PackageNameToken.Token = Tokens[token_i].Token;
          token_i++;
          while (Tokens[token_i].Token != ";")
          {
            PackageNameToken.Token += Tokens[token_i].Token;
            PackageNameToken.EndInd = Tokens[token_i].EndInd;
            token_i++;
          }
          continue;
        }
        if(curT.Token == "class")
        {
          ScriptJavaClassToken toad = new ScriptJavaClassToken(curT);
          token_i++;
          toad.ClassName = new ScriptJavaClassNameToken(Tokens[token_i]);
          if(Tokens[token_i + 1].Token == "extends")
          {
            token_i++;
            token_i++;
            toad.ClassExtendsName = Tokens[token_i];
          }
          toad.Depth = curlyDepth;
          classesDefinedCache.Add(toad);
          classStack.Push(toad);
          continue;
        }
        if(curT.Token == ":" && Tokens[token_i+1].Token.ToLower().StartsWith("0x"))
        {
          ScriptJavaRpkRefToken toad = new ScriptJavaRpkRefToken();
          toad.Token = ":";
          toad.StartInd = curT.StartInd;
          toad.EndInd = curT.EndInd;
          toad.RPKToken = new ScriptJavaToken();
          int back = token_i - 1;
          toad.RPKToken.EndInd = Tokens[back].EndInd;
          toad.RPKToken.StartInd = Tokens[back].StartInd;
          toad.RPKToken.Token = Tokens[back].Token;
          back--;
          while (Tokens[back].Token == "." || Tokens[back].Token.All(x => char.IsLetterOrDigit(x) || ("_-()[],'.".Contains(x) && Tokens[back].Token.Length > 1)))
          {
            toad.RPKToken.StartInd = Tokens[back].StartInd;
            toad.RPKToken.Token = Tokens[back].Token + toad.RPKToken.Token;
            back--;
          }
          token_i++;
          toad.IDToken = Tokens[token_i];
          classStack.Peek().RPKRefs.Add(toad);
          continue;
        }
        foreach(var cls in classesDefinedCache)
        {
          if(cls.ClassName.Token == curT.Token)
          {
            cls.ClassName.othersWithSameStringValue.Add(curT);
          }
        }
      }
    }
  }
}
