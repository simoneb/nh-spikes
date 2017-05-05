using System.Collections.Generic;

namespace nh_spikes.Entities
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual IList<Store> StoresStockedIn { get; protected set; }
        public virtual int Version { get; set; }

        public Product()
        {
            StoresStockedIn = new List<Store>();
        }
    }}