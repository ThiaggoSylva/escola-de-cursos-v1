using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAluno.Apresentacao.Mapping;

public class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<AlunoFormularioViewModel, Aluno>();
        CreateMap<Aluno, AlunoFormularioViewModel>();
        CreateMap<Aluno, AlunoListarViewModel>();
        CreateMap<Aluno, AlunoExcluirViewModel>();
    }
}
