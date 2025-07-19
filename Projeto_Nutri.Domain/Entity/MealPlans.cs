using System;
using System.Collections.Generic;

namespace Projeto_Nutri.Domain.Entity
{
    public class MealPlans
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patients Patient { get; set; } = null!;

        public string Nome { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }

        public ICollection<MealPlanFoods> Alimentos { get; set; } = new List<MealPlanFoods>();

        public MealPlans() { }

        public MealPlans(int patientId, string nome, DateTime dataCriacao)
        {
            PatientId = patientId;
            Nome = nome;
            DataCriacao = dataCriacao;
        }
    }
}
