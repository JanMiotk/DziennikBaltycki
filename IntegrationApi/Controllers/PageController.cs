using IntegrationApi.Interfaces;
using IntegrationApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationApi.Controllers
{
    [Route("page")]
    public class PageController : Controller
    {
        private IDataBaseService _dataBaseService { get; }
        public PageController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpPost]
        [Route("LoadPage")]
        [Authorize(Policy = "User")]
        public  IActionResult page([FromBody] PageNumber pageNumber)
        {
            if (pageNumber==null)
            {
                return BadRequest("Nieprawidłowa wartość");
            }
            else
            {
                 return Ok(_dataBaseService.InsertIntoDataBase(pageNumber.pageNumber));
            }
        }
    }
}