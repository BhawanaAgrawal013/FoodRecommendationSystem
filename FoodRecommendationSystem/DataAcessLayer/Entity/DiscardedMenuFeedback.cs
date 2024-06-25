namespace DataAcessLayer.Entity
{
    public class DiscardedMenuFeedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("DiscardedMenu")]
        public int DiscardedMenuId { get; set; }

        public string DislikeText { get; set; }

        public string LikeText { get; set; }

        [AllowNull]
        public string Recipie {  get; set; }

        public DiscardedMenu DiscardedMenu { get; set; }
    }
}