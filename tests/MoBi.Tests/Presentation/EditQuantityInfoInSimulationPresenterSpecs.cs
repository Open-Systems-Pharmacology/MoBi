using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditQuantityInfoInSimulationPresenter : ContextSpecification<EditQuantityInfoInSimulationPresenter>
   {
      protected ISourceReferenceNavigator _sourceReferenceNavigator;
      private IEditQuantityInfoInSimulationView _view;

      protected override void Context()
      {
         _view = A.Fake<IEditQuantityInfoInSimulationView>();
         _sourceReferenceNavigator = A.Fake<ISourceReferenceNavigator>();
         sut = new EditQuantityInfoInSimulationPresenter(_view, _sourceReferenceNavigator);
      }
   }

   public class When_navigating_to_the_source_of_the_quantity : concern_for_EditQuantityInfoInSimulationPresenter
   {
      private QuantityDTO _quantityDTO;

      protected override void Context()
      {
         base.Context();
         var moleculeAmount = new MoleculeAmount();
         _quantityDTO = new QuantityDTO(moleculeAmount) {SourceReference = new SimulationEntitySourceReference(null, null, null, moleculeAmount)};
         sut.Edit(_quantityDTO);
      }

      protected override void Because()
      {
         sut.NavigateToQuantitySource();
      }

      [Observation]
      public void the_navigation_task_is_used_to_navigate()
      {
         A.CallTo(() => _sourceReferenceNavigator.GoTo(_quantityDTO.SourceReference)).MustHaveHappened();
      }
   }
   
}