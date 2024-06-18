namespace DataAcessLayer.Entity
{
    public class MealMenu
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MealName")]
        public int MealNameId { get; set; }

        public int NumberOfVotes { get; set; }
        public bool WasPrepared { get; set; }

        [MaxLength(50)]
        public string Classification { get; set; }

        public DateTime CreationDate { get; set; }

        public MealName MealName { get; set; }
    }
}