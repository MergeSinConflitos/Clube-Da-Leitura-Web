using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

public interface IRepositorioRevista : IRepositorio<Revista>
{
    bool ExisteRevistaNaCaixa(string caixaId);
}