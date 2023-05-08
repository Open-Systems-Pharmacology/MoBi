using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IParameterValuesView : IView<IParameterValuesPresenter>, IStartValuesView<ParameterValueDTO>
   {
   }
}