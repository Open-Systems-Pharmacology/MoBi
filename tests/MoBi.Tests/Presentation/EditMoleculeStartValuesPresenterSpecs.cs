using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditMoleculeStartValuesPresenter : ContextSpecification<EditMoleculeStartValuesPresenter>
   {
      protected IFormulaCachePresenter _formulaCachePresenter;
      protected IMoleculeStartValuesPresenter _moleculeStartValuesPresenter;
      protected IEditMoleculeStartValuesView _editMoleculeStartValuesView;

      protected override void Context()
      {
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         _moleculeStartValuesPresenter = A.Fake<IMoleculeStartValuesPresenter>();
         _editMoleculeStartValuesView = A.Fake<IEditMoleculeStartValuesView>();
         sut = new EditMoleculeStartValuesPresenter(_editMoleculeStartValuesView, _moleculeStartValuesPresenter, _formulaCachePresenter);
      }
   }

   public class When_editing_the_molecule_start_value_building_block : concern_for_EditMoleculeStartValuesPresenter
   {
      private MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      protected override void Context()
      {
         base.Context();
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
      }

      protected override void Because()
      {
         sut.Edit(_moleculeStartValuesBuildingBlock);
      }

      [Observation]
      public void the_subpresenter_must_also_edit_the_building_block()
      {
         A.CallTo(() => _moleculeStartValuesPresenter.Edit(_moleculeStartValuesBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_formula_subpresenter_must_also_edit_the_building_block()
      {
         A.CallTo(() => _formulaCachePresenter.Edit(_moleculeStartValuesBuildingBlock)).MustHaveHappened();
      }
   }

   public class When_adding_a_molecule_start_value : concern_for_EditMoleculeStartValuesPresenter
   {
      protected override void Because()
      {
         sut.AddNewEmptyStartValue();
      }

      [Observation]
      public void the_subpresenter_is_used_to_add_the_molecule_start_value()
      {
         A.CallTo(() => _moleculeStartValuesPresenter.AddNewEmptyStartValue()).MustHaveHappened();
      }
   }

   public class When_extending_molecule_start_values : concern_for_EditMoleculeStartValuesPresenter
   {
      protected override void Because()
      {
         sut.ExtendStartValues();
      }

      [Observation]
      public void the_subpresenter_is_used_to_extend()
      {
         A.CallTo(() => _moleculeStartValuesPresenter.ExtendStartValues()).MustHaveHappened();
      }
   }

}
