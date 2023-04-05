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
      protected IObserverBuilder _observerBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationFactory = IoC.Resolve<ISimulationFactory>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simulationSettingsFactory = IoC.Resolve<ISimulationSettingsFactory>();
         _observerBuilder = IoC.Resolve<IObserverBuilder>();
         _fileName = Helper.TestFileFullPath("tiny_example_12.xml");
      }

      [Observation]
      public void FormulaParameter_InitialAssignmentCreationTest()
      {
         var simulation = _simulationFactory.Create();
         simulation.Configuration.Module = new Module
         {
            Observer = IoC.Resolve<IMoBiContext>().Create<IObserverBuildingBlock>(),
            SpatialStructure = _moBiProject.SpatialStructureCollection.FirstOrDefault(),
            Reaction = _moBiProject.ReactionBlockCollection.FirstOrDefault(),
            Molecule = _moBiProject.MoleculeBlockCollection.FirstOrDefault(),
            PassiveTransport = _moBiProject.PassiveTransportCollection.FirstOrDefault(),
            EventGroup = _moBiProject.EventBlockCollection.FirstOrDefault()

         };
         simulation.Configuration.Module.AddParameterStartValueBlock(_moBiProject.ParametersStartValueBlockCollection.FirstOrDefault());
         simulation.Configuration.Module.AddMoleculeStartValueBlock(_moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault());
         
         simulation.Configuration.SimulationSettings = _simulationSettingsFactory.CreateDefault();
         
         // var buildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration);
         var name = Guid.NewGuid().ToString();
         var result = _modelConstructor.CreateModelFrom(simulation.Configuration, name);
         result.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_translate_new_units_properly()
      {
         var msv = _moBiProject.MoleculeStartValueBlockCollection.First();
         var glucoseStartValue = msv.First();
         glucoseStartValue.Value.Value.ShouldBeEqualTo(5000);
      }
   }
}
