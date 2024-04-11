using System.ComponentModel.DataAnnotations;

namespace INTEXII.Models {
    public class Recommendation {
        [Key]
        public int CustomerId { get; set; }
        public string ProductRecommendation {  get; set; }
        [Key]
        public int ProductId { get; set; }
        public float RankMean { get; set; }
        public string BasedOnLiked { get; set; }
    }
}
