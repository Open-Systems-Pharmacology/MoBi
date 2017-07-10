using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_OutputSelectionsRetriever : ContextSpecification<IOutputSelectionsRetriever>
   {
      protected IMoBiApplicationController _applicationController;
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _applicationController = A.Fake<IMoBiApplicationController>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         sut = new OutputSelectionsRetriever(_applicationController, _entityPathResolver);
      }
   }

   public class When_the_output_selection_retriever_is_retrieving_the_output_for_a_given_simulation : concern_for_OutputSelectionsRetriever
   {
      private IMoBiSimulation _simulation;
      private OutputSelections _result;
      private IOutputSelectionsPresenter _presenter;
      private OutputSelections _outputSelection;

      protected override void Context()
      {
         base.Context();
         _outputSelection = new OutputSelections();
         _simulation = A.Fake<IMoBiSimulation>();
         _presenter = A.Fake<IOutputSelectionsPresenter>();
         A.CallTo(() => _presenter.StartSelection(_simulation)).Returns(_outputSelection);
         A.CallTo(() => _applicationController.Start<IOutputSelectionsPresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         _result = sut.OutputSelectionsFor(_simulation);
      }

      [Observation]
      public void should_start_the_output_selection_presenter_with_the_given_simulation_and_return_the_user_selection()
      {
         _result.ShouldBeEqualTo(_outputSelection);
      }
   }

   public class When_the_output_selection_retriever_is_creating_a_quantity_selection_for_a_given_quantity : concern_for_OutputSelectionsRetriever
   {
      private IQuantity _quantity;
      private QuantitySelection _result;

      protected override void Context()
      {
         base.Context();
         _quantity = A.Fake<IQuantity>();
         _quantity.QuantityType = QuantityType.Metabolite;
         A.CallTo(() => _entityPathResolver.PathFor(_quantity)).Returns("QUANTITY_PATH");
      }

      protected override void Because()
      {
         _result = sut.SelectionFrom(_quantity);
      }

      [Observation]
      public void should_return_a_quantity_selection_having_the_quantity_path_for_the_quantity()
      {
         _result.Path.ShouldBeEqualTo("QUANTITY_PATH");
      }

      [Observation]
      public void should_return_a_quantity_selection_having_the_quantity_type_for_the_quantity()
      {
         _result.QuantityType.ShouldBeEqualTo(QuantityType.Metabolite);
      }
   }

   public class When_the_output_selection_retriever_is_updated_the_persistable_outputs_in_a_simulation : concern_for_OutputSelectionsRetriever
   {
      private IMoBiSimulation _simulation;
      private Parameter _p1;
      private OutputSelections _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = new OutputSelections();
         _simulation = A.Fake<IMoBiSimulation>();
         _p1 = new Parameter {Persistable = true};
         A.CallTo(() => _entityPathResolver.PathFor(_p1)).Returns("PARAMETER");
         _simulation.Model.Root = new Container {_p1};
         A.CallTo(() => _simulation.OutputSelections).Returns(_outputSelections);
      }

      protected override void Because()
      {
         sut.UpdatePersistableOutputsIn(_simulation);
      }

      [Observation]
      public void should_add_the_parameters_to_the_output_selection()
      {
         _outputSelections.AllOutputs.Find(x => x.Path == "PARAMETER").ShouldNotBeNull();
      }
   }
}