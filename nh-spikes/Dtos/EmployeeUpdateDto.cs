using AutoMapper.Attributes;
using nh_spikes.Entities;

namespace nh_spikes.Dtos
{
    [MapsTo(typeof(Employee))]
    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}