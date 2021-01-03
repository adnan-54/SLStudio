using SLStudio.Language.Rdf;
using System;
using Xunit;

namespace SLStudio.Language.Tests.Rdf
{
    public class RdfDefinitionFactoryShould
    {
        [Fact]
        public void ThrowWhenInvalidType()
        {
            //arrange
            var factory = new DefaultRdfDefinitionFactory();
            object result = null;
            bool @throw = false;

            //act
            try
            {
                result = factory.CreateFromType(GetType());
            }
            catch (InvalidOperationException)
            {
                @throw = true;
            }

            //assert
            Assert.True(@throw);
            Assert.Null(result);
        }

        [Fact]
        public void CreateWhenValidType()
        {
            //arrange
            var factory = new DefaultRdfDefinitionFactory();
            var type = typeof(RdfDefinition);

            //act
            var result = factory.CreateFromType(type);

            //assert
            Assert.True(result is RdfDefinition);
        }

        [Fact]
        public void CreateFromGenericType()
        {
            //arrange
            var factory = new DefaultRdfDefinitionFactory();

            //act
            var result = factory.CreateFromType<RdfDefinition>();

            //assert
            Assert.True(result is RdfDefinition);
        }

        [Fact]
        public void CreateFromMetadata()
        {
            //arrange
            var factory = new DefaultRdfDefinitionFactory();
            var metadata = new RdfAttributes(typeof(RdfDefinition));

            //act
            var result = factory.CreateFromAttribute(metadata);

            //assert
            Assert.True(result is RdfDefinition);
        }
    }
}