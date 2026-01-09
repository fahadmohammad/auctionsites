using System.ComponentModel.DataAnnotations;

namespace AuctionService_Controllers.Dtos
{
    public class CreateActionDto
    {
        [Required]
        public required string Make { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int ReservePrice { get; set; }
        [Required]
        public DateTime AuctionEnd { get; set; }
    }
}