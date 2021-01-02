using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SLStudio.Data.Rdf.Tests
{
    public class RdfFilePersisterShould
    {
        [Fact]
        public void CreateNew()
        {
            //arrange
            var sut = new DefaultRdfFilePersister();

            //act
            var rdf = sut.New();

            //assert
            Assert.True(string.IsNullOrEmpty(rdf.FilePath));
            Assert.True(string.IsNullOrEmpty(rdf.Content));
            Assert.NotNull(rdf.Metadata);
        }

        [Fact]
        public async Task SaveNew()
        {
            //arrange
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.rdf");
            var sut = new DefaultRdfFilePersister();
            var rdf = sut.New();
            rdf.Content = "lalala";

            //act
            await sut.SaveAs(path, rdf);

            //assert
            Assert.True(File.Exists(path));
        }

        [Fact]
        internal async Task Save()
        {
            //arrange
            var content = "lalala";
            var path = @"Assets\lalala.rdf";
            var sut = new DefaultRdfFilePersister();
            var rdf = await sut.Read(path);
            rdf.Content = content;

            //act
            await sut.Save(rdf);

            //assert
            Assert.True(File.Exists(path));
            var changed = await sut.Read(path);
            Assert.True(changed.Content == content);
        }

        [Fact]
        internal async Task SaveAs()
        {
            //arrange
            var path = @"Assets\lalala.rdf";
            var sut = new DefaultRdfFilePersister();
            var rdf = await sut.Read(path);
            var newPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.rdf");

            //act
            await sut.SaveAs(newPath, rdf);

            //assert
            Assert.True(File.Exists(path));
            Assert.True(File.Exists(newPath));

            var @new = await sut.Read(newPath);
            Assert.True(@new.Content == rdf.Content);
        }

        [Fact]
        internal async Task Read()
        {
            //arrange
            var path = @"Assets\lalala.rdf";
            var fullPath = Path.GetFullPath(path);
            var sut = new DefaultRdfFilePersister();

            //act
            var rdf = await sut.Read(path);

            //assert
            Assert.True(!string.IsNullOrEmpty(rdf.Content));
            Assert.True(rdf.FilePath == fullPath);
        }
    }
}