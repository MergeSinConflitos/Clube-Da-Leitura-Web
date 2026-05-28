using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;

namespace ClubeDaLeitura.WebApp.ModuloAmigo.Dominio;

public class Amigo : EntidadeBase<Amigo>
{
    public string Nome { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    //public List<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>(); 

    public Amigo()
    {
        
    } 

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public override List<string> Validar() 
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O campo \"Nome\" deve ser preenchido");
        else if (Nome.Length < 2 || Nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 2 e 100 caracteres");

        if (string.IsNullOrWhiteSpace(NomeResponsavel))
            erros.Add("O campo \"Nome do Responsável\" deve ser preenchido");
        else if (NomeResponsavel.Length < 2 || NomeResponsavel.Length > 100)
            erros.Add("O campo \"Nome do Responsável\" deve conter entre 2 e 100 caracteres");

        string telefoneEncurtado = Telefone.Replace(" ", "").Replace("-", "");
        int contadorDigitos = 0;
        bool contemLetraOuSimbolo = false;

        for (int i = 0; i < telefoneEncurtado.Length; i++)
        {
            if (char.IsDigit(telefoneEncurtado[i]))
                contadorDigitos++;
            else
            {
                contemLetraOuSimbolo = true;
                break;
            }
        }

        if (contadorDigitos < 10 || contadorDigitos > 11)
            erros.Add("O campo \"Telefone\" deve conter entre 10 e 11 dígitos");

        if (contemLetraOuSimbolo)
            erros.Add("O campo \"Telefone\" deve conter apenas dígitos");

        return erros;
    }

    public override void AtualizarDados(Amigo entidadeAtualizada)
    {
       
        Nome = entidadeAtualizada.Nome;
        NomeResponsavel = entidadeAtualizada.NomeResponsavel;
        Telefone = entidadeAtualizada.Telefone;
    }
}