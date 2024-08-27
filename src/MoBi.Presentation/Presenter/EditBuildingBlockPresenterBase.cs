using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class EditBuildingBlockPresenterBase<TView, TPresenter, TBuildingBlock, TBuilder> : SingleStartPresenter<TView, TPresenter>,
      ISingleStartPresenter<TBuildingBlock>,
      IListener<EntitySelectedEvent>
      where TView : IView<TPresenter>, IEditBuildingBlockBaseView
      where TPresenter : IPresenter, ISingleStartPresenter
      where TBuildingBlock : IBuildingBlock, IEnumerable<TBuilder>
      where TBuilder : class
   {
      private readonly IFormulaCachePresenter _formulaCachePresenter;

      public abstract void Edit(TBuildingBlock objectToEdit);

      protected EditBuildingBlockPresenterBase(TView view, IFormulaCachePresenter formulaCachePresenter) : base(view)
      {
         _formulaCachePresenter = formulaCachePresenter;
         _view.SetFormulaCacheView(_formulaCachePresenter.BaseView);
         AddSubPresenters(formulaCachePresenter);
      }

      protected void EditFormulas(IBuildingBlock buildingBlockWithFormulaCache)
      {
         _formulaCachePresenter.Edit(buildingBlockWithFormulaCache);
      }

      public override void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TBuildingBlock>());
      }

      protected TBuildingBlock BuildingBlock => Subject.DowncastTo<TBuildingBlock>();

      protected virtual (bool canHandle, IContainer containerObject) SpecificCanHandle(EntitySelectedEvent entitySelectedEvent)
      {
         return (false, null);
      }

      protected virtual void EnsureItemsVisibility(IContainer container, IParameter parameter = null)
      {
         /*nothing to do here*/
      }

      protected virtual void SelectBuilder(TBuilder builder)
      {
         _view.ShowDefault();
      }

      private void selectObjectAndContainer(IContainer containerObject, IObjectBase selectedObject)
      {
         _view.Display();
         if (selectedObject is IFormula formula)
         {
            _view.ShowFormulas();
            _formulaCachePresenter.Select(formula);
            return;
         }

         _view.ShowDefault();
         if (selectedObject.IsAnImplementationOf<ApplicationMoleculeBuilder>())
         {
            return;
         }

         if (selectedObject is TBuilder builder)
         {
            SelectBuilder(builder);
            return;
         }

         EnsureItemsVisibility(containerObject, selectedObject as IParameter);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         var (canHandle, containerObject) = CanHandle(eventToHandle);
         if (!canHandle)
            return;
         
         selectObjectAndContainer(containerObject, eventToHandle.ObjectBase);
      }

      internal virtual (bool canHandle, IContainer containerObject) CanHandle(EntitySelectedEvent entitySelectedEvent)
      {
         var selectedObject = entitySelectedEvent.ObjectBase;
         if (selectedObject is IFormula formula)
            return (BuildingBlock.FormulaCache.Contains(formula), null);

         if (selectedObject is IParameter parameter)
            return (buildingBlockContains(parameter.RootContainer as TBuilder), parameter.ParentContainer);

         if (selectedObject is TBuilder builder)
            return (BuildingBlock.Contains(builder), null);

         return SpecificCanHandle(entitySelectedEvent);
      }

      private bool buildingBlockContains(TBuilder builder)
      {
         if (builder == null)
            return false;

         return BuildingBlock.Contains(builder);
      }

      internal virtual bool CanHandle(IObjectBaseEvent objectBaseEvent)
      {
         return Equals(objectBaseEvent.ObjectBase, BuildingBlock);
      }
   }
}