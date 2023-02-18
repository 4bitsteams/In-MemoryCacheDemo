using In_MemoryCacheDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace In_MemoryCacheDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCash;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCash)
        {
            _logger = logger;
            _memoryCash = memoryCash;
        }

        public IActionResult Index()
        {
            string cashEntry;

            if (!_memoryCash.TryGetValue("myCashKey",out cashEntry))
            {
                cashEntry= TakeLongTime();
                var cashEntryOption = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCash.Set("myCashKey", cashEntry, cashEntryOption);
            }
            ViewBag.Result = cashEntry;
            return View();
        }

        private string TakeLongTime()
        {
            Thread.Sleep(1500);
            return "I Am Finishing for IO: result here"; ;
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
}