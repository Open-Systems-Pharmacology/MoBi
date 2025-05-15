using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public interface IProjectTask
   {
      MoBiProject GetProject(string fileName);
      IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName);
      IReadOnlyList<Simulation> GetSimulations(MoBiProject moBiProject);
   }

   public class ProjectTask : IProjectTask
   {
      private readonly IMoBiContext _moBiContext;
      private readonly IContextPersistor _contextPersistor;

      public ProjectTask(IMoBiContext moBiContext, IContextPersistor contextPersistor)
      {
         _moBiContext = moBiContext;
         _contextPersistor = contextPersistor;
      }

      public IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject) =>
         moBiProject.Modules.AllNames();

      public IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName)
      {
         var module = moBiProject.ModuleByName(moduleName);
         if (module != null)
            return module.BuildingBlocks.AllNames();

         return new List<string>();
      }

      public MoBiProject GetProject(string fileName)
      {
         // Load the project from the file 
         _contextPersistor.CloseProject(_moBiContext);
         _contextPersistor.Load(_moBiContext, fileName);
         return _moBiContext.CurrentProject;
      }

      public IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => x.Name).ToList();

      public IReadOnlyList<Simulation> GetSimulations(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x=> new Simulation(x)).ToList();
   }
}