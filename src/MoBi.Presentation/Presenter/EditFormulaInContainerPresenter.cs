using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaInContainerPresenter : ICommandCollectorPresenter, ISubjectPresenter, IEditFormulaPresenter
   {
      /// <summary>
      ///    Triggers the use case to create a new formula
      /// </summary>
      void AddNewFormula();

      /// <summary>
      ///    Initializes the editor with <paramref name="formulaOwner" /> and a method to retrieve the formula
      ///    <paramref name="formulaDecoder" />
      /// </summary>
      /// <typeparam name="TObjectWithFormula"></typeparam>
      /// <param name="formulaOwner">The object which contains the reference to the formula to be edited</param>
      /// <param name="buildingBlock">The building block that the formula belongs in</param>
      /// <param name="formulaDecoder">The decoder which can retrieve the formula from the owner</param>
      void Init<TObjectWithFormula>(TObjectWithFormula formulaOwner, IBuildingBlock buildingBlock, FormulaDecoder<TObjectWithFormula> formulaDecoder)
         where TObjectWithFormula : IEntity, IWithDimension;

      /// <summary>
      ///    Initializes the editor with <paramref name="parameter" />
      /// </summary>
      void Init(IParameter parameter, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Initializes the editor with the <paramref name="formulaOwner" />
      /// </summary>
      /// <param name="formulaOwner">The object which contains the reference to the formula to be edited</param>
      /// <param name="buildingBlock">The building block that the formula belongs in</param>
      void Init(IUsingFormula formulaOwner, IBuildingBlock buildingBlock);
   }

   public class EditFormulaInContainerPresenter : EditFormulaPresenter<IEditFormulaInContainerView, IEditFormulaInContainerPresenter>, IEditFormulaInContainerPresenter,
      IListener<RemovedEvent>,
      IListener<FormulaChangedEvent>
   {
      public EditFormulaInContainerPresenter(IEditFormulaInContainerView view, IFormulaPresenterCache formulaPresenterCache, IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper, FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask, ICircularReferenceChecker circularReferenceChecker) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         
      }

      public void Init<TObjectWithFormula>(TObjectWithFormula formulaOwner, IBuildingBlock buildingBlock, FormulaDecoder<TObjectWithFormula> formulaDecoder)
         where TObjectWithFormula : IEntity, IWithDimension
      {
         Initialize(formulaOwner, buildingBlock, formulaDecoder);
      }

      public void Init(IParameter parameter, IBuildingBlock buildingBlock)
      {
         if (IsRHS)
            Init(parameter, buildingBlock, new RHSFormulaDecoder());
         else
            Init(parameter.DowncastTo<IUsingFormula>(), buildingBlock);
      }

      public void Init(IUsingFormula formulaOwner, IBuildingBlock buildingBlock) => Init(formulaOwner, buildingBlock, new UsingFormulaDecoder());

      public void AddNewFormula()
      {
         var formulaType = _formulaDTO == null ? DefaultFormulaType : _formulaDTO.Type;
         var (command, formula) = _formulaTask.CreateNewFormulaInBuildingBlock(formulaType, FormulaDimension, AllFormulaNames, _buildingBlock);
         if (formula == null)
            return;

         AddCommand(command);

         SelectFormula(formula);
         UpdateFormula();

         //once setup has been performed, raise the change event to notify presenters that formula was added
         OnStatusChanged();
      }

      public void AddNewFormula(string formulaName)
      {
         SelectFormulaByTypeAndName(_formulaDTO.Type, formulaName);
         UpdateFormula();
      }

      protected override bool ShouldUpdateOwner()
      {
         return _formula != null && _context.ObjectRepository.ContainsObjectWithId(_formula.Id);
      }

      protected override ConstantFormula CreateNewConstantFormula()
      {
         var newFormula =  _formulaTask.CreateNewFormula<ConstantFormula>(FormulaDimension);
         //it is important to register the constant formula in the repository here otherwise it won't be found
         //when rolling back commands
         _context.ObjectRepository.Register(newFormula);

         return newFormula;
      }
   }
}