using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Application.DTO
{
    public class FoodsDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Caloriaspor100g { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
