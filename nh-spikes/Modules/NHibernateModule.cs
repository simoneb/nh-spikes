using Nancy;
using NHibernate;

namespace nh_spikes.Modules
{
    public class NHibernateModule : NancyModule
    {
        public ISession Session { get; }

        public NHibernateModule(ISession session)
        {
            Session = session;
        }
    }
}