using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class RevistaController : Controller
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;

    public RevistaController(IRepositorioRevista repositorioRevista, IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms = new List<ListarRevistasViewModel>();

        foreach (Revista r in revistas)
        {
            ListarRevistasViewModel viewModel = new ListarRevistasViewModel(
                r.Id,
                r.Titulo,
                r.NumeroDeEdicao,
                r.AnoDePublicacao,
                r.Caixa.Etiqueta
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }


    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Caixas = CarregarCaixas();
        CadastrarRevistasViewModel cadastrarRevistasViewModel = new CadastrarRevistasViewModel(
            string.Empty,
            0,
            0,
            string.Empty
        );

        return View(cadastrarRevistasViewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarRevistasViewModel cadastrarRevistasView)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(cadastrarRevistasView.CaixaId);

        if (caixa == null)
        {
            ModelState.AddModelError(
                   nameof(cadastrarRevistasView.CaixaId),
                   "Selecione um id valido"
               );
        }

        bool jaExiste = repositorioRevista.SelecionarTodos().Any
        (r => r.Titulo.ToLower() == cadastrarRevistasView.Titulo.ToLower()
        && r.NumeroDeEdicao == cadastrarRevistasView.NumeroDeEdicao);

        if (jaExiste)
        {
            ModelState.AddModelError(nameof(cadastrarRevistasView.Titulo), "Essa edição já está cadastrada ");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Caixas = CarregarCaixas();
            return View(cadastrarRevistasView);
        }
        Revista novaRevista = new Revista(
            cadastrarRevistasView.Titulo,
            cadastrarRevistasView.NumeroDeEdicao,
            cadastrarRevistasView.AnoDePublicacao,
            caixa);


        repositorioRevista.Cadastrar(novaRevista);

        return RedirectToAction(nameof(Listar));
    }


    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        EditarRevistasViewModel editarvms = new EditarRevistasViewModel(
        id,
        revista.Titulo,
        revista.NumeroDeEdicao,
        revista.AnoDePublicacao,
        revista.Caixa.Id
        );

        ViewBag.Caixas = CarregarCaixas();

        return View(editarvms);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistasViewModel editarVm)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(editarVm.CaixaId);

        if (caixa == null)
            ModelState.AddModelError(
                nameof(editarVm.CaixaId),
                "Selecione um id valido"
            );

        if (!ModelState.IsValid)
        {
            ViewBag.Caixas = CarregarCaixas();
            return View(editarVm);
        }

        Revista revistaAtualizada = new Revista(
            editarVm.Titulo,
            editarVm.NumeroDeEdicao,
            editarVm.AnoDePublicacao,
            caixa);

        repositorioRevista.Editar(editarVm.Id, revistaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistasViewModel excluirvms = new ExcluirRevistasViewModel(
        id,
        revista.Titulo,
        revista.NumeroDeEdicao,
        revista.AnoDePublicacao,
        revista.Caixa.Etiqueta);
        return View(excluirvms);
    }

    [HttpPost]
    [ActionName("Excluir")]
    public ActionResult ExcluirConfirmado(ExcluirRevistasViewModel excluirVm)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(excluirVm.Id);

        if (revista != null)
            repositorioRevista.Excluir(revista);

        return RedirectToAction(nameof(Listar));


    }



    private List<SelectListItem> CarregarCaixas()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        List<SelectListItem> listarvms = new List<SelectListItem>();

        foreach (Caixa c in caixas)
        {
            SelectListItem viewModel = new SelectListItem(
                c.Etiqueta,
                c.Id
            );

            listarvms.Add(viewModel);
        }

        return listarvms;
    }
}

