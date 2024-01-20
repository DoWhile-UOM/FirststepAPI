using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobFieldController : ControllerBase
    {
        private readonly DataContext _context;

        public JobFieldController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllJobFields")]

        public async Task<ActionResult<IEnumerable<JobField>>> GetJobFields()
        {
            return Ok(await _context.JobFields.ToListAsync());
        }

        [HttpPost]
        [Route("AddJobField")]

        public async Task<ActionResult<JobField>> AddJobField(JobField jobField)
        {
            jobField.field_id = 0;

            _context.JobFields.Add(jobField);
            await _context.SaveChangesAsync();

            return Ok(await _context.JobFields.ToListAsync());
        }

        [HttpPut]
        [Route("UpdateJobField")]

        public async Task<IActionResult> UpdateJobField(JobField reqJobField)
        {
            var dbJobField = await _context.JobFields.FindAsync(reqJobField.field_id);
            if (dbJobField == null)
            {
                return BadRequest("JobField not found.");
            }

            dbJobField.field_name = reqJobField.field_name;

            await _context.SaveChangesAsync();

            return Ok(await _context.JobFields.ToListAsync());
        }

        [HttpDelete]
        [Route("DeleteJobFieldById{id}")]

        public async Task<IActionResult> DeleteJobFieldById(int id)
        {
            var dbJobField = await _context.JobFields.FindAsync(id);
            if (dbJobField == null)
            {
                return BadRequest("JobField not found.");
            }

            _context.JobFields.Remove(dbJobField);
            await _context.SaveChangesAsync();

            return Ok(await _context.JobFields.ToListAsync());
        }
    }
}
