using System;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Serialization.Converter.v3_1
{
   public interface ISpatialStructureUpdaterTo3_1_3
   {
      void UpdateSpatialStructre(IMoBiSpatialStructure spatialStructure);
      void UpdateDistributedParameterTo3_1_3(IDistributedParameter distributedParameter);
      void UpdateContainerTo3_1_3(IContainer container);
   }

   internal class SpatialStructureUpdaterTo3_1_3 : ISpatialStructureUpdaterTo3_1_3
   {
      public void UpdateSpatialStructre(IMoBiSpatialStructure spatialStructure)
      {
         foreach (var topContainer in spatialStructure.TopContainers)
         {
            UpdateContainerTo3_1_3(topContainer);
         }
      }

      public void UpdateContainerTo3_1_3(IContainer container)
      {
         container.GetAllChildren<IDistributedParameter>(
            dp => dp.Formula.IsAnImplementationOf<LogNormalDistributionFormula>())
            .Each(UpdateDistributedParameterTo3_1_3);
      }

      public void UpdateDistributedParameterTo3_1_3(IDistributedParameter distributedParameter)
      {
         var geoSDParameter = distributedParameter.GetSingleChildByName<IParameter>(Constants.Distribution.GEOMETRIC_DEVIATION);
         var constantFormula = ((ConstantFormula) geoSDParameter.Formula);
         constantFormula.Value = Math.Exp(constantFormula.Value);
      }
   }
}