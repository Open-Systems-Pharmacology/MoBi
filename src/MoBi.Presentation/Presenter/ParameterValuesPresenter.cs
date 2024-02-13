using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterValuesPresenter : IStartValuesPresenter<ParameterValueDTO>, IEditPresenter<ParameterValuesBuildingBlock>
   {
      void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension);
   }

   public class ParameterValuesPresenter
      : PathAndValueBuildingBlockPresenter<IParameterValuesView,
            IParameterValuesPresenter,
            ParameterValuesBuildingBlock,
            ParameterValueDTO, ParameterValue>,
         IParameterValuesPresenter
   {
      private readonly IParameterValuesTask _parameterValuesTask;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;

      public ParameterValuesPresenter(
         IParameterValuesView view,
         IParameterValueToParameterValueDTOMapper valueMapper,
         IParameterValuesTask parameterValuesTask,
         IParameterValuesCreator parameterValuesCreator,
         IMoBiContext context,
         IDisplayUnitRetriever displayUnitRetriever,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IParameterValueDistributedPathAndValueEntityPresenter distributedParameterPresenter,
         IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, valueMapper, parameterValuesTask, parameterValuesCreator, context, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _parameterValuesTask = parameterValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper = parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;
         view.HideIsPresentView();
         view.HideNegativeValuesAllowedView();
      }

      protected override string RemoveCommandDescription()
      {
         return AppConstants.Commands.RemoveMultipleParameterValues;
      }

      protected override IReadOnlyList<ParameterValueDTO> ValueDTOsFor(ParameterValuesBuildingBlock buildingBlock)
      {
         return _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper.MapFrom(buildingBlock).ParameterDTOs;
      }

      public override void AddNewFormula(ParameterValueDTO parameterValueDTO)
      {
         var parameterValue = StartValueFrom(parameterValueDTO);
         AddNewFormula(parameterValueDTO, parameterValue);
      }

      public void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension)
      {
         var macroCommand = new MoBiMacroCommand();
         var pathAndValueEntity = StartValueFrom(valueObject);

         macroCommand.CommandType = AppConstants.Commands.EditCommand;
         macroCommand.Description = AppConstants.Commands.UpdateDimensionsAndUnits;
         macroCommand.ObjectType = new ObjectTypeResolver().TypeFor<ParameterValue>();

         var value = pathAndValueEntity.ConvertToDisplayUnit(pathAndValueEntity.Value);

         macroCommand.AddCommand(_parameterValuesTask.UpdatePathAndValueEntityDimension(_buildingBlock, pathAndValueEntity, newDimension));
         macroCommand.AddCommand(_parameterValuesTask.SetDisplayValueWithUnit(pathAndValueEntity, value, _displayUnitRetriever.PreferredUnitFor(pathAndValueEntity), _buildingBlock));

         AddCommand(macroCommand);
      }
   }
}