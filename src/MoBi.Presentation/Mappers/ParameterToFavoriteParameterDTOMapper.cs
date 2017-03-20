using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterToFavoriteParameterDTOMapper : IMapper<IParameter, FavoriteParameterDTO>
   {
   }

   internal class ParameterToFavoriteParameterDTOMapper : IParameterToFavoriteParameterDTOMapper
   {
      private readonly IPathToPathElementsMapper _pathToPathElementsMapper;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaBuilderMapper;

      public ParameterToFavoriteParameterDTOMapper(IPathToPathElementsMapper pathToPathElementsMapper, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaBuilderMapper)
      {
         _pathToPathElementsMapper = pathToPathElementsMapper;
         _formulaToDTOFormulaBuilderMapper = formulaToDTOFormulaBuilderMapper;
      }

      public FavoriteParameterDTO MapFrom(IParameter parameter)
      {
         return new FavoriteParameterDTO(parameter)
         {
            IsFavorite = true,
            DisplayUnit = parameter.DisplayUnit,
            Dimension = parameter.Dimension,
            PathElements = _pathToPathElementsMapper.MapFrom(parameter),
            Formula = _formulaToDTOFormulaBuilderMapper.MapFrom(parameter.Formula),
            RHSFormula = _formulaToDTOFormulaBuilderMapper.MapFrom(parameter.RHSFormula)
         };

      }
   }
}