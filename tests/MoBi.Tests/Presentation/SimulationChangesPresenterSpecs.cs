using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public class concern_for_SimulationChangesPresenter : ContextSpecification<SimulationChangesPresenter>
   {
      protected ISimulationChangesView _view;

      protected override void Context()
      {
         _view = A.Fake<ISimulationChangesView>();
         sut = new SimulationChangesPresenter(_view);
      }
   }

   public class when_editing_a_simulation : concern_for_SimulationChangesPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         var container = new Container().WithName("top");
         container.Add(new Parameter().WithName("Parameter"));
         _simulation.Model = new Model{Root = new Container()};
         
         _simulation.Model.Root.Add(container);
         _simulation.AddOriginalQuantityValue(new OriginalQuantityValue {Path = "top|Parameter"}.WithDimension(A.Fake<IDimension>()));
         _simulation.AddOriginalQuantityValue(new OriginalQuantityValue {Path = "top|second"}.WithDimension(A.Fake<IDimension>()));
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_bind_the_view_to_the_simulation_original_quantity_values()
      {
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<OriginalQuantityValueDTO>>.That.Matches(x => hasMatchingQuantitiesOnly(x)))).MustHaveHappened();
      }

      private bool hasMatchingQuantitiesOnly(IReadOnlyList<OriginalQuantityValueDTO> originalQuantityValueDTOs)
      {
         return originalQuantityValueDTOs.Count == 1 && originalQuantityValueDTOs.SingleOrDefault(x => Equals(x.Path, "top|Parameter")) != null;
      }
   }
}
