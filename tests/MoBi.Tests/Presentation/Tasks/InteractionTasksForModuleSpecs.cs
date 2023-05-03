using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_InteractionTasksForModule : ContextSpecification<InteractionTasksForModule>
   {
      protected IInteractionTaskContext _context;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         sut = new InteractionTasksForModule(_context, new EditTaskForModule(_context));
      }
   }

   public class When_creating_a_new_module : concern_for_InteractionTasksForModule
   {
      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_application_controller_should_create_new_presenter()
      {
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).MustHaveHappened();
      }
   }

   public class When_the_presenter_doesnt_return_a_module : concern_for_InteractionTasksForModule
   {
      private ICreateModulePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateModulePresenter>();
         A.CallTo(() => _presenter.CreateModule()).Returns(null);
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_result_should_be_null()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<IMoBiCommand>._)).MustNotHaveHappened();
      }
   }

   public class When_the_presenter_returns_a_module : concern_for_InteractionTasksForModule
   {
      private ICreateModulePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateModulePresenter>();
         A.CallTo(() => _presenter.CreateModule()).Returns(new Module());
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_result_should_be_null()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<AddModuleCommand>._)).MustHaveHappened();
      }
   }

   public class When_deleting_a_module : concern_for_InteractionTasksForModule
   {
      private Module _module;

      protected override void Because()
      {
         _module = new Module();
         sut.RemoveModule(_module);
      }

      [Observation]
      public void the_module_should_be_unregistered_from_the_context()
      {
         A.CallTo(() => _context.Context.Unregister(_module)).MustHaveHappened();
      }
   }

   public class When_loading_from_PKML_and_the_dialog_gets_canceled : concern_for_InteractionTasksForModule
   {
      private Module _module;
      private ISelectBuildingBlockTypePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _presenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _presenter.GetBuildingBlockType(_module)).Returns(BuildingBlockType.None);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.LoadBuildingBlocksToModule(_module);
      }

      [Observation]
      public void nothing_should_be_added_to_the_project()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<IMoBiCommand>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_loading_from_PKML_and_filename_dialog_gets_canceled : concern_for_InteractionTasksForModule
   {
      private Module _module;
      private ISelectBuildingBlockTypePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _presenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _presenter.GetBuildingBlockType(_module)).Returns(BuildingBlockType.Observer);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);

         A.CallTo(() => _context.InteractionTask.AskForFileToOpen(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns("");
      }

      protected override void Because()
      {
         sut.LoadBuildingBlocksToModule(_module);
      }

      [Observation]
      public void no_BBs_should_have_been_added_to_the_project()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<IMoBiCommand>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_loading_from_PKML_an_observer_BB : concern_for_InteractionTasksForModule
   {
      private Module _module;
      private ISelectBuildingBlockTypePresenter _presenter;
      private List<ObserverBuildingBlock> _observerList;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _observerList = new List<ObserverBuildingBlock> { new ObserverBuildingBlock().WithId("observer_from_PKML") };
         _presenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _presenter.GetBuildingBlockType(_module)).Returns(BuildingBlockType.Observer);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
         A.CallTo(() => _context.InteractionTask.AskForFileToOpen(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns("observer_file");
         A.CallTo(() => _context.InteractionTask.LoadItems<ObserverBuildingBlock>(A<string>.Ignored))
            .Returns(_observerList);
      }

      protected override void Because()
      {
         sut.LoadBuildingBlocksToModule(_module);
      }

      [Observation]
      public void observer_should_be_added_to_the_module()
      {
         _module.Observers.ShouldNotBeNull();
         _module.Observers.Id.ShouldBeEqualTo("observer_from_PKML");
      }
   }

   public class When_loading_from_PKML_multiple_building_blocks : concern_for_InteractionTasksForModule
   {
      private Module _module;
      private ISelectBuildingBlockTypePresenter _presenter;
      private List<ObserverBuildingBlock> _observerList;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         _observerList = new List<ObserverBuildingBlock>
            { new ObserverBuildingBlock().WithId("observer_from_PKML"), new ObserverBuildingBlock().WithId("SecondId") };
         _presenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _presenter.GetBuildingBlockType(_module)).Returns(BuildingBlockType.Observer);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_presenter);
         A.CallTo(() => _context.InteractionTask.AskForFileToOpen(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns("observer_file");
         A.CallTo(() => _context.InteractionTask.LoadItems<ObserverBuildingBlock>(A<string>.Ignored))
            .Returns(_observerList);
      }

      [Observation]
      public void an_exception_should_be_thrown()
      {
         The.Action(() => sut.LoadBuildingBlocksToModule(_module)).ShouldThrowAn<MoBiException>();
      }
   }
}
