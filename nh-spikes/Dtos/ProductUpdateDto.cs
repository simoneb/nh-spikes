using nh_spikes.Entities;
using AutoMapper.Attributes;

namespace nh_spikes.Dtos
{
    [MapsTo(typeof(Product))]
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}