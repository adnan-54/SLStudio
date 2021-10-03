using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using SLStudio.Core;
using System.Collections.Generic;

namespace SLStudio.JavaEditor
{
    internal class JavaFoldingStrategy : IFoldingStrategy
    {
        private readonly char openingBrace;
        private readonly char closingBrace;

        public JavaFoldingStrategy()
        {
            openingBrace = '{';
            closingBrace = '}';
        }

        public void UpdateFoldings(FoldingManager manager, TextDocument document)
        {
            IEnumerable<NewFolding> newFoldings = CreateNewFoldings(document, out int firstErrorOffset);
            manager.UpdateFoldings(newFoldings, firstErrorOffset);
        }

        public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            return CreateNewFoldings(document);
        }

        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            var newFoldings = new List<NewFolding>();
            var startOffsets = new Stack<int>();
            var lastNewLineOffset = 0;

            for (int i = 0; i < document.TextLength; i++)
            {
                var c = document.GetCharAt(i);
                if (c == openingBrace)
                    startOffsets.Push(i);
                else
                if (c == '\n' || c == '\r')
                    lastNewLineOffset = i + 1;
                else
                if (c == closingBrace && startOffsets.Count > 0)
                {
                    var startOffset = startOffsets.Pop();

                    if (startOffset < lastNewLineOffset)
                        newFoldings.Add(new NewFolding(startOffset, i + 1));
                }
            }

            newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));

            return newFoldings;
        }
    }
}