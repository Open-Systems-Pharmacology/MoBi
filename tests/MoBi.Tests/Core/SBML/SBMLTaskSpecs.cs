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
using OSPSuite.Utility.Extensions;
using System;
using System.Linq;

namespace MoBi.Core.SBML
{
   public class SBMLTaskSpecs : ContextForSBMLIntegration<SbmlTask>
   {
      protected ISimulationFactory _simulationFactory;
      protected IBuildConfigurationFactory _buildConfigurationFactory;
      protected IModelConstructor _modelConstructor;
      protected ISimulationSettingsFactory _simulationSettingsFactory;
      protected IObserverBuilder _observerBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationFactory = IoC.Resolve<ISimulationFactory>();
         _buildConfigurationFactory = IoC.Resolve<IBuildConfigurationFactory>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simulationSettingsFactory = IoC.Resolve<ISimulationSettingsFactory>();
         _observerBuilder = IoC.Resolve<IObserverBuilder>();
         _fileName = Helper.TestFileFullPath("tiny_example_12.xml");
      }

      [Observation]
      public void FormulaParameter_InitialAssignmentCreationTest()
      {
         var simulation = _simulationFactory.Create();
         simulation.MoBiBuildConfiguration.SimulationSettings = _simulationSettingsFactory.CreateDefault();
         simulation.MoBiBuildConfiguration.Observers = IoC.Resolve<IMoBiContext>().Create<IObserverBuildingBlock>();
         simulation.MoBiBuildConfiguration.SpatialStructure = _moBiProject.SpatialStructureCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.ParameterStartValues = _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.Reactions = _moBiProject.ReactionBlockCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.Molecules = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.PassiveTransports = _moBiProject.PassiveTransportCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.MoleculeStartValues = _moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault();
         simulation.MoBiBuildConfiguration.EventGroups = _moBiProject.EventBlockCollection.FirstOrDefault();
         var buildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration);
         var name = Guid.NewGuid().ToString();
         var result = _modelConstructor.CreateModelFrom(buildConfiguration, name);
         result.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_translate_new_units_properly()
      {
         var msv = _moBiProject.MoleculeStartValueBlockCollection.First();
         var glucoseStartValue = msv.First();
         glucoseStartValue.StartValue.Value.ShouldBeEqualTo(5000);
      }
   }
}
