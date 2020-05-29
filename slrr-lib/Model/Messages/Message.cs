using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public enum MessageType
  {
    Error,
    Message
  }

  public class Message
  {
    public string Msg
    {
      get;
      private set;
    }
    public string MsgCorrespondingFileName
    {
      get;
      private set;
    }
    public string StackTrace
    {
      get;
      private set;
    }
    public int ErrorOffset
    {
      get;
      private set;
    }
    public MessageType CurrentMessageType
    {
      get;
      private set;
    }

    public Message(string msg,string fnam,int offset,MessageType type)
    {
      Msg = msg;
      MsgCorrespondingFileName = fnam;
      ErrorOffset = offset;
      StackTrace = Environment.StackTrace;
      CurrentMessageType = type;
    }

    public override string ToString()
    {
      return "(" + ErrorOffset.ToString() + ") " + Msg + " [" + MsgCorrespondingFileName + "]\n\t"+StackTrace.Replace("\n","\n\t").Replace("\r","");
    }
  }
}
