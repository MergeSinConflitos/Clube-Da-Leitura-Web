using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;

public class RepositorioRevistaEmArquivo : RepositorioBaseEmArquivo<Revista>, IRepositorioRevista
{
    public RepositorioRevistaEmArquivo(ContextoJson contexto) : base(contexto)
    { }

    protected override List<Revista> CarregarRegistros()
    {
        return contexto.Revistas;
    }
}