using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Utility;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
   
        private readonly IUnitofWork unitofWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitofWork unitofWork ,IMapper mapper)
        {
            this.unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? SearchValue )
        {
            IEnumerable<Employee> Employees;
            if (string.IsNullOrWhiteSpace(SearchValue))
            {

                 Employees = await unitofWork.Employees.GetAllAsync();
                return View(_mapper.Map<IEnumerable<EmployeeVM>>(Employees));
            }

             Employees = await unitofWork.Employees.GetAllByNameAsync(SearchValue);
            return View(_mapper.Map<IEnumerable<EmployeeVM>>(Employees));
        }
        public IActionResult Create()
        {
            ViewBag.Departments= unitofWork.Departments.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM employeeVm)
        {
            if (ModelState.IsValid)
            {

                employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image,"Images");
                var employee = _mapper.Map<EmployeeVM,Employee>(employeeVm);

                await unitofWork.Employees.AddAsync(employee);
                await unitofWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var Employees = await unitofWork.Employees.GetAllAsync();

            return View(employeeVm);

        }

        public async Task<IActionResult> Details(int? id) => await ReturnViewWithEmployee(id, nameof(Details));




        public async Task<IActionResult> Edit(int? id) => await ReturnViewWithEmployee(id, nameof(Edit));


        [HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Edit(EmployeeVM employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVm.Image is not null)
                    {
                        employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");

                    }
                    unitofWork.Employees.Update(_mapper.Map<EmployeeVM,Employee >(employeeVm));
                  await  unitofWork.CompleteAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }

            }

            return View(employeeVm);
        }


        public async Task<IActionResult> Delete(int? id) => await ReturnViewWithEmployee(id, nameof(Delete));



        [HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Delete(EmployeeVM EmployeeVM, [FromRoute] int id)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    unitofWork.Employees.Delete(_mapper.Map<EmployeeVM, Employee>(EmployeeVM));
                    if (await unitofWork.CompleteAsync()>0 &&EmployeeVM.ImageName is not null)
                    
                       DocumentSettings.DeleteFile(EmployeeVM.ImageName, "Images");
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }

            }

            return View(EmployeeVM);
        }

        public async Task<IActionResult> ReturnViewWithEmployee(int? id, string ViewName)
        {
            if (!id.HasValue)
            {

                return BadRequest();

            }
            var employee = await unitofWork.Employees.GetAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            ViewBag.Departments =  await unitofWork.Departments.GetAllAsync();

            return View(ViewName, _mapper.Map<EmployeeVM>(employee));
        }
    }
}
