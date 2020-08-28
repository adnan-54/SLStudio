using System.Text;

namespace SLStudio.RpkEditor.Data
{
    internal class HexId
    {
        private readonly bool isSuperId;

        public HexId(ResourceMetadata parent, bool isSuperId)
        {
            Parent = parent;
            this.isSuperId = isSuperId;
        }

        public ResourceMetadata Parent { get; }

        public int Id { get; private set; }

        public string Hex => GetHexString();

        private string GetHexString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("0x00");
            stringBuilder.Append(GetExternalRefId());
            stringBuilder.Append($"{Id:X4}");

            return stringBuilder.ToString();
        }

        private string GetExternalRefId()
        {
            //if (isSuperId && Parent.HasExternalReference)
            //    return Parent.ExternalReference.Id.ToString("X2");
            return "00";
        }

        public override string ToString()
        {
            return Hex;
        }
    }
}