using System.Reflection;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public interface IAmountToConcentrationConverter
   {
      void Convert(IReactionBuilder reaction, IFormulaCache formulaCache);
      void Convert(IReactionBuildingBlock reactionBuildingBlock);
      void Convert(IMoleculeBuilder moleculeBuilder, IFormulaCache formulaCache);
      void Convert(MoleculeBuildingBlock moleculeBuildingBlock);
      void Convert(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock);
      void Convert(MoleculeStartValue moleculeStartValue, IFormulaCache formulaCache);
      void Convert(object objectToConvert);
   }

   public class AmountToConcentrationConverter : IAmountToConcentrationConverter,
      IVisitor<MoleculeBuildingBlock>,
      IVisitor<IReactionBuildingBlock>,
      IVisitor<SimulationTransfer>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<SimulationConfiguration>,
      IVisitor<MoleculeStartValuesBuildingBlock>,
      IVisitor<IMoleculeBuilder>,
      IVisitor<IReactionBuilder>
   {
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;
      private readonly IAmoutToConcentrationFormulaMapper _amountToConcentrationFormulaMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaTask _formulaTask;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IDimension _concentrationDimension;
      private readonly IDimension _concentrationPerTimeDimension;

      public AmountToConcentrationConverter(IReactionDimensionRetriever reactionDimensionRetriever,
         IDimensionFactory dimensionFactory, IAmoutToConcentrationFormulaMapper amountToConcentrationFormulaMapper,
         IObjectBaseFactory objectBaseFactory, IFormulaTask formulaTask, IDisplayUnitRetriever displayUnitRetriever, 
         IObjectTypeResolver objectTypeResolver, IFormulaFactory formulaFactory)
      {
         _reactionDimensionRetriever = reactionDimensionRetriever;
         _amountToConcentrationFormulaMapper = amountToConcentrationFormulaMapper;
         _objectBaseFactory = objectBaseFactory;
         _formulaTask = formulaTask;
         _displayUnitRetriever = displayUnitRetriever;
         _objectTypeResolver = objectTypeResolver;
         _formulaFactory = formulaFactory;
         _concentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         _concentrationPerTimeDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME);
      }

      public void Convert(IReactionBuilder reaction, IFormulaCache formulaCache)
      {
         if (!conversionRequired(reaction))
            return;

         reaction.Dimension = _concentrationPerTimeDimension;
         convertExplicitFormula(reaction.Formula, _concentrationPerTimeDimension);
      }

      public void Convert(MoleculeStartValue moleculeStartValue, IFormulaCache formulaCache)
      {
         if (!conversionRequired(moleculeStartValue))
            return;

         moleculeStartValue.Dimension = _concentrationDimension;
         moleculeStartValue.DisplayUnit = _displayUnitRetriever.PreferredUnitFor(moleculeStartValue);

         if (moleculeStartValue.Formula.IsExplicit())
         {
            convertExplicitFormula(moleculeStartValue.Formula, _concentrationDimension);
            return;
         }

         var startValue = moleculeStartValue.Value.GetValueOrDefault(0);
         if (startValue == 0)
            return;

         moleculeStartValue.Formula = createConcentrationFormulaFromConstantValue(startValue, moleculeStartValue.Path.ToPathString(), formulaCache);
      }

      public void Convert(IMoleculeBuilder moleculeBuilder, IFormulaCache formulaCache)
      {
         if (!conversionRequired(moleculeBuilder))
            return;

         moleculeBuilder.Dimension = _concentrationDimension;
         moleculeBuilder.DisplayUnit = _displayUnitRetriever.PreferredUnitFor(moleculeBuilder);

         var defaultStartValue = moleculeBuilder.GetDefaultMoleculeStartValue();

         if (!defaultStartValue.HasValue)
         {
            convertExplicitFormula(moleculeBuilder.DefaultStartFormula, _concentrationDimension);
            return;
         }

         //default start value defined? only created an explicit formula for value !=0
         moleculeBuilder.DefaultStartFormula = defaultStartValue == 0 ? 
            createConstantConcentrationFormula(defaultStartValue.Value) : 
            createConcentrationFormulaFromConstantValue(defaultStartValue.Value, moleculeBuilder.Name, formulaCache);

      }

      private IFormula createConstantConcentrationFormula(double value)
      {
         return _formulaFactory.ConstantFormula(value, _concentrationDimension);
      }

      private ExplicitFormula createConcentrationFormulaFromConstantValue(double defaultStartValue, string usingFormulaName, IFormulaCache formulaCache)
      {
         var explicitFormulaInConcentration = _objectBaseFactory.Create<ExplicitFormula>()
            .WithName($"Amount_to_concentration for {usingFormulaName}")
            .WithDimension(_concentrationDimension);

         var volumeAlias = _formulaTask.AddParentVolumeReferenceToFormula(explicitFormulaInConcentration);
         explicitFormulaInConcentration.FormulaString = $"{defaultStartValue.ConvertedTo<string>()}/{volumeAlias}";
         formulaCache.Add(explicitFormulaInConcentration);
         return explicitFormulaInConcentration;
      }

      private void convertExplicitFormula(IFormula formula, IDimension targetDimension)
      {
         if (Equals(formula.Dimension, targetDimension))
            return;

         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null)
            return;

         explicitFormula.Dimension = targetDimension;
         if (_amountToConcentrationFormulaMapper.HasMappingFor(explicitFormula))
         {
            explicitFormula.FormulaString = _amountToConcentrationFormulaMapper.MappedFormulaFor(explicitFormula);
            return;
         }

         var volumeAlias = _formulaTask.AddParentVolumeReferenceToFormula(explicitFormula);
         explicitFormula.FormulaString = string.Format("({0})/{1}", explicitFormula.FormulaString, volumeAlias);
      }

      public void Convert(IReactionBuildingBlock reactionBuildingBlock)
      {
         reactionBuildingBlock.Each(r => Convert(r, reactionBuildingBlock.FormulaCache));
      }

      public void Convert(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         moleculeBuildingBlock.Each(m => Convert(m, moleculeBuildingBlock.FormulaCache));
      }

      public void Convert(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         moleculeStartValuesBuildingBlock.Each(msv => Convert(msv, moleculeStartValuesBuildingBlock.FormulaCache));
      }

      public void Convert(object objectToConvert)
      {
         try
         {
            this.Visit(objectToConvert);
         }
         catch (TargetInvocationException e)
         {
            if (e.InnerException != null)
               throw e.InnerException;

            throw;
         }
      }

      private bool conversionRequired(MoleculeStartValue moleculeStartValue)
      {
         return conversionRequired(moleculeStartValue, Constants.Dimension.MOLAR_AMOUNT);
      }

      private bool conversionRequired(IMoleculeBuilder moleculeBuilder)
      {
         return conversionRequired(moleculeBuilder, Constants.Dimension.MOLAR_AMOUNT);
      }

      private bool conversionRequired(IReactionBuilder reaction)
      {
         return conversionRequired(reaction, Constants.Dimension.AMOUNT_PER_TIME);
      }

      private bool conversionRequired(IWithDimension withDimension, string amountDimension)
      {
         if (withDimension.IsConcentrationBased() && _reactionDimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.AmountBased)
            throw new CannotConvertConcentrationToAmountException(_objectTypeResolver.TypeFor(withDimension));

         return _reactionDimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.ConcentrationBased
                && string.Equals(withDimension.Dimension.Name, amountDimension);
      }

      public void Visit(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         Convert(moleculeBuildingBlock);
      }

      public void Visit(IReactionBuildingBlock reactionBuildingBlock)
      {
         Convert(reactionBuildingBlock);
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         Visit(simulation.Configuration);
      }

      public void Visit(SimulationConfiguration simulationConfiguration)
      {
         Visit(simulationConfiguration.Molecules);
         Visit(simulationConfiguration.Reactions);
         Visit(simulationConfiguration.MoleculeStartValues);
      }

      public void Visit(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         Convert(moleculeStartValuesBuildingBlock);
      }

      public void Visit(IMoleculeBuilder moleculeBuilder)
      {
         Convert(moleculeBuilder, new FormulaCache());
      }

      public void Visit(IReactionBuilder reactionBuilder)
      {
         Convert(reactionBuilder, new FormulaCache());
      }
   }
}