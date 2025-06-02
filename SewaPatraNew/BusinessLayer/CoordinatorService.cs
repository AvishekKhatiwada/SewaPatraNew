using SewaPatra.DataAccess;
using SewaPatra.Models;

namespace SewaPatra.BusinessLayer
{
    public class CoordinatorService
    {
        private readonly CoordinatorRepository _coordinatorRepository;
        private readonly UserRepository _userRepository;
        public CoordinatorService(CoordinatorRepository repository, UserRepository userRepository)
        {
            _coordinatorRepository = repository;
            _userRepository = userRepository;
        }
        public bool InsertCoordinator(Coordinator coordinator)
        {
            if (coordinator.Password != null)
            {
                coordinator.Password = UserRegister.HashPassword(coordinator.Password);                
            }
            return _coordinatorRepository.InsertCoordinator(coordinator);
        }
        public List<Coordinator> GetAllCoordinator()
        {
            return _coordinatorRepository.GetAllCoordinator();
        }
        public Coordinator GetCoordinatorById(int id)
        {
            return _coordinatorRepository.GetCoordinatorById(id);
        }
        public bool UpdateCoordintor(Coordinator coordinator)
        {
            return _coordinatorRepository.UpdateArea(coordinator);
        }
        public bool DeleteCoordinator(int id)
        {
            return _coordinatorRepository.DeleteCoordinator(id);
        }
    }    
}
