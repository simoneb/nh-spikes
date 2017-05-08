using FluentNHibernate.Mapping;
using nh_spikes.Entities;

namespace nh_spikes.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            References(x => x.Store);
            OptimisticLock.All();
            DynamicUpdate();
        }
    }}