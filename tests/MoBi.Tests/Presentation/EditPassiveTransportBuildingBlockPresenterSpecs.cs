using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Events;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditPassiveTransportBuildingBlockPresenter : ContextSpecification<IEditPassiveTransportBuildingBlockPresenter>
   {
      protected IEditPassiveTransportBuildingBlockView _view;
      private ITransportBuilderToDTOTransportBuilderMapper _mapper;
      protected IEditTransportBuilderPresenter _transporterBuilderPresenter;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IFormulaCachePresenter _formulaCachePresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditPassiveTransportBuildingBlockView>();
         _mapper = A.Fake<ITransportBuilderToDTOTransportBuilderMapper>();
         _transporterBuilderPresenter = A.Fake<IEditTransportBuilderPresenter>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         sut = new EditPassiveTransportBuildingBlockPresenter(_view, _mapper, _transporterBuilderPresenter, _viewItemContextMenuFactory, _formulaMapper, _formulaCachePresenter);
      }
   }

   public class When_creating_the_edit_passive_transport_building_block_presenter : concern_for_EditPassiveTransportBuildingBlockPresenter
   {
      [Observation]
      public void should_remove_formula_types_that_do_not_make_sense_for_passive_transports()
      {
         A.CallTo(() => _transporterBuilderPresenter.RemoveFormulaType<SumFormula>()).MustHaveHappened();
         A.CallTo(() => _transporterBuilderPresenter.RemoveFormulaType<TableFormulaWithOffset>()).MustHaveHappened();
      }
   }
   public class When_handling_the_selecting_event_for_a_formula_that_is_in_the_formula_cache : concern_for_EditPassiveTransportBuildingBlockPresenter
   {
      private IFormula _formula;
      private IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         _passiveTransportBuildingBlock = new PassiveTransportBuildingBlock();
         _passiveTransportBuildingBlock.AddFormula(_formula);
         sut.Edit(_passiveTransportBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_formula, null));
      }

      [Observation]
      public void should_show_the_view_containing_all_the_formulas()
      {
         A.CallTo(() => _view.ShowFormulas()).MustHaveHappened();
      }  

      [Observation]
      public void should_select_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void should_select_the_formula()
      {
         A.CallTo(() => _formulaCachePresenter.Select(_formula)).MustHaveHappened();
      }
   }

   public class When_filtering_the_displayed_transporter_using_a_keywords_that_yield_no_match : concern_for_EditPassiveTransportBuildingBlockPresenter
   {
      protected override void Because()
      {
         sut.Select(null);
      }

      [Observation]
      public void should_hide_view()
      {
         A.CallTo(()=>_view.ClearEditView()).MustHaveHappened();
      }
   }
}