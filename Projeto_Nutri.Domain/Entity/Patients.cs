using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Domain.Entity
{
    public class Patients
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Genero { get; set; }
        public DateTime DataCriacao { get; set; }

        public Patients(string nome, int idade, string genero, DateTime dataCriacao)
        {
            Nome = nome;
            Idade = idade;
            Genero = genero;
            DataCriacao = dataCriacao;
        }

        // Construtor vazio (caso o Entity Framework precise)

        public Patients() { }
    }
}
