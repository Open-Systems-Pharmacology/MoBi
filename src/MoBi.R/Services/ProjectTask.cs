using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using OSPSuite.Core.Domain;
using MoBiSimulation = MoBi.R.Domain.MoBiSimulation;
using CLIProjectTask = MoBi.CLI.Core.Services.ProjectTask;

namespace MoBi.R.Services
{
   public interface IProjectTask : CLI.Core.Services.IProjectTask
   {
      MoBiSimulation SimulationByName(MoBiProject project, string simulationName);
      
      MoBiSimulation[] AllSimulations(MoBiProject moBiProject);
   }

   public class ProjectTask : CLIProjectTask, IProjectTask
   {
      public ProjectTask(IMoBiContext moBiContext, IContextPersistor contextPersistor) : base(moBiContext, contextPersistor)
      {
      }

      public MoBiSimulation SimulationByName(MoBiProject project, string simulationName)
      {
         var simulation = project.Simulations.FindByName(simulationName);
         return simulation == null ? null : new MoBiSimulation(simulation);
      }

      public MoBiSimulation[] AllSimulations(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => new MoBiSimulation(x)).ToArray();
   }
}