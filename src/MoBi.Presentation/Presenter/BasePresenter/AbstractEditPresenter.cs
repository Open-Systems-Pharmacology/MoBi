using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter.BasePresenter
{
   public abstract class AbstractEditPresenter<TView, TPresenter, TObject> : AbstractCommandCollectorPresenter<TView, TPresenter>,
      IEditPresenter<TObject>
      where TView : IView<TPresenter>
      where TPresenter : IPresenter
   {
      protected AbstractEditPresenter(TView view) : base(view)
      {
      }

      public abstract void Edit(TObject objectToEdit);

      public abstract object Subject { get; }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TObject>());
      }
   }

   public abstract class AbstractEntityEditPresenter<TView, TPresenter, TEntity> : AbstractEditPresenter<TView, TPresenter, TEntity>,
      ICreatePresenter<TEntity>
      where TView : IView<TPresenter>
      where TPresenter : IPresenter
      where TEntity : IEntity
   {
      protected AbstractEntityEditPresenter(TView view) : base(view)
      {
      }

      public abstract void Edit(TEntity entity, IEnumerable<IObjectBase> existingObjectsInParent);

      public override void Edit(TEntity entity)
      {
         Edit(entity, entity.ParentContainer);
      }
   }
}