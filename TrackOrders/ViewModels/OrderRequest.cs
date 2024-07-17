using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using TrackOrders.Data.ValueObjects;

namespace TrackOrders.ViewModels
{
    public class OrderRequest
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Value { get; set; }
        public AddressRequest DeliveryAddress { get; set; }
    }
}
