using Reto_sophos2.Models;
using System.Threading.Tasks;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.Servicios
{
    public interface IServicio_API
    {

        public int GetNumberofFacings();
        public int[] GetNumberOfWs();

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
