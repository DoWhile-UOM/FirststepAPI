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

        [HttpPost]
        [Route("AddNewHRManager")]

        public IActionResult AddHRManager(Employee hRManager)
        {
            _service.CreateHRManager(hRManager);
            return Ok();
        }

        [HttpPost]
        [Route("AddNewHRAssistant")]

        public IActionResult AddHRAssistant(Employee hRAssistant)
        {
            _service.CreateHRAssistant(hRAssistant);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateEmployee")]

        public IActionResult UpdateEmployee(Employee reqEmployee)
        {
            _service.Update(reqEmployee);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]

        public IActionResult DeleteEmployee(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
