using Reto_sophos2.Models;
using System.Threading.Tasks;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.Servicios
{
    public interface IServicio_API
    {

        public int GetNumberofFacings();
        public int[] GetNumberOfWs();
        public Task<bool> CreateFight(int result, 
            string heroName, string villainName, string comments);

        public Task<bool> CreateSponsorT(string heroName, int sponsor, Decimal amount, string source);
        public Task<bool> CreateHeroe(IFormFile img, string heroName, string realName, string powers,
            string weaks, string relations, string origin, int age, string cell);
        public Task<bool> EditHeroe(IFormFile img, string heroName, string realName, string powers,
            string weaks, string relations, string origin, int age, string cell);
        public Task<bool> CreateVillain(IFormFile img, string villainName, string realName, string powers,
          string weaks, string relations, string origin, int age, string cell);
        public Task<bool> EditVillain(IFormFile img, string villainName, string realName, string powers,
          string weaks, string relations, string origin, int age, string cell);
        public Task<bool> CreateTask(string heroName, string tname, DateTime fechasSdate, DateTime fechasFdate);
        public Task<bool> EditTask(string heroName, string tname, DateTime fechasSdate, DateTime fechasFdate, int taskID, int? status, string usernameOn);
        public Decimal GetTotalAmount();
        Task<List<Hero>> GetTopHeroes();
		Task<List<Hero>> GetHeroesByabilities(string power);
        Task<List<Hero>> GetHeroesByRelationships(string relations);

        Task<List<Hero>> GetHeroesByName(string name);

        Task<List<Hero>> GetHeroes();

        Task<List<Hero>> GetHeroesSortByAge();
        Task<List<Villain>> GetVillains();

        Task<List<Villain>> GetVillainsByName(string name);
        Task<List<Villain>> GetVillainssByOrigin(string origin);

        Task<List<Villain>> GetVillainssByWeak(string weak);

        Task<List<Sponsor>> GetSponsors(string heroName);

        Task<List<Fight>> GetFights();

        Task<Villain> GetMostFightedVillain(string heroName);
        Task<List<Sponsor>> GetHighestSponsor(string heroName);

        Task<List<Task>> GetTasks(string heroName);

        Task<Villain> GetVillainL(); // villano que más ha perdido con un adolescente 

        Task<Boolean> login(string username, string password);
        
    }
}
