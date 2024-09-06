using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public class concern_for_EditFormulaInPathAndValuesPresenter : ContextSpecification<EditFormulaInPathAndValuesPresenter>
   {
      private IEditFormulaInPathAndValuesView _editFormulaInPathAndValues;
      private IFormulaPresenterCache _formulaPresenterCache;
      private IMoBiContext _moBiContext;
      private IFormulaToFormulaInfoDTOMapper _formulaToFormulaInfoDTOMapper;
      private FormulaTypeCaptionRepository _formulaTypeCaptionRepository;
      private IMoBiFormulaTask _moBiFormulaTask;
      private ICircularReferenceChecker _circularReferenceChecker;
      protected IInteractionTaskContext _interactionTaskContext;
      private ISelectReferenceAtParameterValuePresenter _selectReferenceAtParameterValuePresenter;

      protected override void Context()
      {
         _editFormulaInPathAndValues = A.Fake<IEditFormulaInPathAndValuesView>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _moBiContext = A.Fake<IMoBiContext>();
         _formulaToFormulaInfoDTOMapper = A.Fake<IFormulaToFormulaInfoDTOMapper>();
         _formulaTypeCaptionRepository = A.Fake<FormulaTypeCaptionRepository>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _circularReferenceChecker = A.Fake<ICircularReferenceChecker>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _selectReferenceAtParameterValuePresenter = A.Fake<ISelectReferenceAtParameterValuePresenter>();

         sut = new EditFormulaInPathAndValuesPresenter(_editFormulaInPathAndValues, 
            _formulaPresenterCache, 
            _moBiContext, 
            _formulaToFormulaInfoDTOMapper, 
            _formulaTypeCaptionRepository, 
            _moBiFormulaTask, 
            _circularReferenceChecker, 
            _interactionTaskContext, 
            _selectReferenceAtParameterValuePresenter);
      }
   }

   public class When_initializing_the_presenter : concern_for_EditFormulaInPathAndValuesPresenter
   {
      private ParameterValue _parameterValue;
      private ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      private UsingFormulaDecoder _usingFormulaDecoder;
      private MoBiMacroCommand _moBiMacroCommand;

      protected override void Context()
      {
         base.Context();
         _parameterValue = new ParameterValue();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock { _parameterValue };
         _usingFormulaDecoder = new UsingFormulaDecoder();
         _moBiMacroCommand = new MoBiMacroCommand();
      }

      protected override void Because()
      {
         sut.Init(_parameterValue, _parameterValuesBuildingBlock, _usingFormulaDecoder, _moBiMacroCommand);
      }

      [Observation]
      public void the_command_collector_should_be_the_macro_command()
      {
         sut.CommandCollector.ShouldBeEqualTo(_moBiMacroCommand);
      }
   }

   public class When_changing_formula_type : concern_for_EditFormulaInPathAndValuesPresenter
   {
      private string _formulaName;
      private MoBiMacroCommand _moBiMacroCommand;
      private MoBiMacroCommand _subCommand;

      protected override void Context()
      {
         base.Context();
         _formulaName = "FormulaName";

         var parameterValue = new ParameterValue();
         var parameterValuesBuildingBlock = new ParameterValuesBuildingBlock { parameterValue };

         _moBiMacroCommand = new MoBiMacroCommand();
         _subCommand = new MoBiMacroCommand();
         _moBiMacroCommand.Add(_subCommand);
         sut.Init(parameterValue, parameterValuesBuildingBlock, new UsingFormulaDecoder(), _moBiMacroCommand);
      }

      protected override void Because()
      {
         sut.FormulaTypeSelectionChanged(_formulaName);
      }

      [Observation]
      public void the_macro_command_should_be_cleared_of_previous_commands()
      {
         _moBiMacroCommand.All().ShouldNotContain(_subCommand);
      }

      [Observation]
      public void existing_commands_should_be_canceled()
      {
         A.CallTo(() => _interactionTaskContext.CancelCommand(_moBiMacroCommand)).MustHaveHappened();
      }
   }
}
