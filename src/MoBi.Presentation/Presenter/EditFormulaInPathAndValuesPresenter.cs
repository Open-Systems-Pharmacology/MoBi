using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
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
      public EditFormulaInPathAndValuesPresenter(IEditFormulaInPathAndValuesView view,
         IFormulaPresenterCache formulaPresenterCache,
         IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper,
         FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask,
         ICircularReferenceChecker circularReferenceChecker,
         ISelectReferenceAtParameterValuePresenter referencePresenter) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         ReferencePresenter = referencePresenter;
      }

      public void AddNewFormula(string formulaName = null)
      {
         var (_, newFormula) = _formulaTask.CreateNewFormulaInBuildingBlock(_formulaDTO.Type, FormulaDimension, AllFormulaNames, _buildingBlock, formulaName);
         if (newFormula == null)
            return;

         SelectFormula(newFormula);
         UpdateFormula();
      }

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