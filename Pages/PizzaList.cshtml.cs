using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace ContosoPizza.Pages
{
    public class PizzaListModel : PageModel
    {
        private readonly PizzaService _service;
        static TelemetryConfiguration tctconfiguration = new TelemetryConfiguration("9077be67-ea86-43ac-a958-378c288275c7");
        TelemetryClient tc = new TelemetryClient(tctconfiguration);

        [BindProperty]
        public Pizza NewPizza { get; set; } = default!;
        
        public IList<Pizza> PizzaList { get;set; } = default!;

        public PizzaListModel(PizzaService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            PizzaList = _service.GetPizzas();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewPizza == null)
            {
                return Page();
            }
            _service.AddPizza(NewPizza);
            tc.TrackTrace("New Pizza " + NewPizza.Name +" added");
            
            return RedirectToAction("Get");
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.DeletePizza(id);
            tc.TrackTrace("Pizza deleted: PizzaId :" + id);
            return RedirectToAction("Get");
        }
    }
}
