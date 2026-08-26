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
   public abstract class concern_for_RemoveGlobalMoleculePropertiesUICommand : ContextSpecification<RemoveGlobalMoleculePropertiesUICommand>
   {
      protected IMoBiContext _context;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IDialogCreator _dialogCreator;
      protected MoBiSpatialStructure _spatialStructure;
      protected IContainer _moleculeProperties;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical);
         _spatialStructure = new MoBiSpatialStructure
         {
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>(),
            GlobalMoleculeDependentProperties = _moleculeProperties
         };

         A.CallTo(() => _activeSubjectRetriever.Active<MoBiSpatialStructure>()).Returns(_spatialStructure);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.Yes);

         sut = new RemoveGlobalMoleculePropertiesUICommand(_context, _activeSubjectRetriever, _dialogCreator);
         sut.For(_moleculeProperties);
      }
   }

   public class When_removing_the_global_molecule_properties_from_a_spatial_structure : concern_for_RemoveGlobalMoleculePropertiesUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_remove_the_molecule_properties_container_from_the_spatial_structure()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.ShouldBeNull();
      }

      [Observation]
      public void should_add_the_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustHaveHappened();
      }
   }

   public class When_the_user_cancels_the_removal_of_the_global_molecule_properties : concern_for_RemoveGlobalMoleculePropertiesUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_keep_the_molecule_properties_container()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.ShouldBeEqualTo(_moleculeProperties);
      }

      [Observation]
      public void should_not_add_a_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustNotHaveHappened();
      }
   }

   public class When_removing_a_molecule_properties_container_that_is_not_the_global_one : concern_for_RemoveGlobalMoleculePropertiesUICommand
   {
      protected override void Context()
      {
         base.Context();
         sut.For(new Container().WithName(Constants.MOLECULE_PROPERTIES));
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_keep_the_global_molecule_properties_container()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.ShouldBeEqualTo(_moleculeProperties);
      }

      [Observation]
      public void should_not_add_a_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(A<OSPSuite.Core.Commands.Core.ICommand>._)).MustNotHaveHappened();
      }
   }
}