using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterToParameterDTOMapper : IMapper<IParameter, ParameterDTO>
   {
      ParameterDTO MapFrom(IParameter parameter, TrackableSimulation trackableSimulation);
   }

   public class ParameterToParameterDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IParameterToParameterDTOMapper, OSPSuite.Presentation.Mappers.IParameterToParameterDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaBuilderMapper;
      private readonly IGroupRepository _groupRepository;
      private readonly IFavoriteRepository _favoriteRepository;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IPathToPathElementsMapper _pathToPathElementsMapper;

      public ParameterToParameterDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaBuilderMapper,
         IGroupRepository groupRepository,
         IFavoriteRepository favoriteRepository,
         IEntityPathResolver entityPathResolver,
         IPathToPathElementsMapper pathToPathElementsMapper)
      {
         _formulaToDTOFormulaBuilderMapper = formulaToDTOFormulaBuilderMapper;
         _groupRepository = groupRepository;
         _favoriteRepository = favoriteRepository;
         _entityPathResolver = entityPathResolver;
         _pathToPathElementsMapper = pathToPathElementsMapper;
      }

      public ParameterDTO MapFrom(IParameter parameter)
      {
         var dto = new ParameterDTO(parameter);
         MapProperties(parameter, dto);
         dto.Formula = _formulaToDTOFormulaBuilderMapper.MapFrom(parameter.Formula);
         dto.RHSFormula = _formulaToDTOFormulaBuilderMapper.MapFrom(parameter.RHSFormula);
         dto.BuildMode = parameter.BuildMode;
         dto.Dimension = parameter.Dimension;
         dto.HasRHS = (parameter.RHSFormula != null);
         dto.DisplayUnit = parameter.Dimension.BaseUnit;
         dto.Group = _groupRepository.GroupByName(parameter.GroupName);
         dto.IsAdvancedParameter = !parameter.Visible;
         dto.CanBeVariedInPopulation = parameter.CanBeVariedInPopulation;
         dto.PathElements = _pathToPathElementsMapper.MapFrom(parameter);
         var parameterPath = _entityPathResolver.ObjectPathFor(parameter);
         dto.IsFavorite = _favoriteRepository.Contains(parameterPath);

         return dto;
      }

      public ParameterDTO MapFrom(IParameter parameter, TrackableSimulation trackableSimulation)
      {
         var parameterDTO = MapFrom(parameter);
         parameterDTO.SourceReference = trackableSimulation?.SourceFor(parameter);
         return parameterDTO;
      }

      IParameterDTO IMapper<IParameter, IParameterDTO>.MapFrom(IParameter input)
      {
         return MapFrom(input);
      }
   }
}