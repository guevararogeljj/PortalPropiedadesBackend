using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_marketplace.Controllers
{
    [Route("api/filters")]
    [Produces("application/json")]
    [ApiController]
    public class FiltersController : BaseController
    {
        private readonly IStateService _stateService;
        private readonly ICitiesService _citiesService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IProceduralStageService _proceduralStageService;
        private readonly IFiltersService _filterService;
        private readonly IMaritalstatusService _maritalstatusService;
        private readonly IOccupationService _occupationService;

        public FiltersController(IStateService stateService, ICitiesService citiesService, IPropertyTypeService propertyTypeService, IProceduralStageService proceduralStageService, IFiltersService filterservice, IMaritalstatusService maritalstatusService, IOccupationService occupationService, ILogger<FiltersController> logger) : base(logger)
        {
            this._stateService = stateService;
            this._citiesService = citiesService;
            this._propertyTypeService = propertyTypeService;
            this._proceduralStageService = proceduralStageService;
            this._filterService = filterservice;
            this._maritalstatusService = maritalstatusService;
            this._occupationService = occupationService;
        }

        [HttpGet("states")]
        public async Task<IActionResult> States()
        {
            var result = this._stateService.GetAll();

            return JsonResponse(result);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> Cities(int id)
        {
            var result = this._citiesService.FindAll(id);

            return JsonResponse(result);
        }

        [HttpGet("propertytype")]
        public async Task<IActionResult> PropertyType()
        {
            var result = this._propertyTypeService.GetAll();

            return JsonResponse(result);
        }

        [HttpGet("bedrooms")]
        public async Task<IActionResult> Bedrooms()
        {
            var result = this._filterService.GetAll();

            return JsonResponse(result);
        }

        [HttpGet("parkingspaces")]
        public async Task<IActionResult> Levels()
        {
            var result = this._filterService.FindAll(10);

            return JsonResponse(result);
        }

        [HttpGet("fullbathrooms")]
        public async Task<IActionResult> FullBathrooms()
        {
            var result = this._filterService.GetAll();

            return JsonResponse(result);
        }

        [HttpGet("halfbathrooms")]
        public async Task<IActionResult> HalfBathrooms()
        {
            var result = this._filterService.FindAll(10);

            return JsonResponse(result);
        }

        [Authorize]
        [HttpGet("proceduralStage")]
        public async Task<IActionResult> ProceduralStage()
        {
            var result = this._proceduralStageService.GetAll();

            return JsonResponse(result);
        }

        [Authorize]
        [HttpGet("occupations")]
        public async Task<IActionResult> Occupations()
        {
            var result = this._occupationService.GetAll();

            return JsonResponse(result);
        }

        [Authorize]
        [HttpGet("maritalstatus")]
        public async Task<IActionResult> MaritalStatus()
        {
            var result = this._maritalstatusService.GetAll();

            return JsonResponse(result);
        }


    }
}
