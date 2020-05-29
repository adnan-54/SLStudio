using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class MessageLog
  {
    public static List<Message> Messages
    {
      get;
      private set;
    } = new List<Message>();
    public static IEnumerable<Message> Errors
    {
      get
      {
        return Messages.Where(x => x.CurrentMessageType == MessageType.Error);
      }
    }

    public static void AddError(string msg = "GENERAL ERROR NO MESSAGE", string fnam = "NO ATTACHED FILE", int offset = 0)
    {
      Messages.Add(new Message(msg, fnam, offset, MessageType.Error));
      onErrorAdded(Messages.Last());
    }
    public static void AddMessage(string msg, string fnam = "NO ATTACHED FILE", int offset = 0)
    {
      Messages.Add(new Message(msg, fnam, offset, MessageType.Message));
      onMessageAdded(Messages.Last());
    }

    public static event MessageAddedEventHandler MessageAdded;
    public static event MessageAddedEventHandler ErrorAdded;

    public delegate void MessageAddedEventHandler(Message e);

    private static void onMessageAdded(Message e)
    {
      if (MessageAdded != null)
        MessageAdded(e);
    }
    private static void onErrorAdded(Message e)
    {
      if (ErrorAdded != null)
        ErrorAdded(e);
    }
    private static void messageLog_ErrorAdded(Message e)
    {
      var wasColor = Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(e.Msg);
      Console.ForegroundColor = wasColor;
    }
    private static void messageLog_MessageAdded(Message e)
    {
      Console.WriteLine(e.Msg);
    }

    public static void SetConsoleLogOutput()
    {
      MessageAdded += messageLog_MessageAdded;
      ErrorAdded += messageLog_ErrorAdded;
    }
    public static string MessagesString()
    {
      return Messages.Select(x => x.ToString()).Aggregate((x, y) => x + "\n" + y);
    }
    public static void SaveMessagesToFile(string fnam)
    {
      StringBuilder sb = new StringBuilder();
      foreach(var ln in Messages)
      {
        sb.AppendLine(ln.Msg);
      }
      System.IO.File.WriteAllText(fnam, sb.ToString());
    }
  }
}
