using System.Threading.Tasks;

namespace SLStudio
{
    public interface IMenuToggleHandler : IMenuItemHandler<IMenuToggle>
    {
        Task OnToggle();
    }
}