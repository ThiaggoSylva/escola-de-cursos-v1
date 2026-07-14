using AutoMapper;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.WebApp.Modulos.ModuloCategoria.Apresentacao.ViewModels;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCategoria.Apresentacao.Mapping;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CategoriaFormularioViewModel, Categoria>();
        CreateMap<Categoria, CategoriaFormularioViewModel>();
        CreateMap<Categoria, CategoriaListarViewModel>();
        CreateMap<Categoria, CategoriaExcluirViewModel>();
    }
}
