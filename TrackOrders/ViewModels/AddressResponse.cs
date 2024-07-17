using System.ComponentModel.DataAnnotations;

namespace TrackOrders.ViewModels
{
    public class AddressResponse
    {
        public string Cep { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
