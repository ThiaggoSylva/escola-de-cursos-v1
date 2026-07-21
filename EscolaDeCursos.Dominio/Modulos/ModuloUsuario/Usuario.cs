using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using Microsoft.AspNetCore.Identity;

namespace EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

// Usuario estende o IdentityUser do ASP.NET Core Identity, responsável por
// autenticação (senha, e-mail, telefone, autenticação em dois fatores etc.).
//
// O vínculo com Aluno/Instrutor é o que sustenta a segregação de dados:
// a partir do usuário logado sabemos exatamente qual Aluno ou Instrutor
// ele enxerga, sem depender de nenhuma informação vinda do cliente.
public class Usuario : IdentityUser<Guid>
{
    public string Nome { get; set; } = string.Empty;

    public Perfil Perfil { get; set; } = Perfil.Aluno;

    public Guid? AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    public Guid? InstrutorId { get; set; }
    public Instrutor? Instrutor { get; set; }
}
