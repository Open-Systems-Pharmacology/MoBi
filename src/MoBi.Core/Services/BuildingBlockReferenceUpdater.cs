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
      void UpdateTemplatesReferencesIn(IMoBiProject project);

      /// <summary>
      ///    Ensures that the <paramref name="simulation"/> is referencing template building blocks defined in the <paramref name="project" />.
      ///    This step is required for example after loading a project from file
      /// </summary>
      void UpdateTemplatesReferencesIn(IMoBiSimulation simulation, IMoBiProject project);

      /// <summary>
      ///    Ensures that the <paramref name="buildConfiguration"/> is referencing template building blocks defined in the <paramref name="project" />.
      ///    This step is required for example after loading a project from file
      /// </summary>
      void UpdateTemplatesReferencesIn(IMoBiBuildConfiguration buildConfiguration, IMoBiProject project);
     
      /// <summary>
      ///    Updates the references to the <paramref name="templateBuildingBlock" /> in all simulations using the template.
      ///    This is required for instance when swapping two building blocks. References in simulation are out dated and should
      ///    be updated
      /// </summary>
      void UpdateTemplateReference(IMoBiProject project, IBuildingBlock templateBuildingBlock);

   }  

   public class BuildingBlockReferenceUpdater : IBuildingBlockReferenceUpdater
   {
      public void UpdateTemplatesReferencesIn(IMoBiProject project)
      {
         project.Simulations.Each(s => UpdateTemplatesReferencesIn(s, project));
      }

      public void UpdateTemplatesReferencesIn(IMoBiSimulation simulation, IMoBiProject project)
      {
         //TODO
         // UpdateTemplatesReferencesIn(simulation.MoBiBuildConfiguration, project);
      }

      public void UpdateTemplatesReferencesIn(IMoBiBuildConfiguration buildConfiguration, IMoBiProject project)
      {
         foreach (var buildingBlockInfo in buildConfiguration.AllBuildingBlockInfos())
         {
            buildingBlockInfo.UntypedTemplateBuildingBlock = project.TemplateById(buildingBlockInfo.TemplateBuildingBlockId);
         }
      }

      public void UpdateTemplateReference(IMoBiProject project, IBuildingBlock templateBuildingBlock)
      {
         //TODO
         // foreach (var simulation in project.SimulationsCreatedUsing(templateBuildingBlock))
         // {
         //    var buildingBlockInfo = simulation.MoBiBuildConfiguration.BuildingInfoForTemplate(templateBuildingBlock);
         //    buildingBlockInfo.UntypedTemplateBuildingBlock = templateBuildingBlock;
         // }
      }
   }
}