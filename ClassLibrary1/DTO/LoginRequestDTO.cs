using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Application.DTO
{
    public class LoginRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "NUTRITIONIST"; // ou ADMIN
    }
}
