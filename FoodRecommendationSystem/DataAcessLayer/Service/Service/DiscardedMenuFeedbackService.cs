using Serilog;

namespace DataAcessLayer.Service.Service
{
    public class DiscardedMenuFeedbackService : IDiscardedMenuFeedbackService
    {
        private readonly IRepository<DiscardedMenuFeedback> _discardedMenuFeedbackRepository;

        public DiscardedMenuFeedbackService(IRepository<DiscardedMenuFeedback> discardedMenuFeedbackRepository)
        {
            _discardedMenuFeedbackRepository = discardedMenuFeedbackRepository;
        }

        public void AddDiscardedMenuFeedback(DiscardedMenuFeedbackDTO discardedMenuDTO)
        {
            try
            {
                DiscardedMenuFeedback discardedMenuFeedback = (DiscardedMenuFeedback)discardedMenuDTO;
                _discardedMenuFeedbackRepository.Insert(discardedMenuFeedback);
                _discardedMenuFeedbackRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding discarded menu feedback: {ex.Message}");
                throw new Exception("Error adding discarded menu feedback", ex);
            }
        }

        public List<DiscardedMenuFeedbackDTO> GetDiscardedMenuFeedbackList()
        {
            try
            {
                var discardedMenus = _discardedMenuFeedbackRepository.GetAll();
                return discardedMenus.Select(dm => (DiscardedMenuFeedbackDTO)dm).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting discarded menu feedback list: {ex.Message}");
                throw new Exception("Error getting discarded menu feedback list", ex);
            }
        }

        public DiscardedMenuFeedbackDTO GetDiscardedMenuFeedback(int id)
        {
            try
            {
                var discardedMenu = _discardedMenuFeedbackRepository.GetById(id);
                return (DiscardedMenuFeedbackDTO)discardedMenu;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting discarded menu feedback with id {id}: {ex.Message}");
                throw new Exception($"Error getting discarded menu feedback with id {id}", ex);
            }
        }

        public void UpdateDiscardedMenuFeedback(DiscardedMenuFeedbackDTO discardedMenuDTO)
        {
            try
            {
                var discardedMenu = (DiscardedMenuFeedback)discardedMenuDTO;
                _discardedMenuFeedbackRepository.Update(discardedMenu);
                _discardedMenuFeedbackRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating discarded menu feedback: {ex.Message}");
                throw new Exception("Error updating discarded menu feedback", ex);
            }
        }

        public void DeleteDiscardedMenuFeedback(int id)
        {
            try
            {
                _discardedMenuFeedbackRepository.Delete(id);
                _discardedMenuFeedbackRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting discarded menu feedback with id {id}: {ex.Message}");
                throw new Exception($"Error deleting discarded menu feedback with id {id}", ex);
            }
        }
    }
}
