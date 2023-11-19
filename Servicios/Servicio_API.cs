using Azure;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using Reto_sophos2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.Servicios
{
    public class Servicio_API : IServicio_API
    {
        public string base_url = "https://localhost:7232";
        public int numberOfFacings { get; set; }
        public int[] numberOfWs = new int[3];
        public decimal totalAmount = Decimal.Zero;
        public async Task<List<Fight>> GetFights()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Fights";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Fight>? fights = JsonConvert.DeserializeObject<List<Fight>>(responseBody);
            List<Hero> heroes = await GetHeroes();
			List<Villain> villains = await GetVillains();
            if(fights != null && heroes != null)
			    foreach (Fight fight in fights) {
				    Predicate<Hero> Is = hero => hero.HeroId == fight.HeroId;
				    Predicate<Villain> Is2 = villain => villain.VillainId == fight.VillainId;
				    fight.Hero = heroes.Find(Is);
				    fight.Villain = villains.Find(Is2);
			    }
 
            return fights;
        }

        public async Task<List<Hero>> GetHeroes()
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
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Heroes";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Hero>? heros = JsonConvert.DeserializeObject<List<Hero>>(responseBody);
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
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Heroes";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Hero>? heros = JsonConvert.DeserializeObject<List<Hero>>(responseBody);
            List<Hero> heroin = new List<Hero>();

            if (heros != null)
            foreach (Hero hero in heros) {

                    if (hero.HeroName != null && hero.HeroName.Contains(name) ) { 
                        heroin.Add(hero);
                    }
            
            }

            return heroin;

        }

        public async Task<List<Hero>> GetHeroesByRelationships(string relations)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(base_url);
            var strContent = "api/Heroes";
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Hero>? heros = JsonConvert.DeserializeObject<List<Hero>>(responseBody);
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

            List<Hero> heroes = await GetHeroes();
            List<int?> edades = new List<int?>();
            List<int?> edades2 = new List<int?>();
            List<Hero> heroesR = new List<Hero>();
            foreach (var hero in heroes)
            {
                edades.Add(hero.Age);
                edades2.Add(hero.Age);
            }

            edades.Sort();
            edades.Reverse();

            foreach(var age in edades)
            {
                var posi = edades2.IndexOf(age);

                var heroe = heroes.ElementAt(posi);
               

                if (!heroesR.Contains(heroe))
                {
                    heroesR.Add(heroes.ElementAt(posi));
                    edades2[posi] = 0;
                }
                
            }

            return heroesR;

        }

        public async Task<List<Sponsor>> GetHighestSponsor(string heroName)
        {
            List<Sponsor> sponsors = await GetSponsors(heroName);
            List<Sponsor> sponsorsU = sponsors.Distinct().ToList();
            List<Hero> heroes = await GetHeroes();
            List<int?> sponsorIds = new List<int?>();
            List<Decimal?> montos = new List<Decimal?>();
            List<Decimal?> montos2 = new List<Decimal?>();
            List<Sponsor> sponsors2 = new List<Sponsor>();
            if (heroes != null) {
                Predicate<Hero> Is = hero => hero.HeroName == heroName;
                Hero? hero = heroes.Find(Is);
               
                if (hero != null) {

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
                            else {
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

                    sponsors2.Add(sponsor);
                }
               
            }

            return sponsors2;
            

        }

        public Decimal GetTotalAmount() { 
        return totalAmount; 
        }

        public async Task<Villain> GetMostFightedVillain(string heroName)
        {
            List<Fight> fights = await GetFights();
            List<Hero> heroes = await GetHeroes();
            List<Villain> villains = await GetVillains();
            int foundIDV = 0;
            List<int?> fightsBH = new List<int?>();
          
            Predicate<Hero> Is = hero => hero.HeroName == heroName;
            Villain villain = new Villain();
            List<Fight> tfights = new List<Fight>();

            if(heroes != null){

                Hero? heroS = heroes.Find(Is);

                if (heroS != null)
                {

                    foreach (Fight fight in fights)
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

                    if (NFights.ElementAt(posi) != null) {
                        foundIDV = (int) NFights.ElementAt(posi);

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
			List<Fight> fights = await GetFights();
			List<Hero> heroes = await GetHeroes();
            List<int?> heroesID = new List<int?>();
            List<Hero> heroesin = new List<Hero>();

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
                

                if(!htop3.Contains(heroe)) {
                    htop3[i] = (heroesin.ElementAt(posi));
                    count2[posi] = 0;
                } 
                
            }


            List<Hero> result = new List<Hero>();

            for (int i = 0; i < 3; ++i) result.Add(htop3[i]);

            
            return result;


		}
		public int GetNumberofFacings() {
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
            if (heroes != null) {
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
            Predicate<Hero> isIn = hero =>  hero.HeroName == heroName;
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
            List<Villain> villains = await GetVillains();
            List<Fight> fights = await GetFights();
            List<int?> villainsID = new List<int?>();

            List<Hero> heroes = await GetHeroes();

            foreach (Fight fight in fights)
                villainsID.Add(fight.VillainId);

            var villainsU = villainsID.Distinct();

            int  tmp;

            var count = new List<int>();

            foreach (var v in villainsU)
            {
                tmp = 0;
                foreach (var v2 in fights) { 

                    Predicate<Hero> Is = hero => hero.HeroId == v2.HeroId && hero.Age < 18;
                    Hero? hero = heroes.Find(Is);
                    if (v2.VillainId == v && v2.Result == 0 && hero != null) tmp++; 
                }

                count.Add(tmp);
            }

            var alt = count.IndexOf(count.Max());
            var altV = villainsU.ElementAt(alt);

            numberOfFacings = count.Max();

            Villain villain = new Villain();

            Predicate<Villain> Is2 = villain => villain.VillainId == altV;
            villain = villains.Find(Is2);

            return villain;
          
        }


        public async Task<List<Villain>> GetVillains()
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

                    if (villain.VillainName != null && villain.VillainName.Contains(name))
                    {
                        villainin.Add(villain);
                    }

                }

            return villainin;
        }

        public async Task<List<Villain>> GetVillainssByOrigin(string origin)
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
            var strContent = "api/Users/"+ username;
            HttpResponseMessage response = await client.GetAsync(strContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            User? user = JsonConvert.DeserializeObject<User>(responseBody);

            return (response.IsSuccessStatusCode && user != null && user.Pw == password);
        }

        
    }
}
