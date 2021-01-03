namespace SLStudio.Language.Rdf
{
    public class DescriptionToken
    {
        public string Description { get; }

        public bool Hightlight { get; }

        public DescriptionToken(string value, bool hightlight = false)
        {
            Description = value;
            Hightlight = hightlight;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}