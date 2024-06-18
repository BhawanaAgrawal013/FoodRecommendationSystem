using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class SummaryRatingService : ISummaryRatingService
    {
        private readonly IRepository<SummaryRating> _summaryRatingRepository;

        public SummaryRatingService(IRepository<SummaryRating> summaryRatingRepository)
        {
            _summaryRatingRepository = summaryRatingRepository;
        }

        public void AddSummaryRating(SummaryRatingDTO summaryRatingDTO)
        {
            try
            {
                SummaryRating summaryRating = (SummaryRating)summaryRatingDTO;
                _summaryRatingRepository.Insert(summaryRating);
                _summaryRatingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting summary rating", ex);
            }
        }

        public List<SummaryRatingDTO> GetAllSummaryRatings()
        {
            try
            {
                var summaryRatings = _summaryRatingRepository.GetAll();
                return summaryRatings.Select(summaryRating => (SummaryRatingDTO)summaryRating).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all summary ratings", ex);
            }
        }

        public SummaryRatingDTO GetSummaryRating(int id)
        {
            try
            {
                var summaryRating = _summaryRatingRepository.GetById(id);
                return (SummaryRatingDTO)summaryRating;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the summary rating {id}", ex);
            }
        }

        public SummaryRatingDTO GetSummaryRatingByFoodId(int foodId)
        {
            try
            {
                var summaryRating = _summaryRatingRepository.GetAll().Where(x => x.FoodId == foodId).FirstOrDefault();
                return (SummaryRatingDTO)summaryRating;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the summary rating {foodId}", ex);
            }
        }

        public void UpdateSummaryRating(SummaryRatingDTO summaryRatingDTO)
        {
            try
            {
                var summaryRating = (SummaryRating)summaryRatingDTO;
                _summaryRatingRepository.Update(summaryRating);
                _summaryRatingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the summary rating", ex);
            }
        }

        public void DeleteSummaryRating(int id)
        {
            try
            {
                _summaryRatingRepository.Delete(id);
                _summaryRatingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the summary rating {id}", ex);
            }
        }
    }

}
