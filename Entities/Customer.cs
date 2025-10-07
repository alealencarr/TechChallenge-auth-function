using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge_auth_function.Entities
{
    public class Customer
    {
        public Customer( string cpf, string name, string mail )
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Cpf = cpf;
            Name = name;
            Mail = mail;
            CustomerIdentified= true;
         }
  


        protected Customer()
        {

        }

        public DateTime CreatedAt { get; private set; }

        public Guid Id { get; set; }
        public string? Cpf { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;

        public bool CustomerIdentified = true;

    }

}
