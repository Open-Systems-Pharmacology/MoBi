using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class EditBuildingBlockPresenterBase<TView, TPresenter, TBuildingBlock, TBuilder> : SingleStartPresenter<TView, TPresenter>, 
      ISingleStartPresenter<TBuildingBlock>, 
      IListener<EntitySelectedEvent>
      where TView : IView<TPresenter>, IEditBuildingBlockBaseView
      where TPresenter : IPresenter, ISingleStartPresenter
      where TBuildingBlock : IBuildingBlock<TBuilder>
      where TBuilder : class, IObjectBase
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

      protected virtual Tuple<bool, IObjectBase> SpecificCanHandle(IObjectBase selectedObject)
      {
         return new Tuple<bool, IObjectBase>(false, selectedObject);
      }

      protected virtual void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         /*nothing to do here*/
      }

      protected virtual void SelectBuilder(TBuilder builder)
      {
         _view.ShowDefault();
      }

      private void selectObjectAndParent(IObjectBase parentObject, IObjectBase selectedObject)
      {
         _view.Display();
         var formula = selectedObject as IFormula;
         if (formula != null)
         {
            _view.ShowFormulas();
            _formulaCachePresenter.Select(formula);
            return;
         }

         _view.ShowDefault();
         if (selectedObject.IsAnImplementationOf<IApplicationMoleculeBuilder>())
         {
            return;
         }

         var builder = selectedObject as TBuilder;
         if (builder != null)
         {
            SelectBuilder(builder);
            return;
         }

         EnsureItemsVisibility(parentObject, selectedObject as IParameter);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         var handled = CanHandle(eventToHandle.ObjectBase);
         if (!handled.Item1) return;

         selectObjectAndParent(handled.Item2, eventToHandle.ObjectBase);
      }

      internal virtual Tuple<bool, IObjectBase> CanHandle(IObjectBase selectedObject)
      {
         var formula = selectedObject as IFormula;
         if (formula != null)
            return new Tuple<bool, IObjectBase>(BuildingBlock.FormulaCache.Contains(formula), formula);

         var parameter = selectedObject as IParameter;
         if (parameter != null)
            return new Tuple<bool, IObjectBase>(buildingBlockContains(parameter.RootContainer as TBuilder), parameter.ParentContainer);

         var builder = selectedObject as TBuilder;
         if (builder != null)
            return new Tuple<bool, IObjectBase>(BuildingBlock.Contains(builder), builder);

         return SpecificCanHandle(selectedObject);
      }

      private bool buildingBlockContains(TBuilder builder)
      {
         if (builder == null)
            return false;

         return BuildingBlock.Contains(builder);
      }
   }
}