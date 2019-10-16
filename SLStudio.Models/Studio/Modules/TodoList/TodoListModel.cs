using System;
using System.Collections.Generic;

namespace SLStudio.Models.Studio.Modules.TodoList
{
    public class TodoListModel
    {
        public TodoListModel()
        {
            AddedDate = DateTime.Now;
            Childrens = new List<TodoListModel>();
        }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime AddedDate { get; }
        public DateTime? Deadline { get; set; }
        public DateTime? FinishedDate { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsStared { get; set; }

        public int Progress { get; set; }

        public TodoItemPriority? Priority { get; set; }

        public TodoListModel Parent { get; }
        public IList<TodoListModel> Childrens { get; }
    }

    public enum TodoItemPriority
    {
        Low,
        Medium,
        High
    }
}
