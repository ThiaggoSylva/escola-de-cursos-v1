using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao;

public class MatriculaController(
    IMatriculaServico matriculaServico,
    IAlunoServico alunoServico,
    ITurmaServico turmaServico,
    IMapper mapper
) : Controller
{
    [HttpGet]
    public IActionResult Listar(Guid? alunoId, Guid? turmaId)
    {
        List<Matricula> matriculas;

        if (alunoId is not null)
            matriculas = matriculaServico.SelecionarPorAluno(alunoId.Value);
        else if (turmaId is not null)
            matriculas = matriculaServico.SelecionarPorTurma(turmaId.Value);
        else
            matriculas = matriculaServico.SelecionarTodos();

        ViewBag.AlunoId = alunoId;
        ViewBag.TurmaId = turmaId;
        ViewBag.Aluno = alunoId is null ? null : alunoServico.SelecionarPorId(alunoId.Value);
        ViewBag.Turma = turmaId is null ? null : turmaServico.SelecionarPorId(turmaId.Value);

        return View(mapper.Map<List<MatriculaListarViewModel>>(matriculas));
    }

    [HttpGet]
    public IActionResult Matricular(Guid? turmaId)
    {
        CarregarListasSelecao();

        return View(new MatriculaFormularioViewModel { TurmaId = turmaId ?? Guid.Empty });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Matricular(MatriculaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarListasSelecao();
            return View(viewModel);
        }

        Matricula matricula = mapper.Map<Matricula>(viewModel);

        Result<Matricula> resultado = matriculaServico.Matricular(matricula);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarListasSelecao();
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Aluno matriculado com sucesso!";
        return RedirectToAction(nameof(Listar), new { turmaId = viewModel.TurmaId });
    }

    [HttpGet]
    public IActionResult Cancelar(Guid id)
    {
        Matricula? matricula = matriculaServico.SelecionarPorId(id);

        if (matricula is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<MatriculaCancelarViewModel>(matricula));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancelar(MatriculaCancelarViewModel viewModel)
    {
        Result resultado = matriculaServico.Cancelar(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Matrícula cancelada com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarListasSelecao()
    {
        List<Aluno> alunos = alunoServico.SelecionarTodos();
        List<Turma> turmas = turmaServico.SelecionarTodos();

        ViewBag.Alunos = new SelectList(alunos, nameof(Aluno.Id), nameof(Aluno.Nome));
        ViewBag.Turmas = new SelectList(turmas, nameof(Turma.Id), nameof(Turma.Nome));
    }
}
