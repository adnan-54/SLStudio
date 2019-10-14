using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models.Studio.Modules.TodoList
{
    public class TodoItemModel : ITodoItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime AddedDate { get; }
        public DateTime? Deadline { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsStared { get; set; }

        public int Progress { get; set; }

        public TodoItemPriority Priority { get; set; }

        public ITodoItemModel Parent { get; }
        public IList<ITodoItemModel> Childrens { get; }
    }

    public enum TodoItemPriority
    {
        Low,
        Medium,
        High
    }

    public interface ITodoItemModel
    {
        string Title { get; set; }
        string Description { get; set; }

        DateTime AddedDate { get; }
        DateTime? Deadline { get; set; }

        bool IsCompleted { get; set; }
        bool IsStared { get; set; }

        int Progress { get; set; }

        TodoItemPriority Priority { get; set; }

        ITodoItemModel Parent { get; }
        IList<ITodoItemModel> Childrens { get; }
    }
}
