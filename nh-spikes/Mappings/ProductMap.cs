using FluentNHibernate.Mapping;
using nh_spikes.Entities;

namespace nh_spikes.Mappings
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Price);
            HasManyToMany(x => x.StoresStockedIn)
                .Cascade.All()
                .Inverse()
                .Table("StoreProduct");
            Version(p => p.Version);
            OptimisticLock.Version();
            DynamicUpdate();
        }
    }}