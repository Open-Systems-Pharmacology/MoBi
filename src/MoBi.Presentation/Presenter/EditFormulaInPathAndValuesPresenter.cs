using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaInPathAndValuesPresenter : ICommandCollectorPresenter, ISubjectPresenter, IEditFormulaPresenter
   {
      void Init<TBuildingBlock, TBuilder>(TBuilder formulaOwner, TBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder)
         where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
         where TBuildingBlock : class, ILookupBuildingBlock<TBuilder>;
   }

   public class EditFormulaInPathAndValuesPresenter : EditFormulaPresenter<IEditFormulaInPathAndValuesView, IEditFormulaInPathAndValuesPresenter>, IEditFormulaInPathAndValuesPresenter
   {
      private readonly IObjectBaseNamingTask _namingTask;

      public EditFormulaInPathAndValuesPresenter(IEditFormulaInPathAndValuesView view,
         IFormulaPresenterCache formulaPresenterCache,
         IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper,
         FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask,
         ICircularReferenceChecker circularReferenceChecker,
         ISelectReferenceAtParameterValuePresenter referencePresenter, IObjectBaseNamingTask namingTask) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         _namingTask = namingTask;
         ReferencePresenter = referencePresenter;
      }

      public void AddNewFormula(string formulaName = null)
      {
         if (shouldNameFormula(formulaName, AllFormulaNames.ToList()))
            formulaName = _namingTask.NewName(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, AllFormulaNames);

         if (string.IsNullOrEmpty(formulaName))
            return;

         var (_, newFormula) = _formulaTask.CreateNewFormulaInBuildingBlock(_formulaDTO.Type, FormulaDimension, _buildingBlock, formulaName);
         if (newFormula == null)
            return;

         SelectFormula(newFormula);
         UpdateFormula();
      }

      private static bool shouldNameFormula(string newFormulaName, IReadOnlyList<string> forbiddenNames) => string.IsNullOrEmpty(newFormulaName) || forbiddenNames.Contains(newFormulaName);

      public void Init<TBuildingBlock, TBuilder>(TBuilder formulaOwner, TBuildingBlock buildingBlock, UsingFormulaDecoder formulaDecoder) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit where TBuildingBlock : class, ILookupBuildingBlock<TBuilder>
      {
         ReferencePresenter.Init(null, Array.Empty<IObjectBase>(), formulaOwner);
         Initialize(formulaOwner, buildingBlock, formulaDecoder);
      }

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