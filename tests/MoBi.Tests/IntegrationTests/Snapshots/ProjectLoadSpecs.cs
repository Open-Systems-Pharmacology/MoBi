using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using Newtonsoft.Json.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;

namespace MoBi.IntegrationTests.Snapshots
{
   public class When_loading_a_snapshot : ContextWithLoadedSnapshot
   {
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private IndividualBuildingBlock _individualBuildingBlock;
      private IndividualParameter _individualParameterWithUserFormula;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var starter = IoC.Resolve<IPKSimStarter>();

         var module = new Module
         {
            IsPKSimModule = true,
            Name = "Henrist oral Hot stage extrusion as table"
         };

         A.CallTo(() => starter.LoadModuleFromSnapshot(A<string>._)).Returns(module);

         // EntityValidationTask is a fake
         var validationTask = IoC.Resolve<IEntityValidationTask>();
         A.CallTo(() => validationTask.Validate(A<MoBiSimulation>._)).Returns(true);

         _expressionProfileBuildingBlock =
         [
            new ExpressionParameter { Path = "Organism|VenousBlood|Plasma|CYP3A4|Initial concentration".ToObjectPath(), Dimension = A.Fake<IDimension>(), Value = 1 },
            new ExpressionParameter { Path = "Organism|Bone|Intracellular|CYP3A4|Fraction expressed intracellular".ToObjectPath(), Dimension = A.Fake<IDimension>(), Value = 1 }
         ];

         _individualParameterWithUserFormula = new IndividualParameter { Path = "Organism|Ontogeny factor (albumin)".ToObjectPath(), Dimension = A.Fake<IDimension>(), Value = 1 };
         _individualBuildingBlock =
         [
            _individualParameterWithUserFormula,
            new IndividualParameter { Path = "Organism|pH (blood cells)".ToObjectPath(), Dimension = A.Fake<IDimension>(), Value = 1 },
            new IndividualParameter { Path = "Organism|Age".ToObjectPath(), Dimension = A.Fake<IDimension>(), Value = 1 }
         ];

         A.CallTo(() => starter.LoadExpressionProfileFromSnapshot(A<string>._)).Returns(_expressionProfileBuildingBlock);
         A.CallTo(() => starter.LoadIndividualFromSnapshot(A<string>._)).Returns(_individualBuildingBlock);

         LoadSnapshot("snapshot");
      }

      [Observation]
      public void the_observed_data_is_classified_in_a_folder()
      {
         _project.AllObservedData.Count.ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableObservedData>().Count(x => x.Id.Equals(_project.AllObservedData.First().Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.ObservedData).First().Name.ShouldBeEqualTo("observed data folder");
      }

      [Observation]
      public void the_extension_module_is_classified_in_a_folder()
      {
         _project.Modules.Count(x => !x.IsPKSimModule).ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableModule>().Count(x => x.Id.Equals(_project.Modules.First(m => !m.IsPKSimModule).Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.Module).First().Name.ShouldBeEqualTo("module folder");
      }

      [Observation]
      public void the_pksim_module_is_loaded()
      {
         _project.Modules.Count(x => x.IsPKSimModule).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_parameter_identification_is_classified_in_a_folder()
      {
         _project.AllParameterIdentifications.Count.ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableParameterIdentification>().Count(x => x.Id.Equals(_project.AllParameterIdentifications.First().Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.ParameterIdentification).First().Name.ShouldBeEqualTo("pi folder");
      }

      [Observation]
      public void the_expression_building_blocks_are_loaded()
      {
         _project.ExpressionProfileCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_individual_building_blocks_are_loaded()
      {
         _project.IndividualsCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_individual_and_expression_should_indicate_they_have_been_updated_to_match_the_snapshot()
      {
         _project.IndividualsCollection[0].Count(x => x.HasInitialState).ShouldBeEqualTo(3);
         _project.ExpressionProfileCollection[0].ExpressionParameters.Count(x => x.HasInitialState).ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_user_created_formula_is_used_from_the_serialized_formula_cache()
      {
         _individualParameterWithUserFormula.Formula.Name.ShouldBeEqualTo("ClonedTableFormulaWithXArgument_OntogenyFactorAlbumin");
      }
   }

   public class When_loading_a_snapshot_with_simulations_sharing_modules_in_parallel : ContextWithLoadedSnapshot
   {
      private MoBiProject _parallelProject;
      private MoBiProject _sequentialProject;
      private string _snapshotFile;

      public override void GlobalContext()
      {
         base.GlobalContext();

         var validationTask = IoC.Resolve<IEntityValidationTask>();
         A.CallTo(() => validationTask.Validate(A<MoBiSimulation>._)).Returns(true);

         _snapshotFile = snapshotWithSimulationCopies(DomainHelperForSpecs.TestFileFullPath("snapshot_no_pksim_modules.json"), numberOfSimulations: 4);
         var userSettings = IoC.Resolve<ICoreUserSettings>();

         A.CallTo(() => userSettings.MaximumNumberOfCoresToUse).Returns(4);
         LoadSnapshot(_snapshotFile, isFullPath: true, runSimulations: false);
         _parallelProject = _project;

         A.CallTo(() => userSettings.MaximumNumberOfCoresToUse).Returns(1);
         LoadSnapshot(_snapshotFile, isFullPath: true, runSimulations: false);
         _sequentialProject = _project;
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         File.Delete(_snapshotFile);
      }

      //the copies all reference the same module instances of the project, so the parallel constructions share building blocks
      private static string snapshotWithSimulationCopies(string snapshotFile, int numberOfSimulations)
      {
         var json = JObject.Parse(File.ReadAllText(snapshotFile));
         var simulations = (JArray) json["Simulations"];
         var template = (JObject) simulations[0];
         for (var i = 2; i <= numberOfSimulations; i++)
         {
            var copy = (JObject) template.DeepClone();
            copy["Name"] = $"{template["Name"]}-{i}";
            simulations.Add(copy);
         }

         var file = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
         File.WriteAllText(file, json.ToString());
         return file;
      }

      //sorted entity paths with the formula, dimension and value they carry: catches a race that attaches
      //entities or formulas to the wrong nodes or mixes up their content, without depending on ids
      private static string fingerprintOf(IMoBiSimulation simulation)
      {
         return simulation.Model.Root.GetAllChildren<IEntity>()
            .Select(entityFingerprint)
            .OrderBy(x => x)
            .ToString("\n");
      }

      private static string entityFingerprint(IEntity entity)
      {
         var formula = (entity as IUsingFormula)?.Formula;
         var formulaString = (formula as ExplicitFormula)?.FormulaString;
         var dimension = (entity as IWithDimension)?.Dimension?.Name;
         var value = (entity as IParameter)?.Value.ToString(NumberFormatInfo.InvariantInfo);
         return $"{entity.EntityPath()}|{formula?.Name}|{formulaString}|{dimension}|{value}";
      }

      [Observation]
      public void should_add_all_simulations_in_the_snapshot_order()
      {
         _parallelProject.Simulations.AllNames().ShouldOnlyContainInOrder("test", "test-2", "test-3", "test-4");
      }

      [Observation]
      public void should_create_models_that_are_not_empty()
      {
         _parallelProject.Simulations.Each(simulation => simulation.Model.Root.GetAllChildren<IEntity>().Count.ShouldBeGreaterThan(100));
      }

      [Observation]
      public void should_not_create_duplicate_entity_ids_within_a_model()
      {
         _parallelProject.Simulations.Each(simulation =>
            simulation.Model.Root.GetAllChildren<IEntity>().GroupBy(x => x.Id).Count(group => group.Count() > 1).ShouldBeEqualTo(0));
      }

      //the flag is disabled only while the model is constructed and must not survive on the simulation:
      //it would suppress core progress in later updates and show up in configuration comparisons
      [Observation]
      public void should_not_keep_the_construction_progress_suppression_on_the_loaded_simulations()
      {
         _parallelProject.Simulations.Each(simulation => simulation.Configuration.ShowProgress.ShouldBeTrue());
      }

      [Observation]
      public void should_create_the_same_models_in_parallel_as_sequentially()
      {
         _parallelProject.Simulations.Each(simulation =>
            fingerprintOf(simulation).ShouldBeEqualTo(fingerprintOf(_sequentialProject.Simulations.FindByName(simulation.Name))));
      }
   }
}
