namespace CRM.Api.Data.Entities
{
   
        public class Salesperson
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();

            public string Name { get; set; } = null!;

            public string Phone { get; set; } = null!;

            public string Email { get; set; } = null!;
        }
    
}
