using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryRpkHeader : FileEntry
  {
    protected static readonly int offsetSignature = 0;
    protected static readonly int sizeSiganture = 4;
    protected static readonly int offsetVersion512 = 4;
    protected static readonly int offsetExternalReferencesCount = 8;
    protected static readonly int offsetExternalReferencesUnkownZero = 12;

    public string Siganture
    {
      get
      {
        return GetFixLengthString(offsetSignature, sizeSiganture);
      }
      set
      {
        SetFixLengthString(value, sizeSiganture, offsetSignature);
      }
    }
    public int Version512
    {
      get
      {
        return GetIntFromFile(offsetVersion512);
      }
      set
      {
        SetIntInFile(value, offsetVersion512);
      }
    }
    public int ExternalReferencesCount
    {
      get
      {
        return GetIntFromFile(offsetExternalReferencesCount);
      }
      set
      {
        SetIntInFile(value, offsetExternalReferencesCount);
      }
    }
    public int ExternalReferencesUnkownZero
    {
      get
      {
        return GetIntFromFile(offsetExternalReferencesUnkownZero);
      }
      set
      {
        SetIntInFile(value, offsetExternalReferencesUnkownZero);
      }
    }
    public override int Size
    {
      get
      {
        return 16;
      }
      set
      {
        if(value != 16)
          throw new Exception("RpkHeader must be 16 long");
      }
    }

    public BinaryRpkHeader(FileCacheHolder file,int offset=0,bool cache = false)
    : base(file,offset,cache)
    {

    }
  }
}
