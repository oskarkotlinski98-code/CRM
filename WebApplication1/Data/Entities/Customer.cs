using Newtonsoft.Json;

namespace CRM.Api.Data.Entities
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public Salesperson ResponsibleSeller { get; set; } = null!;
    }
}
