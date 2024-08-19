using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
	public class UsersController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		private readonly IMapper _mapper;
        public UsersController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string email)
		{

			var users = Enumerable.Empty<UserVM>();



			if (string.IsNullOrWhiteSpace(email))
			{

				users =  await _userManager.Users.Select(u => new UserVM
				{
					Id = u.Id,
					FName = u.FName,
					LName = u.LName,
					Email = u.Email,
					Roles = _userManager.GetRolesAsync(u).Result

				}).ToListAsync();




			}
			else {
			
			users= await _userManager.Users.Where(u=>u.Email.ToLower().Contains(email.ToLower())).Select(u=> new UserVM
            {
                Id = u.Id,
                FName = u.FName,
                LName = u.LName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result

            }).ToListAsync();
			
			
			}
			return View(users);

		
		}



		public async Task<IActionResult> Details(string id,string ViewName="Details")
		{
			if (string.IsNullOrWhiteSpace(id)) { return BadRequest(); }


			var user= await _userManager.FindByIdAsync(id);

			if (user ==null) { return NotFound(); }

			var mappedUser = _mapper.Map<AppUser, UserVM>(user);

			mappedUser.Roles = await _userManager.GetRolesAsync(user);

			return View(ViewName, mappedUser);
        }


		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id,nameof(Edit));
		}

		[HttpPost]
        public async Task<IActionResult> Edit(string id,UserVM model)
        {
			if (id!=model.Id) 
			{
				return BadRequest();
			}
			if (!ModelState.IsValid) {
			return View(model);
			}
			try
			{
				var user = await _userManager.FindByIdAsync(id);
				if (user == null) return NotFound();
				user.FName=model.FName;
					user.LName=model.LName;
					await _userManager.UpdateAsync(user);

					return RedirectToAction(nameof(Index));
				
			}
			catch (Exception e)
			{
				ModelState.AddModelError("",e.Message) ;
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
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

			await	 _userManager.DeleteAsync(user);
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
