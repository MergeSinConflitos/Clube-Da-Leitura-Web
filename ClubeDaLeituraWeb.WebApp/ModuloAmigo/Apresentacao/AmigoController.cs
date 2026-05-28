using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloAmigo.Apresentacao;


public class AmigoController : Controller
{
    private readonly IRepositorioAmigo repositorioAmigo;

    public AmigoController(IRepositorioAmigo repositorioAmigo)
    {
        this.repositorioAmigo = repositorioAmigo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Amigo> amigos = repositorioAmigo.SelecionarTodos();

        List<ListarAmigosViewModel> listarVms = new List<ListarAmigosViewModel>();

        foreach (Amigo a in amigos)
        {
            ListarAmigosViewModel viewModel = new ListarAmigosViewModel(
                a.Id,
                a.Nome,
                a.NomeResponsavel,
                a.Telefone
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarAmigoViewModel cadastrarVm = new CadastrarAmigoViewModel(
            string.Empty,
            string.Empty,
            string.Empty
        );
        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarAmigoViewModel cadastrarVm)
    {   
        //Validacion de duplicados
        bool jaExiste = repositorioAmigo.SelecionarTodos().Any(
            a => a.Nome.ToLower() == cadastrarVm.Nome.ToLower() &&
            a.NomeResponsavel.ToLower() == cadastrarVm.NomeResponsavel.ToLower() &&
            a.Telefone == cadastrarVm.Telefone
        );

        if(jaExiste)
        {
            ModelState.AddModelError(nameof(cadastrarVm.Nome), "Já existe um amigo com o mesmo nome e telefone.");
        }

        if(!ModelState.IsValid)
            return View(cadastrarVm);

        Amigo novoAmigo = new Amigo(
            cadastrarVm.Nome,
            cadastrarVm.NomeResponsavel,
            cadastrarVm.Telefone
        );

        repositorioAmigo.Cadastrar(novoAmigo);

        return RedirectToAction(nameof(Listar));    
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        EditarAmigoViewModel editarVm = new EditarAmigoViewModel(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarAmigoViewModel editarVm)
    {
        // validar duplicado ignorando o propio registro

        bool jaExiste = repositorioAmigo.SelecionarTodos().Any(
            a => a.Id != editarVm.Id &&
                 a.Nome.ToLower() == editarVm.Nome.ToLower() &&
                 a.Telefone == editarVm.Telefone
        );

        if (jaExiste)
            ModelState.AddModelError(nameof(editarVm.Nome), "Já existe um amigo com o mesmo nome e telefone.");

        if (!ModelState.IsValid)
            return View(editarVm);

        Amigo amigoAtualizado = new Amigo(
            editarVm.Nome,
            editarVm.NomeResponsavel,
            editarVm.Telefone
        );

        repositorioAmigo.Editar(editarVm.Id, amigoAtualizado);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        ExcluirAmigoViewModel excluirVm = new ExcluirAmigoViewModel(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirAmigoViewModel excluirVm)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(excluirVm.Id);

        // validar empréstimos vinculados cuando este creado el módulo de empréstimos!!!
        /*
        if (amigo != null && amigo.Emprestimos.Count > 0)
        {
            TempData["ERRO"] = "Não é possível excluir este amigo pois existem empréstimos vinculados.";
            return RedirectToAction(nameof(Listar));
        }*/

        if (amigo != null)
            repositorioAmigo.Excluir(amigo);

        return RedirectToAction(nameof(Listar));
    }
}
