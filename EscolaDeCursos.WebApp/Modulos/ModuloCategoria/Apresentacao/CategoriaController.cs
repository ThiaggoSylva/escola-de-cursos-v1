using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloCategoria.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaController(
    ICategoriaServico categoriaServico,
    ICursoServico cursoServico,
    IMapper mapper
) : Controller
{
    [HttpGet]
    public IActionResult Listar()
    {
        List<Categoria> categorias = categoriaServico.SelecionarTodos();

        List<CategoriaListarViewModel> viewModel = categorias
            .Select(categoria =>
            {
                CategoriaListarViewModel item = mapper.Map<CategoriaListarViewModel>(categoria);
                item.QuantidadeCursos = cursoServico.SelecionarPorCategoria(categoria.Id).Count;
                return item;
            })
            .ToList();

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar()
    {
        return View(new CategoriaFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar(CategoriaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Categoria categoria = mapper.Map<Categoria>(viewModel);

        Result<Categoria> resultado = categoriaServico.Cadastrar(categoria);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Categoria cadastrada com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id)
    {
        Categoria? categoria = categoriaServico.SelecionarPorId(id);

        if (categoria is null)
            return RedirectToAction(nameof(Listar));

        CategoriaFormularioViewModel viewModel = mapper.Map<CategoriaFormularioViewModel>(categoria);
        viewModel.Id = categoria.Id;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Editar(Guid id, CategoriaFormularioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Categoria categoriaAtualizada = mapper.Map<Categoria>(viewModel);

        Result<Categoria> resultado = categoriaServico.Editar(id, categoriaAtualizada);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        TempData["MensagemSucesso"] = "Categoria atualizada com sucesso!";

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Categoria? categoria = categoriaServico.SelecionarPorId(id);

        if (categoria is null)
            return RedirectToAction(nameof(Listar));

        CategoriaExcluirViewModel viewModel = mapper.Map<CategoriaExcluirViewModel>(categoria);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(CategoriaExcluirViewModel viewModel)
    {
        Result resultado = categoriaServico.Excluir(viewModel.Id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        TempData["MensagemSucesso"] = "Categoria excluída com sucesso!";

        return RedirectToAction(nameof(Listar));
    }
}
