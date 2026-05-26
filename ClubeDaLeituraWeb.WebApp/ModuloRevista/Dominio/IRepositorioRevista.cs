using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra;

public interface IRepositorioRevista : IRepositorio<Revista>
{
    bool ExisteRevistaNaCaixa(string caixaId);
}