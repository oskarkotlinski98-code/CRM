using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Functions.Entities
{
    public class Customer
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public string Title { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Email { get; set; } = "";

        public string Address { get; set; } = "";

        public Salesperson ResponsibleSeller { get; set; }
    }
}
