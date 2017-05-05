using System;
using System.Collections.Generic;
using System.Linq;
using nh_spikes.Dtos;
using nh_spikes.Entities;
using nh_spikes.Extensions;
using Nancy;
using Nancy.JsonPatch;
using Nancy.ModelBinding;
using NHibernate;
using NHibernate.Linq;

namespace nh_spikes.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule(ISession session)
        {
            Get["/products"] = _ => Response.AsJson(
                session.Query<Product>()
                    .Select(p => new {p.Id, p.Name, p.Price})
                    .ToList()
            );

            Put["/product/{id}"] = EditProduct(session);

            Get["/stores"] = _ => Response.AsJson(
                session.Query<Store>()
                    .Select(s => new
                    {
                        s.Name,
                        Staff = s.Staff.Select(e => new {e.FirstName})
                    }).ToList()
            );

        }

        private Func<dynamic, object> EditProduct(ISession session)
        {
            return _ =>
            {
                var id = (int) _.id;

                var product = session.Get<Product>(id);
                var jsonPatchResult = this.JsonPatch(product);

                session.SaveOrUpdate(product);
                session.Flush();

                return Response.AsJson(jsonPatchResult);
            };
        }
    }
}