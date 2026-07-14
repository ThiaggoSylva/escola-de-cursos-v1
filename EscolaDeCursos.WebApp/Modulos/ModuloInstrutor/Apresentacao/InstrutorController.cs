using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao;

public class InstrutorController(IInstrutorServico instrutorServico, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Listar()
    {
        List<Instrutor> instrutores = instrutorServico.SelecionarTodos();
        return View(mapper.Map<List<InstrutorListarViewModel>>(instrutores));
    }

    [HttpGet]
    public IActionResult Cadastrar() => View(new InstrutorFormularioViewModel());

    [HttpPost]
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
    public IActionResult Editar(Guid id)
    {
        Instrutor? instrutor = instrutorServico.SelecionarPorId(id);

        if (instrutor is null)
            return RedirectToAction(nameof(Listar));

        InstrutorFormularioViewModel viewModel = mapper.Map<InstrutorFormularioViewModel>(instrutor);
        viewModel.Id = instrutor.Id;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, InstrutorFormularioViewModel viewModel)
    {
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
    public IActionResult Excluir(Guid id)
    {
        Instrutor? instrutor = instrutorServico.SelecionarPorId(id);

        if (instrutor is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<InstrutorExcluirViewModel>(instrutor));
    }

    [HttpPost]
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
}
