using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IUserDefinedParametersPresenter : IPresenter<IUserDefinedParametersView>
   {
      void ShowUserDefinedParametersIn(IContainer container);
      void ShowUserDefinedParametersIn(IEnumerable<IContainer> containers);
      Action<IEditParameterListPresenter> ColumnConfiguration { get; set; }
   }

   public class UserDefinedParametersPresenter : AbstractCommandCollectorPresenter<IUserDefinedParametersView, IUserDefinedParametersPresenter>, IUserDefinedParametersPresenter
   {
      private readonly IEditParameterListPresenter _editParameterListPresenter;
      private IEnumerable<IContainer> _containers;
      public Action<IEditParameterListPresenter> ColumnConfiguration { get; set; } = (x) => { };

      public UserDefinedParametersPresenter(
         IUserDefinedParametersView view,
         IEditParameterListPresenter editParameterListPresenter) : base(view)
      {
         _editParameterListPresenter = editParameterListPresenter;
         View.AddParametersView(_editParameterListPresenter.BaseView);
         AddSubPresenters(_editParameterListPresenter);
      }

      public void ShowUserDefinedParametersIn(IContainer container)
      {
         ShowUserDefinedParametersIn(new[] {container});
      }

      public void ShowUserDefinedParametersIn(IEnumerable<IContainer> containers)
      {
         _containers = containers;
         refreshView();
      }

      private void refreshView()
      {
         var parameters = _containers
            .SelectMany(c => c.GetAllChildren<IParameter>(x => !x.IsDefault));

         _editParameterListPresenter.Edit(parameters);

         ColumnConfiguration(_editParameterListPresenter);
      }
   }
}