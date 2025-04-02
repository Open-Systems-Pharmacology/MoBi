using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Helper;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditIndividualParameterPresenter : IPresenter<IEditIndividualParameterView>
   {
      void CreateConstantFormula();
      void Edit(IndividualParameter objectToEdit, IndividualBuildingBlock selectedIndividual);
      void UpdateValue(double? newValue);
      void UpdateUnit(Unit unit);
   }

   public class EditIndividualParameterPresenter : AbstractCommandCollectorPresenter<IEditIndividualParameterView, IEditIndividualParameterPresenter>, IEditIndividualParameterPresenter
   {
      private readonly IIndividualParameterToIndividualParameterDTOMapper _individualParameterToIndividualParameterDTOMapper;
      private IndividualParameterDTO _individualParameterDTO;
      private readonly IEditValueOriginPresenter _editValueOriginPresenter;
      private readonly IEditFormulaInPathAndValuesPresenter _editFormulaInPathAndValuesPresenter;
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTasksForIndividualBuildingBlock;
      private readonly IFormulaFactory _formulaFactory;
      private IndividualBuildingBlock _buildingBlock;
      private IndividualParameter _individualParameter;
      private readonly IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;

      public EditIndividualParameterPresenter(IEditIndividualParameterView view,
         IIndividualParameterToIndividualParameterDTOMapper individualParameterToIndividualParameterDTOMapper,
         IEditValueOriginPresenter editValueOriginPresenter,
         IEditFormulaInPathAndValuesPresenter editFormulaInPathAndValuesPresenter,
         IInteractionTasksForIndividualBuildingBlock interactionTasksForIndividualBuildingBlock,
         IFormulaFactory formulaFactory,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper) : base(view)
      {
         _individualParameterToIndividualParameterDTOMapper = individualParameterToIndividualParameterDTOMapper;
         _editValueOriginPresenter = editValueOriginPresenter;
         _editFormulaInPathAndValuesPresenter = editFormulaInPathAndValuesPresenter;
         _interactionTasksForIndividualBuildingBlock = interactionTasksForIndividualBuildingBlock;
         _formulaFactory = formulaFactory;
         _pathAndValueEntityToDistributedParameterMapper = pathAndValueEntityToDistributedParameterMapper;
         AddSubPresenters(_editValueOriginPresenter, _editFormulaInPathAndValuesPresenter);
         view.AddValueOriginView(_editValueOriginPresenter.BaseView);
         view.AddFormulaView(_editFormulaInPathAndValuesPresenter.BaseView);

         _editValueOriginPresenter.ShowCaption = false;
      }

      public void CreateConstantFormula()
      {
         if (!_individualParameterDTO.Value.HasValue)
            return;

         var baseUnitValue = _individualParameterDTO.ConvertToBaseUnit(_individualParameterDTO.Value);
         var constantFormula = _formulaFactory.ConstantFormula(baseUnitValue, _individualParameterDTO.Dimension);

         AddCommand(_interactionTasksForIndividualBuildingBlock.SetFormula(_buildingBlock, _individualParameter, constantFormula));

         Edit(_individualParameter, _buildingBlock);
      }

      public void Edit(IndividualParameter individualParameter, IndividualBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
         _individualParameter = individualParameter;
         _individualParameterDTO = _individualParameterToIndividualParameterDTOMapper.MapFrom(individualParameter);
         createDistributionValue();
         _view.BindTo(_individualParameterDTO);
         _editValueOriginPresenter.Edit(individualParameter);
         _view.ShowWarningFor(buildingBlock?.Name);
         updateFormulaView();
      }
      
      private void createDistributionValue()
      {
         if (_individualParameter.DistributionType.HasValue)
            _individualParameterDTO.DistributionValue = _pathAndValueEntityToDistributedParameterMapper.MapFrom(_individualParameter, _individualParameter.DistributionType.Value, subParametersFrom(_buildingBlock, _individualParameter)).Value;
      }

      private IReadOnlyList<IndividualParameter> subParametersFrom(IndividualBuildingBlock buildingBlock, IndividualParameter individualParameter)
      {
         return buildingBlock.Where(x => x.ContainerPath.Equals(individualParameter.Path)).ToList();
      }

      public void UpdateValue(double? newValue)
      {
         if (!newValue.HasValue || isStringEquivalent(_individualParameterDTO.Value, newValue)) 
            return;
         
         AddCommand(_interactionTasksForIndividualBuildingBlock.SetValue(_buildingBlock, newValue, _individualParameter));
         Edit(_individualParameter, _buildingBlock);
      }

      private bool isStringEquivalent(double? value, double? newValue)
      {
         if(value.HasValue && newValue.HasValue)
            return value.ToString() == newValue.ToString();

         return false;
      }

      public void UpdateUnit(Unit unit)
      {
         AddCommand(_interactionTasksForIndividualBuildingBlock.SetUnit(_buildingBlock, _individualParameter, unit));
      }

      private void updateFormulaView()
      {
         if (_individualParameter.Formula != null)
         {
            _view.ShowFormulaEdit();
            _editFormulaInPathAndValuesPresenter.Init(_individualParameter, _individualParameter.BuildingBlock as IndividualBuildingBlock, new UsingFormulaDecoder());
         }
         else
            _view.HideFormulaEdit();
      }
   }
}