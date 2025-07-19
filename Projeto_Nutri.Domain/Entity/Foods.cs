namespace Projeto_Nutri.Domain.Entity
{
    public class Foods
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal CaloriasPor100g { get; set; }
        public DateTime DataCriacao { get; set; }

        // Relacionamento com MealPlanFood
        public ICollection<MealPlanFoods> UsosEmPlanos { get; set; } = new List<MealPlanFoods>();

        public Foods(string nome, decimal caloriasPor100g, DateTime dataCriacao)
        {
            Nome = nome;
            CaloriasPor100g = caloriasPor100g;
            DataCriacao = dataCriacao;
        }

        // Construtor padrão para o EF
        public Foods() { }
    }
}
