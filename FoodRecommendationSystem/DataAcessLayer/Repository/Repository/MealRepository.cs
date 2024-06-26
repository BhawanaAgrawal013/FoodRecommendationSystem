﻿namespace DataAcessLayer.Repository.Repository
{
    public class MealRepository : IRepository<Meal>
    {
        private readonly FoodRecommendationContext _context;

        public MealRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Meal> GetAll()
        {
            return _context.Meals.Where(x => !x.IsDeleted).Include(x => x.MealName).Include(x => x.Food).ToList();
        }

        public Meal GetById(int id)
        {
            return _context.Meals.Include(x => x.MealName).Include(x => x.Food).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Meal entity)
        {
            _context.Meals.Add(entity);
        }

        public void Update(Meal entity)
        {
            var existingEntity = _context.Meals.Find(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Meals.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var meal = _context.Meals.Find(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
