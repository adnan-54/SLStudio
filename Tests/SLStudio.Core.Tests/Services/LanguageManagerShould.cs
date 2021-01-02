using SLStudio.Core.LanguageManager;
using Xunit;

namespace SLStudio.Core.Tests
{
    public class LanguageManagerShould
    {
        public LanguageManagerShould()
        {
        }

        [Fact]
        public void ResolveNullLanguageCode()
        {
            //arrange
            var language = new Language(null);

            //assert
            Assert.Equal("Auto", language.DisplayName);
        }
    }
}