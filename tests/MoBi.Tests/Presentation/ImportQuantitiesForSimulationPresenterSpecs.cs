using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;

namespace MoBi.Presentation
{
   public abstract class concern_for_ImportQuantitiesForSimulationPresenter : ContextSpecification<ImportQuantitiesForSimulationPresenter>
   {
      protected IImportQuantityView _view;
      protected IMoBiContext _moBiContext;
      protected IDataTableToImportQuantityDTOMapperForSimulations _mapper;
      protected IQuantityTask _quantityTask;

      protected override void Context()
      {
         _view = A.Fake<IImportQuantityView>();
         _moBiContext = A.Fake<IMoBiContext>();
         _mapper = A.Fake<IDataTableToImportQuantityDTOMapperForSimulations>();
         _quantityTask = A.Fake<IQuantityTask>();
         sut = new ImportQuantitiesForSimulationPresenter(_view, A.Fake<IDialogCreator>(), A.Fake<IImportFromExcelTask>(), _mapper, _quantityTask, _moBiContext);
      }
   }

   public class When_initializing_presenter : concern_for_ImportQuantitiesForSimulationPresenter
   {
      protected override void Because()
      {
         sut.Initialize();
      }

      [Observation]
      public void context_must_be_initialized()
      {
         A.CallTo(() => _moBiContext.HistoryManager).MustHaveHappened();
      }

      [Observation]
      public void view_caption_must_be_set()
      {
         _view.Text.ShouldBeEqualTo(AppConstants.Captions.ImportSimulationParameters);
      }

      [Observation]
      public void view_hint_must_be_set()
      {
         _view.HintLabel.ShouldBeEqualTo(AppConstants.Captions.ImportParameterQuantitiesFileFormatHint);
      }
   }

   public class When_starting_dialog : concern_for_ImportQuantitiesForSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
      }

      protected override void Because()
      {
         sut.ImportQuantitiesForSimulation(_simulation);
      }

      [Observation]
      public void view_must_be_bound_to_dto_object()
      {
         A.CallTo(() => _view.BindTo(A<ImportExcelSheetSelectionDTO>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void view_must_be_displayed()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }
   }
}
