using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;
using EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloTurma.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscolaDeCursos.WebApp.Modulos.ModuloTurma.Apresentacao;

public class TurmaController(
    ITurmaServico turmaServico,
    ICursoServico cursoServico,
    IInstrutorServico instrutorServico,
    IMatriculaServico matriculaServico,
    IMapper mapper
) : Controller
{
    [HttpGet]
    public IActionResult Listar()
    {
        List<Turma> turmas = turmaServico.SelecionarTodos();

        List<TurmaListarViewModel> viewModel = turmas.Select(turma =>
        {
            TurmaListarViewModel item = mapper.Map<TurmaListarViewModel>(turma);
            item.MatriculasAtivas = matriculaServico.SelecionarPorTurma(turma.Id)
                .Count(m => m.Situacao != Dominio.Modulos.ModuloMatricula.SituacaoMatricula.Cancelada);
            return item;
        }).ToList();

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar()
    {
        CarregarListasSelecao();
        return View(new TurmaFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar(TurmaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarListasSelecao();
            return View(viewModel);
        }

        Turma turma = mapper.Map<Turma>(viewModel);
        Result<Turma> resultado = turmaServico.Cadastrar(turma);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarListasSelecao();
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Turma cadastrada com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id)
    {
        Turma? turma = turmaServico.SelecionarPorId(id);

        if (turma is null)
            return RedirectToAction(nameof(Listar));

        TurmaFormularioViewModel viewModel = mapper.Map<TurmaFormularioViewModel>(turma);
        viewModel.Id = turma.Id;

        CarregarListasSelecao();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id, TurmaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarListasSelecao();
            return View(viewModel);
        }

        Turma turmaAtualizada = mapper.Map<Turma>(viewModel);
        Result<Turma> resultado = turmaServico.Editar(id, turmaAtualizada);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarListasSelecao();
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Turma atualizada com sucesso!";
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Turma? turma = turmaServico.SelecionarPorId(id);

        if (turma is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<TurmaExcluirViewModel>(turma));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(TurmaExcluirViewModel viewModel)
    {
        Result resultado = turmaServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Turma excluída com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarListasSelecao()
    {
        List<Curso> cursos = cursoServico.SelecionarTodos();
        List<Instrutor> instrutores = instrutorServico.SelecionarTodos();

        ViewBag.Cursos = new SelectList(cursos, nameof(Curso.Id), nameof(Curso.Titulo));
        ViewBag.Instrutores = new SelectList(instrutores, nameof(Instrutor.Id), nameof(Instrutor.Nome));
    }
}
