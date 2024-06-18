using DataAcessLayer.Entity;
using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Helpers
{
    public class ChefHelper : IChefHelper
    {
        private readonly IMealMenuService _mealMenuService;
        private readonly IMealService _mealService;
        private readonly IMealNameService _mealNameService;

        public ChefHelper(IMealMenuService mealMenuService, IMealNameService mealNameService, IMealService mealService)
        {
            _mealMenuService = mealMenuService;
            _mealNameService = mealNameService;
            _mealService = mealService;
        }

        public void CreateNextMealMenu(List<string> mealNames, string classification)
        {
            var meals = _mealNameService.GetAllMeals();

            foreach (var mealName in mealNames)
            {
                var meal = meals.Where(x => x.MealName == mealName).FirstOrDefault();

                MealMenuDTO mealMenuDTO = new MealMenuDTO()
                {
                    NumberOfVotes = 0,
                    WasPrepared = false,
                    Classification = classification,
                    CreationDate = DateTime.Now.Date,
                    MealName = (MealNameDTO)meal
                };

                _mealMenuService.AddMealMenu(mealMenuDTO);
            }
        }

        public List<MealMenuDTO> ViewEmployeeVotes(string classification, DateTime dateTime)
        {
            var meals = _mealMenuService.GetAllMealMenus().Where(x => x.Classification == classification && x.CreationDate == dateTime).ToList();

            return meals;
        }

        public MealMenuDTO ChooseNextMealMenu(int mealMenuId)
        {
            var mealMenu = _mealMenuService.GetMealMenu(mealMenuId);
            mealMenu.WasPrepared = true;

            _mealMenuService.UpdateMealMenu(mealMenu);

            return mealMenu;
        }

        public List<MealDTO> SendDishesToReview(List<string> mealNames)
        {
            List<MealDTO> meals = new List<MealDTO>();

            foreach (var mealName in mealNames)
            {
                var meal = _mealService.GetAllMeals().Where(x => x.MealName.MealName == mealName);
                meals.AddRange(meal);
            }

            return meals;
        }
    }
}
