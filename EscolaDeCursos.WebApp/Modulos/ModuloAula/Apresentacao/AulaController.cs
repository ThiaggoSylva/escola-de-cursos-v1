using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloAula.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAula.Apresentacao;

// Tela de "Visualização de Módulos" de um curso específico
// (entidade de domínio "Aula", correspondente ao "Módulo" da especificação).
public class AulaController(
    IAulaServico aulaServico,
    ICursoServico cursoServico,
    IMapper mapper
) : Controller
{
    [HttpGet]
    public IActionResult Listar(Guid cursoId)
    {
        Curso? curso = cursoServico.SelecionarPorId(cursoId);

        if (curso is null)
            return RedirectToAction("Listar", "Curso");

        List<Aula> aulas = aulaServico.SelecionarPorCurso(cursoId);

        ViewBag.Curso = curso;

        return View(mapper.Map<List<AulaListarViewModel>>(aulas));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar(Guid cursoId)
    {
        Curso? curso = cursoServico.SelecionarPorId(cursoId);

        if (curso is null)
            return RedirectToAction("Listar", "Curso");

        ViewBag.Curso = curso;

        return View(new AulaFormularioViewModel { CursoId = cursoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar(AulaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Curso = cursoServico.SelecionarPorId(viewModel.CursoId);
            return View(viewModel);
        }

        Aula aula = mapper.Map<Aula>(viewModel);

        Result<Aula> resultado = aulaServico.Cadastrar(aula);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            ViewBag.Curso = cursoServico.SelecionarPorId(viewModel.CursoId);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Módulo cadastrado com sucesso!";

        return RedirectToAction(nameof(Listar), new { cursoId = viewModel.CursoId });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id)
    {
        Aula? aula = aulaServico.SelecionarPorId(id);

        if (aula is null)
            return RedirectToAction("Listar", "Curso");

        AulaFormularioViewModel viewModel = mapper.Map<AulaFormularioViewModel>(aula);
        viewModel.Id = aula.Id;

        ViewBag.Curso = cursoServico.SelecionarPorId(aula.CursoId);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id, AulaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Curso = cursoServico.SelecionarPorId(viewModel.CursoId);
            return View(viewModel);
        }

        Aula aulaAtualizada = mapper.Map<Aula>(viewModel);

        Result<Aula> resultado = aulaServico.Editar(id, aulaAtualizada);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            ViewBag.Curso = cursoServico.SelecionarPorId(viewModel.CursoId);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Módulo atualizado com sucesso!";

        return RedirectToAction(nameof(Listar), new { cursoId = viewModel.CursoId });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Aula? aula = aulaServico.SelecionarPorId(id);

        if (aula is null)
            return RedirectToAction("Listar", "Curso");

        AulaExcluirViewModel viewModel = mapper.Map<AulaExcluirViewModel>(aula);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(AulaExcluirViewModel viewModel)
    {
        Result resultado = aulaServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);
        else
            TempData["MensagemSucesso"] = "Módulo excluído com sucesso!";

        return RedirectToAction(nameof(Listar), new { cursoId = viewModel.CursoId });
    }
}
