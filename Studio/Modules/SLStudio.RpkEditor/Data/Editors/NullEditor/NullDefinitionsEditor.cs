namespace SLStudio.RpkEditor.Data
{
    internal class NullDefinitionsEditor : IDefinitionsEditor
    {
        private static NullDefinitionsEditor instance;

        public bool IsValid => true;

        private NullDefinitionsEditor()
        {
        }

        public static NullDefinitionsEditor Default = instance ??= new NullDefinitionsEditor();

        public void ApplyChanges()
        {
        }

        public void LoadValues()
        {
        }

        public bool Validate()
        {
            return true;
        }
    }
}