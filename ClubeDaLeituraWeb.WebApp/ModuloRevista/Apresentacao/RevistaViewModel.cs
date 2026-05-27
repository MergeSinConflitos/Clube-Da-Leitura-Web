using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

public class AnoValidoAttribute : ValidationAttribute
{
    private const int AnoMinimo = 1845;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int ano)
        {
            var anoAtual = DateTime.Now.Year;

            if (ano < AnoMinimo || ano > anoAtual)
            {
                return new ValidationResult(
                 ErrorMessage ?? $"O campo \"Ano De Publicação\"deve ser entre {AnoMinimo} e {anoAtual}"
                );
            }
        }
        return ValidationResult.Success;
    }
}
public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroDeEdicao,
    int AnoDePublicacao,
    string Caixa,
    StatusRevista Status
);

public record CadastrarRevistasViewModel(

    [Required(ErrorMessage ="O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength =2, ErrorMessage ="O campo \"Titulo\" deve conter entre 2 e 100 caracteres")]
    string Titulo,

    [Range(1,int.MaxValue,ErrorMessage ="O campo \"Numero De Edição\"deve conter um valor maior que 0")]
    int NumeroDeEdicao,

    [AnoValidoAttribute]
    int? AnoDePublicacao,

    [Required(ErrorMessage = "Informe um ID valido")]
    string CaixaId
);

public record EditarRevistasViewModel(
    string Id,

    [Required(ErrorMessage ="O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength =2, ErrorMessage ="O campo \"Titulo\" deve conter entre 2 e 100 caracteres")]
    string Titulo,

    [Range(1,int.MaxValue,ErrorMessage ="O campo \"Numero De Edição\"deve conter um valor maior que 0")]
    int NumeroDeEdicao,

    [AnoValidoAttribute]
    int AnoDePublicacao,

    [Required(ErrorMessage = "Informe um ID valido")]
    string CaixaId
);

public record ExcluirRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroDeEdicao,
    int AnoDePublicacao,
    string Caixa
);