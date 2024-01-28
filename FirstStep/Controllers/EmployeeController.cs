using FirstStep.Models;
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
        [Route("GetEmployeeById/{id}")]

        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpGet]
        [Route("GetAllHRManagers/{company_Id}")]
        
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllHRManagers(int company_Id)
        {
            return Ok(await _service.GetAllHRManagers(company_Id));
        }

        [HttpGet]
        [Route("GetAllHRAssistants/{company_Id}")]

        public async Task<ActionResult<IEnumerable<Employee>>> GetAllHRAssistants(int company_Id)
        {
            return Ok(await _service.GetAllHRAssistants(company_Id));
        }

        [HttpGet]
        [Route("GetAllEmployees/{company_Id}")]

        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees(int company_Id)
        {
            return Ok(await _service.GetAllEmployees(company_Id));
        }

        [HttpPost]
        [Route("AddNewHRManager")]

        public async Task<IActionResult> AddHRManager(HRManager hRManager)
        {
            await _service.CreateHRManager(hRManager);
            return Ok("Successfully Added");
        }

        [HttpPost]
        [Route("AddNewHRAssistant")]

        public async Task<IActionResult> AddHRAssistant(HRAssistant hRAssistant)
        {
            await _service.CreateHRAssistant(hRAssistant);
            return Ok("Successfully Added");
        }

        [HttpPost]
        [Route("AddNewCompanyAdmin")]

        public async Task<IActionResult> AddCompanyAdmin(CompanyAdmin companyAdmin)
        {
            await _service.CreateCompanyAdmin(companyAdmin);
            return Ok("Successfully Added");
        }

        [HttpPut]
        [Route("UpdateEmployee")]

        public async Task<IActionResult> UpdateEmployee(Employee reqEmployee)
        {
            await _service.Update(reqEmployee);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
