using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using EscolaDeCursos.WebApp.Modulos.ModuloAula.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAula.Apresentacao.Mapping;

public class AulaProfile : Profile
{
    public AulaProfile()
    {
        CreateMap<AulaFormularioViewModel, Aula>();
        CreateMap<Aula, AulaFormularioViewModel>();
        CreateMap<Aula, AulaListarViewModel>();
        CreateMap<Aula, AulaExcluirViewModel>();
    }
}
