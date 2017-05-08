using AutoMapper;
using nh_spikes.Modules;
using Nancy;
using Nancy.ModelBinding;

namespace nh_spikes.Extensions
{
    public static class ResponseExtensions
    {
        public static TOutput AutomapBindTo<TInput, TOutput>(
            this NHibernateModule module, object id) where TOutput : class
        {
            var source = module.Bind<TInput>();
            var output = Mapper.Map(
                source, module.Session.Get<TOutput>(id)
            );

            module.Session.Evict(output);
            module.Session.SaveOrUpdate(output);
            return output;
        }

        public static Response AutomapJson<T>(this IResponseFormatter response, object model)
        {
            return response.AsJson(Mapper.Map<T>(model));
        }
    }
}