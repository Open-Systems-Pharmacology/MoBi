using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Serialization.Converter.v3_1
{
   public class Converter30To313 : IMoBiObjectConverter
   {
      private readonly IParameterStartValuesUpdater _parameterStartValuesUpdater;
      private readonly ISpatialStructureUpdaterTo3_1_3 _spatialStructureUpdater;
      private readonly IModelUpdaterTo3_1_3 _modelUpdater;

      public Converter30To313(IParameterStartValuesUpdater parameterStartValuesUpdater, ISpatialStructureUpdaterTo3_1_3 spatialStructureUpdater, IModelUpdaterTo3_1_3 modelUpdater)
      {
         _parameterStartValuesUpdater = parameterStartValuesUpdater;
         _spatialStructureUpdater = spatialStructureUpdater;
         _modelUpdater = modelUpdater;
      }

      private void updateSimulationTo313(IMoBiSimulation sim)
      {
         _modelUpdater.UpdateModel(sim.Model);
         _spatialStructureUpdater.UpdateSpatialStructre((IMoBiSpatialStructure) sim.MoBiBuildConfiguration.SpatialStructure);
         _parameterStartValuesUpdater.UpdateParameterStartvalues(sim.MoBiBuildConfiguration.ParameterStartValues, sim);
         sim.HasChanged = true;
      }

      public int Convert(object objectToUpdate, IMoBiProject project)
      {
         if (objectToUpdate.IsAnImplementationOf<IParameterStartValuesBuildingBlock>())
         {
            _parameterStartValuesUpdater.UpdateParameterStartvalues((IParameterStartValuesBuildingBlock) objectToUpdate, project);
         }
         else if (objectToUpdate.IsAnImplementationOf<IParameterStartValue>())
         {
            _parameterStartValuesUpdater.UpdateParameterStartValue((IParameterStartValue) objectToUpdate, project);
         }
         else if (objectToUpdate.IsAnImplementationOf<IMoBiSimulation>())
         {
            updateSimulationTo313((IMoBiSimulation) objectToUpdate);
         }
         else if (objectToUpdate.IsAnImplementationOf<IMoBiSpatialStructure>())
         {
            _spatialStructureUpdater.UpdateSpatialStructre((IMoBiSpatialStructure) objectToUpdate);
         }
         else if (objectToUpdate.IsAnImplementationOf<IContainer>())
         {
            _spatialStructureUpdater.UpdateContainerTo3_1_3((IContainer) objectToUpdate);
         }
         else if (objectToUpdate.IsAnImplementationOf<IMoBiBuildConfiguration>())
         {
            var buidlConfiguratuion = (IMoBiBuildConfiguration) objectToUpdate;
            _spatialStructureUpdater.UpdateSpatialStructre((IMoBiSpatialStructure) buidlConfiguratuion.SpatialStructure);
            _parameterStartValuesUpdater.UpdateParameterStartvalues(buidlConfiguratuion.ParameterStartValues, project);
         }
         else if (objectToUpdate.IsAnImplementationOf<IDistributedParameter>())
         {
            var distributedParameter = (IDistributedParameter) objectToUpdate;
            if (distributedParameter.Formula.IsAnImplementationOf<LogNormalDistributionFormula>())
            {
               _spatialStructureUpdater.UpdateDistributedParameterTo3_1_3(distributedParameter);
            }
         }


         return ProjectVersions.V3_1_3;
      }

      public int ConvertXml(XElement element, IMoBiProject project)
      {
         return ProjectVersions.V3_1_3;
      }

      public  bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V3_0_1_to_3;
      }
   }
}