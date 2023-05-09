using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForSpatialStructure : ContextSpecification<InteractionTasksForSpatialStructure>
   {
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         sut = new InteractionTasksForSpatialStructure(
            _interactionTaskContext,
            A.Fake<IEditTasksForSpatialStructure>(),
            A.Fake<IMoBiSpatialStructureFactory>());
      }
   }

   public class When_removing_spatial_structure_building_block_that_is_referred_to_in_another_building_block : concern_for_InteractionTasksForSpatialStructure
   {
      private MoBiSpatialStructure _spatialStructure;
      private Module _module;
      private MoBiProject _moBiProject;
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moBiProject = DomainHelperForSpecs.NewProject();
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject).Returns(_moBiProject);
         _spatialStructure = new MoBiSpatialStructure() { Id = "1" };
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock { SpatialStructureId = _spatialStructure.Id, MoleculeBuildingBlockId = "" };
         _module = new Module
         {
            _initialConditionsBuildingBlock,
            _spatialStructure
         };
         _moBiProject.AddModule(_module);
         _moBiProject.AddBuildingBlock(_spatialStructure);
         _moBiProject.AddBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void should_throw_mobi_exception()
      {
         The.Action(() => sut.Remove(_spatialStructure, _module, null, false)).ShouldThrowAn<MoBiException>();
      }
   }
}
