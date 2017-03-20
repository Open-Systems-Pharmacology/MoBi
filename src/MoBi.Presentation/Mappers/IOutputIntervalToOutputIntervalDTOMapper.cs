using System;
using System.Linq.Expressions;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.Mappers
{
   public interface IOutputIntervalToOutputIntervalDTOMapper : IMapper<OutputInterval, OutputIntervalDTO>
   {
   }

   public class OutputIntervalToOutputIntervalDTOMapper : IOutputIntervalToOutputIntervalDTOMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterMapper;

      public OutputIntervalToOutputIntervalDTOMapper(IParameterToParameterDTOMapper parameterMapper)
      {
         _parameterMapper = parameterMapper;
      }

      public OutputIntervalDTO MapFrom(OutputInterval outputInterval)
      {
         var outputIntervalDTO = new OutputIntervalDTO {OutputInterval = outputInterval};

         outputIntervalDTO.StartTimeParameter = MapFrom(outputInterval.StartTime, outputIntervalDTO, x => x.StartTime, x => x.StartTimeParameter);
         outputIntervalDTO.EndTimeParameter = MapFrom(outputInterval.EndTime, outputIntervalDTO, x => x.EndTime, x => x.EndTimeParameter);
         outputIntervalDTO.ResolutionParameter = MapFrom(outputInterval.Resolution, outputIntervalDTO, x => x.Resolution, x => x.ResolutionParameter);
         return outputIntervalDTO;
      }

      public ParameterDTO MapFrom<TPropertyType, TParameterContainerDTO>(IParameter parameter, TParameterContainerDTO parameterContainerDTO,
         Expression<Func<TParameterContainerDTO, TPropertyType>> parameterValue,
         Func<TParameterContainerDTO, IParameterDTO> parameterDelegate) where TParameterContainerDTO : IValidatableDTO
      {
         var parameterDTO = _parameterMapper.MapFrom(parameter).DowncastTo<ParameterDTO>();
         parameterDTO.ValueChanged += (o, e) => parameterContainerDTO.AddNotifiableFor(parameterValue);
         parameterContainerDTO.Rules.Add(ParameterDTORules.ParameterIsValid(parameterValue, parameterDelegate));
         return parameterDTO;
      }
   }
}