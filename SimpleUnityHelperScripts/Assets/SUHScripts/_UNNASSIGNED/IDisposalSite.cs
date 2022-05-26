using System;

public interface IDisposalSite
{
    public void Add(IDisposable disposable);
    public void Add(Action disposal);
}
