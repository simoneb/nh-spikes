using System.Collections.Generic;

namespace nh_spikes.Dtos
{
    public class StoreDto
    {
        public string Name { get; set; }
        public IList<EmployeeDto> Staff { get; set; }
    }

    public class EmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}