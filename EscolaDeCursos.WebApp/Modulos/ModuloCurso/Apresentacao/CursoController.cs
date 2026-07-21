using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloCurso.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCurso.Apresentacao;

public class CursoController(
    ICursoServico cursoServico,
    ICategoriaServico categoriaServico,
    IMapper mapper
) : Controller
{
    [HttpGet]
    public IActionResult Listar(Guid? categoriaId)
    {
        List<Curso> cursos = categoriaId is null
            ? cursoServico.SelecionarTodos()
            : cursoServico.SelecionarPorCategoria(categoriaId.Value);

        List<CursoListarViewModel> viewModel = mapper.Map<List<CursoListarViewModel>>(cursos);

        ViewBag.CategoriaId = categoriaId;
        ViewBag.CategoriaTitulo = categoriaId is null
            ? null
            : categoriaServico.SelecionarPorId(categoriaId.Value)?.Titulo;

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar()
    {
        CarregarCategorias();
        return View(new CursoFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar(CursoFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategorias();
            return View(viewModel);
        }

        Curso curso = mapper.Map<Curso>(viewModel);

        Result<Curso> resultado = cursoServico.Cadastrar(curso);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarCategorias();
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Curso cadastrado com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id)
    {
        Curso? curso = cursoServico.SelecionarPorId(id);

        if (curso is null)
            return RedirectToAction(nameof(Listar));

        CursoFormularioViewModel viewModel = mapper.Map<CursoFormularioViewModel>(curso);
        viewModel.Id = curso.Id;

        CarregarCategorias();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id, CursoFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategorias();
            return View(viewModel);
        }

        Curso cursoAtualizado = mapper.Map<Curso>(viewModel);

        Result<Curso> resultado = cursoServico.Editar(id, cursoAtualizado);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarCategorias();
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Curso atualizado com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Curso? curso = cursoServico.SelecionarPorId(id);

        if (curso is null)
            return RedirectToAction(nameof(Listar));

        CursoExcluirViewModel viewModel = mapper.Map<CursoExcluirViewModel>(curso);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(CursoExcluirViewModel viewModel)
    {
        Result resultado = cursoServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        TempData["MensagemSucesso"] = "Curso excluído com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarCategorias()
    {
        List<Categoria> categorias = categoriaServico.SelecionarTodos();

        ViewBag.Categorias = new SelectList(categorias, nameof(Categoria.Id), nameof(Categoria.Titulo));
    }
}
