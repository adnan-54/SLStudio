using System.Collections.Generic;

namespace SlrrLib.Geom
{
    public interface IModelFactory
    {
        IEnumerable<NamedModel> GenModels(string modelGroup);

        IEnumerable<string> GetModelGroups();
    }
}