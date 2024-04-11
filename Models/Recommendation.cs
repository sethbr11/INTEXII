using System.ComponentModel.DataAnnotations;

namespace INTEXII.Models {
    public class Recommendation {
        [Key]
        public int CustomerId { get; set; }
        public int RecProdId1 { get; set; }
        public int RecProdId2 { get; set; }
        public int RecProdId3 { get; set; }
        public int RecProdId4 { get; set; }
    }
}
