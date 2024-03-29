using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_CheckNameVisitor : ContextSpecification<ICheckNameVisitor>
   {
      protected IObjectBase _changedObject;
      protected string _changedName;
      protected string _newName;
      protected IObjectTypeResolver _objectTypeResolver;
      protected IAliasCreator _aliasCreator;
      protected IMoBiContext _context;
      protected MoBiProject _project;
      protected IParameterValuePathTask _psvTask;
      protected IInitialConditionPathTask _msvTask;
      protected ICloneManager _cloneManager;

      protected override void Context()
      {
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _aliasCreator = A.Fake<IAliasCreator>();
         _changedObject = A.Fake<IObjectBase>();
         _psvTask = A.Fake<IParameterValuePathTask>();
         _msvTask = A.Fake<IInitialConditionPathTask>();
         _changedObject.Name = "OLD";
         _changedName = _changedObject.Name;
         _newName = "new";
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IMoBiContext>();
         _cloneManager = A.Fake<ICloneManager>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         sut = new CheckNameVisitor(_objectTypeResolver, _aliasCreator, _psvTask, _msvTask, _cloneManager);
      }
   }

   internal class When_visiting_an_objectBase_with_changed_Name : concern_for_CheckNameVisitor
   {
      private IContainer _objectBase;
      private IEnumerable<IStringChange> _changes;
      private SpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _objectBase = new Container();
         _objectBase.Name = _changedName;
         _spatialStructure = new SpatialStructure().WithName("Org").WithId("Org").WithTopContainer(_objectBase);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _spatialStructure, _changedName);
      }

      [Observation]
      public void should_add_String_change_for_object_to_changes()
      {
         _changes.Count().ShouldBeEqualTo(1);
         var change = _changes.First() as StringChange<IObjectBase>;
         change.EntityToEdit.ShouldBeEqualTo(_objectBase);
         change.ChangeCommand.IsAnImplementationOf<EditObjectBasePropertyInBuildingBlockCommand>();
      }
   }

   internal class When_visiting_an_Formula_with_changed_Name : concern_for_CheckNameVisitor
   {
      private IFormula _formula;
      private FormulaUsablePath _path;
      private IEnumerable<IStringChange> _changes;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         _formula.Name = _changedName;
         _path = new FormulaUsablePath(new[] {"A", "B", _changedName}) {Alias = _changedName};
         A.CallTo(() => _aliasCreator.CreateAliasFrom(_changedName)).Returns(_changedName);
         A.CallTo(() => _aliasCreator.CreateAliasFrom(_newName)).Returns(_newName);
         _formula.AddObjectPath(_path);
         _moleculeBuildingBlock = new MoleculeBuildingBlock().WithName("M").WithId("M");
         _moleculeBuildingBlock.AddFormula(_formula);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _moleculeBuildingBlock, _changedName);
      }

      [Observation]
      public void should_add_String_change_for_Formula_to_changes()
      {
         _changes.Count().ShouldBeEqualTo(3);
         var change = _changes.First() as StringChange<IFormula>;
         change.ShouldNotBeNull();
         change.EntityToEdit.ShouldBeEqualTo(_formula);
         change.ChangeCommand.IsAnImplementationOf<RenameObjectBaseCommand>().ShouldBeTrue();
      }

      [Observation]
      public void should_add_String_change_for_Formula_to_change_Alias()
      {
         var change = _changes.ToList()[2] as StringChange<IFormula>;
         change.ShouldNotBeNull();
         change.EntityToEdit.ShouldBeEqualTo(_formula);
         change.ChangeCommand.IsAnImplementationOf<EditFormulaAliasCommand>().ShouldBeTrue();
      }

      [Observation]
      public void should_add_String_change_for_Formula_to_change_path()
      {
         var change = _changes.ToList()[1] as StringChange<IFormula>;
         change.ShouldNotBeNull();
         change.EntityToEdit.ShouldBeEqualTo(_formula);
         change.ChangeCommand.IsAnImplementationOf<ChangePathElementAtFormulaUseablePathCommand>().ShouldBeTrue();
      }
   }

   internal class When_visiting_an_initial_conditions_building_block_with_changed_Name : concern_for_CheckNameVisitor
   {
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private InitialCondition _initialCondition;
      private ObjectPath _path;
      private IEnumerable<IStringChange> _changes;
      private InitialCondition _initialCondition2;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock {Name = _changedName};
         _initialCondition = new InitialCondition();
         _path = new ObjectPath(new[] {"A", "B", _changedName});
         _initialCondition.Path = _path;
         _initialConditionsBuildingBlock.Add(_initialCondition);

         _module.Add(_initialConditionsBuildingBlock);

         _initialCondition2 = new InitialCondition();
         _path = new ObjectPath(new[] {"A", _changedName, "B"});
         _initialCondition2.Path = _path;
         _initialConditionsBuildingBlock.Add(_initialCondition2);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _initialConditionsBuildingBlock, _changedName);
      }

      [Observation]
      public void should_create_correct_count_of_changes()
      {
         _changes.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_add_String_change_for_object_to_changes()
      {
         var change = _changes.First() as StringChange<IBuildingBlock>;
         change.ShouldNotBeNull();
         change.EntityToEdit.ShouldBeEqualTo(_initialConditionsBuildingBlock);
         change.ChangeCommand.IsAnImplementationOf<RenameObjectBaseCommand>().ShouldBeTrue();
         change.ChangeCommand.ObjectType.ShouldBeEqualTo(ObjectTypes.InitialConditionsBuildingBlock);
      }

      [Observation]
      public void should_execute_EditName_for_MSV()
      {
         A.CallTo(() => _msvTask.UpdateNameCommand(_initialConditionsBuildingBlock, _initialCondition, _newName)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_EditContainerPath_for_MSV2()
      {
         A.CallTo(() => _msvTask.UpdateContainerPathCommand(_initialConditionsBuildingBlock, _initialCondition2, A<int>._, _newName)).MustHaveHappened();
      }
   }

   internal class When_visiting_an_Parameter_ValueBuildingBlock_with_changed_Name : concern_for_CheckNameVisitor
   {
      private ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      private ParameterValue _parameterValue;
      private ObjectPath _path;
      private IEnumerable<IStringChange> _changes;
      private ParameterValue _parameterValue2;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock
         {
            Name = _changedName
         };
         _parameterValue = new ParameterValue();
         _path = new ObjectPath(new[] {"A", "B", _changedName});
         _parameterValue.Path = _path;
         _parameterValuesBuildingBlock.Add(_parameterValue);
         _parameterValue2 = new ParameterValue();
         _path = new ObjectPath(new[] {"A", _changedName, "B"});
         _parameterValue2.Path = _path;
         _parameterValuesBuildingBlock.Add(_parameterValue2);
         _module.Add(_parameterValuesBuildingBlock);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _parameterValuesBuildingBlock, _changedName);
      }

      [Observation]
      public void should_create_correct_count_of_changes()
      {
         _changes.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_add_String_change_for_ParameterValuesBuildingBlock_to_changes()
      {
         var change = _changes.First() as StringChange<IBuildingBlock>;
         change.EntityToEdit.ShouldBeEqualTo(_parameterValuesBuildingBlock);
         change.ChangeCommand.IsAnImplementationOf<RenameObjectBaseCommand>().ShouldBeTrue();
         change.ChangeCommand.ObjectType.ShouldBeEqualTo(ObjectTypes.ParameterValuesBuildingBlock);
      }

      [Observation]
      public void should_execute_EditName_for_PSV()
      {
         A.CallTo(() => _psvTask.UpdateNameCommand(_parameterValuesBuildingBlock, _parameterValue, _newName)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_EditContainerpath_for_PSV2()
      {
         A.CallTo(() => _psvTask.UpdateContainerPathCommand(_parameterValuesBuildingBlock, _parameterValue2, A<int>._, _newName)).MustHaveHappened();
      }
   }

   internal class When_visiting_a_Formula_with_many_to_change : concern_for_CheckNameVisitor
   {
      private ExplicitFormula _theFormula;
      private FormulaUsablePath _formulaUsablePathToChange;
      private FormulaUsablePath _formulaUsablePathNotToChange;
      private string _oldAlias;
      private string _newAlias;
      private IEnumerable<IStringChange> _changes;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _newAlias = "new_Name";
         _changedObject.Name = "Name really old";
         _changedName = _changedObject.Name;
         _oldAlias = "Name_really_old";
         _theFormula = new ExplicitFormula(String.Format("k_{0}*{0}", _oldAlias));
         _formulaUsablePathNotToChange = new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, "BLA", String.Format("k_{0}", _oldAlias)}) {Alias = String.Format("k_{0}", _oldAlias)};
         _theFormula.AddObjectPath(_formulaUsablePathNotToChange);
         _formulaUsablePathToChange = new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, _changedName}) {Alias = _oldAlias};
         _theFormula.AddObjectPath(_formulaUsablePathToChange);
         _theFormula.Name = "F1";
         A.CallTo(() => _aliasCreator.CreateAliasFrom(_changedName)).Returns(_oldAlias);
         A.CallTo(() => _aliasCreator.CreateAliasFrom(_newName)).Returns(_newAlias);
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).Returns(_theFormula);
         _moleculeBuildingBlock = new MoleculeBuildingBlock().WithName("M").WithId("M");
         _moleculeBuildingBlock.AddFormula(_theFormula);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _moleculeBuildingBlock, _changedName);
      }

      [Observation]
      public void should_create_two_changes_for_formula_Path_change()
      {
         var changes =
            _changes.Where(change => change.IsAnImplementationOf<IStringChange>()).Where(
               change => change.ChangeCommand.IsAnImplementationOf<ChangePathElementAtFormulaUseablePathCommand>());
         changes.Count().ShouldBeEqualTo(1);
         changes =
            _changes.Where(change => change.IsAnImplementationOf<StringChange<IFormula>>()).Where(
               change => change.ChangeCommand.IsAnImplementationOf<EditFormulaAliasCommand>());
         changes.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_create_right_change_for_the_formula_string()
      {
         _changes.Count(change => change.ChangeCommand.IsAnImplementationOf<EditFormulaStringCommand>()).ShouldBeEqualTo(1);
      }
   }

   internal class When_visiting_an_observer_builder : concern_for_CheckNameVisitor
   {
      private ObserverBuilder _observer;
      private IEnumerable<IStringChange> _changes;
      private ObserverBuildingBlock _observerBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _observer = new ObserverBuilder().WithName("NNNNNN");
         _observer.ContainerCriteria.Add(new MatchTagCondition(_changedName));
         _observerBuildingBlock = new ObserverBuildingBlock().WithName("Observer").WithId("Observer");
         _observerBuildingBlock.Add(_observer);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _observerBuildingBlock, _changedName);
      }

      [Observation]
      public void should_add_change_for_Match_Tag_condition()
      {
         _changes.Count().ShouldBeEqualTo(1);
         var change = _changes.First();
         change.ChangeCommand.ShouldBeAnInstanceOf<EditTagCommand<ObserverBuilder>>();
      }
   }

   internal class When_checking_for_dependent_changes_in_a_transport_builder : concern_for_CheckNameVisitor
   {
      private IEnumerable<IStringChange> _resultChanges;
      private PassiveTransportBuildingBlock _test;
      private readonly string _oldName = "OldName";

      protected override void Context()
      {
         base.Context();
         var transportBuilder = new TransportBuilder().WithName("Trans");
         transportBuilder.AddMoleculeNameToExclude(_oldName);
         var transportBuilder2 = new TransportBuilder().WithName("Trans2");
         transportBuilder2.AddMoleculeName(_oldName);
         _test = new PassiveTransportBuildingBlock().WithName("Test");
         _test.Add(transportBuilder);
         _test.Add(transportBuilder2);
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _test, _oldName);
      }

      [Observation]
      public void should_return_changes_for_molecule_lists()
      {
         _resultChanges.Count().ShouldBeEqualTo(2);
         _resultChanges.FirstOrDefault(sc => sc.ChangeCommand.IsAnImplementationOf<ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand>()).ShouldNotBeNull();
         _resultChanges.FirstOrDefault(sc => sc.ChangeCommand.IsAnImplementationOf<ChangeMoleculeNameAtMoleculeDependentBuilderCommand>()).ShouldNotBeNull();
      }
   }

   internal class When_checking_for_dependent_changes_in_simulation_settings_with_output_selection_using_the_modified_path : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private SimulationSettings _simulationSettings;
      private readonly string _oldName = "OldName";

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection($"A|B|{_oldName}", QuantityType.Drug));
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("A|B|C", QuantityType.Drug));
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _simulationSettings, _oldName);
      }

      [Observation]
      public void should_return_changes_for_output_selection()
      {
         _resultChanges.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_checking_for_dependent_changes_in_a_container_referenced_in_neighborhood_name_and_neighbors : concern_for_CheckNameVisitor
   {
      private IEnumerable<IStringChange> _resultChanges;
      private SpatialStructure _spatialStructure;
      private NeighborhoodBuilder _neighborhood;
      private Container _container;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure
         {
            NeighborhoodsContainer = new Container()
         };

         _container = new Container().WithName("Muscle");
         _neighborhood = new NeighborhoodBuilder().WithName("Arterial_blood_to_Muscle_cell");
         _neighborhood.FirstNeighborPath = new ObjectPath("Organism", "Muscle", "BloodCells");
         _neighborhood.SecondNeighborPath = new ObjectPath("Organism", "ArterialBlood", "BloodCells");
         _spatialStructure.AddTopContainer(_container);
         _spatialStructure.AddNeighborhood(_neighborhood);
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(_container, "Tumor", _spatialStructure, _container.Name);
      }

      [Observation]
      public void should_return_the_change_for_the_neighborhood_name_and_neighbors()
      {
         _resultChanges.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_command_should_be_the_one_expected()
      {
         //Executing so that we can check the results
         _resultChanges.Each(c => c.ChangeCommand.Execute(_context));
         _neighborhood.Name.ShouldBeEqualTo("Arterial_blood_to_Tumor_cell");
         _neighborhood.FirstNeighborPath.ToPathString().ShouldBeEqualTo("Organism|Tumor|BloodCells");
      }
   }

   internal class When_checking_for_dependent_changes_in_simulation_settings_with_output_selection_not_using_the_modified_path : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private SimulationSettings _simulationSettings;
      private readonly string _oldName = "OldName";

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("A|B|C", QuantityType.Drug));
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _simulationSettings, _oldName);
      }

      [Observation]
      public void should_not_return_any_change()
      {
         _resultChanges.Count.ShouldBeEqualTo(0);
      }
   }

   internal class When_checking_for_dependent_changes_in_simulation_settings_with_chart_template_using_the_modified_path : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private SimulationSettings _simulationSettings;
      private readonly string _oldName = "OldName";
      private CurveChartTemplate _curveChartTemplate;
      private CurveTemplate _curveTemplate;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _curveChartTemplate = new CurveChartTemplate();
         _simulationSettings.AddChartTemplate(_curveChartTemplate);
         _curveTemplate = new CurveTemplate
         {
            xData = {Path = $"A|B|{_oldName}"},
            yData = {Path = $"C|D|{_oldName}"},
         };
         _curveChartTemplate.Curves.Add(_curveTemplate);

         A.CallTo(() => _cloneManager.Clone(_curveChartTemplate)).Returns(_curveChartTemplate);
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _simulationSettings, _oldName);
      }

      [Observation]
      public void should_return_changes_for_chart_template()
      {
         _resultChanges.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_update_xpath_and_ypath_used_in_the_curve_template()
      {
         _curveTemplate.xData.Path.ShouldBeEqualTo("A|B|NewName");
         _curveTemplate.yData.Path.ShouldBeEqualTo("C|D|NewName");
      }
   }

   internal class When_checking_for_dependent_changes_in_simulation_settings_with_chart_template_not_using_the_modified_path : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private SimulationSettings _simulationSettings;
      private readonly string _oldName = "OldName";
      private CurveChartTemplate _curveChartTemplate;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _curveChartTemplate = new CurveChartTemplate();
         _simulationSettings.AddChartTemplate(_curveChartTemplate);
         _curveChartTemplate.Curves.Add(new CurveTemplate
         {
            xData = {Path = "A|B|D"},
            yData = {Path = "C|D|E"}
         });
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _simulationSettings, _oldName);
      }

      [Observation]
      public void should_not_return_changes_for_chart_template()
      {
         _resultChanges.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_renaming_an_application_builder : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private readonly string _oldName = "OldName";
      private ApplicationBuilder _applicationBuilder;
      private EventGroupBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new EventGroupBuildingBlock();
         _applicationBuilder = new ApplicationBuilder
         {
            SourceCriteria = Create.Criteria(x => x.With(_oldName))
         };
         var appTransport = new TransportBuilder
         {
            SourceCriteria = Create.Criteria(x => x.With(_oldName)),
            TargetCriteria = Create.Criteria(x => x.With(_oldName))
         };
         _applicationBuilder.AddTransport(appTransport);

         _buildingBlock.Add(_applicationBuilder);
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(_applicationBuilder, "NewName", _buildingBlock, _oldName);
      }

      [Observation]
      public void should_also_check_dependencies_to_tags_defined_in_underlying_transporter()
      {
         _resultChanges.Count.ShouldBeEqualTo(3);
      }
   }
}