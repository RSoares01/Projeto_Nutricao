namespace Projeto_Nutri.Application.DTO
{
    public class MealPlansDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public string NomeDoPaciente { get; set; } = string.Empty;

        public decimal CaloriasTotais { get; set; }

        public List<MealPlanFoodReadDTO> Alimentos { get; set; } = new();
    }

    public class MealPlanFoodReadDTO
    {
        public int FoodId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal CaloriasPor100g { get; set; }
        public decimal TamanhoDaPorcaoEmGramas { get; set; }

        public decimal CaloriasTotais =>
            Math.Round((CaloriasPor100g / 100m) * TamanhoDaPorcaoEmGramas, 2);
    }

    namespace Projeto_Nutri.Application.DTO
    {
        public class MealPlanCreateDTO
        {
            public int PatientId { get; set; }
            public string Nome { get; set; } = string.Empty;

            public List<MealPlanFoodCreateDTO> Alimentos { get; set; } = new();
        }

        public class MealPlanFoodCreateDTO
        {
            public int FoodId { get; set; }
            public decimal TamanhoDaPorcaoEmGramas { get; set; }
        }
    }

}
