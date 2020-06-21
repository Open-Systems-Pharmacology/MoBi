using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class AbstractParameterBasePresenter<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter>, IParameterPresenter
      where TPresenter : IPresenter
      where TView : IView<TPresenter>
   {
      protected readonly IQuantityTask _quantityTask;
      protected readonly IInteractionTaskContext _interactionTaskContext;
      protected IBuildingBlock _buildingBlock;
      protected readonly IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected readonly IInteractionTasksForParameter _parameterTask;
      protected readonly IFavoriteTask _favoriteTask;

      protected AbstractParameterBasePresenter(TView view, IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask) : base(view)
      {
         _quantityTask = quantityTask;
         _interactionTaskContext = interactionTaskContext;
         _formulaMapper = formulaMapper;
         _parameterTask = parameterTask;
         _favoriteTask = favoriteTask;
      }

      public virtual IBuildingBlock BuildingBlock
      {
         get => _buildingBlock;
         set => _buildingBlock = value;
      }

      public IFormulaCache FormulaCache => BuildingBlock?.FormulaCache;

      public void SetParameterUnit(IParameterDTO parameterDTO, Unit displayUnit)
      {
         ExecuteQuantityTaskAction(parameterDTO,
            (p, sim) => _quantityTask.SetQuantityDisplayUnit(p, displayUnit, sim),
            (p, bb) => _quantityTask.SetQuantityDisplayUnit(p, displayUnit, bb));
      }

      public bool IsFixedValue(IParameterDTO parameterDTO)
      {
         var parameter = ParameterFrom(parameterDTO);
         return parameter.IsFixedValue && !parameter.IsDistributed();
      }

      public void OnParameterValueSet(IParameterDTO parameterDTO, double valueInGuiUnit)
      {
         ExecuteQuantityTaskAction(parameterDTO,
            (p, sim) => _quantityTask.SetQuantityDisplayValue(p, valueInGuiUnit, sim),
            (p, bb) => _quantityTask.SetQuantityDisplayValue(p, valueInGuiUnit, bb));
      }

      public void OnParameterValueOriginSet(IParameterDTO parameterDTO, ValueOrigin valueOrigin)
      {
         ExecuteQuantityTaskAction(parameterDTO,
            (p, sim) => _quantityTask.UpdateQuantityValueOriginInSimulation(p, valueOrigin, sim),
            (p, bb) => _quantityTask.UpdateQuantityValueOriginInBuildingBlock(p, valueOrigin, bb));
      }

      protected IParameter ParameterFrom(IParameterDTO parameterDTO)
      {
         return parameterDTO?.Parameter;
      }

      protected virtual IEnumerable<IParameter> ParametersFrom(IEnumerable<IParameterDTO> parametersDTO)
      {
         return parametersDTO.Select(ParameterFrom);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         if (FormulaCache == null)
            return new List<FormulaBuilderDTO>();

         return FormulaCache.MapAllUsing(_formulaMapper);
      }


      public IReadOnlyList<IDimension> GetDimensions()
      {
         return _interactionTaskContext.Context.DimensionFactory.DimensionsSortedByName;
      }

      public void SetIsFavorite(IParameterDTO parameterDTO, bool isFavorite)
      {
         _favoriteTask.SetParameterFavorite(ParameterFrom(parameterDTO), isFavorite);
      }

      public void ResetValueFor(IParameterDTO parameterDTO)
      {
         try
         {
            var parameter = ParameterFrom(parameterDTO);
            ExecuteQuantityTaskAction(parameterDTO, _quantityTask.ResetQuantityValue, _quantityTask.ResetQuantityValue);
            parameterDTO.Value = parameter.ValueInDisplayUnit;
         }
         catch (Exception)
         {
            parameterDTO.Value = double.NaN;
         }
      }

      protected void ExecuteQuantityTaskAction(IParameterDTO parameterDTO, Func<IParameter, IMoBiSimulation, ICommand> simulationActionFunc, Func<IParameter, IBuildingBlock, ICommand> buildingBlockActionFunc)
      {
         var parameter = ParameterFrom(parameterDTO);
         if (BuildingBlock != null)
         {
            AddCommand(buildingBlockActionFunc(parameter, BuildingBlock));
            return;
         }

         var simulation = _interactionTaskContext.Context.Get<IMoBiSimulation>(parameter.Origin.SimulationId) ?? _interactionTaskContext.Active<IMoBiSimulation>();
         if (simulation == null)
            return;

         AddCommand(simulationActionFunc(parameter, simulation));
      }
   }
}