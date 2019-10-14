using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models.Studio.Modules.TodoList
{
    public class TodoListModel : ITodoListModel
    {
        public TodoListModel()
        {
            Items = new List<ITodoItemModel>();
        }

        public IList<ITodoItemModel> Items { get; }
        public ITodoItemModel SelectedItem { get; set; }

        public void AddNewItem()
        {
            throw new NotImplementedException();
        }

        public void EditItem()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem()
        {
            throw new NotImplementedException();
        }
    }

    public interface ITodoListModel
    {
        IList<ITodoItemModel> Items { get; }
        ITodoItemModel SelectedItem { get; set; }

        void AddNewItem();
        void EditItem();
        void RemoveItem();
    }
}
