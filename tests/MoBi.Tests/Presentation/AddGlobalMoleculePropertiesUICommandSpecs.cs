using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_AddGlobalMoleculePropertiesUICommand : ContextSpecification<AddGlobalMoleculePropertiesUICommand>
   {
      protected IMoBiContext _context;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _spatialStructure = new MoBiSpatialStructure
         {
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };

         A.CallTo(() => _activeSubjectRetriever.Active<MoBiSpatialStructure>()).Returns(_spatialStructure);
         A.CallTo(() => _spatialStructureFactory.CreateGlobalMoleculeDependentProperties())
            .Returns(new Container().WithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical));

         sut = new AddGlobalMoleculePropertiesUICommand(_activeSubjectRetriever, _context, _spatialStructureFactory);
      }
   }

   public class When_adding_the_global_molecule_properties_to_a_spatial_structure : concern_for_AddGlobalMoleculePropertiesUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_add_a_molecule_properties_container_to_the_spatial_structure()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.Name.ShouldBeEqualTo(Constants.MOLECULE_PROPERTIES);
      }

      [Observation]
      public void should_add_the_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustHaveHappened();
      }
   }

   public class When_adding_the_global_molecule_properties_to_a_spatial_structure_that_already_has_them : concern_for_AddGlobalMoleculePropertiesUICommand
   {
      protected override void Context()
      {
         base.Context();
         _spatialStructure.GlobalMoleculeDependentProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_not_add_another_molecule_properties_container()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustNotHaveHappened();
      }
   }
}