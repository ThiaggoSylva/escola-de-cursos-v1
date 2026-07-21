using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao;

// A lista completa de Alunos e o cadastro/exclusão são restritos à equipe da
// escola (Administrador/Instrutor). Um usuário Aluno só pode visualizar e
// editar o seu próprio registro, nunca o de outro aluno (segregação de dados).
//
// Observação: [Authorize] de classe e de ação são combinados com E (o usuário
// precisa satisfazer os dois), por isso as roles são declaradas por ação,
// e não na classe, para permitir que o Aluno edite apenas o próprio registro.
public class AlunoController(IAlunoServico alunoServico, IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Administrador,Instrutor")]
    public IActionResult Listar()
    {
        List<Aluno> alunos = alunoServico.SelecionarTodos();
        return View(mapper.Map<List<AlunoListarViewModel>>(alunos));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Cadastrar() => View(new AlunoFormularioViewModel());

    [HttpPost]
    [Authorize(Roles = nameof(Perfil.Administrador))]
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
    [Authorize(Roles = "Administrador,Instrutor,Aluno")]
    public IActionResult Editar(Guid id)
    {
        if (!PodeAcessar(id))
            return Forbid();

        Aluno? aluno = alunoServico.SelecionarPorId(id);

        if (aluno is null)
            return RedirectToAction(nameof(Listar));

        AlunoFormularioViewModel viewModel = mapper.Map<AlunoFormularioViewModel>(aluno);
        viewModel.Id = aluno.Id;

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Instrutor,Aluno")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, AlunoFormularioViewModel viewModel)
    {
        if (!PodeAcessar(id))
            return Forbid();

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
    [Authorize(Roles = nameof(Perfil.Administrador))]
    public IActionResult Excluir(Guid id)
    {
        Aluno? aluno = alunoServico.SelecionarPorId(id);

        if (aluno is null)
            return RedirectToAction(nameof(Listar));

        return View(mapper.Map<AlunoExcluirViewModel>(aluno));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Perfil.Administrador))]
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

    // Um Aluno só pode mexer no próprio registro; Administrador e Instrutor
    // (papéis de gestão) podem acessar qualquer um.
    private bool PodeAcessar(Guid alunoId)
    {
        if (User.EhAdministrador() || User.EhInstrutor())
            return true;

        return User.ObterAlunoId() == alunoId;
    }
}
