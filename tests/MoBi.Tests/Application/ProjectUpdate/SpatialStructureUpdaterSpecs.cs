using System;
using MoBi.Core.Serialization.Converter.v3_1;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Application.ProjectUpdate
{
   public abstract class concern_for_SpatialStructureUpdaterSpecs : ContextSpecification<ISpatialStructureUpdaterTo3_1_3>
   {
      protected override void Context()
      {
         sut = new SpatialStructureUpdaterTo3_1_3();
      }
   }

   class When_updating_a_distributed_parameter_with_a_Log_normal_distribution : concern_for_SpatialStructureUpdaterSpecs
   {
      private IDistributedParameter _distributedParameter;
      private double _geometricDeviation = 1;
      private Parameter _geoSDParameter;

      protected override void Context()
      {
         base.Context();
         _distributedParameter = new DistributedParameter().WithName("DP").WithFormula(new LogNormalDistributionFormula());
         _distributedParameter.Add(
            new Parameter().WithName(Constants.Distribution.MEAN).WithFormula(new ConstantFormula().WithValue(2)));
         _geoSDParameter = new Parameter().WithName(Constants.Distribution.GEOMETRIC_DEVIATION).WithFormula(new ConstantFormula().WithValue(_geometricDeviation));
         _distributedParameter.Add(_geoSDParameter);

      }

      protected override void Because()
      {
         sut.UpdateDistributedParameterTo3_1_3(_distributedParameter);
      }

      [Observation]
      public void should_change_Geometric_Deviation_to_its_exp_value()
      {
         _geoSDParameter.Value.ShouldBeEqualTo(Math.Exp(_geometricDeviation));
      }
   }
}	