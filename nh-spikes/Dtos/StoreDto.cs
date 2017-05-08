using System.Collections.Generic;

namespace nh_spikes.Dtos
{
    public class StoreDto
    {
        public string Name { get; set; }
        public IList<EmployeeUpdateDto> Staff { get; set; }
    }
}