using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Infra;

public class RepositorioCaixaEmArquivo : RepositorioBaseEmArquivo<Caixa>, IRepositorioCaixa
{
    public RepositorioCaixaEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Caixa> CarregarRegistros()
    {
        return contexto.Caixas;
    }
}