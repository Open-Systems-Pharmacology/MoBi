using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForParameter : ContextSpecification<IInteractionTasksForParameter>
   {
      protected IMoBiDimensionFactory _dimensionFactory;
      protected IInteractionTaskContext _context;
      private IEditTaskFor<IParameter> _editTasks;
      protected IMoBiFormulaTask _formulaTask;
      protected IQuantityTask _quantityTask;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTasks = A.Fake<IEditTaskFor<IParameter>>();
         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _quantityTask = A.Fake<IQuantityTask>();
         sut = new InteractionTasksForParameter(_context, _editTasks, _dimensionFactory, _formulaTask, _quantityTask);
      }
   }

   public class When_adding_a_new_parameter_and_default_dimension_is_not_found : concern_for_InteractionTasksForParameter
   {
      private IContainer _parentContainer;
      private IBuildingBlock _buildingBlock;
      private Parameter _parameter;
      private IDimension _noDimension;
      private ConstantFormula _defaultConstantFormula;

      protected override void Context()
      {
         base.Context();
         _noDimension = A.Fake<IDimension>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parentContainer = new ReactionBuilder();
         var modalPresenter = A.Fake<IModalPresenter>();
         var editParameterPresenter = A.Fake<IEditParameterPresenter>();
         _parameter = new Parameter();
         A.CallTo(() => _context.Context.Create<IParameter>()).Returns(_parameter);
         A.CallTo(() => _context.UserSettings.ParameterDefaultDimension).Returns("UNKNOWN");
         A.CallTo(_context.ApplicationController).WithReturnType<IModalPresenter>().Returns(modalPresenter);
         A.CallTo(() => modalPresenter.Show(null)).Returns(true);
         A.CallTo(() => modalPresenter.SubPresenter).Returns(editParameterPresenter);
         A.CallTo(() => _context.ApplicationController.Start<ISelectReferenceAtParameterPresenter>()).Returns(A.Fake<ISelectReferenceAtParameterPresenter>());
         A.CallTo(() => _dimensionFactory.Dimension("UNKNOWN")).Throws<KeyNotFoundException>();
         A.CallTo(() => _dimensionFactory.NoDimension).Returns(_noDimension);
         _defaultConstantFormula = new ConstantFormula();
         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(_defaultConstantFormula);
      }

      protected override void Because()
      {
         sut.AddNew(_parentContainer, _buildingBlock);
      }

      [Observation]
      public void should_get_the_no_dimension_from_dimension_factory()
      {
         _parameter.Dimension.ShouldBeEqualTo(_noDimension);
      }
   }

   public class When_adding_a_new_parameter_and_default_dimension_is_unknown : concern_for_InteractionTasksForParameter
   {
      private IContainer _parentContainer;
      private IBuildingBlock _buildingBlock;
      private Parameter _parameter;
      private IDimension _dimension;
      private ConstantFormula _defaultConstantFormula;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parentContainer = new ReactionBuilder();
         var modalPresenter = A.Fake<IModalPresenter>();
         var editParameterPresenter = A.Fake<IEditParameterPresenter>();
         _parameter = new Parameter();
         A.CallTo(() => _context.Context.Create<IParameter>()).Returns(_parameter);
         A.CallTo(() => _context.UserSettings.ParameterDefaultDimension).Returns("UNKNOWN");
         A.CallTo(_context.ApplicationController).WithReturnType<IModalPresenter>().Returns(modalPresenter);
         A.CallTo(() => modalPresenter.Show(null)).Returns(true);
         A.CallTo(() => modalPresenter.SubPresenter).Returns(editParameterPresenter);
         A.CallTo(() => _context.ApplicationController.Start<ISelectReferenceAtParameterPresenter>()).Returns(A.Fake<ISelectReferenceAtParameterPresenter>());
         A.CallTo(_dimensionFactory).WithReturnType<IDimension>().Returns(_dimension);
         _defaultConstantFormula = new ConstantFormula();
         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(_defaultConstantFormula);
      }

      protected override void Because()
      {
         sut.AddNew(_parentContainer, _buildingBlock);
      }

      [Observation]
      public void should_call_for_right_new_objects()
      {
         A.CallTo(() => _context.Context.Create<IParameter>()).MustHaveHappened();
      }

      [Observation]
      public void should_get_the_no_dimension_from_dimension_factory()
      {
         _parameter.Dimension.ShouldBeEqualTo(_dimension);
      }

      [Observation]
      public void should_set_the_parameter_as_visible()
      {
         _parameter.Visible.ShouldBeTrue();
      }

      [Observation]
      public void should_set_the_default_group_to_the_mobi_group()
      {
         _parameter.GroupName.ShouldBeEqualTo(Constants.Groups.MOBI);
      }

      [Observation]
      public void should_set_the_default_formula_to_a_constant_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_defaultConstantFormula);
      }

      [Observation]
      public void should_set_the_build_mode_to_global_for_a_parameter_added_into_a_reaction()
      {
         _parameter.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
      }
   }

   public class When_removing_a_parameter_that_can_be_removed : concern_for_InteractionTasksForParameter
   {
      private IParameter _parameter;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _parameter = new Parameter().WithName("TOTO").WithParentContainer(_container);
      }

      protected override void Because()
      {
         sut.Remove(_parameter, _container, A.Fake<IBuildingBlock>(), silent: true);
      }

      [Observation]
      public void should_really_remove_the_parameter()
      {
         _container.ContainsName("TOTO").ShouldBeFalse();
      }
   }

   public class When_removing_a_concentration_parameter_in_a_molecule_builder : concern_for_InteractionTasksForParameter
   {
      private IParameter _parameter;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new MoleculeBuilder();
         _parameter = new Parameter().WithName(Constants.Parameters.CONCENTRATION).WithParentContainer(_container);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Remove(_parameter, _container, A.Fake<IBuildingBlock>())).ShouldThrowAn<MoBiException>();
      }
   }


}