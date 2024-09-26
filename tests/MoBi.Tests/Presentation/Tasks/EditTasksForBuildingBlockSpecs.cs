using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTasksForBuildingBlock : ContextSpecification<EditTasksForBuildingBlock<IBuildingBlock>>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      private IMoBiContext _context;
      private MoBiProject _project;
      protected SpatialStructure _spatialStructure;
      protected IInteractionTask _interactionTask;
      protected BuildingBlock _buildingBlock;

      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _context = A.Fake<IMoBiContext>();
         _project = DomainHelperForSpecs.NewProject();
         _spatialStructure = new SpatialStructure();
         _spatialStructure.Name = "TestName";
         _interactionTask = A.Fake<IInteractionTask>();
         _applicationController = A.Fake<IMoBiApplicationController>();

         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(A<IObjectBase>.Ignored, A<IReadOnlyList<string>>.Ignored)).Returns("Test");

         _project.AddModule(new Module()
         {
            _spatialStructure
         });

         sut = new EditTasksForBuildingBlock<IBuildingBlock>(_interactionTaskContext);
      }
   }

   public class When_renaming_a_buildingblock : concern_for_EditTasksForBuildingBlock
   {
      protected override void Because()
      {
         sut.Rename(_spatialStructure, Enumerable.Empty<IObjectBase>(), _spatialStructure);
      }

      [Observation]
      public void check_usages_for_must_not_have_happened()
      {
         A.CallTo(() => _interactionTaskContext.CheckNamesVisitor.GetPossibleChangesFrom(A<IObjectBase>.Ignored, A<string>.Ignored, A<IBuildingBlock>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
      }
   }
}