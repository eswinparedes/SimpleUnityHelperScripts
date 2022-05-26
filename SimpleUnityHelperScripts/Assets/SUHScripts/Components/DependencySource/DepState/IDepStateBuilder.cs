using SUHScripts;
using SUHScripts.Functional;

namespace SUHScripts
{
    public interface IDepStateBuilder<T>
    {
        public Option<IState<T>> Build(IDependencySource deps);
    }
    public interface IDepStateBuilder
    {
        public Option<IState> Build(IDependencySource deps);
    }

}
