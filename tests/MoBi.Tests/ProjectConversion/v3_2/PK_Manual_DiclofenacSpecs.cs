using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class PK_Manual_DiclofenacSpecs : ContextWithLoadedProject
   {
      protected IMoBiProject _project;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("PK_Manual_Diclofenac");
      }
   }

   internal class When_converting_old_Dicolfenac_example : PK_Manual_DiclofenacSpecs
   {
      [Observation]
      public void should_set_project_Has_Changed_flag_to_false()
      {
         //project does not need to be saved necessarily. It was just open
         _project.HasChanged.ShouldBeFalse();
      }

      [Observation]
      public void should_set_all_simulations_to_changed()
      {
         _project.Simulations.Each(sim => sim.HasChanged.ShouldBeTrue());
      }
   }

   internal class When_recreating_existing_simulation : PK_Manual_DiclofenacSpecs
   {
      private IMoBiSimulation _simulation;
      private IModelConstructor _modelConstructor;
      private IMoBiBuildConfiguration _buildConfiguration;
      private CreationResult _results;
      private ISimModelManager _simModelManeger;
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simModelManeger = IoC.Resolve<ISimModelManager>();
         _buildConfiguration = IoC.Resolve<IBuildConfigurationFactory>().CreateFromReferencesUsedIn(_project.Simulations.Last().MoBiBuildConfiguration);
         _buildConfiguration.ShowProgress = false;
         _context = IoC.Resolve<IMoBiContext>();
      }

      protected override void Because()
      {
         _results = _modelConstructor.CreateModelFrom(_buildConfiguration, "test");
      }

      [Observation]
      public void should_be_able_to_create_simulation()
      {
         _results.State.ShouldNotBeEqualTo(ValidationState.Invalid, _results.ValidationResult.Messages.SelectMany(x => x.Details).ToString("\n"));
         _simulation = new MoBiSimulation {BuildConfiguration = _buildConfiguration, Model = _results.Model};
         _simulation.Id = "Sim";
         _context.Register(_simulation);
         _simModelManeger.RunSimulation(_simulation);
      }
   }

   internal class When_creating_and_running_a_new_simulation : PK_Manual_DiclofenacSpecs
   {
      private IMoBiSimulation _simulation;
      private IModelConstructor _modelConstructor;
      private IMoBiBuildConfiguration _buildConfiguration;
      private CreationResult _results;
      private ISimModelManager _simModelManeger;
      private IMoBiContext _context;
      private ICoreCalculationMethodRepository _calculatonMethodRepository;

      protected override void Context()
      {
         base.Context();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simModelManeger = IoC.Resolve<ISimModelManager>();
         _context = IoC.Resolve<IMoBiContext>();
         _calculatonMethodRepository = IoC.Resolve<ICoreCalculationMethodRepository>();
         _buildConfiguration = createBuildConfiguration();
      }

      private IMoBiBuildConfiguration createBuildConfiguration()
      {
         var moBiBuildConfiguration = new MoBiBuildConfiguration()
         {
            SpatialStructure = _project.SpatialStructureCollection.First(),
            Molecules = _project.MoleculeBlockCollection.First(),
            Reactions = _project.ReactionBlockCollection.First(),
            PassiveTransports = _project.PassiveTransportCollection.First(),
            Observers = _project.ObserverBlockCollection.First(),
            EventGroups = _project.EventBlockCollection.First(),
            MoleculeStartValues = _project.MoleculeStartValueBlockCollection.First(),
            ParameterStartValues = _project.ParametersStartValueBlockCollection.First(),
            SimulationSettings = _project.SimulationSettingsCollection.First()
         };
         _calculatonMethodRepository.All().Each(moBiBuildConfiguration.AddCalculationMethod);
         moBiBuildConfiguration.ShowProgress = false;
         moBiBuildConfiguration.ShouldValidate = true;
         return moBiBuildConfiguration;
      }

      protected override void Because()
      {
         _results = _modelConstructor.CreateModelFrom(_buildConfiguration, "test");
      }

      [Observation]
      public void should_be_able_to_create_simulation()
      {
         _results.State.ShouldNotBeEqualTo(ValidationState.Invalid, _results.ValidationResult.Messages.SelectMany(x => x.Details).ToString("\n"));
         _simulation = new MoBiSimulation {BuildConfiguration = _buildConfiguration, Model = _results.Model};
         _simulation.Id = "Sim";
         _context.Register(_simulation);
         _simModelManeger.RunSimulation(_simulation);
      }
   }

   internal class When_converting_the_molecule_building_block_ : PK_Manual_DiclofenacSpecs
   {
      private IMoleculeBuildingBlock _moleculeBuildingBlock;

      [Observation]
      public void should_have_converted_the_molecule_building_block()
      {
         _moleculeBuildingBlock = _project.MoleculeBlockCollection.First();
         _moleculeBuildingBlock.ShouldNotBeNull();
      }
   }

   public class When_converting_the_output_schema_in_the_project_PK_Manual_Diclofenac : PK_Manual_DiclofenacSpecs
   {
      private List<OutputInterval> _intervals;

      [Observation]
      public void should_have_converted_the_existing_intervals()
      {
         _intervals = _project.Simulations.First().Settings.OutputSchema.Intervals.ToList();
         _intervals.Count.ShouldBeEqualTo(1);
         var interval = _intervals[0];
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.START_TIME).Value.ShouldBeEqualTo(0);
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.END_TIME).Value.ShouldBeEqualTo(1440);
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.RESOLUTION).Value.ShouldBeEqualTo(4.0 / 60.0, 0.00000001);
      }
   }
}