using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra;
using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Infra;



public class RepositorioCaixaEmArquivo : RepositorioBaseEmArquivo<Caixa>, IRepositorioCaixa
{
    public RepositorioCaixaEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Caixa> CarregarRegistros()
    {
        return contexto.Caixas;
    }
}
