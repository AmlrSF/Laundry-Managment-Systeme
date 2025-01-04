// Service
using Laverie.API.Infrastructure.repositories;
using Laverie.Domain.Entities;

namespace Laverie.API.Services
{
    public class ProprietaireService
    {
        private readonly ProprietaireRepo _repo;

        public ProprietaireService(ProprietaireRepo repo)
        {
            _repo = repo;
        }

        public List<User> GetAll() => _repo.GetAll();

        public User GetById(int id) => _repo.GetById(id);

        public void Create(User proprietaire) => _repo.Create(proprietaire);

        public void Update(User proprietaire) => _repo.Update(proprietaire);

        public void Delete(int id) => _repo.Delete(id);

        public User Login(string email, string password) => _repo.Login(email, password);
    }
}
