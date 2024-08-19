using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
          
        }

        public async Task <IEnumerable<Employee>> GetAllAsync(string address)
        =>await _context.Employees.Where(a=>a.Address==address).ToListAsync();

        public async Task< IEnumerable<Employee>> GetAllByNameAsync(string Name)
        {
          return await _context.Employees.Include(e=>e.Department).Where(a=>a.Name.ToLower().Contains(Name.ToLower())).ToListAsync();
        }
    }
}
