using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.WebApp.Modulos.ModuloCurso.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCurso.Apresentacao.Mapping;

public class CursoProfile : Profile
{
    public CursoProfile()
    {
        CreateMap<CursoFormularioViewModel, Curso>();

        CreateMap<Curso, CursoFormularioViewModel>();

        CreateMap<Curso, CursoListarViewModel>()
            .ForMember(dest => dest.CategoriaTitulo,
                opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Titulo : string.Empty));

        CreateMap<Curso, CursoExcluirViewModel>();
    }
}
