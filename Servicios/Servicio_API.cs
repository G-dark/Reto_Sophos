using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using Reto_sophos2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.Servicios
{
    public class Servicio_API : IServicio_API
    {
        public string base_url = "https://localhost:32768";
        public int numberOfFacings { get; set; }
        public int[] numberOfWs = new int[3];
        public decimal totalAmount = Decimal.Zero;
        public async Task<List<Fight>?> GetFights()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Fights";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Fight>? fights = JsonConvert.DeserializeObject<List<Fight>>(responseBody);
            List<Hero>? heroes = await GetHeroes();
            List<Villain>? villains = await GetVillains();
            if (fights != null && heroes != null && villains != null)
                foreach (Fight fight in fights)
                {
                    Predicate<Hero> Is = hero => hero.HeroId == fight.HeroId;
                    Predicate<Villain> Is2 = villain => villain.VillainId == fight.VillainId;
                    fight.Hero = heroes.Find(Is);
                    fight.Villain = villains.Find(Is2);
                }

            return fights;
        }

        public async Task<List<Hero>?> GetHeroes()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Heroes";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Hero>? heros = JsonConvert.DeserializeObject<List<Hero>>(responseBody);

            return heros;
        }

        public async Task<List<Hero>> GetHeroesByabilities(string power)
        {
            List<Hero>? heros = await GetHeroes();
            List<Hero> heroin = new List<Hero>();

            if (heros != null)
                foreach (Hero hero in heros)
                {

                    if (hero.Powers != null && hero.Powers.Contains(power))
                    {
                        heroin.Add(hero);
                    }

                }

            return heroin;
        }

        public async Task<List<Hero>> GetHeroesByName(string name)
        {
            List<Hero>? heros = await GetHeroes();
            List<Hero> heroin = new List<Hero>();

            if (heros != null)
                foreach (Hero hero in heros)
                {

                    if (hero.HeroName != null && hero.HeroName.Contains(name))
                    {
                        heroin.Add(hero);
                    }

                }

            return heroin;

        }

        public async Task<List<Hero>> GetHeroesByRelationships(string relations)
        {
            List<Hero>? heros = await GetHeroes();
            List<Hero> heroin = new List<Hero>();

            if (heros != null)
                foreach (Hero hero in heros)
                {

                    if (hero.Relations != null && hero.Relations.Contains(relations))
                    {
                        heroin.Add(hero);
                    }

                }

            return heroin;
        }

        public async Task<List<Hero>> GetHeroesSortByAge()
        {

            List<Hero>? heroes = await GetHeroes();
            List<int?> edades = new List<int?>();
            List<int?> edades2 = new List<int?>();
            List<Hero> heroesR = new List<Hero>();

            if (heroes != null) {

                foreach (var hero in heroes)
                {
                    edades.Add(hero.Age);
                    edades2.Add(hero.Age);
                }

                edades.Sort();
                edades.Reverse();

                foreach (var age in edades)
                {
                    var posi = edades2.IndexOf(age);

                    var heroe = heroes.ElementAt(posi);


                    if (!heroesR.Contains(heroe))
                    {
                        heroesR.Add(heroes.ElementAt(posi));
                        edades2[posi] = 0;
                    }

                }
            }
            

            return heroesR;

        }

        public async Task<List<Sponsor>> GetHighestSponsor(string heroName)
        {
            List<Sponsor> sponsors = await GetSponsors(heroName);
            List<Sponsor> sponsorsU = sponsors.Distinct().ToList();
            List<Hero>? heroes = await GetHeroes();
            List<int?> sponsorIds = new List<int?>();
            List<Decimal?> montos = new List<Decimal?>();
            List<Decimal?> montos2 = new List<Decimal?>();
            List<Sponsor> sponsors2 = new List<Sponsor>();
            if (heroes != null && sponsors != null)
            {
                Predicate<Hero> Is = hero => hero.HeroName == heroName;
                Hero? hero = heroes.Find(Is);

                if (hero != null)
                {

                    foreach (var sp in sponsors)
                    {
                        if (sp.HeroId == hero.HeroId)
                        {
                            if (!sponsorIds.Contains(sp.SponsorId))
                            {
                                sponsorIds.Add(sp.SponsorId);
                                montos.Add(sp.Amount);
                                montos2.Add(sp.Amount);
                            }
                            else
                            {
                                var posi = sponsorIds.IndexOf(sp.SponsorId);
                                montos[posi] += (sp.Amount);
                                montos2[posi] += (sp.Amount);
                            }
                        }
                    }


                    montos.Sort();
                    montos.Reverse();

                    var posi2 = montos2.IndexOf(montos[0]);

                    var id = sponsorIds.ElementAt(posi2);
                    totalAmount = (decimal)montos[0];

                    Sponsor? sponsor = sponsors.Find(sponsor => sponsor.SponsorId == id);

                    if (sponsor != null) sponsors2.Add(sponsor);
                }

            }

            return sponsors2;


        }

        public Decimal GetTotalAmount()
        {
            return totalAmount;
        }

        public async Task<Villain?> GetMostFightedVillain(string heroName)
        {
            List<Fight>? fights = await GetFights();
            List<Hero>? heroes = await GetHeroes();
            List<Villain>? villains = await GetVillains();
            int foundIDV = 0;
            List<int?> fightsBH = new List<int?>();

            Predicate<Hero> Is = hero => hero.HeroName == heroName;
           
            Villain? villain=null;
            List<Fight> tfights = new List<Fight>();

            if (heroes != null && fights != null && villains != null)
            {

                Hero? heroS = heroes.Find(Is);


                if (heroS != null)
                {
                    Predicate<Fight> Iss = fight => fight.HeroId == heroS.HeroId;

                    var fightsin = fights.FindAll(Iss);

                    if (fightsin != null && fightsin.Count > 0) {

                        villain = new Villain();

                        foreach (Fight fight in fightsin)
                        {
                            if (fight.HeroId.Equals(heroS.HeroId))
                            {
                                fightsBH.Add(fight.VillainId);
                                tfights.Add(fight);
                            }
                        }

                        var NFights = fightsBH.Distinct();
                        List<int> counts = new List<int>();

                        foreach (var item in NFights)
                        {
                            List<Fight> fs = new List<Fight>();
                            foreach (var item2 in tfights)
                            {
                                if (item == item2.VillainId)
                                {

                                    fs.Add(item2);
                                }
                            }

                            counts.Add(fs.Count());
                        }

                        var max = counts.Max();
                        int posi = counts.IndexOf(max);

                        foundIDV = (int)NFights.ElementAt(posi);

                        Predicate<Villain> Is2 = villain => villain.VillainId == foundIDV;
                        numberOfFacings = max;
                        villain = villains.Find(Is2);


                    }


                }
            }




            return villain;


        }

        public async Task<List<Hero>> GetTopHeroes()
        {
            List<Fight>? fights = await GetFights();
            List<Hero>? heroes = await GetHeroes();
            List<int?> heroesID = new List<int?>();
            List<Hero> heroesin = new List<Hero>();
            List<Hero> result = new List<Hero>();

            if (heroes != null && fights != null)
            {
                foreach (Fight fight in fights)
                    heroesID.Add(fight.HeroId);

                var HeroesU = heroesID.Distinct();

                int tmp;

                var count = new List<int>();
                var count2 = new List<int>();

                if (heroes != null)
                    foreach (var v in HeroesU)
                    {
                        tmp = 0;
                        foreach (var v2 in fights)
                        {

                            Predicate<Hero> Is = hero => hero.HeroId == v2.HeroId;
                            Hero? hero = heroes.Find(Is);

                            if (v2.HeroId == v && v2.Result == 0 && hero != null) { tmp++; if (!heroesin.Contains(hero)) heroesin.Add(hero); }
                        }

                        count.Add(tmp);
                        count2.Add(tmp);
                    }
                count2.RemoveAll(h => h == 0);
                count.RemoveAll(h => h == 0);



                count.Sort();

                count.Reverse();

                Hero[] htop3 = new Hero[3];

                for (int i = 0; i < 3; ++i)
                {
                    var element = count.ElementAt(i);
                    numberOfWs[i] = element;
                    var posi = count2.IndexOf(element);

                    var heroe = heroesin.ElementAt(posi);


                    if (!htop3.Contains(heroe))
                    {
                        htop3[i] = (heroesin.ElementAt(posi));
                        count2[posi] = 0;
                    }

                }


               

                for (int i = 0; i < 3; ++i) result.Add(htop3[i]);

            }
          


            return result;


        }
        public int GetNumberofFacings()
        {
            return numberOfFacings;
        }

        public int[] GetNumberOfWs()
        {
            return numberOfWs;
        }
        
        public async Task<List<Sponsor>> GetSponsors(string heroName)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Sponsors";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Sponsor>? sponsors = JsonConvert.DeserializeObject<List<Sponsor>>(responseBody);
            var heroes = await GetHeroes();
            Predicate<Hero> isIn = hero => hero.HeroName == heroName;
            List<Sponsor> sponsorin = new List<Sponsor>();
            if (heroes != null)
            {
                Hero? heroe = heroes.Find(isIn);

                if (sponsors != null)
                    foreach (Sponsor sponsor in sponsors)
                    {

                        if (heroe != null && sponsor.HeroId.Equals(heroe.HeroId))
                        {
                            sponsorin.Add(sponsor);
                        }

                    }
            }


            return sponsorin;
        }

        public async Task<List<Task>> GetTasks(string heroName)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Tasks";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Task>? tasks = JsonConvert.DeserializeObject<List<Task>>(responseBody);
            var heroes = await GetHeroes();
            Predicate<Hero> isIn = hero => hero.HeroName == heroName;
            List<Task> taskin = new List<Task>();
            if (heroes != null)
            {
                Hero? heroe = heroes.Find(isIn);


                if (tasks != null)
                    foreach (Task task in tasks)
                    {

                        if (heroe != null && task.HeroId.Equals(heroe.HeroId))
                        {
                            taskin.Add(task);
                        }

                    }
            }

            return taskin;
        }

        public async Task<Villain> GetVillainL()
        {
            List<Villain>? villains = await GetVillains();
            List<Fight>? fights = await GetFights();
            List<int?> villainsID = new List<int?>();
            Villain? villain = null;
            List<Hero>? heroes = await GetHeroes();

            if (villains != null && heroes != null && fights != null) {
                foreach (Fight fight in fights)
                    villainsID.Add(fight.VillainId);

                var villainsU = villainsID.Distinct();

                int tmp;

                var count = new List<int>();

                foreach (var v in villainsU)
                {
                    tmp = 0;
                    foreach (var v2 in fights)
                    {

                        Predicate<Hero> Is = hero => hero.HeroId == v2.HeroId && hero.Age < 18;
                        Hero? hero = heroes.Find(Is);
                        if (v2.VillainId == v && v2.Result == 0 && hero != null) tmp++;
                    }

                    count.Add(tmp);
                }

                var alt = count.IndexOf(count.Max());
                var altV = villainsU.ElementAt(alt);

                numberOfFacings = count.Max();

                

                Predicate<Villain> Is2 = villain => villain.VillainId == altV;
                villain = villains.Find(Is2);
            }

           

            return villain;

        }

        public async Task<List<Villain>?> GetVillains()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Villains";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Villain>? villains = JsonConvert.DeserializeObject<List<Villain>>(responseBody);


            return villains;
        }

        public async Task<List<Villain>> GetVillainsByName(string name)
        {
            List<Villain>? villains = await GetVillains();
            List<Villain> villainin = new List<Villain>();

            if (villains != null)
                foreach (Villain villain in villains)
                {

                    if (villain.VillainName != null && villain.VillainName.Contains(name))
                    {
                        villainin.Add(villain);
                    }

                }

            return villainin;
        }

        public async Task<List<Villain>> GetVillainssByOrigin(string origin)
        {
            
            List<Villain>? villains = await GetVillains();
            List<Villain> villainin = new List<Villain>();

            if (villains != null)
                foreach (Villain villain in villains)
                {

                    if (villain.Origin != null && villain.Origin.Contains(origin))
                    {
                        villainin.Add(villain);
                    }

                }

            return villainin;
        }

        public async Task<List<Villain>> GetVillainssByWeak(string weak)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Villains";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Villain>? villains = JsonConvert.DeserializeObject<List<Villain>>(responseBody);
            List<Villain> villainin = new List<Villain>();

            if (villains != null)
                foreach (Villain villain in villains)
                {

                    if (villain.Weaks != null && villain.Weaks.Contains(weak))
                    {
                        villainin.Add(villain);
                    }

                }

            return villainin;
        }

        public async Task<bool> login(string username, string password)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            //var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "aplication/json");
            var strContent = "api/Users/" + username;
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            User? user = JsonConvert.DeserializeObject<User>(responseBody);
            
            var resp = response.IsSuccessStatusCode && user != null && user.Pw == password;
          
            return resp;
        }

        public async Task<bool> CreateFight(int result, string heroName, string villainName, string comments)
        {
            List<Hero>? heroes = await GetHeroes();
            List<Villain>? villains = await GetVillains();
            Predicate<Hero> Is = hero => hero.HeroName == heroName;
            Predicate<Villain> Is2 = villain => villain.VillainName == villainName;

            if (heroes != null && villains != null)
            {
                Hero? hero = heroes.Find(Is);
                Villain? villain = villains.Find(Is2);


                if (villain != null && hero != null)
                {

                    var client = new HttpClient();
                    client.BaseAddress = new Uri(base_url);
                    var strContent = "api/Fights";


                    var response2 = new Fight()
                    {
                        Result = result,
                        HeroId = hero.HeroId,
                        VillainId = villain.VillainId,
                        Comments = comments
                    };
                    HttpResponseMessage response = await client.PostAsJsonAsync(strContent, response2);

                    return response.IsSuccessStatusCode;

                }
                else
                {
                    return false;
                }

            }
            else {

                return false;
            
            }
           



        }
        public async Task<bool> CreateHeroe(IFormFile img, string heroName, string realName, string powers,
            string weaks, string relations, string origin, int age, string cell)
        {
            var heroes = await GetHeroes();
            Predicate<Hero> Is = h => h.HeroName == heroName;
            Hero? hero = new Hero();

            if (heroes != null) { hero = heroes.Find(Is); }
                

            if (img != null && img.Length > 0 && hero == null)
            {

                byte[] contenidoArchivo;

                using (MemoryStream ms = new MemoryStream())
                {
                    await img.CopyToAsync(ms);
                    contenidoArchivo = ms.ToArray();
                }


                var client = new HttpClient();
                client.BaseAddress = new Uri(base_url);
                var strContent = "api/Heroes";

                var response2 = new Hero()
                {
                    Img = contenidoArchivo,
                    HeroName = heroName,
                    RealName = realName,
                    Powers = powers,
                    Weaks = weaks,
                    Relations = relations,
                    Age = age,
                    Cellphone = cell,
                    Origin = origin
                };
                HttpResponseMessage response = await client.PostAsJsonAsync(strContent, response2);
                return response.IsSuccessStatusCode;
            }

            return false;


        }
        public async Task<bool> CreateVillain(IFormFile img, string villainName, string realName, string powers,
           string weaks, string relations, string origin, int age, string cell)
        {
            var villains = await GetVillains();
            Predicate<Villain> Is = v => v.VillainName == villainName;
            Villain? villain = new Villain();
            if (villains != null) { villain = villains.Find(Is); }


            if (img != null && img.Length > 0 && villain == null)
            {

                byte[] contenidoArchivo;

                using (MemoryStream ms = new MemoryStream())
                {
                    await img.CopyToAsync(ms);
                    contenidoArchivo = ms.ToArray();
                }


                var client = new HttpClient();
                client.BaseAddress = new Uri(base_url);
                var strContent = "api/Villains";

                var response2 = new Villain()
                {
                    Img = contenidoArchivo,
                    VillainName = villainName,
                    RealName = realName,
                    Powers = powers,
                    Weaks = weaks,
                    Relations = relations,
                    Age = age,
                    Cellphone = cell,
                    Origin = origin
                };
                HttpResponseMessage response = await client.PostAsJsonAsync(strContent, response2);
                return response.IsSuccessStatusCode;
            }

            return false;


        }
        public async Task<bool> CreateSponsorT(string heroName, int sponsor, Decimal amount, string source)
        {
            List<Hero>? heroes = await GetHeroes();
           
            Predicate<Hero> Is = hero => hero.HeroName == heroName;

            Hero? hero = null;
            if (heroes != null) { hero = heroes.Find(Is); } 
          


            if (hero != null)
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(base_url);
                var strContent = "api/Sponsors";


                var response2 = new Sponsor()
                {
                    SponsorId = sponsor,
                    HeroId = hero.HeroId,
                    Amount = amount,
                    SourceOm = source
                };

                HttpResponseMessage response = await client.PostAsJsonAsync(strContent, response2);

                return response.IsSuccessStatusCode;

            }
            else
            {
                return false;
            }



        }

        public async Task<bool> CreateTask(string heroName, string tname, DateTime fechasSdate, DateTime fechasFdate) {
            
            var heroes = await GetHeroes();

            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Tasks";
            HttpResponseMessage responseT = await client.GetAsync(strContent);
            string responseBody = await responseT.Content.ReadAsStringAsync();
            List<Task>? tasks = JsonConvert.DeserializeObject<List<Task>>(responseBody);

            Predicate<Hero> Is = h => h.HeroName == heroName;
            Hero? hero = new Hero();

            if (heroes != null) { hero = heroes.Find(Is); }

            if (hero != null && tasks != null)
            {

                Task task = tasks.Last();

                var response2 = new Task()
                {
                    TaskId = task.TaskId + 1,
                    TaskName = tname,
                    HeroId = hero.HeroId,
                    Sdate = fechasSdate,
                    Fdate = fechasFdate,
                    TaskStatus = 1,
                    EditedBy = null,
                    EditedAt = DateTime.Now,
                    Hero = null
                };
                    
                    HttpResponseMessage response = await client.PostAsJsonAsync(strContent, response2);
                    return response.IsSuccessStatusCode;

            }
            else {
                return false; 
            }
            
            
        }

        public async Task<bool> EditHeroe(IFormFile? img, string heroName, string realName, string powers, string weaks, string relations, string origin, int age, string cell)
        {
            var heroes = await GetHeroes();
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Heroes";
            Predicate<Hero> Is2 = h => h.HeroName == heroName;

            if (heroes != null)
            {
                Hero? hero = heroes.Find(Is2);
              
                if (hero != null)
                {
                    byte[] contenidoArchivo;

                    if (img != null && img.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await img.CopyToAsync(ms);
                            contenidoArchivo = ms.ToArray();
                        }

                    }
                    else {

                        contenidoArchivo = new byte[0];
                    }


                    var jsonvar = new Hero()
                    {
                        HeroName = hero.HeroName,
                        HeroId =hero.HeroId,
                        RealName = realName == "" ? hero.RealName : realName,
                        Img = img == null ? hero.Img : contenidoArchivo,
                        Powers = powers == "" ? hero.Powers : powers,
                        Weaks = weaks == null ? hero.Weaks : weaks,
                        Relations = relations == "" ? hero.Relations : relations,
                        Origin = origin == "" ? hero.Origin : origin,
                        Cellphone = cell == "" ? hero.Cellphone:cell,
                        Age = age == -1 ? hero.Age: age 
                    };
                    strContent += "/" + hero.HeroId;
                    HttpResponseMessage response = await client.PutAsJsonAsync(strContent, jsonvar);
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EditVillain(IFormFile? img, string villainName, string realName, string powers, string weaks, string relations, string origin, int age, string cell)
        {
            var villains = await GetVillains();
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Villains";
            Predicate<Villain> Is2 = v => v.VillainName == villainName;

            if (villains != null)
            {
               Villain? villain = villains.Find(Is2);

                if (villain != null)
                {
                    byte[] contenidoArchivo;

                    if (img != null && img.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await img.CopyToAsync(ms);
                            contenidoArchivo = ms.ToArray();
                        }

                    }
                    else
                    {

                        contenidoArchivo = new byte[0];
                    }


                    var jsonvar = new Villain()
                    {
                        VillainName = villain.VillainName,
                        VillainId = villain.VillainId,
                        RealName = realName == "" ? villain.RealName : realName,
                        Img = img == null ? villain.Img : contenidoArchivo,
                        Powers = powers == "" ? villain.Powers : powers,
                        Weaks = weaks == null ? villain.Weaks : weaks,
                        Relations = relations == "" ? villain.Relations : relations,
                        Origin = origin == "" ? villain.Origin : origin,
                        Cellphone = cell == "" ? villain.Cellphone : cell,
                        Age = age == -1 ? villain.Age : age
                    };
                    strContent += "/" + villain.VillainId;
                    HttpResponseMessage response = await client.PutAsJsonAsync(strContent, jsonvar);
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        
        public async Task<bool> EditTask(string heroName, string tname, DateTime fechasSdate, DateTime fechasFdate, int taskID, int? status, string usernameOn)
        {
            var heroes = await GetHeroes();
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Tasks";
            HttpResponseMessage responseT = await client.GetAsync(strContent);
            string responseBody = await responseT.Content.ReadAsStringAsync();
            List<Task>? tasks = JsonConvert.DeserializeObject<List<Task>>(responseBody);
            Predicate<Task> Is = t => t.TaskId == taskID;
            Predicate<Hero> Is2 = h => h.HeroName == heroName;
            if (heroes != null && tasks != null)
            {
                Task? task = tasks.Find(Is);
                Hero? hero = heroes.Find(Is2);

                if (task != null && hero != null)
                {

                    var jsonvar = new Task()
                    {
                        TaskId = taskID,
                        TaskName = tname == "" ? task.TaskName : tname,
                        HeroId = hero.HeroId,
                        Sdate = fechasSdate == DateTime.MinValue ? task.Sdate : fechasSdate,
                        Fdate = fechasFdate == DateTime.MinValue ? task.Fdate : fechasFdate,
                        TaskStatus = status == null ? task.TaskStatus : status,
                        EditedBy = usernameOn,
                        EditedAt = DateTime.Now,
                        Hero = null
                    };
                    strContent += "/" + taskID;
                    HttpResponseMessage response = await client.PutAsJsonAsync(strContent, jsonvar);
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }

            }
            else {
                return false;
            }
           

            
        }
    }
}