using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SlrrLib.Model
{
  public class CfgLineToken
  {
    private string tokenVal;
    private float floatvalCache = 0;
    private bool floatvalCacheSet = false;
    private int intvalCache = 0;
    private bool intvaleCacheSet = false;
    private int hexIntvalCache = 0;
    private bool hexIntvaleCacheSet = false;

    public string Value
    {
      get
      {
        return tokenVal;
      }
      set
      {
        tokenVal = value;
        floatvalCacheSet = false;
        intvaleCacheSet = false;
        hexIntvaleCacheSet = false;
      }
    }
    public bool IsComment
    {
      get
      {
        return Value.StartsWith(";") ||
               Value.StartsWith("//") ||
               Value.StartsWith("#");
      }
      set
      {
        if (value && !IsComment)
          Value = ";" + Value;
        if(!value && IsComment)
        {
          if(Value.StartsWith("//"))
            Value = Value.Substring(2);
          else
            Value = Value.Substring(1);
        }
      }
    }
    public float ValueAsFloat
    {
      get
      {
        try
        {
          if(!floatvalCacheSet)
          {
            floatvalCache = float.Parse(Value, CultureInfo.InvariantCulture);
            floatvalCacheSet = true;
          }
          return floatvalCache;
        }
        catch(Exception)
        {

        }
        return 0;
      }
      set
      {
        floatvalCacheSet = true;
        floatvalCache = value;
        Value = value.ToString("F6",CultureInfo.InvariantCulture);
      }
    }
    public int ValueAsInt
    {
      get
      {
        try
        {
          if (!intvaleCacheSet)
          {
            intvalCache = int.Parse(Value, CultureInfo.InvariantCulture);
            intvaleCacheSet = true;
          }
          return intvalCache;
        }
        catch(Exception)
        {
          return ValueAsHexInt;
        }
      }
      set
      {
        intvaleCacheSet = true;
        intvalCache = value;
        Value = value.ToString("D",CultureInfo.InvariantCulture);
      }
    }
    public int ValueAsHexInt
    {
      get
      {
        try
        {
          if(!hexIntvaleCacheSet)
          {
            hexIntvalCache = int.Parse(Value.ToLower().Replace("0x", ""), NumberStyles.HexNumber);
            hexIntvaleCacheSet = true;
          }
          return hexIntvalCache;
        }
        catch(Exception)
        {

        }
        return 0;
      }
      set
      {
        hexIntvalCache = value;
        hexIntvaleCacheSet = true;
        Value = "0x"+value.ToString("X8");
      }
    }
    public bool IsValueFloat
    {
      get
      {
        return Value.All(x => char.IsDigit(x) || x == '.' || x == '-')
               && Value.Count(x => x == '.') <= 1
               && Value.Count(x => x == '-') <= 1;
      }
    }
    public bool IsValueInt
    {
      get
      {
        return Value.All(x => char.IsDigit(x) || x == '-')
               && Value.Count(x => x == '-') <= 1;
      }
    }
    public bool IsValueHexInt
    {
      get
      {
        return Value.All(x => "0123456789ABCDEFabcdefxX".Contains(x)) && Value.Count(x => x == 'x' || x == 'X') == 1;
      }
    }
    public string TypeStr
    {
      get
      {
        if (Value == "0")
          return "Z";
        if (IsValueInt)
          return "I";
        if (IsValueFloat)
          return "F";
        if (IsValueHexInt)
          return "H";
        return "S";
      }
    }

    public CfgLineToken(string val)
    {
      Value = val;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}
