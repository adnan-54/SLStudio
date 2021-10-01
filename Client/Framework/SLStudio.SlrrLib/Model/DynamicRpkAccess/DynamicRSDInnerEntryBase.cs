using System.IO;

namespace SlrrLib.Model
{
    public abstract class DynamicRSDInnerEntryBase
    {
        public abstract void Save(BinaryWriter bw);

        public abstract int GetSize();
    }
}