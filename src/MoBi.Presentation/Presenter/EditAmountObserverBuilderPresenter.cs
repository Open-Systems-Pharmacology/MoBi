using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditAmountObserverBuilderPresenter : IEditObserverBuilderPresenter, ICreatePresenter<AmountObserverBuilder>
   {
   }

   public class EditAmountObserverBuilderPresenter : EditObserverBuilderPresenter<AmountObserverBuilder>, IEditAmountObserverBuilderPresenter
   {
      private readonly IObserverBuilderToDTOObserverBuilderMapper _observerBuilderMapper;

      public EditAmountObserverBuilderPresenter(
         IEditObserverBuilderView view,
         IMoBiContext context,
         IEditTaskFor<AmountObserverBuilder> editTasks,
         IObserverBuilderToDTOObserverBuilderMapper observerBuilderMapper,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IEditFormulaPresenter editFormulaPresenter,
         ISelectReferenceAtAmountObserverPresenter selectReferencePresenter,
         IMoleculeDependentBuilderPresenter moleculeListPresenter,
         IDescriptorConditionListPresenter<ObserverBuilder> descriptorConditionListPresenter)
         : base(view, editFormulaPresenter, selectReferencePresenter, context, viewItemContextMenuFactory, editTasks, moleculeListPresenter, descriptorConditionListPresenter)
      {
         _observerBuilderMapper = observerBuilderMapper;
      }

      protected override ObserverBuilderDTO MapFrom(AmountObserverBuilder observerBuilder)
      {
         return _observerBuilderMapper.MapFrom(observerBuilder);
      }
   }
}