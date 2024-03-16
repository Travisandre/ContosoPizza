using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoPizza.Pages
{
    public class IndexModel : PageModel
    {
        public TimeSpan TimeInBusiness {get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["Title"] =  "The Home for Pizza Lovers";
            TimeInBusiness = DateTime.Now - new DateTime(2018, 8, 14);
        }

       
    }
}