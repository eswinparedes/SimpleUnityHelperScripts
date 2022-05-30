using System.Collections.Generic;
using System;

namespace SUHScripts
{

    public class DisposalSiteSubject : IDisposalSite, IDisposable
    {
        List<IDisposable> m_disposables = new List<IDisposable>();
        List<Action> m_disposals = new List<Action>();

        bool m_isDisposed = false;

        public void Add(IDisposable disposable)
        {
            if (m_isDisposed)
            {
                disposable.Dispose();
                return;
            }
            else
            {
                m_disposables.Add(disposable);
            }
        }

        public void Add(Action disposal)
        {
            if (m_isDisposed)
            {
                disposal();
                return;
            }
            else
            {
                m_disposals.Add(disposal);
            }
        }

        public void Dispose()
        {
            if (!m_isDisposed)
            {
                for (int i = 0; i < m_disposables.Count; i++)
                {
                    m_disposables[i].Dispose();
                }
                m_disposables.Clear();
                m_disposables = null;

                for (int i = 0; i < m_disposals.Count; i++)
                {
                    m_disposals[i]();
                }
                m_disposals.Clear();
                m_disposals = null;

                m_isDisposed = true;
            }
        }
    }

}