namespace SUHScripts
{
    //TODO: Separate into 2 interfaces "PoolableGet and PoolableRelease"
    public interface IPoolableComponent
    {
        void OnGet();
        void OnRelease();
    }

}