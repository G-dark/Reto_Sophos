using Microsoft.AspNetCore.Mvc;
using Reto_sophos2.Models;
using Reto_sophos2.Servicios;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Task = Reto_sophos2.Models.Task;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Configuration;

namespace Reto_sophos2.Controllers
{

    public class HomeController : Controller
    {
        private readonly IServicio_API servicioAPI;

        private string usernameOn="admin";
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
            ViewBag.respuesta = null;
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
            ViewBag.respuesta = null;

            if (pattern != null && opcion != null)
            switch (opcion) {

                case "nombre":
                    heroesR = await servicioAPI.GetHeroesByName(pattern);
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

        public IActionResult Crear() {

            return View("Crear");
        }

        [HttpPost]
        public IActionResult CrearHSP()
        {
            
            string? opcion = HttpContext.Request.Form["Dropdown1"];

            if (opcion != null)
            {

                switch (opcion)
                {

                    case "heroe":
                        
                        return View("CrearHeroe");

                    case "sponsorT":

                        return View("CrearSponsorT");

                    case "pelea":

                        return View("CrearPelea");


                }
            }
            else {

                return View("Crear");
            }

            return View("Crear");

        }

        [HttpPost]
        public async Task<IActionResult> CrearP()
        {
            string? result = HttpContext.Request.Form["result"];
            string? heroName = HttpContext.Request.Form["heroname"];
            string? villainName = HttpContext.Request.Form["villainname"];
            string? comments = HttpContext.Request.Form["comments"];
            int resultint;

            var resultado = false;
            if (result != null && heroName != null && villainName != null && comments != null)
            {
               resultint = int.Parse(result);

               resultado = await servicioAPI.CreateFight(resultint, heroName, villainName, comments);

            }
            ViewBag.respuesta = resultado;
            return View();


        }

        [HttpPost("/heroec/")]
        
        public async Task<IActionResult> CrearH(IFormFile imagen) {

            var response = false;

            string? heroName = HttpContext.Request.Form["heroname"];
            string? realName = HttpContext.Request.Form["realname"];
            string? powers = HttpContext.Request.Form["powers"];
            string? weaks = HttpContext.Request.Form["weaks"];
            string? relations = HttpContext.Request.Form["relations"];
            string? age  = HttpContext.Request.Form["age"];
            string? cell = HttpContext.Request.Form["phone"];
            string? origin = HttpContext.Request.Form["origin"];

            if (imagen != null && heroName != null && realName != null && 
                powers != null && weaks != null && relations != null
                && age != null && cell != null && origin != null)
            {
                response = await servicioAPI.CreateHeroe(imagen, heroName, realName, powers,
                weaks, relations, origin, int.Parse(age), cell);
                ViewBag.respuesta = response;
                return View("CrearP");
            }
            

            return View("CrearP");
        
        }

        [HttpPost]
        public async Task<IActionResult> CrearS()
        {

            var response = false;

            string? heroName = HttpContext.Request.Form["heroname"];
            string? sponsorID = HttpContext.Request.Form["sponsor"];
            string? amount = HttpContext.Request.Form["amount"];
            string? source = HttpContext.Request.Form["source"];

            

            if ( heroName != null && sponsorID != null && amount != null && source != null)
            {
                var sponsorint = int.Parse(sponsorID);
                var decamount = Decimal.Parse(amount);
                try
                {
                    response = await servicioAPI.CreateSponsorT(heroName, sponsorint, decamount, source);
                    ViewBag.respuesta = response;
                    return View("CrearP");

                } catch (Exception ex)
                {
                    response = false;
                }
               
            }


            return View("CrearP");

        }

        public IActionResult VillainsCrear()
        {
            ViewBag.respuesta = null;
            return View();
        }

        [HttpPost("/villainc/")]
        public async Task<IActionResult> CrearV(IFormFile imagen) {

            var response = false;

            string? villainName = HttpContext.Request.Form["villainname"];
            string? realName = HttpContext.Request.Form["realname"];
            string? powers = HttpContext.Request.Form["powers"];
            string? weaks = HttpContext.Request.Form["weaks"];
            string? relations = HttpContext.Request.Form["relations"];
            string? age = HttpContext.Request.Form["age"];
            string? cell = HttpContext.Request.Form["phone"];
            string? origin = HttpContext.Request.Form["origin"];

            if (imagen != null && villainName != null && realName != null &&
                powers != null && weaks != null && relations != null
                && age != null && cell != null && origin != null)
            {
                response = await servicioAPI.CreateVillain(imagen, villainName, realName, powers,
                weaks, relations, origin, int.Parse(age), cell);
                ViewBag.respuesta = response;
                return View("VillainsCrear");
            }


            return View("VillainsCrear");

        }

        [HttpPost]
        public async Task<IActionResult> CrearT() {

            var response = false;

            string? heroName = HttpContext.Request.Form["heroname"];
            string? fechaS = HttpContext.Request.Form["fechas"];
            string? fechaF = HttpContext.Request.Form["fechaf"];
            string? tname = HttpContext.Request.Form["taskname"];


            if (heroName != null && fechaS != null && fechaF != null && tname != null)
            {
                var fechasSdate = DateTime.Parse(fechaS);
                var fechasFdate = DateTime.Parse(fechaF);
                
                    response = await servicioAPI.CreateTask(heroName, tname, fechasSdate, fechasFdate);
                    ViewBag.respuesta = response;
                    return View("AgendaWeek");
            }


            return View("AgendaWeek");

        }

        public async Task<IActionResult> EditT() {
            var response = false;

            string? heroName = HttpContext.Request.Form["heroname2"];
            string? fechaS = HttpContext.Request.Form["fechas2"];
            string? fechaF = HttpContext.Request.Form["fechaf2"];
            string? tname = HttpContext.Request.Form["taskname2"];
            string? status = HttpContext.Request.Form["status"];
            string? taskid_ = HttpContext.Request.Form["taskid"];

            int taskid = int.Parse(taskid_);
            int? intstatus = status.Equals("") ? null : int.Parse(status);

            if (heroName != null && fechaS != null && fechaF != null && tname != null )
            {
                var fechasSdate = fechaS.Equals("") ? DateTime.MinValue: DateTime.Parse(fechaS);
                var fechasFdate = fechaF.Equals("") ? DateTime.MinValue: DateTime.Parse(fechaF);
                
                response = await servicioAPI.EditTask(heroName, tname, fechasSdate, fechasFdate, taskid, intstatus, usernameOn);
                ViewBag.respuesta = response;
                return View("AgendaWeek");



            }


            return View("AgendaWeek");
            
        }

        [HttpPost("/heroee/")]
        public async Task<IActionResult> EditH(IFormFile imagen) {

            var response = false;

            string? heroName = HttpContext.Request.Form["heroname"];
            string? realName = HttpContext.Request.Form["realname"];
            string? powers = HttpContext.Request.Form["powers"];
            string? weaks = HttpContext.Request.Form["weaks"];
            string? relations = HttpContext.Request.Form["relations"];
            string? age = HttpContext.Request.Form["age"];
            string? cell = HttpContext.Request.Form["phone"];
            string? origin = HttpContext.Request.Form["origin"];

            int ageint = age.Equals("") ? -1 : int.Parse(age);

            if ( heroName != null )
            {
                response = await servicioAPI.EditHeroe(imagen, heroName, realName, powers,
                weaks, relations, origin, ageint, cell);
                ViewBag.respuesta = response;
                return View("SearchHeroes");
            }

            return View("SearchHeroes");
        }

        [HttpPost]
        public async Task<IActionResult> EditV(IFormFile imagen)
        {

            var response = false;

            string? heroName = HttpContext.Request.Form["villainname"];
            string? realName = HttpContext.Request.Form["realname"];
            string? powers = HttpContext.Request.Form["powers"];
            string? weaks = HttpContext.Request.Form["weaks"];
            string? relations = HttpContext.Request.Form["relations"];
            string? age = HttpContext.Request.Form["age"];
            string? cell = HttpContext.Request.Form["phone"];
            string? origin = HttpContext.Request.Form["origin"];

            int ageint = age.Equals("") ? -1 : int.Parse(age);

            if (heroName != null)
            {
                response = await servicioAPI.EditVillain(imagen, heroName, realName, powers,
                weaks, relations, origin, ageint, cell);
                ViewBag.respuesta = response;
                return View("SearchVillains");
            }

            return View("SearchVillains");
        }

    }

}
