using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao;

// Listagem pública para qualquer usuário autenticado (funciona como um
// "corpo docente"). Cadastro/exclusão restritos a Administrador. Um
// Instrutor só pode editar o próprio registro (segregação de dados).
public class InstrutorController(IInstrutorServico instrutorServico, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Listar()
    {
        List<Instrutor> instrutores = instrutorServico.SelecionarTodos();
        return View(mapper.Map<List<InstrutorListarViewModel>>(instrutores));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar() => View(new InstrutorFormularioViewModel());

    [HttpPost]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(InstrutorFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Instrutor instrutor = mapper.Map<Instrutor>(viewModel);
        Result<Instrutor> resultado = instrutorServico.Cadastrar(instrutor);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Instrutor cadastrado com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Instrutor")]
    public IActionResult Editar(Guid id)
    {
        if (!PodeAcessar(id))
            return Forbid();

        Instrutor? instrutor = instrutorServico.SelecionarPorId(id);

        if (instrutor is null)
            return RedirectToAction(nameof(Listar));

        InstrutorFormularioViewModel viewModel = mapper.Map<InstrutorFormularioViewModel>(instrutor);
        viewModel.Id = instrutor.Id;

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Instrutor")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, InstrutorFormularioViewModel viewModel)
    {
        if (!PodeAcessar(id))
            return Forbid();

        if (!ModelState.IsValid)
            return View(viewModel);

        Instrutor instrutorAtualizado = mapper.Map<Instrutor>(viewModel);
        Result<Instrutor> resultado = instrutorServico.Editar(id, instrutorAtualizado);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Instrutor atualizado com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Instrutor? instrutor = instrutorServico.SelecionarPorId(id);

        if (instrutor is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<InstrutorExcluirViewModel>(instrutor));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    [ValidateAntiForgeryToken]
    public IActionResult Excluir(InstrutorExcluirViewModel viewModel)
    {
        Result resultado = instrutorServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Instrutor excluído com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    private bool PodeAcessar(Guid instrutorId)
    {
        if (User.EhAdministrador())
            return true;

        return User.ObterInstrutorId() == instrutorId;
    }
}
