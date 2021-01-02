using ProtoBuf;

namespace SLStudio.Data.Rdf
{
    [ProtoContract]
    internal class RdfFileProto
    {
        public RdfFileProto()
        {
        }

        public RdfFileProto(string content, string studioVersion, RdfFileMetadata metadata)
        {
            Content = content;
            StudioVersion = studioVersion;
            Author = metadata?.Author;
            LogicalPath = metadata?.LogicalPath;
            TargetVersion = metadata?.TargetVersion;
            FileVersion = metadata?.FileVersion;
            Password = metadata?.Password;
        }

        [ProtoMember(1)]
        public string Author { get; set; }

        [ProtoMember(2)]
        public string LogicalPath { get; set; }

        [ProtoMember(3)]
        public string TargetVersion { get; set; }

        [ProtoMember(4)]
        public string FileVersion { get; set; }

        [ProtoMember(5)]
        public string StudioVersion { get; set; }

        [ProtoMember(6)]
        public string Password { get; set; }

        [ProtoMember(7)]
        public string Content { get; set; }
    }
}