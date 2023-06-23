using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForMoleculeBuilder : IInteractionTasksForBuilder<MoleculeBuilder>
   {
      void AddPKSimMoleculeTo(MoleculeBuildingBlock moleculeBuildingBlock);

      /// <summary>
      ///    Adds the concentration parameter M/V referencing the volume of the container referencing the
      ///    <paramref name="moleculeBuilder" />.
      ///    The parameter is added only if it was not defined already
      /// </summary>
      void AddConcentrationParameterTo(MoleculeBuilder moleculeBuilder, MoleculeBuildingBlock moleculeBuildingBlock);
   }

   public class InteractionTasksForMoleculeBuilder : InteractionTasksForBuilder<MoleculeBuilder, MoleculeBuildingBlock>, IInteractionTasksForMoleculeBuilder
   {
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly IParameterFactory _parameterFactory;
      private readonly IMoBiFormulaTask _formulaTask;

      public InteractionTasksForMoleculeBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<MoleculeBuilder> editTask,
         IReactionDimensionRetriever dimensionRetriever, IParameterFactory parameterFactory, ICoreCalculationMethodRepository calculationMethodRepository, IMoBiFormulaTask formulaTask)
         : base(interactionTaskContext, editTask)
      {
         _dimensionRetriever = dimensionRetriever;
         _parameterFactory = parameterFactory;
         _calculationMethodRepository = calculationMethodRepository;
         _formulaTask = formulaTask;
      }

      public override IMoBiCommand GetRemoveCommand(MoleculeBuilder moleculeBuilder, MoleculeBuildingBlock parent, IBuildingBlock buildingBlock1)
      {
         return new RemoveMoleculeBuilderCommand(parent, moleculeBuilder);
      }

      public override IMoBiCommand GetRemoveCommand(MoleculeBuilder builder, MoleculeBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(MoleculeBuilder moleculeBuilder, MoleculeBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(moleculeBuilder, parent);
      }

      public void AddPKSimMoleculeTo(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         using (var presenter = ApplicationController.Start<ICreatePKSimMoleculePresenter>())
         {
            var moleculeBuilder = presenter.CreateMolecule(moleculeBuildingBlock);
            if (moleculeBuilder == null)
               return;

            //this will ensure that the default start formula is created on the fly
            setDefaultStartFormula(moleculeBuilder);
            AddConcentrationParameterTo(moleculeBuilder, moleculeBuildingBlock);
            setDefaultStartFormula(moleculeBuilder);
            setDefaultDimensionIn(moleculeBuilder);
            _interactionTaskContext.Context.AddToHistory(AddItemsToProject(new[] {moleculeBuilder}, moleculeBuildingBlock, moleculeBuildingBlock));
         }
      }

      public void AddConcentrationParameterTo(MoleculeBuilder moleculeBuilder, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         if (moleculeBuilder.Parameters.ExistsByName(AppConstants.Parameters.CONCENTRATION))
            return;

         var concentrationParameter = _parameterFactory.CreateConcentrationParameter(moleculeBuildingBlock.FormulaCache);
         moleculeBuilder.AddParameter(concentrationParameter);
      }

      public override IMoBiCommand GetAddCommand(MoleculeBuilder builder, MoleculeBuildingBlock buildingBlock)
      {
         return new AddMoleculeBuilderCommand(buildingBlock, builder);
      }

      public override MoleculeBuilder CreateNewEntity(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculeBuilder = base.CreateNewEntity(moleculeBuildingBlock);
         AddConcentrationParameterTo(moleculeBuilder, moleculeBuildingBlock);
         setDefaults(moleculeBuilder);
         return moleculeBuilder;
      }

      private void setDefaults(MoleculeBuilder moleculeBuilder)
      {
         setDefaultStartFormula(moleculeBuilder);
         setDefaultDimensionIn(moleculeBuilder);
         _calculationMethodRepository.GetAllCategoriesDefault()
            .Each(cm => moleculeBuilder.AddUsedCalculationMethod(new UsedCalculationMethod(cm.Category, AppConstants.DefaultNames.EmptyCalculationMethod)));
         moleculeBuilder.QuantityType = QuantityType.Drug;
      }

      private void setDefaultStartFormula(MoleculeBuilder moleculeBuilder)
      {
         moleculeBuilder.DefaultStartFormula = _formulaTask.CreateNewFormula<ConstantFormula>(_dimensionRetriever.MoleculeDimension);
      }

      private void setDefaultDimensionIn(MoleculeBuilder moleculeBuilder)
      {
         moleculeBuilder.Dimension = _dimensionRetriever.MoleculeDimension;
         moleculeBuilder.DisplayUnit = _interactionTaskContext.DisplayUnitFor(moleculeBuilder);
      }
   }
}