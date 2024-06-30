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
            DiscardedMenuFeedback discardedMenuFeedback = (DiscardedMenuFeedback)discardedMenuDTO;
            _discardedMenuFeedbackRepository.Insert(discardedMenuFeedback);
            _discardedMenuFeedbackRepository.Save();
        }

        public List<DiscardedMenuFeedbackDTO> GetDiscardedMenuFeedbackList()
        {
            var discardedMenus = _discardedMenuFeedbackRepository.GetAll();
            return discardedMenus.Select(dm => (DiscardedMenuFeedbackDTO)dm).ToList();
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
                throw new Exception($"Error getting the food {id}", ex);
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
                throw new Exception("Error updating the food", ex);
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
                throw new Exception($"Error deleting the food {id}", ex);
            }
        }
    }
}
