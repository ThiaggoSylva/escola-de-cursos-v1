namespace EscolaDeCursos.Dominio.Compartilhado.Identidade;

// Perfis de acesso da aplicação. Usados tanto como Roles do ASP.NET Identity
// quanto para orientar as regras de segregação de dados entre usuários.
public enum Perfil
{
    Administrador = 1,
    Instrutor = 2,
    Aluno = 3
}
