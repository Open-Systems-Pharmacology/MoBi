using System;
using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_ExpressionProfileInitialConditionsPresenter : ContextSpecification<ExpressionProfileInitialConditionsPresenter>
   {
      protected IInitialConditionsTask<ExpressionProfileBuildingBlock> _initialConditionsTask;
      protected IClipboardManager _clipboardManager;

      protected override void Context()
      {
         _initialConditionsTask = A.Fake<IInitialConditionsTask<ExpressionProfileBuildingBlock>>();
         _clipboardManager = A.Fake<IClipboardManager>();

         sut = new ExpressionProfileInitialConditionsPresenter(
            A.Fake<IInitialConditionsView>(),
            _initialConditionsTask,
            A.Fake<IInitialConditionsCreator>(),
            A.Fake<IMoBiContext>(),
            A.Fake<IFormulaToValueFormulaDTOMapper>(),
            A.Fake<IDimensionFactory>(),
            A.Fake<IInitialConditionsDistributedInExpressionProfilePresenter>(),
            A.Fake<IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper>(),
            _clipboardManager);
      }
   }

   internal class When_pasting_initial_conditions_into_an_expression_profile : concern_for_ExpressionProfileInitialConditionsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _clipboardManager.ClipBoardContainsType<InitialCondition>()).Returns(true);
      }

      protected override void Because()
      {
         sut.PasteFromClipBoard();
      }

      [Observation]
      public void paste_should_not_be_available()
      {
         sut.CanPasteFromClipBoard().ShouldBeFalse();
      }

      [Observation]
      public void the_clipboard_should_not_be_pasted()
      {
         A.CallTo(() => _clipboardManager.PasteFromClipBoard(A<Action<InitialCondition>>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_expression_profile_should_not_be_extended()
      {
         A.CallTo(() => _initialConditionsTask.AddOrReplaceEntities(A<ExpressionProfileBuildingBlock>._, A<IReadOnlyList<InitialCondition>>._)).MustNotHaveHappened();
      }
   }
}
