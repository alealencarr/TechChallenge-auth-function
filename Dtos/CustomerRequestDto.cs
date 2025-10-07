using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge_auth_function.Dtos
{
    public record CustomerRequestDto
    {
        public required string Cpf { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
    }

}
