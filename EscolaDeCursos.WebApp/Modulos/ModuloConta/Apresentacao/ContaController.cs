using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using EscolaDeCursos.WebApp.Modulos.ModuloConta.Apresentacao.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace EscolaDeCursos.WebApp.Modulos.ModuloConta.Apresentacao;

// Módulo responsável por autenticação, cadastro de novos usuários (sempre
// como Aluno) e configuração da autenticação em dois fatores (MFA), que é
// obrigatória para todos os perfis de usuário da aplicação.
public class ContaController(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager,
    IAlunoServico alunoServico
) : Controller
{
    private const string NomeAplicacao = "EscolaDeCursos";

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registrar() => View(new ContaRegistrarViewModel());

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(ContaRegistrarViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        // Reaproveita as regras de negócio já existentes para o cadastro
        // de Aluno (CPF/e-mail/telefone únicos etc.).
        Aluno aluno = new()
        {
            Nome = viewModel.Nome,
            Email = viewModel.Email,
            Telefone = viewModel.Telefone,
            Cpf = viewModel.Cpf
        };

        Result<Aluno> resultadoAluno = alunoServico.Cadastrar(aluno);

        if (resultadoAluno.IsFailed)
        {
            foreach (IError erro in resultadoAluno.Errors)
                ModelState.AddModelError(string.Empty, erro.Message);

            return View(viewModel);
        }

        Usuario usuario = new()
        {
            UserName = viewModel.Email,
            Email = viewModel.Email,
            Nome = viewModel.Nome,
            Perfil = Perfil.Aluno,
            AlunoId = aluno.Id
        };

        IdentityResult resultadoUsuario = await userManager.CreateAsync(usuario, viewModel.Senha);

        if (!resultadoUsuario.Succeeded)
        {
            // Não deixa o Aluno órfão (sem usuário de acesso) caso a criação
            // do usuário falhe por algum motivo (ex.: e-mail já usado como login).
            alunoServico.Excluir(aluno.Id);

            foreach (IdentityError erro in resultadoUsuario.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(viewModel);
        }

        await userManager.AddToRoleAsync(usuario, nameof(Perfil.Aluno));
        await signInManager.SignInAsync(usuario, isPersistent: false);

        TempData["MensagemSucesso"] =
            "Cadastro realizado com sucesso! Configure a autenticação em dois fatores para continuar.";

        return RedirectToAction(nameof(ConfigurarAutenticador));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null) =>
        View(new ContaLoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(ContaLoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        SignInResult resultado = await signInManager.PasswordSignInAsync(
            viewModel.Email, viewModel.Senha, viewModel.LembrarMe, lockoutOnFailure: true);

        if (resultado.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(VerificarCodigo), new
            {
                lembrarMe = viewModel.LembrarMe,
                returnUrl = viewModel.ReturnUrl
            });
        }

        if (resultado.IsLockedOut)
        {
            ModelState.AddModelError(
                string.Empty,
                "Conta bloqueada temporariamente após várias tentativas inválidas. Tente novamente mais tarde.");

            return View(viewModel);
        }

        if (!resultado.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(viewModel);
        }

        return RedirecionarAposLogin(viewModel.ReturnUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult VerificarCodigo(bool lembrarMe, string? returnUrl = null) =>
        View(new ContaVerificarCodigoViewModel { LembrarMe = lembrarMe, ReturnUrl = returnUrl });

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerificarCodigo(ContaVerificarCodigoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        string codigo = NormalizarCodigo(viewModel.Codigo);

        SignInResult resultado = await signInManager.TwoFactorAuthenticatorSignInAsync(
            codigo, viewModel.LembrarMe, rememberClient: viewModel.LembrarDispositivo);

        if (resultado.IsLockedOut)
        {
            ModelState.AddModelError(
                string.Empty, "Conta bloqueada temporariamente após várias tentativas inválidas.");

            return View(viewModel);
        }

        if (!resultado.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Código de verificação inválido.");
            return View(viewModel);
        }

        return RedirecionarAposLogin(viewModel.ReturnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> ConfigurarAutenticador()
    {
        Usuario? usuario = await userManager.GetUserAsync(User);

        if (usuario is null)
            return RedirectToAction(nameof(Login));

        if (usuario.TwoFactorEnabled)
        {
            TempData["MensagemSucesso"] = "A autenticação em dois fatores já está ativa na sua conta.";
            return RedirectToAction("Index", "Home");
        }

        return View(await MontarViewModelAutenticadorAsync(usuario));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfigurarAutenticador(ContaConfigurarAutenticadorViewModel viewModel)
    {
        Usuario? usuario = await userManager.GetUserAsync(User);

        if (usuario is null)
            return RedirectToAction(nameof(Login));

        if (!ModelState.IsValid)
        {
            ContaConfigurarAutenticadorViewModel recarregado = await MontarViewModelAutenticadorAsync(usuario);
            return View(recarregado);
        }

        string codigo = NormalizarCodigo(viewModel.Codigo);

        bool codigoValido = await userManager.VerifyTwoFactorTokenAsync(
            usuario, userManager.Options.Tokens.AuthenticatorTokenProvider, codigo);

        if (!codigoValido)
        {
            ModelState.AddModelError(
                nameof(viewModel.Codigo),
                "Código inválido. Verifique o horário do celular e o aplicativo autenticador e tente novamente.");

            ContaConfigurarAutenticadorViewModel recarregado = await MontarViewModelAutenticadorAsync(usuario);
            return View(recarregado);
        }

        await userManager.SetTwoFactorEnabledAsync(usuario, true);

        IEnumerable<string> codigosRecuperacao =
            await userManager.GenerateNewTwoFactorRecoveryCodesAsync(usuario, 8);

        // Atualiza as claims do cookie atual para refletir MfaConfigurado = true,
        // liberando o acesso ao restante da aplicação.
        await signInManager.RefreshSignInAsync(usuario);

        return View("CodigosRecuperacao", codigosRecuperacao.ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AcessoNegado() => View();

    private IActionResult RedirecionarAposLogin(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    private async Task<ContaConfigurarAutenticadorViewModel> MontarViewModelAutenticadorAsync(Usuario usuario)
    {
        string? chave = await userManager.GetAuthenticatorKeyAsync(usuario);

        if (string.IsNullOrEmpty(chave))
        {
            await userManager.ResetAuthenticatorKeyAsync(usuario);
            chave = await userManager.GetAuthenticatorKeyAsync(usuario);
        }

        string uriAutenticador = GerarUriAutenticador(usuario.Email!, chave!);

        return new ContaConfigurarAutenticadorViewModel
        {
            ChaveManual = FormatarChave(chave!),
            QrCodeBase64 = GerarQrCodeBase64(uriAutenticador)
        };
    }

    private static string NormalizarCodigo(string codigo) =>
        codigo.Replace(" ", string.Empty).Replace("-", string.Empty);

    private static string FormatarChave(string chave)
    {
        int totalGrupos = (int)Math.Ceiling(chave.Length / 4.0);

        IEnumerable<string> grupos = Enumerable.Range(0, totalGrupos)
            .Select(i => chave.Substring(i * 4, Math.Min(4, chave.Length - i * 4)));

        return string.Join(" ", grupos);
    }

    private static string GerarUriAutenticador(string email, string chave) =>
        $"otpauth://totp/{Uri.EscapeDataString(NomeAplicacao)}:{Uri.EscapeDataString(email)}" +
        $"?secret={chave}&issuer={Uri.EscapeDataString(NomeAplicacao)}&digits=6";

    private static string GerarQrCodeBase64(string texto)
    {
        using QRCodeGenerator geradorQr = new();
        using QRCodeData dadosQr = geradorQr.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCodePng = new(dadosQr);
        byte[] bytes = qrCodePng.GetGraphic(10);
        return Convert.ToBase64String(bytes);
    }
}
