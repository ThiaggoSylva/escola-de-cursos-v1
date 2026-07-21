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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao;

// Núcleo da segregação de dados de matrículas: o Id de Aluno/Instrutor usado
// para filtrar NUNCA vem de query string/formulário quando o usuário logado
// é Aluno ou Instrutor — vem sempre da claim gravada no login (ver
// ClaimsPrincipalExtensions), então um Aluno jamais consegue ver ou mexer na
// matrícula de outro Aluno trocando o parâmetro da URL.
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

        if (User.EhAluno())
        {
            // Aluno só enxerga as próprias matrículas, independentemente do
            // que vier na query string.
            Guid meuAlunoId = User.ObterAlunoId() ?? Guid.Empty;
            matriculas = matriculaServico.SelecionarPorAluno(meuAlunoId);
            alunoId = meuAlunoId;
            turmaId = null;
        }
        else if (User.EhInstrutor())
        {
            // Instrutor só enxerga matrículas das turmas que ele leciona.
            Guid meuInstrutorId = User.ObterInstrutorId() ?? Guid.Empty;

            List<Guid> minhasTurmas = turmaServico.SelecionarTodos()
                .Where(t => t.InstrutorId == meuInstrutorId)
                .Select(t => t.Id)
                .ToList();

            matriculas = turmaId is not null && minhasTurmas.Contains(turmaId.Value)
                ? matriculaServico.SelecionarPorTurma(turmaId.Value)
                : minhasTurmas.SelectMany(matriculaServico.SelecionarPorTurma).ToList();

            alunoId = null;
        }
        else if (alunoId is not null)
        {
            matriculas = matriculaServico.SelecionarPorAluno(alunoId.Value);
        }
        else if (turmaId is not null)
        {
            matriculas = matriculaServico.SelecionarPorTurma(turmaId.Value);
        }
        else
        {
            matriculas = matriculaServico.SelecionarTodos();
        }

        ViewBag.AlunoId = alunoId;
        ViewBag.TurmaId = turmaId;
        ViewBag.Aluno = alunoId is null ? null : alunoServico.SelecionarPorId(alunoId.Value);
        ViewBag.Turma = turmaId is null ? null : turmaServico.SelecionarPorId(turmaId.Value);

        return View(mapper.Map<List<MatriculaListarViewModel>>(matriculas));
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Aluno")]
    public IActionResult Matricular(Guid? turmaId)
    {
        CarregarListasSelecao();

        MatriculaFormularioViewModel viewModel = new() { TurmaId = turmaId ?? Guid.Empty };

        // Aluno só pode matricular a si mesmo: o campo de aluno nem é
        // exibido no formulário (ver View), o valor é sempre o da claim.
        if (User.EhAluno())
            viewModel.AlunoId = User.ObterAlunoId() ?? Guid.Empty;

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Aluno")]
    [ValidateAntiForgeryToken]
    public IActionResult Matricular(MatriculaFormularioViewModel viewModel)
    {
        // Mesmo que o formulário seja adulterado, força o AlunoId da claim
        // quando quem está matriculando é um Aluno.
        if (User.EhAluno())
            viewModel.AlunoId = User.ObterAlunoId() ?? Guid.Empty;

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
    [Authorize(Roles = "Administrador,Aluno")]
    public IActionResult Cancelar(Guid id)
    {
        Matricula? matricula = matriculaServico.SelecionarPorId(id);

        if (matricula is null)
            return RedirectToAction(nameof(Listar));

        if (!PodeAcessar(matricula))
            return Forbid();

        return View(mapper.Map<MatriculaCancelarViewModel>(matricula));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Aluno")]
    [ValidateAntiForgeryToken]
    public IActionResult Cancelar(MatriculaCancelarViewModel viewModel)
    {
        Matricula? matricula = matriculaServico.SelecionarPorId(viewModel.Id);

        if (matricula is null)
            return RedirectToAction(nameof(Listar));

        if (!PodeAcessar(matricula))
            return Forbid();

        Result resultado = matriculaServico.Cancelar(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Matrícula cancelada com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    // Administrador acessa qualquer matrícula; Aluno só a própria.
    private bool PodeAcessar(Matricula matricula)
    {
        if (User.EhAdministrador())
            return true;

        return User.EhAluno() && User.ObterAlunoId() == matricula.AlunoId;
    }

    private void CarregarListasSelecao()
    {
        List<Turma> turmas = turmaServico.SelecionarTodos();
        ViewBag.Turmas = new SelectList(turmas, nameof(Turma.Id), nameof(Turma.Nome));

        // O seletor de alunos só faz sentido para quem pode matricular
        // terceiros (Administrador); um Aluno matricula apenas a si mesmo.
        if (User.EhAdministrador())
        {
            List<Aluno> alunos = alunoServico.SelecionarTodos();
            ViewBag.Alunos = new SelectList(alunos, nameof(Aluno.Id), nameof(Aluno.Nome));
        }
    }
}
