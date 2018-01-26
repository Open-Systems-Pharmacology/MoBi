using System;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditValueOriginPresenter : IPresenter<IEditValueOriginView>
   {
      void Edit(IWithValueOrigin withValueOrigin);
      Action<ValueOrigin> ValueOriginUpdated { get; set; }
      Func<bool> ValueOriginEditable { get; set; }
      bool ShowCaption { get; set; }
   }

   public class EditValueOriginPresenter : AbstractPresenter<IEditValueOriginView, IEditValueOriginPresenter>, IEditValueOriginPresenter
   {
      private ValueOriginDTO _valueOriginDTO;
      public Action<ValueOrigin> ValueOriginUpdated { get; set; } = x => { };
      public Func<bool> ValueOriginEditable { get; set; } = () => true;

      public bool ShowCaption
      {
         get => _view.ShowCaption;
         set => _view.ShowCaption = value;
      }

      public EditValueOriginPresenter(IEditValueOriginView view) : base(view)
      {
      }

      public void Edit(IWithValueOrigin withValueOrigin)
      {
         if (withValueOrigin == null)
            return;

         _valueOriginDTO = new ValueOriginDTO(withValueOrigin);
         _view.BindTo(_valueOriginDTO);
      }
   }
}