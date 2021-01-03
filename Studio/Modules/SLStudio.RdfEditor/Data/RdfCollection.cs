using SLStudio.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RdfEditor
{
    public class RdfCollection : BindableCollection<RdfMetadata>
    {
        private readonly List<string> rawLines;

        public RdfCollection() : this(Enumerable.Empty<RdfMetadata>())
        {
        }

        public RdfCollection(IEnumerable<RdfMetadata> collection) : base(collection)
        {
            rawLines = this.Select(c => c.ToString()).ToList();
        }

        public IEnumerable<RdfMetadata> All => this;

        public RdfMetadata FromLineNumber(int lineNumber)
        {
            var index = lineNumber - 1;
            if (index >= 0 && index < Items.Count)
                return Items[index];

            return null;
        }

        public int LineNumber(RdfMetadata metadata)
        {
            return IndexOf(metadata) + 1;
        }

        public override string ToString()
        {
            for (int i = 0; i < Count; i++)
            {
                if (i < rawLines.Count)
                {
                    if (rawLines[i] == this[i].ToString())
                        continue;
                    else
                        rawLines[i] = this[i].ToString();
                }
                else
                    rawLines.Add(this[i].ToString());
            }

            if (rawLines.Count > Count)
                rawLines.RemoveRange(Count, rawLines.Count - Count);

            return string.Join(Environment.NewLine, rawLines);
        }
    }
}