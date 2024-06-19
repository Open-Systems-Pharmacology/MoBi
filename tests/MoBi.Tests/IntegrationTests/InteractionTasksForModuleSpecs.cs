using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Domain.Services;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.Core;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Tasks;
using MoBi.Presentation;
using NUnit.Framework;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.IntegrationTests
{
   public class concern_for_InteractionTasksForModule : ContextForIntegration<InteractionTasksForModule>
   {
      private Module _module;
      private ISelectBuildingBlockTypePresenter _selectTypePresenter;
      private IInteractionTaskContext _context;
      private IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();

         var interactionTask = new InteractionTask(IoC.Resolve<ISerializationTask>(), _dialogCreator, A.Fake<IIconRepository>(),
            A.Fake<INameCorrector>(), new CloneManagerForBuildingBlockForSpecs(),
            A.Fake<IAdjustFormulasVisitor>(), A.Fake<IObjectTypeResolver>(), A.Fake<IForbiddenNamesRetriever>()
         );

         _context = new InteractionTaskContext(A.Fake<IMoBiContext>(), A.Fake<IMoBiApplicationController>(),
            interactionTask, A.Fake<IActiveSubjectRetriever>(), A.Fake<IUserSettings>(),
            A.Fake<IDisplayUnitRetriever>(), _dialogCreator,
            A.Fake<ICommandTask>(), A.Fake<IObjectTypeResolver>(), A.Fake<IMoBiFormulaTask>(),
            A.Fake<IMoBiConfiguration>(), A.Fake<DirectoryMapSettings>(), A.Fake<ICheckNameVisitor>(), A.Fake<IBuildingBlockRepository>(),
            A.Fake<IObjectBaseNamingTask>());

         _selectTypePresenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_selectTypePresenter);
         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._)).Returns(DomainHelperForSpecs.TestFileFullPath("Sim_V12.pkml"));
         _module = new Module();

         sut = new InteractionTasksForModule(_context, new EditTaskForModule(_context));
      }

      [TestCase(BuildingBlockType.SpatialStructure)]
      [TestCase(BuildingBlockType.Reaction)]
      [TestCase(BuildingBlockType.EventGroup)]
      [TestCase(BuildingBlockType.PassiveTransport)]
      [TestCase(BuildingBlockType.Molecule)]
      [TestCase(BuildingBlockType.Observer)]
      [TestCase(BuildingBlockType.InitialConditions)]
      [TestCase(BuildingBlockType.ParameterValues)]
      public void the_building_block_is_loaded_to_the_module(BuildingBlockType type)
      {
         A.CallTo(() => _selectTypePresenter.GetBuildingBlockType(A<Module>._)).Returns(type);
         sut.LoadBuildingBlocksToModule(_module);
         _module.BuildingBlocks.Count.ShouldBeEqualTo(1);
      }
   }
}
