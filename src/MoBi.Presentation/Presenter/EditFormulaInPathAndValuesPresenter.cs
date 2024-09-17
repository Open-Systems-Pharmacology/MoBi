using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaInPathAndValuesPresenter : ICommandCollectorPresenter, ISubjectPresenter, IEditFormulaPresenter
   {
      void Init<TBuildingBlock, TBuilder>(TBuilder formulaOwner, TBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder) 
         where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
         where TBuildingBlock : class, IBuildingBlock<TBuilder>;

      IFormula Formula { get; }
   }

   public class EditFormulaInPathAndValuesPresenter : EditFormulaPresenter<IEditFormulaInPathAndValuesView, IEditFormulaInPathAndValuesPresenter>, IEditFormulaInPathAndValuesPresenter
   {
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private PathAndValueEntity _clonedBuilder;

      public EditFormulaInPathAndValuesPresenter(IEditFormulaInPathAndValuesView view, 
         IFormulaPresenterCache formulaPresenterCache, 
         IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper, 
         FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask, 
         ICircularReferenceChecker circularReferenceChecker,
         ISelectReferenceAtParameterValuePresenter referencePresenter, 
         ICloneManagerForBuildingBlock cloneManager) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         _cloneManager = cloneManager;
         ReferencePresenter = referencePresenter;
         
      }

      public void FormulaTypeSelectionChanged(string formulaName)
      {
         var (_, newFormula) = _formulaTask.CreateNewFormulaInBuildingBlock(_formulaDTO.Type, FormulaDimension, AllFormulaNames, _buildingBlock, formulaName);
         if (newFormula == null)
            return;

         SelectFormula(newFormula);
         UpdateFormula();
      }


      public void Init<TBuildingBlock, TBuilder>(TBuilder formulaOwner, TBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit where TBuildingBlock : class, IBuildingBlock<TBuilder>
      {
         InitializeWith(new MoBiMacroCommand());

         var clonedBuildingBlock = _cloneManager.Clone(buildingBlock);
         _clonedBuilder = clonedBuildingBlock.FindByName(formulaOwner.Name);

         ReferencePresenter.Init(null, Array.Empty<IObjectBase>(), _clonedBuilder);
         Initialize(_clonedBuilder, clonedBuildingBlock, formulaDecoder);
      }

      public IFormula Formula => _clonedBuilder?.Formula;

      protected override bool ShouldUpdateOwner()
      {
         return true;
      }

      protected override ConstantFormula CreateNewConstantFormula()
      {
         return _formulaTask.CreateNewFormula<ConstantFormula>(FormulaDimension);
      }
   }
}