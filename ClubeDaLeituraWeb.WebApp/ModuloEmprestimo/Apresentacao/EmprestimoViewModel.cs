using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;


public record ListarEmprestimosViewModel(
    string Id,
    string NomeAmigo,
    string TituloRevista,
    DateTime Abertura,
    DateTime ConclusaoPrevista,
    StatusEmprestimo Status,
    bool EstaAtrasado
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "O campo \"Amigo\" deve ser preenchido.")]
    string AmigoId,

    [Required(ErrorMessage = "O campo \"Revista\" deve ser preenchido.")]
    string RevistaId,


    List<Amigo>? Amigos,
    List<Revista>? Revistas
);

public record ConcluirEmprestimoViewModel(
    string Id,
    string NomeAmigo,
    string TituloRevista,
    DateTime Abertura,
    DateTime ConclusaoPrevista
);