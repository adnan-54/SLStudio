namespace SlrrLib.Model
{
    public class ScriptJavaToken
    {
        public int StartInd;
        public int EndInd;
        public string Token;

        public override string ToString()
        {
            return Token;
        }
    }
}