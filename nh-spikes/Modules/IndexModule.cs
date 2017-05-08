using Nancy;

namespace nh_spikes.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/{id}"] = _ => View["index", new {Id = (int) _.id}];
        }
    }
}