using nh_spikes.Entities;
using AutoMapper.Attributes;

namespace nh_spikes.Dtos
{
    [MapsFrom(typeof(Product))]
    public class ProductViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}