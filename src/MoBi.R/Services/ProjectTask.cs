using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Services;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;
using ISimulationFactory = MoBi.R.Services.ISimulationFactory;

namespace MoBi.R.Services
{
    public interface IProjectTask
   {
      MoBiProject GetProject(string fileName);
      IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetExpressionProfileNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(string moduleName);
      IReadOnlyList<Simulation> GetSimulations();
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

      public IReadOnlyList<string> GetExpressionProfileNames(MoBiProject moBiProject)
      {
         var expressionProfiles = moBiProject.ExpressionProfileCollection;
         if (expressionProfiles != null)
            return expressionProfiles.AllNames();

         return new List<string>();
      }

      public IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject) =>
         moBiProject.Modules.AllNames();

      public IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(string moduleName)
      {
         var module = _moBiContext.CurrentProject.ModuleByName(moduleName);
         if (module != null)
            return module.BuildingBlocks.AllNames();

         return new List<string>();
      }

      public MoBiProject GetProject(string fileName)
      {
         _contextPersistor.CloseProject(_moBiContext);
         _contextPersistor.Load(_moBiContext, fileName);
         return _moBiContext.CurrentProject;
      }

      public IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => x.Name).ToList();

      public IReadOnlyList<Simulation> GetSimulations() =>
         _moBiContext.CurrentProject.Simulations.Select(x => new Simulation(x)).ToList();
   }
}