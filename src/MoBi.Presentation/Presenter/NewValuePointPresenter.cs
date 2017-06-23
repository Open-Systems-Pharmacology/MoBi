using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface INewValuePointPresenter : IDisposablePresenter
   {
      IDimension Dimension { get; set; }
      ValuePoint GetNewValuePoint();
   }

   internal class NewValuePointPresenter : AbstractDisposablePresenter<INewValuePointView, INewValuePointPresenter>, INewValuePointPresenter
   {
      private readonly IMoBiContext _context;
      private ValuePointDTO _valuePointDTO;
      public IDimension Dimension { get; set; }

      public NewValuePointPresenter(INewValuePointView view, IMoBiContext context) : base(view)
      {
         _context = context;
      }

      public ValuePoint GetNewValuePoint()
      {
         _valuePointDTO = new ValuePointDTO
         {
            X = new ValueEditDTO {Dimension = _context.DimensionFactory.Dimension(Constants.Dimension.TIME)},
            Y = new ValueEditDTO {Dimension = Dimension}
         };

         _view.BindTo(_valuePointDTO);

         _view.Display();
         if (_view.Canceled) 
            return null;

         return new ValuePoint(_valuePointDTO.X.KernelValue, _valuePointDTO.Y.KernelValue);
      }
   }
}