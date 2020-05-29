using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptClassIntCollectionChunk : ScriptClassChunk
  {
    public ScriptClassIntCollectionChunk(FileCacheHolder file, int offset, bool cache = false)
    : base(file, offset, cache)
    {

    }
    public IEnumerable<int> IntData
    {
      get
      {
        int numOfInts = RawDataOfEntry.Length / 4;
        List<int> ret = new List<int>();
        for(int i = 0; i != numOfInts; ++i)
        {
          ret.Add(BitConverter.ToInt32(RawDataOfEntry, i * 4));
        }
        return ret;
      }
    }
  }
}
