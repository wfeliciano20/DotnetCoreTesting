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
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOptions? sortOptions = SortOptions.ASC)
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
            List<PersonResponse> people = await _peopleService.GetAllPeople(searchBy, searchString, sortBy, sortOptions);
            return View(people);
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // values for dropdown menu
            List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
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
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                // values for dropdown menu
                List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
                ViewBag.AllCountries = allCountries.Select(country =>
                new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse addedPerson = await _peopleService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "People");
        }


        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Update(Guid personID)
        {

            // Check if Id is valid and person exists
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personID);


            if (personResponse is null)
            {
                // Person not found redirect to index
                return RedirectToAction("Index");
            }

            // provide values for dropdown menu
            List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
            ViewBag.AllCountries = allCountries.Select(country =>
            new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            });


            // provide the model to the view
            return View(personResponse.ToPersonUpdateRequest());
        }

        [Route("[action]/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Update(PersonUpdateRequest personUpdateRequest)
        {
            // Check if Id is valid and person exists
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);


            if (personResponse is null)
            {
                // Person not found redirect to index
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                // redirect to same view

                // provide values for menu
                List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
                ViewBag.AllCountries = allCountries.Select(country =>
                new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                });
                // Generate server side error list to render on view
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            // update the person
            PersonResponse updatedPerson = await _peopleService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index", "People");
        }


        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personID);

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [Route("[action]/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            if (await _peopleService.DeletePerson(personUpdateRequest.PersonID))
            {
                return RedirectToAction("Index");
            }

            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); ;
        }
    }
}