using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationApi.Interfaces;
using IntegrationApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationApi.Controllers
{
    [Route("offers")]
    public class BestOffersController : Controller
    {
        private IOccasion _occasion { get; }
        public BestOffersController(IOccasion okazje)
        {
            _occasion = okazje;
        }


        [HttpGet]
        [Route("GetBestOffers")]
        public IActionResult NajlepszeOferty()
        {
            // Najtańsze ogłoszenia z ceną za metr w danym mieście
            return Ok(_occasion.ReturnTheBestOffers());
        }

    }
}