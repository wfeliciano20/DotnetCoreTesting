using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD_Testing_DEMO.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPeopleService _peopleService;

        private readonly ICountriesService _countriesService;

        public PeopleController(ILogger<PeopleController> logger, IPeopleService peopleService, ICountriesService countriesService)
        {
            _logger = logger;
            _peopleService = peopleService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOptions? sortOptions = SortOptions.ASC)
        {
            Console.WriteLine(searchBy);
            Console.WriteLine(searchString);
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name"},
                { nameof(PersonResponse.Email), "Email"},
                { nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.Address), "Address"},
                { nameof(PersonResponse.Country), "Country"}
            };
            ViewBag.SearchString = searchString;
            ViewBag.SearchBy = searchBy;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOptions = sortOptions.ToString();
            List<PersonResponse> people = _peopleService.GetAllPeople(searchBy, searchString, sortBy, sortOptions);
            return View(people);
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {

            List<CountryResponse> allCountries = _countriesService.GetAllCountries();
            ViewBag.AllCountries = allCountries.Select(country =>
            new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            });
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> allCountries = _countriesService.GetAllCountries();
                ViewBag.AllCountries = allCountries.Select(country =>
                new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse addedPerson = _peopleService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "People");
        }


        [Route("[action]/{personID}")]
        [HttpGet]
        public IActionResult Update(Guid personID)
        {

            PersonResponse? personResponse = _peopleService.GetPersonByPersonID(personID);


            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            List<CountryResponse> allCountries = _countriesService.GetAllCountries();
            ViewBag.AllCountries = allCountries.Select(country =>
            new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            });



            return View(personResponse.ToPersonUpdateRequest());
        }

        [Route("[action]/{personID}")]
        [HttpPost]
        public IActionResult Update(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? personResponse = _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);


            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                List<CountryResponse> allCountries = _countriesService.GetAllCountries();
                ViewBag.AllCountries = allCountries.Select(country =>
                new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse updatedPerson = _peopleService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index", "People");
        }
    }
}