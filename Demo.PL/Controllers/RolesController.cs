using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMapper _mapper;
        public RolesController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {

            var roles = Enumerable.Empty<RoleVM>();



            if (string.IsNullOrWhiteSpace(name))
            {

                roles = await _roleManager.Roles.Select(r => new RoleVM
                {
                    Id = r.Id,
                   Name=r.Name
              

                }).ToListAsync();


                return View(roles);

            }
         
            var role= await _roleManager.FindByNameAsync(name);

            if (role is null) return View(Enumerable.Empty<RoleVM>());

            var mappedRole = new RoleVM
            {
                Id = role.Id,
                Name = role.Name,

            };

            return View(roles);


        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleVM model)
        {
            if (ModelState.IsValid)
            {
            
                var mappedRole = _mapper.Map<IdentityRole>(model);
            
                var result= await _roleManager.CreateAsync(mappedRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));

                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            
            }     

        
        return View(model);
        }



        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (string.IsNullOrWhiteSpace(id)) { return BadRequest(); }


            var user = await _roleManager.FindByIdAsync(id);

            if (user == null) { return NotFound(); }

            var mappedRole = _mapper.Map<IdentityRole, RoleVM>(user);


            return View(ViewName, mappedRole);
        }


        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleVM model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return NotFound();
                role.Name = model.Name;
                await _roleManager.UpdateAsync(role);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(model);
        }



        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, nameof(Delete));
        }


        [HttpPost]




        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return NotFound();

                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }

    }
}
