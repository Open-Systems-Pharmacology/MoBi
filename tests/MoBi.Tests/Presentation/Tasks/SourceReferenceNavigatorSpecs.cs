using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SourceReferenceNavigator : ContextSpecification<SourceReferenceNavigator>
   {
      protected IMoBiContext _context;
      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         sut = new SourceReferenceNavigator(_applicationController, _context);
      }
   }

   public class When_navigating_to_entity_source : concern_for_SourceReferenceNavigator
   {
      private SimulationEntitySourceReference _reference;
      private IEntity _source;
      private IBuildingBlock _buildingBlock;
      private Module _module;
      private IEntity _simulationEntity;

      protected override void Context()
      {
         base.Context();
         _source = new Container();
         _buildingBlock = new IndividualBuildingBlock();
         _module = new Module();
         _simulationEntity = new Container();
         _reference = new SimulationEntitySourceReference(_source, _buildingBlock, _module, _simulationEntity);
      }

      protected override void Because()
      {
         sut.GoTo(_reference);
      }

      [Observation]
      public void the_application_controller_opens_the_building_block_and_navigates()
      {
         A.CallTo(() => _applicationController.Select(_reference.Source, _buildingBlock, _context.HistoryManager)).MustHaveHappened();
      }
   }

}