using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryResEntry : FileEntry
  {
    protected static readonly int offsetSuperID = 0;//int
    protected static readonly int offsetTypeID = 4;//int
    protected static readonly int offsetTypeOfEntry = 8; //byte
    protected static readonly int offsetAdditionalType = 9; //byte
    protected static readonly int offsetIsParentCompatible = 10;//float
    protected static readonly int offsetFileOffsetOfRSD = 14;//int
    protected static readonly int offsetRSDLength = 18;//int
    protected static readonly int offsetAliasLength = 22;//max 32, length includes trailing zero
    protected static readonly int maxAliasLength = 32;
    protected static readonly int offsetAlias = 23;//variable length String

    private BinaryRsdEntry rsd = null;
    private IEnumerable<FileEntry> hiddenEntries = null;

    public BinaryResEntry(FileCacheHolder file,int offset,bool cache = false)
    :base(file,offset,cache)
    {

    }

    public int SuperID
    {
      get
      {
        return GetIntFromFile(offsetSuperID);
      }
      set
      {
        SetIntInFile(value, offsetSuperID);
      }
    }
    public int SuperIDExternalRefPart
    {
      get
      {
        return SuperID >> 16;
      }
      set
      {
        int idInExternalRPK = SuperID & 0x0000FFFF;
        SuperID = (value << 16) & idInExternalRPK;
      }
    }
    public int SuperIDExternalIDPart
    {
      get
      {
        return SuperID & 0x0000FFFF;
      }
      set
      {
        int refPart = SuperID >> 16;
        SuperID = refPart & (value & 0x0000FFFF);
      }
    }
    public int TypeID
    {
      get
      {
        return GetIntFromFile(offsetTypeID);
      }
      set
      {
        SetIntInFile(value, offsetTypeID);
      }
    }
    public int TypeIDExternalRefPart
    {
      get
      {
        return TypeID >> 16;
      }
      set
      {
        int idInExternalRPK = TypeID & 0x0000FFFF;
        TypeID = (value << 16) & idInExternalRPK;
      }
    }
    public int TypeIDExternalIDPart
    {
      get
      {
        return TypeID & 0x0000FFFF;
      }
      set
      {
        int refPart = TypeID >> 16;
        TypeID = refPart & (value & 0x0000FFFF);
      }
    }
    public byte TypeOfEntry
    {
      get
      {
        return GetByteFromFile(offsetTypeOfEntry);
      }
      set
      {
        SetByteInFile(value, offsetTypeOfEntry);
      }
    }
    public byte AdditionalType
    {
      get
      {
        return GetByteFromFile(offsetAdditionalType);
      }
      set
      {
        bool floatsWerePresent = AdditionalFloatsPresent;
        SetByteInFile(value, offsetAdditionalType);
        if(!floatsWerePresent && AdditionalFloatsPresent)
        {
          AdditionalFloatList = Enumerable.Repeat<float>(0.0f,12);
        }
      }
    }
    public float IsParentCompatible
    {
      get
      {
        return GetFloatFromFile(offsetIsParentCompatible);
      }
      set
      {
        SetFloatInFile(value, offsetIsParentCompatible);
      }
    }
    public int FileOffsetOfRSD
    {
      get
      {
        return GetIntFromFile(offsetFileOffsetOfRSD);
      }
      set
      {
        SetIntInFile(value, offsetFileOffsetOfRSD);
      }
    }
    public int RSDLength
    {
      get
      {
        return GetIntFromFile(offsetRSDLength);
      }
      set
      {
        SetIntInFile(value, offsetRSDLength);
      }
    }
    public byte AliasLength
    {
      get
      {
        return GetByteFromFile(offsetAliasLength);
      }
      set
      {
        SetByteInFile(value, offsetAliasLength);
      }
    }
    public string Alias
    {
      get
      {
        return GetFixLengthString(offsetAlias, AliasLength);
      }
      set
      {
        string toSet = value;
        if (toSet.Length > maxAliasLength)
          toSet = toSet.Substring(0, maxAliasLength);
        toSet = toSet.Substring(0, toSet.Length - 1) + "\0";
        int oldLength = AliasLength;
        var bytes = ASCIIEncoding.ASCII.GetBytes(toSet);
        int newLength = bytes.Length;
        LengthChangingReplace(bytes, 0, newLength, offsetAlias, oldLength);
        AliasLength = (byte)newLength;
      }
    }
    public BinaryRsdEntry RSD
    {
      get
      {
        if (rsd == null)
        {
          rsd = new BinaryRsdEntry(RSDLength, Cache, FileOffsetOfRSD);
          hiddenEntries = LoadDanglingHiddenEntries();
        }
        return rsd;
      }
      set
      {
        rsd = value;
      }
    }
    public bool AdditionalFloatsPresent
    {
      get
      {
        byte addType = GetByteFromFile(offsetAdditionalType, true);
        return ((addType & 1) != 0);
      }
    }
    public override int Size
    {
      get
      {
        int additionalFloatsLength = 0;
        if (AdditionalFloatsPresent)
          additionalFloatsLength = 48;
        return 23 + GetByteFromFile(offsetAliasLength, true)+additionalFloatsLength;
      }
      set
      {
        if (value != Size)
          throw new Exception("Size must always be: 23+aliasLength+additionalFloatsLength");
      }
    }
    public IEnumerable<float> AdditionalFloatList
    {
      get
      {
        if (!AdditionalFloatsPresent)
          return Enumerable.Empty<float>();
        int offsetStart = offsetAdditionalFloatList;
        List<float> ret = new List<float>();
        for(int i = 0; i != 12; ++i)
        {
          ret.Add(GetFloatFromFile(offsetStart + (i * 4)));
        }
        return ret;
      }
      set
      {
        if (!AdditionalFloatsPresent)
          return;
        if (value.Count() != 12)
          throw new Exception("AdditionalFloatList should have 12 floats");
        int i = 0;
        int offsetStart = offsetAdditionalFloatList;
        foreach(var f in value)
        {
          SetFloatInFile(f, offsetStart + (i * 4));
          i++;
        }
      }
    }
    public int NextResEntriesRSDOffset
    {
      get;
      set;
    } = 0;
    public IEnumerable<FileEntry> DanglingHiddenEntries//should be empty on proper parsing of an rpk
    {
      get
      {
        if (hiddenEntries == null)
          hiddenEntries = LoadDanglingHiddenEntries();
        return hiddenEntries;
      }
      set
      {
        hiddenEntries = value;
      }
    }

    private string correspondingRSDAsString
    {
      get
      {
        return ASCIIEncoding.ASCII.GetString(Cache.GetFileData(), FileOffsetOfRSD, RSDLength);
      }
      set
      {
        string toConvert = value;
        if (!toConvert.EndsWith("\r\n"))
          toConvert += "\r\n";
        var bytes = ASCIIEncoding.ASCII.GetBytes(toConvert);
        LengthChangingReplace(bytes, 0, bytes.Length, FileOffsetOfRSD - Offset, RSDLength);
        RSDLength = bytes.Length;
      }
    }
    private int offsetAdditionalFloatList
    {
      get
      {
        return 23 + GetByteFromFile(offsetAliasLength, true);
      }
    }

    public List<FileEntry> GetFlatHiddenEntries()
    {
      List<FileEntry> flatHiddenEntries = new List<FileEntry>();
      foreach (var innerEntry in DanglingHiddenEntries.Union(getRootOwnedEntries()))
      {
        flatHiddenEntries.Add(innerEntry);
        fillFlatHiddenEntries(flatHiddenEntries, innerEntry);
      }
      return flatHiddenEntries;
    }
    public IEnumerable<FileEntry> LoadDanglingHiddenEntries()
    {
      List<FileEntry> ret = new List<FileEntry>();
      if (TypeOfEntry == 1)
      {
        int currOffset = FileOffsetOfRSD + RSDLength;
        while (currOffset < NextResEntriesRSDOffset)
        {
          var signature = GetFixLengthString(currOffset-Offset, 4,true);
          int signatureInt = GetIntFromFile(currOffset - Offset, true);
          if (signatureInt == 0)
          {
            var datArr = new BinarySpatialNode(Cache, currOffset);
            ret.Add(datArr);
          }
          else if (signature == "POLY")
          {
            ret.Add(new BinaryInnerPolyEntry(Cache, currOffset));
          }
          else if (signature == "PHYS")
          {
            ret.Add(new BinaryInnerPhysEntry(Cache, currOffset));
          }
          else if (signature == "RSD\0")
          {
            ret.Add(new BinaryRSDInnerEntry(Cache, currOffset));
          }
          else if (signature == "EXTP")
          {
            ret.Add(new BinaryEXTPInnerEntry(Cache, currOffset));
          }
          currOffset += ret.Last().Size;
        }
        if(currOffset != NextResEntriesRSDOffset)
        {
          throw new Exception("currOffset != NextResEntriesRSDOffset ? not sure if this is an error");
        }
        //fill ownedEntries; ret is ordered by offset (ascending)
        Dictionary<int, FileEntry> offsetToHiddenEntry = new Dictionary<int, FileEntry>();
        foreach(var hiddenEntry in ret)
        {
          offsetToHiddenEntry[hiddenEntry.Offset] = hiddenEntry;
        }
        List<int> toRemove = new List<int>(ret.Count);
        foreach (var hiddenEntry in ret.Union(RSD.InnerEntries))//copy list will be modified
        {
          if(hiddenEntry is BinarySpatialNode)
          {
            var hiddenUnkEntry = hiddenEntry as BinarySpatialNode;
            foreach(var dataEntry in hiddenUnkEntry.DataArray)
            {
              var dataEntryOwnedEntries = new List<FileEntry>();
              int currentOffset = dataEntry.UnkownOffset;
              while (dataEntryOwnedEntries.Sum(x => x.Size) != dataEntry.UnkownSizeAtOffset)
              {
                dataEntryOwnedEntries.Add(offsetToHiddenEntry[currentOffset]);
                int currentOffsetIndex = ret.BinarySearch(offsetToHiddenEntry[currentOffset], new FileEntryOffsetComparer());
                currentOffsetIndex++;
                if (currentOffsetIndex >= 0 && currentOffsetIndex < ret.Count)
                  currentOffset = ret[currentOffsetIndex].Offset;
                else
                {
                  if (dataEntryOwnedEntries.Sum(x => x.Size) != dataEntry.UnkownSizeAtOffset)
                    throw new Exception("dataEntryOwnedEntries.Sum(x => x.Size) != dataEntry.UnkownSizeAtOffset");
                  toRemove.Add(currentOffsetIndex - 1);
                  break;
                }
                toRemove.Add(currentOffsetIndex - 1);
              }
              dataEntry.OwnedEntries = dataEntryOwnedEntries;
            }
          }
        }
        foreach (var rem in toRemove.OrderByDescending(x => x))
        {
          ret.RemoveAt(rem);
        }
      }
      return ret;
    }
    public override string ToString()
    {
      return "("+TypeOfEntry.ToString()+")(0x"+TypeID.ToString("X8")+")"+Alias + "|" + RSD.RSDDataString;
    }

    private bool isRSDProperString()
    {
      string acceptableChars = "-_.&\\\0\r\n\t ;/()#";
      return correspondingRSDAsString.All(x => char.IsLetterOrDigit(x) || acceptableChars.Any(y => y == x));
    }
    private IEnumerable<FileEntry> getRootOwnedEntries()
    {
      List<FileEntry> ret = new List<FileEntry>();
      foreach (var innerEntry in RSD.InnerEntries)
      {
        if (innerEntry is BinarySpatialNode)
        {
          var innerUnkEntry = innerEntry as BinarySpatialNode;
          foreach (var dat in innerUnkEntry.DataArray)
          {
            ret.AddRange(dat.OwnedEntries);
          }
        }
      }
      return ret;
    }
    private void fillFlatHiddenEntries(List<FileEntry> flatHiddenEntries, FileEntry currentEntry)
    {
      if (currentEntry is BinarySpatialNode)
      {
        var currentUnkEntry = currentEntry as BinarySpatialNode;
        foreach (var dat in currentUnkEntry.DataArray)
        {
          foreach (var ownedEntry in dat.OwnedEntries)
          {
            flatHiddenEntries.Add(ownedEntry);
          }
        }
        foreach (var dat in currentUnkEntry.DataArray)
        {
          foreach (var ownedEntry in dat.OwnedEntries)
          {
            fillFlatHiddenEntries(flatHiddenEntries, ownedEntry);
          }
        }
      }
    }

    private class FileEntryOffsetComparer : IComparer<FileEntry>
    {
      int IComparer<FileEntry>.Compare(FileEntry x, FileEntry y)
      {
        return x.Offset.CompareTo(y.Offset);
      }
    }
  }
}
