using System;
using System.Collections.Generic;
using System.Linq;
using IntegrationApi.Interfaces;
using IntegrationApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;

namespace IntegrationApi.Controllers
{
    [Route("entries")]
    public class EntriesController : Controller
    {
        private IDataBaseService _dataBaseService { get; }
        private IConfiguration _configuration { get; }

        public EntriesController(IDataBaseService dataBaseService, IConfiguration configuration)
        {
            _dataBaseService = dataBaseService;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetInfo")]
        [AllowAnonymous]
        public Dictionary<string, string> GetInfo()
        {
            return new Dictionary<string, string>
            {
                { "connectionString", _configuration["ConnectionStrings:Sql"]},
                { "integrationName", _configuration["AuthorInformations:IntegrationName"]},
                { "studentName", _configuration["AuthorInformations:StudentName"] },
                { "studentIndex", _configuration["AuthorInformations:StudentIndex"] }
            };
        }

        [HttpGet]
        [Route("GetEntries")]
        [Authorize(Policy = "User")]
        public IActionResult GetEntries(string pageLimit, string pageId)
        {
            if(pageLimit == null && pageId == null)
            {
                return RedirectToAction("GetAllEntries");
            }
            else if (pageLimit == null || pageId == null)
            {
                return BadRequest("Nieprawidłowa wartość");
            }
            else if(!pageLimit.All(x => char.IsDigit(x)) || !pageId.All(x => char.IsDigit(x)))
            {
                return BadRequest("Nieprawidłowa wartość");
            }
            else
            {
                return Ok(_dataBaseService.ReturnRecords(Convert.ToInt32(pageLimit), Convert.ToInt32(pageId)));
            }

        }
        [Route("GetAllEntries")]
        [Authorize(Policy = "User")]
        public IActionResult GetAllEntries()
        {
            return Ok(_dataBaseService.ReturnRecords());
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "User")]
        public IActionResult GetEntry(string id)
        {

            if(!id.All(x => char.IsDigit(x)))
            {
                return BadRequest("Żadanie musi być liczbą");
            }
            else
            {
                return Ok(_dataBaseService.ReturnRecord(Convert.ToInt32(id)));
            }
        }

        [HttpPut]
        [Route("UpdateEntry")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpdateEntry(int? id,[FromBody]Entry Entry)
        {
            if(ModelState.IsValid)
            {
                if(id != null)
                {
                    Entry.ID = (int)id;
                    bool Sukces = _dataBaseService.UpdateRecords(Entry);

                    if (Sukces == false)
                    {
                        return NotFound("Nie istnieje element o podanym Id");
                    }
                    return Ok("Pomyslnie zaktualizowany");
                }
                else
                {
                    return BadRequest("Potrzebny jest id elementu ktory ma zostac zmodyfikowany");
                }
            }
            else
            {
                return BadRequest(Entry);
            }
        }
    }
}