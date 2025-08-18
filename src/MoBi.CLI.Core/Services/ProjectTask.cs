using System;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.R.Domain;
using OSPSuite.Utility;

namespace MoBi.CLI.Core.Services
{
   public interface IProjectTask
   {
      MoBiProject LoadProject(string fileName);

      string[] AllModuleNames(MoBiProject moBiProject);

      string[] AllExpressionProfileNames(MoBiProject moBiProject);

      string[] AllIndividualNames(MoBiProject moBiProject);

      string[] AllSimulationNames(MoBiProject moBiProject);

      Simulation SimulationByName(MoBiProject project, string simulationName);

      string[] AllBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName);

      Simulation[] AllSimulations(MoBiProject moBiProject);

      IndividualBuildingBlock[] AllIndividuals(MoBiProject moBiProject);

      InitialConditionsBuildingBlock[] AllInitialConditions(MoBiProject moBiProject);

      ParameterValuesBuildingBlock[] AllParameterValues(MoBiProject moBiProject);

      Module ModuleByName(MoBiProject moBiProject, string name);

      IndividualBuildingBlock IndividualBuildingBlockByName(MoBiProject moBiProject, string name);

      ExpressionProfileBuildingBlock[] ExpressionProfileBuildingBlocksByName(MoBiProject moBiProject, params string[] names);

      void CloseProject();

      DataRepository[] AllObservedDataSets(MoBiProject project);

      ParameterIdentification[] AllParameterIdentifications(MoBiProject project);

      string[] AllParameterIdentificationNames(MoBiProject project);
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

      public ParameterIdentification[] AllParameterIdentifications(MoBiProject project) =>
         project.AllParameterIdentifications.ToArray();

      public string[] AllParameterIdentificationNames(MoBiProject project) =>
         project.AllParameterIdentifications.AllNames().ToArray();

      public DataRepository[] AllObservedDataSets(MoBiProject project) =>
         project.AllObservedData.ToArray();

      public string[] AllExpressionProfileNames(MoBiProject moBiProject) =>
         moBiProject.ExpressionProfileCollection.AllNames().ToArray();

      public ExpressionProfileBuildingBlock[] ExpressionProfileBuildingBlocksByName(MoBiProject moBiProject, params string[] names) =>
         moBiProject.ExpressionProfileCollection.Where(p => names.Contains(p.Name)).ToArray();

      public Simulation SimulationByName(MoBiProject project, string simulationName)
      {
         var simulation = project.Simulations.FindByName(simulationName);
         return simulation == null ? null : new Simulation(simulation);
      }

      public string[] AllBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName) =>
         moBiProject.ModuleByName(moduleName).BuildingBlocks?.AllNames().ToArray();

      public MoBiProject LoadProject(string fileName)
      {
         if (!FileHelper.FileExists(fileName))
            throw new InvalidOperationException(Error.FileDoesNotExist(fileName));
         // Load the project from the file 
         _contextPersistor.CloseProject(_moBiContext);
         _contextPersistor.Load(_moBiContext, fileName);
         return _moBiContext.CurrentProject;
      }

      public void CloseProject() => _contextPersistor.CloseProject(_moBiContext);

      public string[] AllSimulationNames(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => x.Name).ToArray();

      public Simulation[] AllSimulations(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => new Simulation(x)).ToArray();

      public Module ModuleByName(MoBiProject moBiProject, string name) =>
         moBiProject.Modules.FindByName(name);

      public IndividualBuildingBlock IndividualBuildingBlockByName(MoBiProject moBiProject, string name) =>
         moBiProject.IndividualsCollection.FindByName(name);

      public string[] AllIndividualNames(MoBiProject moBiProject) =>
         moBiProject.IndividualsCollection.AllNames().ToArray();

      public IndividualBuildingBlock[] AllIndividuals(MoBiProject moBiProject) =>
         moBiProject.IndividualsCollection.ToArray();

      public InitialConditionsBuildingBlock[] AllInitialConditions(MoBiProject moBiProject) =>
         moBiProject.Modules.SelectMany(x => x.InitialConditionsCollection).ToArray();

      public ParameterValuesBuildingBlock[] AllParameterValues(MoBiProject moBiProject) =>
         moBiProject.Modules.SelectMany(x => x.ParameterValuesCollection).ToArray();

      public string[] AllModuleNames(MoBiProject moBiProject) => moBiProject.Modules.AllNames().ToArray();
   }
}