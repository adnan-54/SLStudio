using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public abstract class DynamicRSDInnerEntryBase
  {
    public abstract void Save(BinaryWriter bw);
    public abstract int GetSize();
  }
}
