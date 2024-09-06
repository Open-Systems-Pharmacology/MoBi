using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaInPathAndValuesPresenter : ICommandCollectorPresenter, ISubjectPresenter, IEditFormulaPresenter
   {
      void Init<TBuilder>(TBuilder formulaOwner, IBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder, MoBiMacroCommand macroCommand) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit;
   }

   public class EditFormulaInPathAndValuesPresenter : EditFormulaPresenter<IEditFormulaInPathAndValuesView, IEditFormulaInPathAndValuesPresenter>, IEditFormulaInPathAndValuesPresenter
   {
      private readonly IInteractionTaskContext _interactionTaskContext;
      private MoBiMacroCommand _localMacroCommand;

      public EditFormulaInPathAndValuesPresenter(IEditFormulaInPathAndValuesView view, 
         IFormulaPresenterCache formulaPresenterCache, 
         IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper, 
         FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask, 
         ICircularReferenceChecker circularReferenceChecker,
         IInteractionTaskContext interactionTaskContext,
         ISelectReferenceAtParameterValuePresenter referencePresenter) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         _interactionTaskContext = interactionTaskContext;
         ReferencePresenter = referencePresenter;
      }

      public void FormulaTypeSelectionChanged(string formulaName)
      {
         rollBackChanges();

         var (command, newFormula) = _formulaTask.CreateNewFormulaInBuildingBlock(_formulaDTO.Type, FormulaDimension, AllFormulaNames, _buildingBlock, formulaName);
         if (newFormula == null)
            return;

         AddCommand(command);
         SelectFormula(newFormula);
         UpdateFormula();
      }

      private void rollBackChanges()
      {
         if (_localMacroCommand.IsEmpty) 
            return;

         _interactionTaskContext.CancelCommand(_localMacroCommand);
         _localMacroCommand.Clear();
      }

      public void Init<TBuilder>(TBuilder formulaOwner, IBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder, MoBiMacroCommand macroCommand) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
      {
         _localMacroCommand = macroCommand;
         ReferencePresenter.Init(null, Enumerable.Empty<IObjectBase>().ToList(), formulaOwner);
         InitializeWith(_localMacroCommand);
         Init(formulaOwner, buildingBlock, formulaDecoder);
      }
   }
}