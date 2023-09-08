using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditInitialConditionsPresenter : ContextSpecification<EditInitialConditionsPresenter>
   {
      protected IFormulaCachePresenter _formulaCachePresenter;
      protected IInitialConditionsPresenter _initialConditionsPresenter;
      protected IEditInitialConditionsView _editInitialConditionsView;

      protected override void Context()
      {
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         _initialConditionsPresenter = A.Fake<IInitialConditionsPresenter>();
         _editInitialConditionsView = A.Fake<IEditInitialConditionsView>();
         sut = new EditInitialConditionsPresenter(_editInitialConditionsView, _initialConditionsPresenter, _formulaCachePresenter);
      }
   }

   public class When_editing_the_molecule_start_value_building_block : concern_for_EditInitialConditionsPresenter
   {
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      protected override void Context()
      {
         base.Context();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
      }

      protected override void Because()
      {
         sut.Edit(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_subpresenter_must_also_edit_the_building_block()
      {
         A.CallTo(() => _initialConditionsPresenter.Edit(_initialConditionsBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_formula_subpresenter_must_also_edit_the_building_block()
      {
         A.CallTo(() => _formulaCachePresenter.Edit(_initialConditionsBuildingBlock)).MustHaveHappened();
      }
   }

   public class When_adding_a_molecule_start_value : concern_for_EditInitialConditionsPresenter
   {
      protected override void Because()
      {
         sut.AddNewEmptyInitialCondition();
      }

      [Observation]
      public void the_subpresenter_is_used_to_add_the_molecule_start_value()
      {
         A.CallTo(() => _initialConditionsPresenter.AddNewEmptyStartValue()).MustHaveHappened();
      }
   }
}
