using MMLib.RapidPrototyping.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;

namespace SLStudio.Core.Helpers
{
    public static class StringHelpers
    {
        public static string RandomString(int minLen = 0, int maxLen = 0, bool allowNumbers = true, bool allowWhiteSpace = true, bool allowRepeatingChar = true, CharacterCasing characterCasing = CharacterCasing.Normal)
        {
            if (minLen <= 0)
                minLen = 0;
            if (maxLen <= 0)
                maxLen = 0;
            if (minLen > maxLen)
                minLen = maxLen;
            if (maxLen < minLen)
                maxLen = minLen;

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            Thread.Sleep(1);
            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            if (minLen == 0 && maxLen == 0)
            {
                minLen = random.Next(0, 50);
                maxLen = random.Next(minLen, 10000);
            }

            var lenght = random.Next(minLen, maxLen);
            var stringChars = new char[lenght];

            for (int i = 0; i < stringChars.Length; i++)
            {
                var selectedChar = chars[random.Next(chars.Length)];

                if (char.IsWhiteSpace(selectedChar) && !allowWhiteSpace)
                    while (char.IsWhiteSpace(selectedChar))
                        selectedChar = RandomString(minLen: 1, maxLen: 1, allowNumbers: allowNumbers, allowWhiteSpace: false, allowRepeatingChar: allowRepeatingChar).ToCharArray()[0];

                if (char.IsNumber(selectedChar) && !allowNumbers)
                    while (char.IsNumber(selectedChar))
                        selectedChar = RandomString(minLen: 1, maxLen: 1, allowNumbers: false, allowWhiteSpace: allowWhiteSpace, allowRepeatingChar: allowRepeatingChar).ToCharArray()[0];

                if (!allowRepeatingChar && i > 0 && selectedChar == stringChars[i - 1])
                    while (selectedChar == stringChars[i - 1])
                        selectedChar = RandomString(minLen: 1, maxLen: 1, allowNumbers: allowNumbers, allowWhiteSpace: allowWhiteSpace, allowRepeatingChar: false).ToCharArray()[0];

                stringChars[i] = selectedChar;
            }

            var result = new string(stringChars);

            if (characterCasing == CharacterCasing.Lower)
                result = result.ToLower();
            else
            if (characterCasing == CharacterCasing.Upper)
                result = result.ToUpper();

            return result;
        }

        public static string RandomClass()
        {
            var classes = typeof(StringHelpers).Assembly.GetTypes().Where(t => t.IsClass);
            Thread.Sleep(1);
            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var selectedClass = classes.ElementAt(random.Next(0, classes.Count() - 1));
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regexItem.IsMatch(selectedClass.Name))
                while (!regexItem.IsMatch(selectedClass.Name))
                    selectedClass = classes.ElementAt(random.Next(0, classes.Count() - 1));

            return selectedClass.Name;
        }

        public static string RandomInterface()
        {
            var interfaces = typeof(StringHelpers).Assembly.GetTypes().Where(t => t.IsInterface);
            Thread.Sleep(1);
            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var selectedInterface = interfaces.ElementAt(random.Next(0, interfaces.Count() - 1));
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regexItem.IsMatch(selectedInterface.Name))
                while (!regexItem.IsMatch(selectedInterface.Name))
                    selectedInterface = interfaces.ElementAt(random.Next(0, interfaces.Count() - 1));

            return selectedInterface.Name;
        }

        public static string RandomNamespace()
        {
            var namespaces = typeof(StringHelpers).Assembly.GetTypes().Select(t => t.Namespace).Distinct();
            Thread.Sleep(1);
            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var selectedNamespace = namespaces.ElementAt(random.Next(0, namespaces.Count() - 1));
            var regexItem = new Regex("^[a-zA-Z0-9 .]*$");

            if (!regexItem.IsMatch(selectedNamespace))
                while (!regexItem.IsMatch(selectedNamespace))
                    selectedNamespace = namespaces.ElementAt(random.Next(0, namespaces.Count() - 1));

            return selectedNamespace;
        }

        public static string LoremIpsum(int paragraphs = 0, int maxSentences = 0)
        {
            if (paragraphs <= 0)
            {
                Thread.Sleep(1);
                var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                paragraphs = random.Next(1, 15);
            }

            if (maxSentences <= 0)
            {
                Thread.Sleep(1);
                var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                maxSentences = random.Next(3, 15);
            }

            var loremIpsumGenerator = new LoremIpsumGenerator();
            return loremIpsumGenerator.Next(paragraphs, maxSentences);
        }

        public static string RandomWord()
        {
            var wordGenerator = new WordGenerator();
            return wordGenerator.Next();
        }

        public static IEnumerable<string> RandomWords(int count = 0)
        {
            if (count <= 0)
            {
                Thread.Sleep(1);
                var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                count = random.Next(1, 40);
            }
            var wordGenerator = new WordGenerator();
            return wordGenerator.Next(count);
        }

        public static string RandomName(bool firstNameOnly = true)
        {
            var personGenerator = new PersonGenerator();
            var selectedPerson = personGenerator.Next();
            return firstNameOnly ? selectedPerson.FirstName : $"{selectedPerson.FirstName} {selectedPerson.LastName}";
        }
    }
}