﻿using Serilog;

namespace DataAcessLayer.Helpers
{
    public class ChefHelper : IChefHelper
    {
        private readonly IMealMenuService _mealMenuService;
        private readonly IMealNameService _mealNameService;

        public ChefHelper(IMealMenuService mealMenuService, IMealNameService mealNameService)
        {
            _mealMenuService = mealMenuService;
            _mealNameService = mealNameService;
        }

        public void CreateNextMealMenu(List<string> mealNames, string classification)
        {
            try
            {
                var meals = _mealNameService.GetAllMeals();

                var existingOption = _mealMenuService.GetAllMealMenus().Any(x => x.CreationDate == DateTime.Now.Date && x.Classification == classification);

                if (existingOption)
                    throw new Exception($"Already rolled out the menu for {classification}");

                foreach (var mealName in mealNames)
                {
                    var meal = meals.FirstOrDefault(x => x.MealName == mealName) ?? throw new Exception($"Meal '{mealName}' not found.");
                    MealMenuDTO mealMenuDTO = new MealMenuDTO()
                    {
                        NumberOfVotes = 0,
                        WasPrepared = false,
                        Classification = classification,
                        CreationDate = DateTime.Now.Date,
                        MealName = meal
                    };

                    _mealMenuService.AddMealMenu(mealMenuDTO);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating next meal menu: {ex.Message}");
                throw new Exception($"Error creating next meal menu: {ex.Message}");
            }
        }

        public List<MealMenuDTO> ViewEmployeeVotes(string classification, DateTime dateTime)
        {
            try
            {
                var meals = _mealMenuService.GetAllMealMenus().Where(x => x.Classification == classification && x.CreationDate == dateTime).ToList();

                if (meals == null || !meals.Any())
                {
                    throw new Exception($"No meals found for classification '{classification}' on date '{dateTime.ToShortDateString()}'.");
                }

                return meals;
            }
            catch (Exception ex)
            {
                Log.Error($"Error viewing employee votes: {ex.Message}");
                throw new Exception($"Error viewing employee votes: {ex.Message}");
            }
        }

        public MealMenuDTO ChooseNextMealMenu(int mealMenuId)
        {
            try
            {
                var mealMenu = _mealMenuService.GetMealMenu(mealMenuId) ?? throw new Exception($"Meal menu with ID '{mealMenuId}' not found.");
                mealMenu.WasPrepared = true;
                _mealMenuService.UpdateMealMenu(mealMenu);

                return mealMenu;
            }
            catch (Exception ex)
            {
                Log.Error($"Error choosing next meal menu: {ex.Message}");
                throw new Exception($"Error choosing next meal menu: {ex.Message}");
            }
        }
    }
}
