using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SewaPatra.BusinessLayer;
using SewaPatra.Models;

namespace SewaPatra.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AreaService _AreaService;
    private readonly CoordinatorService _CoordinatorService;
    private readonly DonorService _DonorService;

    public HomeController(ILogger<HomeController> logger, AreaService areaService, CoordinatorService coordinatorService,DonorService donorService)
    {
        _logger = logger;
        _AreaService = areaService;
        _CoordinatorService = coordinatorService;
        _DonorService = donorService;
    }

    public IActionResult Index()
    {
        ViewBag.AreaCount = _AreaService.CountAreas();
        ViewBag.AreaCountLastWeek = _AreaService.CountAreasLastWeek();
        ViewBag.CoordinatorCount = _CoordinatorService.CountCoordinator();
        ViewBag.CoordinatorCountLastWeek = _CoordinatorService.CountCoordinatorLastWeek();
        ViewBag.DonorCount = _DonorService.CountDonor();
        ViewBag.DonorCountLastWeek = _DonorService.CountDonorLastWeek();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
