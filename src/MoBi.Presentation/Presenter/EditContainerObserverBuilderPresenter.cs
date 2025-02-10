using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerObserverBuilderPresenter : IEditObserverBuilderPresenter, ICreatePresenter<ContainerObserverBuilder>
   {
   }

   internal class EditContainerObserverBuilderPresenter : EditObserverBuilderPresenter<ContainerObserverBuilder>, IEditContainerObserverBuilderPresenter
   {
      private readonly IObserverBuilderToDTOObserverBuilderMapper _observerBuilderMapper;

      public EditContainerObserverBuilderPresenter(IEditObserverBuilderView view, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory, IObserverBuilderToDTOObserverBuilderMapper observerBuilderMapper, IEditTaskFor<ContainerObserverBuilder> editTasks, IEditFormulaInContainerPresenter editFormulaPresenter, ISelectReferenceAtContainerObserverPresenter selectReferencePresenter, IMoleculeDependentBuilderPresenter moleculeListPresenter, IDescriptorConditionListPresenter<ObserverBuilder> descriptorConditionListPresenter) :
         base(view, editFormulaPresenter, selectReferencePresenter, context, viewItemContextMenuFactory, editTasks, moleculeListPresenter, descriptorConditionListPresenter)
      {
         _observerBuilderMapper = observerBuilderMapper;
      }

      protected override ObserverBuilderDTO MapFrom(ContainerObserverBuilder observerBuilder) => _observerBuilderMapper.MapFrom(observerBuilder);
   }
}