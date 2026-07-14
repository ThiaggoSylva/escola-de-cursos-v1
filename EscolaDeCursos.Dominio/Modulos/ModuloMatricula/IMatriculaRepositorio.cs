using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

public interface IMatriculaRepositorio : IRepositorio<Matricula>
{
    bool AlunoJaMatriculadoNaTurma(Guid alunoId, Guid turmaId, Guid? ignorarId = null);
    int ContarMatriculasNaTurma(Guid turmaId);
    List<Matricula> SelecionarPorAluno(Guid alunoId);
    List<Matricula> SelecionarPorTurma(Guid turmaId);
}
