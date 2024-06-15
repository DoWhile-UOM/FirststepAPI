using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetEmployeeById/{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpGet]
        [Route("GetAllHRManagers/{company_Id:int}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllHRManagers(int company_Id)
        {
            return Ok(await _service.GetAllHRManagers(company_Id));
        }

        [HttpGet]
        [Route("GetAllHRAssistants/{company_Id:int}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllHRAssistants(int company_Id)
        {
            return Ok(await _service.GetAllHRAssistants(company_Id));
        }

        [HttpGet]
        [Route("GetAllEmployees/{company_Id:int}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees(int company_Id)
        {
            return Ok(await _service.GetAllEmployees(company_Id));
        }


        [HttpPost]
        [Route("AddNewHRManager")]
        public async Task<IActionResult> AddHRManager(AddEmployeeDto newHRManager)
        {
            await _service.CreateHRManager(newHRManager);
            return Ok("Successfully Added");
        }

        [HttpPost]
        [Route("AddNewHRAssistant")]
        public async Task<IActionResult> AddHRAssistant(AddEmployeeDto newHRAssistant)
        {
            await _service.CreateHRAssistant(newHRAssistant);
            return Ok("Successfully Added");
        }

        [HttpPost]
        [Route("AddNewCompanyAdmin")]
        public async Task<IActionResult> AddCompanyAdmin(AddCompanyAdminDto newCompanyAdmin)
        {
            await _service.CreateCompanyAdmin(newCompanyAdmin);
            return Ok("Successfully Added");
        }

        [HttpPut]
        [Route("UpdateEmployee/{id:int}")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDto reqEmployee, int id)
        {
            await _service.Update(id, reqEmployee);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
