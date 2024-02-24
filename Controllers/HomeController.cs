using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CacheTest.Models;
using Microsoft.AspNetCore.OutputCaching;
namespace CacheTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOutputCacheStore _outputCacheStore;

        public HomeController(ILogger<HomeController> logger, IOutputCacheStore ocs)
        {
            _logger = logger;
            _outputCacheStore = ocs;
        }

        /// <summary>
        /// Renders the Index view.
        /// </summary>
        /// <returns>The view result for the Index action.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renders the Privacy view.
        /// </summary>
        /// <returns>The view result for the Privacy action.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Renders the Error view with an ErrorViewModel.
        /// </summary>
        /// <returns>The view result for the Error action.</returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Returns a string with the quote number specified by the <paramref name="number"/> parameter.
        /// The output is cached for a duration of 60 seconds.
        /// </summary>
        /// <param name="number">The quote number.</param>
        /// <returns>The quote string.</returns>
        [OutputCache(Duration = 3600, NoStore = false, PolicyName = "ByNumber")]
        public string Quote([FromQuery]int number)
        {
            return $"This is the quote number: {number}";
        }

        public async Task<string> RemoveQuote([FromQuery]int number, CancellationToken ct)
        {
            await _outputCacheStore.EvictByTagAsync(number.ToString(), ct);
            return $"This is the quote number: {number}";
        }
    }
}
