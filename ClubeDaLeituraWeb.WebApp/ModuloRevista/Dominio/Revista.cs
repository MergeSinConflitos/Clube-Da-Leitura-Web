using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;

public class Revista : EntidadeBase<Revista>
{
    public string Titulo { get; set; }
    public int NumeroDeEdicao { get; set; }
    public int AnoDePublicacao { get; set; }
    public Caixa Caixa { get; set; }
    public StatusRevista Status { get; set; }

    public Revista(string titulo, int numeroDeEdicao, int anoDePublicacao, Caixa caixa)
    {
        Titulo = titulo;
        NumeroDeEdicao = numeroDeEdicao;
        AnoDePublicacao = anoDePublicacao;
        Caixa = caixa;
    }

    public Revista()
    { }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo))
        {
            erros.Add("O campo '/Titulo/' é obrigatorio");
        }
        else if (Titulo.Length < 2 || Titulo.Length > 100)
        {
            erros.Add("O campo '/Titulo/' deve ter entre 2 e 100 caracteres");

        }


        if (NumeroDeEdicao < 0)
        {
            erros.Add("Informe um numero maior ou igual a 0");

        }

        int anoValido = DateTime.Now.Year;

        if (AnoDePublicacao < 1 || AnoDePublicacao > anoValido)
        {
            erros.Add("Informe uma data valida");

        }

        if (Caixa == null)
        {
            erros.Add("O campo '/Caixa/' é obrigatorio");

        }

        return erros;
    }


    public override void AtualizarDados(Revista entidadeAtualizada)
    {
        Revista revistaAtualizada = (Revista)entidadeAtualizada;

        Titulo = revistaAtualizada.Titulo;
        NumeroDeEdicao = revistaAtualizada.NumeroDeEdicao;
        AnoDePublicacao = revistaAtualizada.AnoDePublicacao;
        Caixa = revistaAtualizada.Caixa;
    }
}

public enum StatusRevista
{
    Disponivel, Emprestada
}