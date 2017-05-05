using System;
using AutoMapper;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using nh_spikes.Dtos;
using nh_spikes.Entities;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace nh_spikes
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static ISessionFactory SessionFactory;

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            Mapper.Initialize(c =>
            {
                c.CreateMap<Store, StoreDto>();
                c.CreateMap<Employee, EmployeeDto>();
            });

            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            CreateSessionFactory();
            CreateData();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            pipelines.AfterRequest.AddItemToEndOfPipeline(CloseSession(container));
        }

        private static Action<NancyContext> CloseSession(TinyIoCContainer container)
        {
            return ctx => container.Resolve<ISession>().Close();
        }

        private static void CreateSessionFactory()
        {
            SessionFactory = Fluently
                .Configure()
                .Database(MsSqlConfiguration
                    .MsSql2012
                    .ConnectionString(@"Server=.\sqlexpress;Database=nh-spikes;Integrated Security=true")
                    .ShowSql
                )
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Program>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            var schemaExport = new SchemaExport(config);
            schemaExport.Drop(false, true);
            schemaExport.Create(false, true);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register(SessionFactory.OpenSession());
        }

        static void CreateData()
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // create a couple of Stores each with some Products and Employees
                    var barginBasin = new Store {Name = "Bargin Basin"};
                    var superMart = new Store {Name = "SuperMart"};

                    var potatoes = new Product {Name = "Potatoes", Price = 3.60};
                    var fish = new Product {Name = "Fish", Price = 4.49};
                    var milk = new Product {Name = "Milk", Price = 0.79};
                    var bread = new Product {Name = "Bread", Price = 1.29};
                    var cheese = new Product {Name = "Cheese", Price = 2.10};
                    var waffles = new Product {Name = "Waffles", Price = 2.41};

                    var daisy = new Employee {FirstName = "Daisy", LastName = "Harrison"};
                    var jack = new Employee {FirstName = "Jack", LastName = "Torrance"};
                    var sue = new Employee {FirstName = "Sue", LastName = "Walkters"};
                    var bill = new Employee {FirstName = "Bill", LastName = "Taft"};
                    var joan = new Employee {FirstName = "Joan", LastName = "Pope"};

                    // add products to the stores, there's some crossover in the products in each
                    // store, because the store-product relationship is many-to-many
                    AddProductsToStore(barginBasin, potatoes, fish, milk, bread, cheese);
                    AddProductsToStore(superMart, bread, cheese, waffles);

                    // add employees to the stores, this relationship is a one-to-many, so one
                    // employee can only work at one store at a time
                    AddEmployeesToStore(barginBasin, daisy, jack, sue);
                    AddEmployeesToStore(superMart, bill, joan);

                    // save both stores, this saves everything else via cascading
                    session.SaveOrUpdate(barginBasin);
                    session.SaveOrUpdate(superMart);

                    transaction.Commit();
                }
            }
        }

        public static void AddProductsToStore(Store store, params Product[] products)
        {
            foreach (var product in products)
            {
                store.AddProduct(product);
            }
        }

        public static void AddEmployeesToStore(Store store, params Employee[] employees)
        {
            foreach (var employee in employees)
            {
                store.AddEmployee(employee);
            }
        }
    }
}