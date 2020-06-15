using System.Collections.Generic;

namespace SlrrLib.Model
{
    public class ScriptJavaClassNameToken : ScriptJavaToken
    {
        public List<ScriptJavaToken> othersWithSameStringValue = new List<ScriptJavaToken>();

        public ScriptJavaClassNameToken(ScriptJavaToken cpy)
        {
            Token = cpy.Token;
            StartInd = cpy.StartInd;
            EndInd = cpy.EndInd;
        }

        public void PropagateClassNameChange()
        {
            foreach (var other in othersWithSameStringValue)
                other.Token = Token;
        }
    }
}