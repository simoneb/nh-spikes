using AutoMapper;
using Nancy;

namespace nh_spikes.Extensions
{
    public static class ResponseExtensions
    {
        public static Response AutomapJson<T>(this IResponseFormatter response, object model)
        {
            return response.AsJson(Mapper.Map<T>(model));
        }
    }
}