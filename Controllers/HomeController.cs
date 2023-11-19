using Microsoft.AspNetCore.Mvc;
using Reto_sophos2.Models;
using Reto_sophos2.Servicios;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Task = Reto_sophos2.Models.Task;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Reto_sophos2.Controllers
{

    public class HomeController : Controller
    {
        private readonly IServicio_API servicioAPI;
        public HomeController(IServicio_API servicioAPI) { 

            this.servicioAPI = servicioAPI;
        
        }
   
        public IActionResult Index()
        {
            return View();
        }

        //[Route("home")]
        public IActionResult MiVista()
        {
            return View("FirstView");
        }


        //[Route("Heros_view")]
        public IActionResult Heroes()
        {
            return View("Heroes");
        }

        public IActionResult Villains()
        {
           
            return View("Villains");
           
        }

        public async Task<IActionResult> SearchVillains()
        {
            List<Villain> VillainR = new List<Villain>();
            string? pattern = HttpContext.Request.Form["pattern"];
            string? opcion = HttpContext.Request.Form["Dropdown1"];

            
                switch (opcion)
                {

                    case "nombre":
                        VillainR = await servicioAPI.GetVillainsByName(pattern);

                        // usar un viewbag mejor? 
                        return View(VillainR);

                    case "origen":
                        VillainR = await servicioAPI.GetVillainssByOrigin(pattern);
                        return View(VillainR);

                    case "debilidad":
                        VillainR = await servicioAPI.GetVillainssByWeak(pattern);
                        return View(VillainR);

                    case "todo":
                        VillainR = await servicioAPI.GetVillains();
                        return View(VillainR);


                }
            return View(VillainR);

        }
        public IActionResult Living()
        {
            return View("Living");
        }

        public IActionResult Info()
        {
            return View("Information");
        }

        public IActionResult Agenda()
        {
            return View("Agenda");
        }
        [HttpPost]
        public async Task<IActionResult> AgendaWeek()
        {
            string? heroName = HttpContext.Request.Form["heroname"];
            List<Task> tasks = await servicioAPI.GetTasks(heroName);
            return View(tasks);
        }
        public IActionResult Sponsors()
        {
            return View("Sponsors");
        }

        [HttpPost]
        public async Task<IActionResult> SponsorsDetails()
        {
            string? heroName = HttpContext.Request.Form["heroname"];
            string? opcion = HttpContext.Request.Form["Dropdown1"];


            List<Sponsor> sponsors2 = new List<Sponsor>();

            if (opcion != null && heroName != null)
                switch (opcion)
                {
                    case "high":
                        sponsors2 = await servicioAPI.GetHighestSponsor(heroName);
                        return View(sponsors2);

                    case "todo":
                        sponsors2 = await servicioAPI.GetSponsors(heroName);
                        return View(sponsors2);
                }

            return View(sponsors2);
        }


        public async Task <IActionResult> Peleas()
        {
            List<Fight> fights = await servicioAPI.GetFights();
            return View(fights);
        }
        public async Task<IActionResult> PeleasG()
        {
            List<Hero> heroesR = new List<Hero>();
            Villain villain = new Villain();
            string? pattern = HttpContext.Request.Form["heroname"];
            string? opcion = HttpContext.Request.Form["Dropdown1"];

            if(opcion != null && pattern != null)
            switch (opcion)
            {

                case "Villano":
                    villain = await servicioAPI.GetMostFightedVillain(pattern);
                    ViewBag.Nombre = villain.VillainName;
                    ViewBag.RN = villain.RealName;
                    ViewBag.Poderes = villain.Powers;
                    ViewBag.Debilidades = villain.Weaks;
                    ViewBag.Age = villain.Age;
                    ViewBag.img = villain.Img;
                    ViewBag.cantbat = servicioAPI.GetNumberofFacings();


                    return View(null);

                case "VillanoL":
                    villain = await servicioAPI.GetVillainL();
                    ViewBag.Nombre = villain.VillainName;
                    ViewBag.RN = villain.RealName;
                    ViewBag.Poderes = villain.Powers;
                    ViewBag.Debilidades = villain.Weaks;
                    ViewBag.Age = villain.Age;
                    ViewBag.img = villain.Img;
                    ViewBag.cantbat = servicioAPI.GetNumberofFacings();
                    return View(null);

                case "Heroes":
                    heroesR = await servicioAPI.GetTopHeroes();
                    ViewBag.ws = servicioAPI.GetNumberOfWs();
                    return View(heroesR);
                    
            }
            return View();
        }

        //[Route("Home/PanelView")]
        [HttpPost]
        public async Task<IActionResult> PanelView()
        {
            string? username = HttpContext.Request.Form["username"];
            string? password = HttpContext.Request.Form["password"];

            if (username != "" && password != "")
            {

                if (await servicioAPI.login(username, password))
                {
                    return View("Living");

                }
                else
                {

                    return View("Error");
                }
            }
            else {

                return View("Error");

            }

         
            
            
        }

        [HttpPost]
        public async Task<IActionResult> SearchHeroes()
        {
            List<Hero> heroesR = new List<Hero>();
            string? pattern = HttpContext.Request.Form["pattern"];
            string? opcion = HttpContext.Request.Form["Dropdown1"];

            if(pattern != null && opcion != null)
            switch (opcion) {

                case "nombre":
                    heroesR = await servicioAPI.GetHeroesByName(pattern);

                    // usar un viewbag mejor? 
                    return View(heroesR);

                case "habilidad":
                    heroesR = await servicioAPI.GetHeroesByabilities(pattern);
                    return View(heroesR);

                case "relacion":
                    heroesR = await servicioAPI.GetHeroesByRelationships(pattern);
                    return View(heroesR);

                case "edad":
                    heroesR = await servicioAPI.GetHeroesSortByAge();
                    return View(heroesR);

                case "todo":
                    heroesR = await servicioAPI.GetHeroes();
                    return View(heroesR);


            }
            return View(heroesR);

        }

        


    }

}
