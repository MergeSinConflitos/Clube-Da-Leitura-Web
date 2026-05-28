using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioRevista repositorioRevista;

    public EmprestimoController(
        IRepositorioEmprestimo repositorioEmprestimo,
        IRepositorioAmigo repositorioAmigo,
        IRepositorioRevista repositorioRevista)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarTodos();

        List<ListarEmprestimosViewModel> listarVms = new List<ListarEmprestimosViewModel>();

        foreach (Emprestimo e in emprestimos)
        {   
            if(e.EstaAtrasado)
                e.Status = StatusEmprestimo.Atrasado;
            
            ListarEmprestimosViewModel viewModel = new ListarEmprestimosViewModel(
                e.Id,
                e.Amigo.Nome,
                e.Revista.Titulo,
                e.Abertura,
                e.ConclusaoPrevista,
                e.Status,
                e.EstaAtrasado
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {   
        //amigo sim emprestimo activo
       List<Amigo> amigosDisponiveis = repositorioAmigo.SelecionarTodos()
        .Where(a =>!repositorioEmprestimo.SelecionarTodos()
            .Any(e => e.Amigo.Id == a.Id && e.Status == StatusEmprestimo.Aberto))
            .ToList();

        //revista disponibles  

        List<Revista> revistasDisponiveis = repositorioRevista.SelecionarTodos()
        .Where(r => r.Status == StatusRevista.Disponivel)
        .ToList();
        
        CadastrarEmprestimoViewModel cadastrarVm = new CadastrarEmprestimoViewModel(
            string.Empty,
            string.Empty,
            amigosDisponiveis,
            revistasDisponiveis
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel cadastrarVm)
    {
        List<Amigo> amigosDisponiveis = repositorioAmigo.SelecionarTodos()
            .Where(a => !repositorioEmprestimo.SelecionarTodos()
                .Any(e => e.Amigo.Id == a.Id && e.Status == StatusEmprestimo.Aberto))
            .ToList();

        List<Revista> revistasDisponiveis = repositorioRevista.SelecionarTodos()
            .Where(r => r.Status == StatusRevista.Disponivel)
            .ToList();

        // valida se amigo  tem emprestimo activo

        bool amigoTemEmprestimoAtivo = repositorioEmprestimo.SelecionarTodos()
            .Any(e => e.Amigo.Id == cadastrarVm.AmigoId && e.Status == StatusEmprestimo.Aberto);

        if (amigoTemEmprestimoAtivo)
            ModelState.AddModelError(nameof(cadastrarVm.AmigoId), "Este amigo já possui um empréstimo ativo.");

        if (!ModelState.IsValid)
        {
            CadastrarEmprestimoViewModel vmComListas = new CadastrarEmprestimoViewModel(
                cadastrarVm.AmigoId,
                cadastrarVm.RevistaId,
                amigosDisponiveis,
                revistasDisponiveis
            );
            return View(vmComListas);
        }

        Amigo? amigo = repositorioAmigo.SelecionarPorId(cadastrarVm.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(cadastrarVm.RevistaId);

        if (amigo == null || revista == null)
        {
            ModelState.AddModelError(string.Empty, "Amigo ou revista não encontrados.");
            return View(cadastrarVm);
        }

        Emprestimo novoEmprestimo = new Emprestimo(revista, amigo);
        novoEmprestimo.Abrir();

        repositorioEmprestimo.Cadastrar(novoEmprestimo);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Concluir(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo == null)
            return RedirectToAction(nameof(Listar));

        ConcluirEmprestimoViewModel concluirVm = new ConcluirEmprestimoViewModel(
            emprestimo.Id,
            emprestimo.Amigo.Nome,
            emprestimo.Revista.Titulo,
            emprestimo.Abertura,
            emprestimo.ConclusaoPrevista
        );

        return View(concluirVm);
    }

    [HttpPost]
    public ActionResult Concluir(ConcluirEmprestimoViewModel concluirVm)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(concluirVm.Id);

        if (emprestimo == null)
        {
            TempData["ERRO"] = "Empréstimo não encontrado.";
            return RedirectToAction(nameof(Listar));
        }

        emprestimo.Concluir();

        repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);

        return RedirectToAction(nameof(Listar));
    }

}
