using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
      
        private readonly IUnitofWork _unitofWork;

        public DepartmentController(IUnitofWork unitofWork)
        {
          
            this._unitofWork = unitofWork;
        }
        public async Task<IActionResult> Index()
        {
           var departments= await _unitofWork.Departments.GetAllAsync();
            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]

		public async  Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
               await _unitofWork.Departments.AddAsync(department);
                await _unitofWork.CompleteAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(department);
           
        }

        public async Task<IActionResult> Details(int? id)=> await ReturnViewWithDepartment(id,nameof(Details));




        public async Task<IActionResult> Edit(int? id) => await ReturnViewWithDepartment(id, nameof(Edit));


        [HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if(id!=department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _unitofWork.Departments.Update(department);
                    _unitofWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("",ex.Message);
                }

            }

            return View(department);
        }


        public async Task< IActionResult> Delete(int? id) => await ReturnViewWithDepartment(id, nameof(Delete));



        [HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Delete(Department department , [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitofWork.Departments.Delete(department);
                    _unitofWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }
               
            }

            return View(department);
        }

        public async Task< IActionResult> ReturnViewWithDepartment(int? id,string ViewName)
        {
            if (!id.HasValue)
            {

                return BadRequest();

            }
            var dep = await _unitofWork.Departments.GetAsync(id.Value);
            if (dep is null)
            {
                return NotFound();
            }
            return View(ViewName,dep);
        }
    }
}
