using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Engine.Sbml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;
using System;
using System.Linq;

namespace MoBi.Core.SBML
{
   public class SBMLTaskSpecs : ContextForSBMLIntegration<SbmlTask>
   {
      protected ISimulationFactory _simulationFactory;
      protected IModelConstructor _modelConstructor;
      protected ISimulationSettingsFactory _simulationSettingsFactory;
      protected ObserverBuilder _observerBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationFactory = IoC.Resolve<ISimulationFactory>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simulationSettingsFactory = IoC.Resolve<ISimulationSettingsFactory>();
         _observerBuilder = IoC.Resolve<ObserverBuilder>();
         _fileName = Helper.TestFileFullPath("tiny_example_12.xml");
      }

      [Observation]
      public void FormulaParameter_InitialAssignmentCreationTest()
      {
         var simulation = _simulationFactory.Create();
         var moduleConfiguration = new ModuleConfiguration(new Module
         {
            IoC.Resolve<IMoBiContext>().Create<ObserverBuildingBlock>(),
            SBMLModule.SpatialStructure,
            SBMLModule.Reactions,
            SBMLModule.Molecules,
            SBMLModule.PassiveTransports,
            SBMLModule.EventGroups

         });

         var parameterStartValuesBuildingBlock = SBMLModule.ParameterStartValuesCollection.FirstOrDefault();
         var moleculeStartValuesBuildingBlock = SBMLModule.InitialConditionsCollection.FirstOrDefault();
         
         moduleConfiguration.Module.Add(parameterStartValuesBuildingBlock);
         moduleConfiguration.Module.Add(moleculeStartValuesBuildingBlock);

         moduleConfiguration.SelectedInitialConditions = moleculeStartValuesBuildingBlock;
         moduleConfiguration.SelectedParameterStartValues = parameterStartValuesBuildingBlock;

         simulation.Configuration.AddModuleConfiguration(moduleConfiguration);

         simulation.Configuration.SimulationSettings = _simulationSettingsFactory.CreateDefault();
         
         // var buildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration);
         var name = Guid.NewGuid().ToString();
         var result = _modelConstructor.CreateModelFrom(simulation.Configuration, name);
         result.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_translate_new_units_properly()
      {
         var msv = SBMLModule.InitialConditionsCollection.First();
         var glucoseStartValue = msv.First();
         glucoseStartValue.Value.Value.ShouldBeEqualTo(5000);
      }
   }
}
