namespace DataAcessLayer.ModelDTOs
{
    public class DiscardedMenuFeedbackDTO
    {
        public int Id { get; set; }

        public int DiscardedMenuId { get; set; }

        public string DislikeText { get; set; }

        public string LikeText { get; set; }

        public string Recipie { get; set; }

        public DiscardedMenu DiscardedMenu { get; set; }

        public static implicit operator DiscardedMenuFeedbackDTO(DiscardedMenuFeedback discardedMenuFeedback)
        {
            if (discardedMenuFeedback == null) return null;

            return new DiscardedMenuFeedbackDTO()
            {
                Id = discardedMenuFeedback.Id,
                DiscardedMenuId = discardedMenuFeedback.DiscardedMenuId,
                DislikeText = discardedMenuFeedback.DislikeText,
                LikeText = discardedMenuFeedback.LikeText,
                Recipie = discardedMenuFeedback.Recipie
            };
        }

        public static implicit operator DiscardedMenuFeedback(DiscardedMenuFeedbackDTO discardedMenuFeedbackDTO)
        {
            if (discardedMenuFeedbackDTO == null) return null;

            return new DiscardedMenuFeedback()
            {
                Id = discardedMenuFeedbackDTO.Id,
                DiscardedMenuId = discardedMenuFeedbackDTO.DiscardedMenuId,
                DislikeText = discardedMenuFeedbackDTO.DislikeText,
                LikeText = discardedMenuFeedbackDTO?.LikeText,
                Recipie = discardedMenuFeedbackDTO.Recipie,
            };
        }
    }
}
