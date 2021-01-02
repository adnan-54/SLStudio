using ProtoBuf;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.Data.Rdf
{
    public class DefaultRdfFilePersister : IRdfFilePersister
    {
        public IRdfFile New()
        {
            return RdfFile.Builder.Create();
        }

        public Task<IRdfFile> Read(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
                throw new FileNotFoundException($"File {file.FullName} does not exist");

            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var proto = Serializer.Deserialize<RdfFileProto>(stream);
            var rdf = RdfFile.Builder
                .With(path, proto.Content)
                .With(new RdfFileMetadata(proto.LogicalPath, proto.TargetVersion, proto.Author, proto.FileVersion, proto.StudioVersion, proto.Password))
                .Create();

            return Task.FromResult(rdf);
        }

        public Task Save(IRdfFile rdf)
        {
            if (string.IsNullOrEmpty(rdf.FilePath))
                throw new InvalidOperationException("'FilePath' is null or empty");

            var proto = new RdfFileProto(rdf.Content, GetAssemblyVersion(), rdf.Metadata);
            using (var stream = new FileStream(rdf.FilePath, FileMode.Truncate, FileAccess.Write, FileShare.Read))
                Serializer.Serialize(stream, proto);

            return Task.FromResult(true);
        }

        public Task<IRdfFile> SaveAs(string path, IRdfFile rdf)
        {
            var file = new FileInfo(path);
            var proto = new RdfFileProto(rdf.Content, GetAssemblyVersion(), rdf.Metadata);

            var fileMode = file.Exists ? FileMode.Truncate : FileMode.Create;
            using (var stream = new FileStream(file.FullName, fileMode, FileAccess.Write, FileShare.Read))
                Serializer.Serialize(stream, proto);

            return Read(path);
        }

        private static string GetAssemblyVersion()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.ToString();
        }
    }
}