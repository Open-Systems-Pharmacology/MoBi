using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Exceptions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public interface IDistributedParameterToDistributedParameterDTOMapper : IMapper<IDistributedParameter, DistributedParameterDTO>
   {
   }

   internal class DistributedParameterToDistributedParameterDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IDistributedParameterToDistributedParameterDTOMapper
   {
      private readonly IDistributionFormulaToDistributionFormulaTypeMaper _distributionFormulaToDistributedFormulaTypeMapper;
      private readonly IDimensionFactory _dimensionFactory;

      public DistributedParameterToDistributedParameterDTOMapper(IDistributionFormulaToDistributionFormulaTypeMaper distributionFormulaToDistributedFormulaTypeMapper, IDimensionFactory dimensionFactory)
      {
         _distributionFormulaToDistributedFormulaTypeMapper = distributionFormulaToDistributedFormulaTypeMapper;
         _dimensionFactory = dimensionFactory;
      }

      public DistributedParameterDTO MapFrom(IDistributedParameter distributedParameter)
      {
         var dto = new DistributedParameterDTO(distributedParameter);
         MapProperties(distributedParameter, dto);
         dto.Value = mapFrom(distributedParameter);
         dto.Percentile = distributedParameter.Percentile;
         dto.FormulaType = _distributionFormulaToDistributedFormulaTypeMapper.MapFrom(distributedParameter.Formula);
         dto.Mean = getValueForDistrubutionPropertyFromChild(Constants.Distribution.MEAN, distributedParameter);
         dto.Deviation = getValueForDistrubutionPropertyFromChild(Constants.Distribution.DEVIATION, distributedParameter);
         dto.Maximum = getValueForDistrubutionPropertyFromChild(Constants.Distribution.MAXIMUM, distributedParameter);
         dto.Minimum = getValueForDistrubutionPropertyFromChild(Constants.Distribution.MINIMUM, distributedParameter);
         dto.GeometricDeviation = getValueForDistrubutionPropertyFromChild(Constants.Distribution.GEOMETRIC_DEVIATION, distributedParameter);
         return dto;
      }

      private DistributionParameterDTO getValueForDistrubutionPropertyFromChild(string childName, IDistributedParameter distributedParameter)
      {
         var parameter = distributedParameter.GetSingleChildByName<IParameter>(childName);
         if (parameter == null)
            return new DistributionParameterDTO(dummyParameter()) {Dimension = _dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS)};

         return mapFrom(parameter);
      }

      private Parameter dummyParameter()
      {
         return new Parameter().WithFormula(new ConstantFormula(0)).WithDimension(_dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS));
      }

      private DistributionParameterDTO mapFrom(IParameter parameter)
      {
         var dto = new DistributionParameterDTO(parameter) {Dimension = parameter.Dimension};
         dto.DisplayUnit = parameter.DisplayUnit;
         return dto;
      }
   }

   internal interface IDistributionFormulaToDistributionFormulaTypeMaper : IMapper<IFormula, DistributionFormulaType>
   {
   }

   internal class DistributionFormulaToDistributionFormulaTypeMaper : IDistributionFormulaToDistributionFormulaTypeMaper
   {
      public DistributionFormulaType MapFrom(IFormula formula)
      {
         if (formula == null)
         {
            return DistributionFormulaType.DiscreteDistribution;
         }
         if (formula.IsAnImplementationOf<DiscreteDistributionFormula>() || formula.IsConstant())
         {
            return DistributionFormulaType.DiscreteDistribution;
         }
         if (formula.IsAnImplementationOf<UniformDistributionFormula>())
         {
            return DistributionFormulaType.UniformDistribution;
         }
         if (formula.IsAnImplementationOf<NormalDistributionFormula>())
         {
            return DistributionFormulaType.NormalDistribution;
         }
         if (formula.IsAnImplementationOf<LogNormalDistributionFormula>())
         {
            return DistributionFormulaType.LogNormalDistribution;
         }
         throw new MoBiException(AppConstants.Exceptions.UnknownDistributedFormula(formula.GetType()));
      }
   }
}