using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.WebApp.Modulos.ModuloTurma.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloTurma.Apresentacao.Mapping;

public class TurmaProfile : Profile
{
    public TurmaProfile()
    {
        CreateMap<TurmaFormularioViewModel, Turma>();
        CreateMap<Turma, TurmaFormularioViewModel>();

        CreateMap<Turma, TurmaListarViewModel>()
            .ForMember(dest => dest.CursoTitulo, opt => opt.MapFrom(src => src.Curso != null ? src.Curso.Titulo : string.Empty))
            .ForMember(dest => dest.InstrutorNome, opt => opt.MapFrom(src => src.Instrutor != null ? src.Instrutor.Nome : string.Empty))
            .ForMember(dest => dest.MatriculasAtivas, opt => opt.Ignore());

        CreateMap<Turma, TurmaExcluirViewModel>();
    }
}
