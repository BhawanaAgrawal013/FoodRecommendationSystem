namespace DataAcessLayer.Service.IService
{
    public interface IDiscardedMenuFeedbackService
    {
        void AddDiscardedMenuFeedback(DiscardedMenuFeedbackDTO discardedMenuDTO);
        void DeleteDiscardedMenuFeedback(int id);
        DiscardedMenuFeedbackDTO GetDiscardedMenuFeedback(int id);
        List<DiscardedMenuFeedbackDTO> GetDiscardedMenuFeedbackList();
        void UpdateDiscardedMenuFeedback(DiscardedMenuFeedbackDTO discardedMenuDTO);
    }
}