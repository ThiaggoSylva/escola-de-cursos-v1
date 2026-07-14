using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao.Mapping;

public class InstrutorProfile : Profile
{
    public InstrutorProfile()
    {
        CreateMap<InstrutorFormularioViewModel, Instrutor>();
        CreateMap<Instrutor, InstrutorFormularioViewModel>();
        CreateMap<Instrutor, InstrutorListarViewModel>();
        CreateMap<Instrutor, InstrutorExcluirViewModel>();
    }
}
