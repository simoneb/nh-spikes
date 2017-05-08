using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using nh_spikes.Dtos;
using nh_spikes.Entities;
using nh_spikes.Extensions;
using nh_spikes.SignalR;
using Nancy;
using NHibernate;
using NHibernate.Linq;

namespace nh_spikes.Modules
{
    public class MainModule : NHibernateModule
    {
        public MainModule(ISession session) : base(session)
        {
            Get["/products"] = _ =>
            {
                var products = session.Query<Product>()
                    .Select(p => new {p.Id, p.Name, p.Price})
                    .ToList();

                var context = GlobalHost.ConnectionManager.GetHubContext<ProductHub>();
                context.Clients.All.productsViewed(products);

                return Response.AsJson(products);
            };

            Get["/product/{id}"] = _ =>
            {
                var product = session.Get<Product>((int)_.id);

                return Response.AutomapJson<ProductViewDto>(product);
            };

            Put["/product/{id}"] = EditProduct(session);
            Put["/employee/{id}"] = EditEmployee(session);

            Get["/stores"] = _ => Response.AsJson(
                session.Query<Store>()
                    .Select(s => new
                    {
                        s.Name,
                        Staff = s.Staff.Select(e => new {e.FirstName})
                    })
                    .ToList()
            );
        }

        private static IHubContext ProductHub
        {
            get { return GlobalHost.ConnectionManager.GetHubContext<ProductHub>(); }
        }

        private Func<dynamic, object> EditProduct(ISession session)
        {
            return _ =>
            {
                var id = (int) _.id;

                var product = this.AutomapBindTo<ProductUpdateDto, Product>(id);
                session.Flush();

                return Response.AsJson(new
                {
                    product.Name
                });
            };
        }

        private Func<dynamic, object> EditEmployee(ISession session)
        {
            return _ =>
            {
                var id = (int) _.id;

                var employee = this.AutomapBindTo<EmployeeUpdateDto, Employee>(id);
                session.Flush();

                return Response.AsJson(new
                {
                    employee.FirstName,
                    employee.LastName
                });
            };
        }
    }
}