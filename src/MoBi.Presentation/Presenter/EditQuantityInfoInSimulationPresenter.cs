using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using static MoBi.Assets.AppConstants.UsedCalculationMethods.Categories;

namespace MoBi.Presentation.Presenter
{
   public interface IEditQuantityInfoInSimulationPresenter : IPresenter<IEditQuantityInfoInSimulationView>
   {
      void Edit(QuantityDTO quantityDTO);
      void NavigateToQuantitySource();
   }

   public class EditQuantityInfoInSimulationPresenter : AbstractPresenter<IEditQuantityInfoInSimulationView, IEditQuantityInfoInSimulationPresenter>, IEditQuantityInfoInSimulationPresenter
   {
      private QuantityDTO _quantityDTO;
      private readonly ISourceReferenceNavigator _sourceReferenceNavigator;

      public EditQuantityInfoInSimulationPresenter(IEditQuantityInfoInSimulationView editQuantityInfoInSimulationView,
         ISourceReferenceNavigator sourceReferenceNavigator) : base(editQuantityInfoInSimulationView)
      {
         _sourceReferenceNavigator = sourceReferenceNavigator;
      }

      public void Edit(QuantityDTO quantityDTO)
      {
         _quantityDTO = quantityDTO;
         _view.BindTo(quantityDTO);
         if(quantityDTO.UsedCalculationMethods.Any())
            _view.BindTo(quantityDTO.UsedCalculationMethods.Where(x => x.Category.IsOneOf(DiffusionIntCell, DistributionCellular)).ToList());
      }

      public void NavigateToQuantitySource() => _sourceReferenceNavigator.GoTo(_quantityDTO.SourceReference);
   }
}