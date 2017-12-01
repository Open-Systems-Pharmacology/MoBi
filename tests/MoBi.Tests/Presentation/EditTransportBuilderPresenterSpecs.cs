using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTransportBuilderPresenter : ContextSpecification<IEditTransportBuilderPresenter>
   {
      protected IEditTransportBuilderView _view;
      protected ITransportBuilderToDTOTransportBuilderMapper _transportBuilderMapper;
      protected IEditTaskFor<ITransportBuilder> _editTask;
      protected IViewItemContextMenuFactory _contextMenuFactory;
      protected IFormulaToFormulaBuilderDTOMapper _formulaBuilderDTOMapper;
      protected IEditParameterListPresenter _editParameterListPresenter;
      protected IEditFormulaPresenter _editFormulaPresenter;
      protected ISelectReferenceAtTransportPresenter _selectReferenceAtTransportPresenter;
      protected IMoBiContext _context;
      protected IMoleculeDependentBuilderPresenter _moleculeListPresenter;
      protected IDescriptorConditionListPresenter<ITransportBuilder> _sourceCriteriaPresenter;
      protected IDescriptorConditionListPresenter<ITransportBuilder> _targetCriteriaPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditTransportBuilderView>();
         _transportBuilderMapper = A.Fake<ITransportBuilderToDTOTransportBuilderMapper>();
         _editTask = A.Fake<IEditTaskFor<ITransportBuilder>>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _formulaBuilderDTOMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _editParameterListPresenter = A.Fake<IEditParameterListPresenter>();
         _editFormulaPresenter = A.Fake<IEditFormulaPresenter>();
         _selectReferenceAtTransportPresenter = A.Fake<ISelectReferenceAtTransportPresenter>();
         _context = A.Fake<IMoBiContext>();
         _moleculeListPresenter = A.Fake<IMoleculeDependentBuilderPresenter>();
         _sourceCriteriaPresenter = A.Fake<IDescriptorConditionListPresenter<ITransportBuilder>>();
         _targetCriteriaPresenter = A.Fake<IDescriptorConditionListPresenter<ITransportBuilder>>();

         sut = new EditTransportBuilderPresenter(_view,_transportBuilderMapper,_editTask,_contextMenuFactory,
            _formulaBuilderDTOMapper,_editParameterListPresenter,_editFormulaPresenter,_selectReferenceAtTransportPresenter,
            _context,_moleculeListPresenter,_sourceCriteriaPresenter,_targetCriteriaPresenter);
      }
   }


   public class When_creating_the_edit_transport_building_block_presenter : concern_for_EditTransportBuilderPresenter
   {
      [Observation]
      public void should_remove_formula_types_that_do_not_make_sense_for_transports()
      {
         A.CallTo(() => _editFormulaPresenter.RemoveFormulaType<SumFormula>()).MustHaveHappened();
         A.CallTo(() => _editFormulaPresenter.RemoveFormulaType<TableFormula>()).MustHaveHappened();
         A.CallTo(() => _editFormulaPresenter.RemoveFormulaType<TableFormulaWithOffset>()).MustHaveHappened();
      }
   }
}	
