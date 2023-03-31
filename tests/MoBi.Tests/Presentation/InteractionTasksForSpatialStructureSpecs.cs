using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using MoBi.Helpers;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForSpatialStructure : ContextSpecification<InteractionTasksForSpatialStructure>
   {
      protected override void Context()
      {
         sut = new InteractionTasksForSpatialStructure(
            A.Fake<IInteractionTaskContext>(),
            A.Fake<IEditTasksForSpatialStructure>(),
            A.Fake<IMoBiSpatialStructureFactory>());
      }
   }

   public class When_removing_spatial_structure_building_block_that_is_referred_to_in_another_building_block : concern_for_InteractionTasksForSpatialStructure
   {
      private IMoBiSpatialStructure _spatialStructure;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new MoBiSpatialStructure() { Id = "1" };
         _project = DomainHelperForSpecs.NewProject();

         _project.AddBuildingBlock(new MoleculeStartValuesBuildingBlock { SpatialStructureId = _spatialStructure.Id, MoleculeBuildingBlockId = "" });
      }

      [Observation]
      public void should_throw_mobi_exception()
      {
         The.Action(() => sut.Remove(_spatialStructure, _project, null, false)).ShouldThrowAn<MoBiException>();
      }
   }
}
