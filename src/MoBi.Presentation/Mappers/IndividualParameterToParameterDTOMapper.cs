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
      private readonly IFormulaFactory _formulaFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public IndividualParameterToParameterDTOMapper(IParameterToParameterDTOMapper parameterToParameterDTOMapper, IParameterValueToParameterMapper parameterValueToParameterMapper, IFormulaFactory formulaFactory, ICloneManagerForBuildingBlock cloneManager)
      {
         _parameterToParameterDTOMapper = parameterToParameterDTOMapper;
         _parameterValueToParameterMapper = parameterValueToParameterMapper;
         _formulaFactory = formulaFactory;
         _cloneManager = cloneManager;
      }
      
      public ParameterDTO MapFrom(IndividualBuildingBlock individualBuildingBlock, IndividualParameter individualParameter)
      {
         var parameterDTO = _parameterToParameterDTOMapper.MapFrom(parameterForIndividualParameter(individualBuildingBlock, individualParameter));
         parameterDTO.IsIndividualPreview = true;
         return parameterDTO;
      }

      private IParameter parameterForIndividualParameter(IndividualBuildingBlock selectedIndividual, IndividualParameter individualParameter)
      {
         var parameter = _parameterValueToParameterMapper.MapFrom(individualParameter);
         if (parameter is IDistributedParameter distributedParameter)
            selectedIndividual.Where(x => isSubParameter(x, individualParameter)).Each(subParameter => distributedParameter.Add(parameterForIndividualParameter(selectedIndividual, subParameter)));
         else
         {
            if (individualParameter.Value.HasValue)
               parameter.Formula = _formulaFactory.ConstantFormula(individualParameter.Value.Value, individualParameter.Dimension);
            else if (individualParameter.Formula != null)
               parameter.Formula = _cloneManager.Clone(individualParameter.Formula);
         }
         return parameter;
      }

      private bool isSubParameter(IndividualParameter subParameter, IndividualParameter distributedParameter)
      {
         return subParameter.ContainerPath.Equals(distributedParameter.Path);
      }
   }
}