using System;
using System.Collections.Generic;

namespace Projeto_Nutri.Domain.Entity
{
    public class Patients
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Genero { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }

        // Relacionamento com MealPlans
        public ICollection<MealPlans> MealPlans { get; set; } = new List<MealPlans>();

        public Patients(string nome, int idade, string genero, DateTime dataCriacao)
        {
            Nome = nome;
            Idade = idade;
            Genero = genero;
            DataCriacao = dataCriacao;
        }

        public Patients() { }
    }
}
