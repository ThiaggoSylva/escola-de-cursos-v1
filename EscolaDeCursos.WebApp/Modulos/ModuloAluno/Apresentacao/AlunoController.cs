using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao;

public class AlunoController(IAlunoServico alunoServico, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Listar()
    {
        List<Aluno> alunos = alunoServico.SelecionarTodos();
        return View(mapper.Map<List<AlunoListarViewModel>>(alunos));
    }

    [HttpGet]
    public IActionResult Cadastrar() => View(new AlunoFormularioViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(AlunoFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Aluno aluno = mapper.Map<Aluno>(viewModel);
        Result<Aluno> resultado = alunoServico.Cadastrar(aluno);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Aluno cadastrado com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Editar(Guid id)
    {
        Aluno? aluno = alunoServico.SelecionarPorId(id);

        if (aluno is null)
            return RedirectToAction(nameof(Listar));

        AlunoFormularioViewModel viewModel = mapper.Map<AlunoFormularioViewModel>(aluno);
        viewModel.Id = aluno.Id;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, AlunoFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Aluno alunoAtualizado = mapper.Map<Aluno>(viewModel);
        Result<Aluno> resultado = alunoServico.Editar(id, alunoAtualizado);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Aluno atualizado com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Excluir(Guid id)
    {
        Aluno? aluno = alunoServico.SelecionarPorId(id);

        if (aluno is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<AlunoExcluirViewModel>(aluno));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Excluir(AlunoExcluirViewModel viewModel)
    {
        Result resultado = alunoServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Aluno excluído com sucesso!";

        return RedirectToAction(nameof(Listar));
    }
}
