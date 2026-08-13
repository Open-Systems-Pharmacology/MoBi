using FakeItEasy;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_RemovedNeighborhoodsDialogTask : ContextSpecification<IRemovedNeighborhoodsDialogTask>
   {
      protected IDialogCreator _dialogCreator;
      protected ValidationResult _validationResult;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _validationResult = new ValidationResult();
         sut = new RemovedNeighborhoodsDialogTask(_dialogCreator);
      }
   }

   public class When_showing_the_removed_neighborhoods_for_a_validation_result_with_removal_warnings : concern_for_RemovedNeighborhoodsDialogTask
   {
      protected override void Context()
      {
         base.Context();
         var neighborhoodWithoutNeighbors = new NeighborhoodBuilder().WithName("removed_neighborhood");
         _validationResult.AddMessage(NotificationType.Warning, neighborhoodWithoutNeighbors, "The neighborhood 'removed_neighborhood' was removed");

         //a warning for a neighborhood with neighbors (e.g. a logical neighbor warning) is not a removal
         var connectedNeighborhood = new NeighborhoodBuilder().WithName("connected_neighborhood");
         connectedNeighborhood.FirstNeighborPath = new ObjectPath("A");
         connectedNeighborhood.SecondNeighborPath = new ObjectPath("B");
         _validationResult.AddMessage(NotificationType.Warning, connectedNeighborhood, "Container 'A' is defined as logical");
      }

      protected override void Because()
      {
         sut.ShowRemovedNeighborhoodsFrom(_validationResult);
      }

      [Observation]
      public void should_show_a_dialog_describing_only_the_removed_neighborhoods()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>.That.Matches(x =>
            x.Contains("The neighborhood 'removed_neighborhood' was removed") &&
            !x.Contains("logical")))).MustHaveHappened();
      }
   }

   public class When_showing_the_removed_neighborhoods_for_a_validation_result_without_removal_warnings : concern_for_RemovedNeighborhoodsDialogTask
   {
      protected override void Context()
      {
         base.Context();
         var connectedNeighborhood = new NeighborhoodBuilder().WithName("connected_neighborhood");
         connectedNeighborhood.FirstNeighborPath = new ObjectPath("A");
         connectedNeighborhood.SecondNeighborPath = new ObjectPath("B");
         _validationResult.AddMessage(NotificationType.Warning, connectedNeighborhood, "Container 'A' is defined as logical");
      }

      protected override void Because()
      {
         sut.ShowRemovedNeighborhoodsFrom(_validationResult);
      }

      [Observation]
      public void should_not_show_a_dialog()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }
   }

   public class When_showing_the_removed_neighborhoods_for_an_undefined_validation_result : concern_for_RemovedNeighborhoodsDialogTask
   {
      protected override void Because()
      {
         sut.ShowRemovedNeighborhoodsFrom(null);
      }

      [Observation]
      public void should_not_show_a_dialog()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }
   }
}
