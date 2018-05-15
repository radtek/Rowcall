using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomAPI.Data;
using ClassroomAPI.Models;
using ClassroomAPI.Models.ClassRoomModels;
using RowcallBackend.Models.ClassRoomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomAPI.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Route("api/[controller]")]
    public class ClassRoomController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassRoomController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var classRooms = _dbContext.ClassRoom.Where(x => x.TeacherId == currentUser.Id);
            return Json(classRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var classRoom = await _dbContext.ClassRoom.Include(x => x.Students).Include(x => x.Tokens).SingleOrDefaultAsync(x => x.Id == id && x.TeacherId == currentUser.Id);

            if(classRoom == null)
            {
                return NotFound(); 
            }
            return Json(classRoom); 
        }

        [HttpPost]
        public async Task<JsonResult> Create([FromBody] CreateClassRoomDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var classRoom = new ClassRoom() { Name = dto.Name, TeacherId = currentUser.Id };
            _dbContext.ClassRoom.Add(classRoom);
            _dbContext.SaveChanges();
            return Json(classRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ClassRoom item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var classRoom = await _dbContext.ClassRoom.SingleOrDefaultAsync(x => x.Id == id && x.TeacherId == currentUser.Id);

            if (classRoom == null)
            {
                return NotFound();
            }

            classRoom.Name = item.Name;
            classRoom.TeacherId = item.TeacherId;
            classRoom.Students = item.Students;
            classRoom.Tokens = item.Tokens;

            _dbContext.ClassRoom.Update(classRoom);
            _dbContext.SaveChanges();

            return Ok(); 
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddUserByEmail([FromBody] AddUserDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            _dbContext.UserClass.Add(new UserClass() { ApplicationUserId = user.Id, ClassRoomId = model.ClassRoomId });
            _dbContext.SaveChanges();
            return Json("All good bro!"); 
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsForClass(int classRoomId)
        {
            var classRoom = await _dbContext.ClassRoom
                .Include(x => x.Students)
                .ThenInclude(x => x.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id == classRoomId);
            var students = classRoom.Students.Select(x => x.ApplicationUser.Email).ToList();
            return Json(students); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Check with current user => You must not could delete others classrooms...
            var currentUser = await _userManager.GetUserAsync(User);
            var classRoom = await _dbContext.ClassRoom.SingleOrDefaultAsync(x => x.Id == id && x.TeacherId == currentUser.Id);

            if (classRoom == null)
            {
                return NotFound();
            }
            _dbContext.ClassRoom.Remove(classRoom);
            _dbContext.SaveChanges();
            return new NoContentResult(); 
        }

    }
}
