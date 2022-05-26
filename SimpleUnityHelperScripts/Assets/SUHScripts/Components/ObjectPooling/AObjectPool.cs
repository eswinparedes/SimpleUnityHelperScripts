namespace SUHScripts
{
    public abstract class AObjectPool<T>
    {
        public abstract T Get();
        public abstract bool Release(T obj);
    }
}