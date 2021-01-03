namespace SLStudio.Language.Rdf
{
    public abstract class RdfDefinitionBase : IRdfDefinition
    {
        public RdfDescription Description => GetDescription();

        protected abstract RdfDescription GetDescription();

        public abstract override string ToString();
    }

    public interface IRdfDefinition
    {
        RdfDescription Description { get; }

        string ToString();
    }
}