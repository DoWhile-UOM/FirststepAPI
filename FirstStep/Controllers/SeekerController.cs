﻿using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerController : ControllerBase
    {
        private readonly ISeekerService _service;

        public SeekerController(ISeekerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllSeekers")]

        public async Task<ActionResult<IEnumerable<Seeker>>> GetAllSeekers()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetSeeker/{seekerId:int}")]

        public async Task<ActionResult<Seeker>> GetSeekerById(int seekerId)
        {
            return Ok(await _service.GetById(seekerId));
        }

        [HttpPost]
        [Route("AddSeeker")]

        public async Task<IActionResult> AddSeeker(AddSeekerDto newSeeker)
        {
            await _service.Create(newSeeker);
            return Ok($"Suessfully added new seeker: {newSeeker.first_name} {newSeeker.last_name}");
        }

        [HttpPut]
        [Route("UpdateSeeker/{seekerId:int}")]

        public async Task<IActionResult> UpdateSeeker(Seeker reqseeker, int seekerId)
        {
            if (seekerId != reqseeker.user_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(seekerId, reqseeker);
            return Ok($"Successfully Updated SeekerID: {reqseeker.first_name} {reqseeker.last_name}");
        }

        [HttpDelete]
        [Route("DeleteSeeker/{seekerId:int}")]

        public async Task<IActionResult> DeleteSeeker(int seekerId)
        {
            await _service.Delete(seekerId);
            return Ok();
        }
    }
}
