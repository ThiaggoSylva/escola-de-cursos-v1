using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursos.WebApp.Modulos.ModuloConta.Apresentacao.ViewModels;

// Cadastro público: sempre cria um usuário com perfil Aluno. Cadastro de
// Instrutores/Administradores é feito internamente pela equipe da escola
// (via tela administrativa), não pelo formulário público.
public class ContaRegistrarViewModel
{
    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    [Display(Name = "Confirmar senha")]
    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}

public class ContaLoginViewModel
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    [Display(Name = "Lembrar de mim")]
    public bool LembrarMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class ContaVerificarCodigoViewModel
{
    [Display(Name = "Código do aplicativo autenticador")]
    [Required(ErrorMessage = "Informe o código gerado pelo aplicativo autenticador.")]
    [StringLength(7, MinimumLength = 6, ErrorMessage = "Informe o código de 6 dígitos.")]
    public string Codigo { get; set; } = string.Empty;

    public bool LembrarMe { get; set; }

    [Display(Name = "Confiar neste dispositivo por 30 dias")]
    public bool LembrarDispositivo { get; set; }

    public string? ReturnUrl { get; set; }
}

public class ContaConfigurarAutenticadorViewModel
{
    // Chave secreta exibida para digitação manual no aplicativo autenticador
    public string ChaveManual { get; set; } = string.Empty;

    // QR Code (PNG em base64) gerado localmente com o segredo TOTP do usuário
    public string QrCodeBase64 { get; set; } = string.Empty;

    [Display(Name = "Código de verificação")]
    [Required(ErrorMessage = "Informe o código gerado pelo aplicativo autenticador para confirmar a configuração.")]
    [StringLength(7, MinimumLength = 6, ErrorMessage = "Informe o código de 6 dígitos.")]
    public string Codigo { get; set; } = string.Empty;
}
