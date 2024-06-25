namespace DataAcessLayer.Entity
{
    public class DiscardedMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MealName")]
        public int MealNameId { get; set; }

        [AllowNull]
        public bool IsDiscarded { get; set; }

        public MealName MealName { get; set; }

        public ICollection<DiscardedMenuFeedback> DiscardedMenuFeedbacks { get; set; }
    }
}