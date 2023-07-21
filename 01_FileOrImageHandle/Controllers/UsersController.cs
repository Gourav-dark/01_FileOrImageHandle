using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _01_FileOrImageHandle.ContextFile;
using _01_FileOrImageHandle.Models;

namespace _01_FileOrImageHandle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ContextDbConn _context;
        private readonly IWebHostEnvironment _env;

        public UsersController(ContextDbConn context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'ContextDbConn.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPut]
        public async Task<IActionResult> UploadImage(Guid Id, IFormFile File)
        {
            try
            {
                var existuser=await _context.Users.FindAsync(Id);
                if (File != null && File.Length > 0 && existuser!=null)
                {
                    string fileName = Id.ToString() + "_profile";
                    string fileExtension = Path.GetExtension(File.FileName); // Get the file extension from the uploaded file
                    string fileNameWithExtension = fileName + fileExtension; // Combine the file name and extension

                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages", fileNameWithExtension);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }

                    string imageUrl = $"{Request.Scheme}://{Request.Host}/ProfileImages/{fileNameWithExtension}"; // Generate the image URL
                    existuser.ImageUrl = imageUrl;
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "Profile Image Updated", ImageUrl = imageUrl });
                }
                else
                {
                    return BadRequest("No file uploaded");
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
