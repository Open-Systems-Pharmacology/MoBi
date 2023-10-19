using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   internal class concern_for_InteractionTasksForContainer : ContextSpecification<InteractionTasksForContainer>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      private IEditTaskFor<IContainer> _editTask;
      private IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _editTask = A.Fake<IEditTaskForContainer>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();

         sut = new InteractionTasksForContainer(_interactionTaskContext, _editTask, _objectPathFactory);
      }
   }

   internal class When_importing_a_container_into_spatial_structure_and_there_is_an_existing_container_with_the_same_path : concern_for_InteractionTasksForContainer
   {
      private MoBiSpatialStructure _spatialStructure;
      private IContainer _topContainer;
      private IContainer _importedContainer;
      private MoBiSpatialStructure _importedSpatialStructure;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new MoBiSpatialStructure().WithName("Organism");
         _spatialStructure.DiagramManager = new SpatialStructureDiagramManager();
         _spatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);
         _topContainer = new Container().WithName("Organism");
         _spatialStructure.AddTopContainer(_topContainer);
         var existingContainer = new Container().WithName("Muscle");
         existingContainer.Add(new Container().WithName("BloodCells"));
         _topContainer.Add(existingContainer);
         _importedContainer = new Container().WithName("Muscle");
         _importedContainer.Add(new Container().WithName("BloodCells"));
         _importedContainer.ParentPath = new ObjectPath("Organism");
         _importedSpatialStructure = new MoBiSpatialStructure();
         _importedSpatialStructure.AddTopContainer(_importedContainer);
         _importedSpatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);

         _topContainer.GetAllContainersAndSelf<IContainer>().Each(x => x.Mode = ContainerMode.Physical);
         _importedContainer.GetAllContainersAndSelf<IContainer>().Each(x => x.Mode = ContainerMode.Physical);

         addCollidingNeighborhoodsTo(_importedSpatialStructure);
         addCollidingNeighborhoodsTo(_spatialStructure);

         var filePath = "filepath";
         A.CallTo(() => _interactionTaskContext.InteractionTask.AskForFileToOpen(A<string>._, A<string>._, A<string>._)).Returns(filePath);
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadItems<MoBiSpatialStructure>(filePath)).Returns(new[] { _importedSpatialStructure });
         A.CallTo(() => _interactionTaskContext.Active<MoBiSpatialStructure>()).Returns(_spatialStructure);
         A.CallTo(() => _interactionTaskContext.InteractionTask.CorrectName(_importedContainer, A<IEnumerable<string>>._)).Invokes(x => x.Arguments.Get<IContainer>(0).Name = "Tumour");
      }

      private void addCollidingNeighborhoodsTo(MoBiSpatialStructure spatialStructure)
      {
         spatialStructure.AddNeighborhood(new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("Organism", "Muscle", "BloodCells"),
            SecondNeighborPath = new ObjectPath("Organism", "SomethingElse")
         }.WithName("Muscle_bls_SomethingElse"));
      }

      protected override void Because()
      {
         sut.AddExisting(_topContainer, _spatialStructure);
      }

      [Observation]
      public void neighborhoods_with_renamed_container_should_be_renamed()
      {
         _importedSpatialStructure.Neighborhoods.First().FirstNeighborPath.PathAsString.ShouldBeEqualTo("Organism|Tumour|BloodCells");
      }

      [Observation]
      public void the_colliding_neighborhoods_should_be_renamed()
      {
         _importedSpatialStructure.Neighborhoods.First().Name.ShouldBeEqualTo("Tumour_bls_SomethingElse");
      }

      [Observation]
      public void the_name_corrector_must_be_used_to_rename_the_container()
      {
         _importedContainer.Name.ShouldBeEqualTo("Tumour");
      }
   }
}