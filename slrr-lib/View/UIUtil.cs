using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.View
{
  public class UIUtil
  {
    public static float ParseOrZero(string text)
    {
      float parsed = 0;
      if (float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed))
      {
        return parsed;
      }
      return 0;
    }
    public static string FloatToString(float f)
    {
      return f.ToString("F8", System.Globalization.CultureInfo.InvariantCulture);
    }
    public static bool ParseOrFalse(string text,out float f)
    {
      return float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f);
    }
    public static bool ParseOrFalse(string text,out int i)
    {
      return int.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i);
    }
    public static float ParseOrThrow(string text)
    {
      return float.Parse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
    }
  }
}
