using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
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
      protected IMoBiProject _project;
      protected IParameterStartValuePathTask _psvTask;
      protected IMoleculeStartValuePathTask _msvTask;
      protected ICloneManager _cloneManager;

      protected override void Context()
      {
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _aliasCreator = A.Fake<IAliasCreator>();
         _changedObject = A.Fake<IObjectBase>();
         _psvTask = A.Fake<IParameterStartValuePathTask>();
         _msvTask = A.Fake<IMoleculeStartValuePathTask>();
         _changedObject.Name = "OLD";
         _changedName = _changedObject.Name;
         _newName = "new";
         _project = new MoBiProject();
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
      private ISpatialStructure _spatialStructure;

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
      private IFormulaUsablePath _path;
      private IEnumerable<IStringChange> _changes;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;

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
         change.ChangeCommand.IsAnImplementationOf<EditObjectBasePropertyInBuildingBlockCommand>().ShouldBeTrue();
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

   internal class When_visiting_an_MoleculesStartValueBuildingBlock_with_changed_Name : concern_for_CheckNameVisitor
   {
      private IMoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      private MoleculeStartValue _moleculeStartValue;
      private IObjectPath _path;
      private IEnumerable<IStringChange> _changes;
      private MoleculeStartValue _moleculeStartValue2;

      protected override void Context()
      {
         base.Context();
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock {Name = _changedName};
         _moleculeStartValue = new MoleculeStartValue();
         _path = new ObjectPath(new[] {"A", "B", _changedName});
         _moleculeStartValue.Path = _path;
         _moleculeStartValuesBuildingBlock.Add(_moleculeStartValue);
         _project.AddBuildingBlock(_moleculeStartValuesBuildingBlock);
         _moleculeStartValue2 = new MoleculeStartValue();
         _path = new ObjectPath(new[] {"A", _changedName, "B"});
         _moleculeStartValue2.Path = _path;
         _moleculeStartValuesBuildingBlock.Add(_moleculeStartValue2);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _project, _changedName);
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
         change.EntityToEdit.ShouldBeEqualTo(_moleculeStartValuesBuildingBlock);
         change.ChangeCommand.IsAnImplementationOf<EditObjectBasePropertyInBuildingBlockCommand>().ShouldBeTrue();
         change.ChangeCommand.ObjectType.ShouldBeEqualTo(ObjectTypes.MoleculeStartValuesBuildingBlock);
      }

      [Observation]
      public void should_execute_EditName_for_MSV()
      {
         A.CallTo(() => _msvTask.UpdateStartValueNameCommand(_moleculeStartValuesBuildingBlock, _moleculeStartValue, _newName)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_EditContainerPath_for_MSV2()
      {
         A.CallTo(() => _msvTask.UpdateStartValueContainerPathCommand(_moleculeStartValuesBuildingBlock, _moleculeStartValue2, A<int>._, _newName)).MustHaveHappened();
      }
   }

   internal class When_visiting_an_ParameterStartValueBuildingBlock_with_changed_Name : concern_for_CheckNameVisitor
   {
      private IParameterStartValuesBuildingBlock _parameterStartValuesBuildingBlock;
      private ParameterStartValue _parameterStartValue;
      private IObjectPath _path;
      private IEnumerable<IStringChange> _changes;
      private ParameterStartValue _parameterStartValue2;

      protected override void Context()
      {
         base.Context();
         _parameterStartValuesBuildingBlock = new ParameterStartValuesBuildingBlock();
         _parameterStartValuesBuildingBlock.Name = _changedName;
         _parameterStartValue = new ParameterStartValue();
         _path = new ObjectPath(new[] {"A", "B", _changedName});
         _parameterStartValue.Path = _path;
         _parameterStartValuesBuildingBlock.Add(_parameterStartValue);
         _parameterStartValue2 = new ParameterStartValue();
         _path = new ObjectPath(new[] {"A", _changedName, "B"});
         _parameterStartValue2.Path = _path;
         _parameterStartValuesBuildingBlock.Add(_parameterStartValue2);
         _project.AddBuildingBlock(_parameterStartValuesBuildingBlock);
      }

      protected override void Because()
      {
         _changes = sut.GetPossibleChangesFrom(_changedObject, _newName, _project, _changedName);
      }

      [Observation]
      public void should_create_correct_count_of_changes()
      {
         _changes.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_add_String_change_for_ParameterStartValuesBuildingBlock_to_changes()
      {
         var change = _changes.First() as StringChange<IBuildingBlock>;
         change.EntityToEdit.ShouldBeEqualTo(_parameterStartValuesBuildingBlock);
         change.ChangeCommand.IsAnImplementationOf<EditObjectBasePropertyInBuildingBlockCommand>().ShouldBeTrue();
         change.ChangeCommand.ObjectType.ShouldBeEqualTo(ObjectTypes.ParameterStartValuesBuildingBlock);
      }

      [Observation]
      public void should_execute_EditName_for_PSV()
      {
         A.CallTo(() => _psvTask.UpdateStartValueNameCommand(_parameterStartValuesBuildingBlock, _parameterStartValue, _newName)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_EditContainerpath_for_PSV2()
      {
         A.CallTo(() => _psvTask.UpdateStartValueContainerPathCommand(_parameterStartValuesBuildingBlock, _parameterStartValue2, A<int>._, _newName)).MustHaveHappened();
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
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      
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
      private IObserverBuilder _observer;
      private IEnumerable<IStringChange> _changes;
      private IObserverBuildingBlock _observerBuildingBlock;

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
         change.ChangeCommand.ShouldBeAnInstanceOf<EditTagCommand<IObserverBuilder>>();
      }
   }

   internal class When_checking_for_dependent_changes_in_a_transport_builder : concern_for_CheckNameVisitor
   {
      private IEnumerable<IStringChange> _resultChanges;
      private IPassiveTransportBuildingBlock _test;
      private readonly string _oldName = "OldName";

      protected override void Context()
      {
         base.Context();
         var transportbuilder = new TransportBuilder().WithName("Trans");
         transportbuilder.AddMoleculeNameToExclude(_oldName);
         var transportbuilder2 = new TransportBuilder().WithName("Trans2");
         transportbuilder2.AddMoleculeName(_oldName);
         _test = new PassiveTransportBuildingBlock().WithName("Test");
         _test.Add(transportbuilder);
         _test.Add(transportbuilder2);
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
      private ISimulationSettings _simulationSettings;
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

   internal class When_checking_for_dependent_changes_in_simulation_settings_with_output_selection_not_using_the_modified_path : concern_for_CheckNameVisitor
   {
      private IReadOnlyList<IStringChange> _resultChanges;
      private ISimulationSettings _simulationSettings;
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
      private ISimulationSettings _simulationSettings;
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
      private ISimulationSettings _simulationSettings;
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
      private IApplicationBuilder _applicationBuilder;

      protected override void Context()
      {
         base.Context();
         _applicationBuilder = new ApplicationBuilder();
         _applicationBuilder.SourceCriteria = Create.Criteria(x => x.With(_oldName));
         var appTransport = new TransportBuilder();
         appTransport.SourceCriteria = Create.Criteria(x => x.With(_oldName));
         appTransport.TargetCriteria = Create.Criteria(x => x.With(_oldName));
         _applicationBuilder.AddTransport(appTransport);
      }

      protected override void Because()
      {
         _resultChanges = sut.GetPossibleChangesFrom(A.Fake<IObjectBase>(), "NewName", _applicationBuilder, _oldName);
      }

      [Observation]
      public void should_also_check_dependencies_to_tags_defined_in_underlying_transporter()
      {
         _resultChanges.Count.ShouldBeEqualTo(3);
      }
   }
}