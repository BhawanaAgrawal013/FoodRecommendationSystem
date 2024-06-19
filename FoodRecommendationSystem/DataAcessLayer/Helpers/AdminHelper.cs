﻿using DataAcessLayer.Common;

namespace DataAcessLayer.Helpers
{
    public class AdminHelper : IAdminHelper
    {
        private readonly IMealService _mealService;
        private readonly IFoodService _foodService;
        private readonly IMealNameService _mealNameService;

        public AdminHelper(IMealService mealService, IFoodService foodService, IMealNameService mealNameService)
        {
            _mealService = mealService;
            _foodService = foodService;
            _mealNameService = mealNameService;
        }

        public List<FullMenu> GetFullMenu()
        {
            var meals = _mealService.GetAllMeals().GroupBy(x => x.MealName.MealName).Distinct().ToList();

            List<FullMenu> fullMenus = new List<FullMenu>();

            foreach (var meal in meals)
            {
                FullMenu fullMenu = new FullMenu()
                {
                    Name = meal.Select(x => x.MealName.MealName).FirstOrDefault(),
                    Price = meal.Sum(x => x.Food.Price),
                    Classification = meal.Select(x => x.MealName.MealType).FirstOrDefault()
                };

                fullMenus.Add(fullMenu);
            }

            return fullMenus;
        }

        public MealDTO AddMenuItem(MealDTO mealDTO)
        {
            var existingFood = _foodService.GetAllFoods().Where(x => x.Name == mealDTO.Food.Name).FirstOrDefault();

            if (existingFood == null)
            {
                _foodService.AddFood(mealDTO.Food);
            }

            var existingMealName = _mealNameService.GetAllMeals().Where(x => x.MealName == mealDTO.MealName.MealName).FirstOrDefault();

            if (existingMealName == null)
            {
                _mealNameService.AddMealName(mealDTO.MealName);
            }

            mealDTO.Food = _foodService.GetAllFoods().Where(x => x.Name == mealDTO.Food.Name).FirstOrDefault();
            mealDTO.MealName = _mealNameService.GetAllMeals().Where(x => x.MealName == mealDTO.MealName.MealName).FirstOrDefault();

            _mealService.AddMeal(mealDTO);

            return mealDTO;
        }

        public MealDTO UpdateMenuItem(MealDTO mealDTO)
        {
            var existingFood = _foodService.GetAllFoods().Where(x => x.Name == mealDTO.Food.Name).FirstOrDefault();

            if (existingFood == null)
            {
                _foodService.AddFood(mealDTO.Food);
            }
            else
            {
                _foodService.UpdateFood(mealDTO.Food);
            }

            var existingMealName = _mealNameService.GetAllMeals().Where(x => x.MealName == mealDTO.MealName.MealName).FirstOrDefault();

            if (existingMealName == null)
            {
                _mealNameService.AddMealName(mealDTO.MealName);
            }
            else
            {
                _mealNameService.UpdateMealName(mealDTO.MealName);
            }

            var existingMeal = _mealService.GetAllMeals().Where(x => x.Id == mealDTO.Id).FirstOrDefault();

            _mealService.UpdateMeal(existingMeal);

            return mealDTO;
        }

        public string DeleteMenuItem(MealDTO mealDTO)
        {
            var existingMeal = _mealService.GetAllMeals().Where(x => x.Id == mealDTO.Id).FirstOrDefault();

            if(existingMeal == null)
            {
                return "Meal does not exist";
            }
            else
            {
                _mealService.DeleteMeal(mealDTO.Id);

                return "Meal deleted sucessfully";
            }
        }
    }
}