using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTransportBuilderSpecs : ContextSpecification<IEditTransportBuilderPresenter>
   {
      private ITransportBuilderToTransportBuilderDTOMapper _transporBuilderMapper;
      private IEditTaskFor<ITransportBuilder> _taskForPassiveTranportBuilder;
      private IViewItemContextMenuFactory _contexteMenuFactory;
      protected IEditTransportBuilderView _view;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IEditParametersInContainerPresenter _parameterPresenter;
      private IEditFormulaPresenter _formulaPresenter;
      private ISelectReferenceAtTransportPresenter _referencePresenter;
      private IMoBiContext _context;
      private IMoleculeDependentBuilderPresenter _moleculeDepdendentBuilderPresenter;
      private IDescriptorConditionListPresenter<ITransportBuilder> _sourceCriteriaPresenter;
      private IDescriptorConditionListPresenter<ITransportBuilder> _targetCriteriaPresenter;

      protected override void Context()
      {
         _transporBuilderMapper=A.Fake<ITransportBuilderToTransportBuilderDTOMapper>();
         _taskForPassiveTranportBuilder= A.Fake<IEditTasksForBuildingBlock<ITransportBuilder>>();
         _contexteMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _view= A.Fake<IEditTransportBuilderView>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _formulaPresenter = A.Fake<IEditFormulaPresenter>();
         _referencePresenter= A.Fake<ISelectReferenceAtTransportPresenter>();
         _context = A.Fake<IMoBiContext>();
         _moleculeDepdendentBuilderPresenter= A.Fake<IMoleculeDependentBuilderPresenter>();
         _sourceCriteriaPresenter= A.Fake<IDescriptorConditionListPresenter<ITransportBuilder>>();
         _targetCriteriaPresenter = A.Fake<IDescriptorConditionListPresenter<ITransportBuilder>>();
         sut = new EditTransportBuilderPresenter(_view,_transporBuilderMapper, _taskForPassiveTranportBuilder,
                                                        _contexteMenuFactory, _formulaMapper, _parameterPresenter,
                                                        _formulaPresenter, _referencePresenter, _context,
                                                        _moleculeDepdendentBuilderPresenter, _sourceCriteriaPresenter, _targetCriteriaPresenter);
      }
   }

   class When_told_to_select_a_child_parameter : concern_for_EditTransportBuilderSpecs
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
      }
      protected override void Because()
      {
         sut.SelectParameter(_parameter);
      }

      [Observation]
      public void should_tell_view_to_show_parameters()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened();
      }

      [Observation]
      public void should_tell_parameter_presenter_to_select_presenter()
      {
         A.CallTo(() => _parameterPresenter.Select(_parameter)).MustHaveHappened();
      }
   }
}	