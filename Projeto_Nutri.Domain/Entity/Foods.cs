namespace Projeto_Nutri.Domain.Entity
{
    public class Foods
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Caloriaspor100g { get; set; }
        public DateTime DataCriacao { get; set; }

        public Foods(string nome, decimal caloriasPor100g, DateTime dataCriacao)
        {
            Nome = nome;
            Caloriaspor100g = caloriasPor100g;
            DataCriacao = dataCriacao;
        }

        // Construtor vazio (caso o Entity Framework precise)
        public Foods() { }
    }
}
