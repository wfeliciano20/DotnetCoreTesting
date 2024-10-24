using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace CRUD_Testing_DEMO.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPeopleService _peopleService;

        public PeopleController(ILogger<PeopleController> logger, IPeopleService peopleService)
        {
            _logger = logger;
            _peopleService = peopleService;
        }

        [Route("Index")]

        [Route("/")]
        public IActionResult Index()
        {

            List<PersonResponse> people = _peopleService.GetAllPeople(null, null, null, null);
            return View(people);
        }

    }
}