using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditAmountObserverBuilderPresenter : IEditObserverBuilderPresenter, ICreatePresenter<IAmountObserverBuilder>
   {
   }

   public class EditAmountObserverBuilderPresenter : EditObserverBuilderPresenter<IAmountObserverBuilder>, IEditAmountObserverBuilderPresenter
   {
      private readonly IObserverBuilderToDTOObserverBuilderMapper _observerBuilderMapper;

      public EditAmountObserverBuilderPresenter(
         IEditObserverBuilderView view,
         IMoBiContext context,
         IEditTaskFor<IAmountObserverBuilder> editTasks,
         IObserverBuilderToDTOObserverBuilderMapper observerBuilderMapper,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IEditFormulaPresenter editFormulaPresenter,
         ISelectReferenceAtAmountObserverPresenter selectReferencePresenter,
         IMoleculeDependentBuilderPresenter moleculeListPresenter,
         IDescriptorConditionListPresenter<IObserverBuilder> descriptorConditionListPresenter)
         : base(view, editFormulaPresenter, selectReferencePresenter, context, viewItemContextMenuFactory, editTasks, moleculeListPresenter, descriptorConditionListPresenter)
      {
         _observerBuilderMapper = observerBuilderMapper;
      }

      protected override ObserverBuilderDTO MapFrom(IAmountObserverBuilder observerBuilder)
      {
         return _observerBuilderMapper.MapFrom(observerBuilder);
      }
   }
}