using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;

public class RepositorioRevistaEmArquivo : RepositorioBaseEmArquivo<Revista>, IRepositorioRevista
{
    public RepositorioRevistaEmArquivo(ContextoJson contexto) : base(contexto)
    { }

    public bool ExisteRevistaNaCaixa(string caixaId)
    {
        return contexto.Revistas.Any(r => r.Caixa?.Id == caixaId);
    }

    protected override List<Revista> CarregarRegistros()
    {
        return contexto.Revistas;
    }
}