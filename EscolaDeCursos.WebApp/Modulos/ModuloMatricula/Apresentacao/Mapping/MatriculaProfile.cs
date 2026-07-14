using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao.Mapping;

public class MatriculaProfile : Profile
{
    public MatriculaProfile()
    {
        CreateMap<MatriculaFormularioViewModel, Matricula>();

        CreateMap<Matricula, MatriculaListarViewModel>()
            .ForMember(dest => dest.AlunoNome, opt => opt.MapFrom(src => src.Aluno != null ? src.Aluno.Nome : string.Empty))
            .ForMember(dest => dest.TurmaNome, opt => opt.MapFrom(src => src.Turma != null ? src.Turma.Nome : string.Empty))
            .ForMember(dest => dest.CursoTitulo, opt => opt.MapFrom(src => src.Turma != null && src.Turma.Curso != null ? src.Turma.Curso.Titulo : string.Empty));

        CreateMap<Matricula, MatriculaCancelarViewModel>()
            .ForMember(dest => dest.AlunoNome, opt => opt.MapFrom(src => src.Aluno != null ? src.Aluno.Nome : string.Empty))
            .ForMember(dest => dest.TurmaNome, opt => opt.MapFrom(src => src.Turma != null ? src.Turma.Nome : string.Empty));
    }
}
