using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
       Task<IEnumerable<Employee>> GetAllAsync(string Address);

        Task<IEnumerable<Employee>> GetAllByNameAsync(string Name);
    }
}
