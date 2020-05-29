using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryRpkExternalReference : FileEntry
  {
    protected static readonly int offsetUnkownIndexZero = 0;
    protected static readonly int offsetIndexOfReference = 2;
    protected static readonly int offsetReferenceString = 4;
    protected static readonly int sizeReferenceString = 60;

    public string ReferenceString
    {
      get
      {
        var ret = GetFixLengthString(offsetReferenceString, sizeReferenceString);
        if(ret.Contains('\0'))
          ret = ret.Substring(0,ret.IndexOf('\0'));
        return ret;
      }
      set
      {
        SetFixLengthString(value, sizeReferenceString, offsetReferenceString);
      }
    }
    public short IndexOfReference
    {
      get
      {
        return GetShortFromFile(offsetIndexOfReference);
      }
      set
      {
        if (value == 0)
          throw new Exception("External reference index cannot be 0");
        SetShortInFile(value, offsetIndexOfReference);
      }
    }
    public short UnkownIndexZero
    {
      get
      {
        return GetShortFromFile(offsetUnkownIndexZero);
      }
      set
      {
        SetShortInFile(value, offsetUnkownIndexZero);
      }
    }
    public override int Size
    {
      get
      {
        return 64;
      }
      set
      {
        if (value != 64)
          throw new Exception("RpkExternalReference must be 64 long");
      }
    }

    public BinaryRpkExternalReference(FileCacheHolder file, int offset, bool cache = false)
    : base(file,offset,cache)
    {
    }

    public override string ToString()
    {
      return ReferenceString;
    }
  }
}
