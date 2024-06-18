namespace DataAcessLayer.Service.IService
{
    public interface ISummaryRatingService
    {
        void AddSummaryRating(SummaryRatingDTO summaryRatingDTO);
        void DeleteSummaryRating(int id);
        List<SummaryRatingDTO> GetAllSummaryRatings();
        SummaryRatingDTO GetSummaryRating(int id);
        void UpdateSummaryRating(SummaryRatingDTO summaryRatingDTO);
        SummaryRatingDTO GetSummaryRatingByFoodId(int foodId);
    }
}