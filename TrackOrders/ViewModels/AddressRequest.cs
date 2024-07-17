using System.ComponentModel.DataAnnotations;

namespace TrackOrders.ViewModels
{
    public class AddressRequest
    {
        [Required]
        [RegularExpression("([0-9]+)")]

        public string Code { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Number { get; set; }
        public string District { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }
}
