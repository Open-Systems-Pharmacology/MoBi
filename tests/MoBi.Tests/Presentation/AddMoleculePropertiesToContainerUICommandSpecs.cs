using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_AddMoleculePropertiesToContainerUICommand : ContextSpecification<AddMoleculePropertiesToContainerUICommand>
   {
      protected IMoBiContext _context;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IContainer _container;
      protected MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _container = new Container().WithName("Muscle").WithMode(ContainerMode.Physical);
         _spatialStructure = new MoBiSpatialStructure
         {
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };
         _spatialStructure.AddTopContainer(_container);

         A.CallTo(() => _context.Create<IContainer>()).Returns(new Container());
         A.CallTo(() => _activeSubjectRetriever.Active<MoBiSpatialStructure>()).Returns(_spatialStructure);

         sut = new AddMoleculePropertiesToContainerUICommand(_context, _activeSubjectRetriever);
         sut.For(_container);
      }
   }

   public class When_adding_molecule_properties_to_a_physical_container : concern_for_AddMoleculePropertiesToContainerUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_add_a_molecule_properties_container_to_the_container()
      {
         _container.Container(Constants.MOLECULE_PROPERTIES).ShouldNotBeNull();
      }

      [Observation]
      public void should_add_the_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustHaveHappened();
      }
   }
}
