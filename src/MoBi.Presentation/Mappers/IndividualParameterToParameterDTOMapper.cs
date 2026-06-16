using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IIndividualParameterToParameterDTOMapper
   {
      ParameterDTO MapFrom(IndividualBuildingBlock individualBuildingBlock, IndividualParameter individualParameter);
   }
   
   public class IndividualParameterToParameterDTOMapper : IIndividualParameterToParameterDTOMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterToParameterDTOMapper;
      private readonly IParameterValueToParameterMapper _parameterValueToParameterMapper;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public IndividualParameterToParameterDTOMapper(IParameterToParameterDTOMapper parameterToParameterDTOMapper, IParameterValueToParameterMapper parameterValueToParameterMapper, ICloneManagerForBuildingBlock cloneManager)
      {
         _parameterToParameterDTOMapper = parameterToParameterDTOMapper;
         _parameterValueToParameterMapper = parameterValueToParameterMapper;
         _cloneManager = cloneManager;
      }
      
      public ParameterDTO MapFrom(IndividualBuildingBlock individualBuildingBlock, IndividualParameter individualParameter)
      {
         var parameter = parameterForIndividualParameter(individualBuildingBlock, individualParameter);
         var parameterDTO = _parameterToParameterDTOMapper.MapFrom(parameter);
         parameterDTO.IsIndividualPreview = true;
         return parameterDTO;
      }

      private IParameter parameterForIndividualParameter(IndividualBuildingBlock selectedIndividual, IndividualParameter individualParameter)
      {
         //the mapper already takes care of applying the value (constant formula for plain parameters,
         //fixed value via the distributed parameter Value setter for distributed ones).
         var parameter = _parameterValueToParameterMapper.MapFrom(individualParameter);
         if (parameter is IDistributedParameter distributedParameter)
         {
            //the factory pre-creates the sub-parameters referenced by the distribution formula plus a Percentile.
            //If the individual provides matching sub-parameters, update the existing entries so we don't end up
            //with duplicate children (which would throw a NotUniqueNameException).
            var subParameters = selectedIndividual.Where(x => isSubParameter(x, individualParameter));
            subParameters.Each(subParameter =>
            {
               var existing = distributedParameter.GetSingleChildByName<IParameter>(subParameter.Name);
               if (existing != null)
               {
                  if (subParameter.Value.HasValue)
                     existing.Value = subParameter.Value.Value;
                  return;
               }
               distributedParameter.Add(parameterForIndividualParameter(selectedIndividual, subParameter));
            });
         }
         else if (individualParameter.Formula != null && !individualParameter.Value.HasValue)
         {
            //only clone an explicit formula here when no value was provided, because a value would already
            //have been turned into a constant formula by the mapper above and would otherwise be overwritten.
            parameter.Formula = _cloneManager.Clone(individualParameter.Formula, new FormulaCache());
         }
         return parameter;
      }

      private bool isSubParameter(IndividualParameter subParameter, IndividualParameter distributedParameter)
      {
         return subParameter.ContainerPath.Equals(distributedParameter.Path);
      }
   }
}