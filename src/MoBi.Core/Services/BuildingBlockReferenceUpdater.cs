using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IBuildingBlockReferenceUpdater
   {
      /// <summary>
      ///    Ensures that all simulations are referencing template building blocks defined in the <paramref name="project" />.
      ///    This step is required for example after loading a project from file
      /// </summary>
      void UpdateTemplatesReferencesIn(MoBiProject project);

      /// <summary>
      ///    Ensures that the <paramref name="simulation"/> is referencing template building blocks defined in the <paramref name="project" />.
      ///    This step is required for example after loading a project from file
      /// </summary>
      void UpdateTemplatesReferencesIn(IMoBiSimulation simulation, MoBiProject project);
     
      /// <summary>
      ///    Updates the references to the <paramref name="templateBuildingBlock" /> in all simulations using the template.
      ///    This is required for instance when swapping two building blocks. References in simulation are out dated and should
      ///    be updated
      /// </summary>
      void UpdateTemplateReference(MoBiProject project, IBuildingBlock templateBuildingBlock);

   }  

   public class BuildingBlockReferenceUpdater : IBuildingBlockReferenceUpdater
   {
      public void UpdateTemplatesReferencesIn(MoBiProject project)
      {
         project.Simulations.Each(s => UpdateTemplatesReferencesIn(s, project));
      }

      public void UpdateTemplatesReferencesIn(IMoBiSimulation simulation, MoBiProject project)
      {
         //TODO SIMULATION_CONFIGURATION
         // UpdateTemplatesReferencesIn(simulation.MoBiBuildConfiguration, project);
      }

      public void UpdateTemplateReference(MoBiProject project, IBuildingBlock templateBuildingBlock)
      {
         //TODO SIMULATION_CONFIGURATION
         // foreach (var simulation in project.SimulationsCreatedUsing(templateBuildingBlock))
         // {
         //    var buildingBlockInfo = simulation.MoBiBuildConfiguration.BuildingInfoForTemplate(templateBuildingBlock);
         //    buildingBlockInfo.UntypedTemplateBuildingBlock = templateBuildingBlock;
         // }
      }
   }
}