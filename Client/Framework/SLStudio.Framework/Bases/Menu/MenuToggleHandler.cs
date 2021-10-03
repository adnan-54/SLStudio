namespace SLStudio
{
    public abstract class MenuToggleHandler : MenuHandler<IMenuToggle>
    {
        protected virtual void OnToggle()
        {
        }
    }
}