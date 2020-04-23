using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditDistributedParameterPresenter : IEditPresenter<IDistributedParameter>,
      ICanEditPropertiesPresenter,
      IPresenter<IEditDistributedParameterView>,
      IPresenterWithFormulaCache,
      ICreatePresenter<IDistributedParameter>
   {
      IReadOnlyList<IDimension> GetDimensions();
      IDimension PercentDimension { get; }
      IDimension NoDimension { get; }
      void SetPercentile(double newValue);
      void UpdateDistributionFormula();
      void SetParameterValue(DistributionParameterDTO distributedParameter, double valueInGuiUnit);
      void SetParameterUnit(DistributionParameterDTO distributedParameter, Unit unit);
      IEnumerable<DistributionFormulaType> AllFormulaTypes();
      string DisplayFormulaTypeFor(DistributionFormulaType distributionFormulaType);
      void DimensionChanged();
   }

   public class EditDistributedParameterPresenter : AbstractEntityEditPresenter<IEditDistributedParameterView, IEditDistributedParameterPresenter, IDistributedParameter>, IEditDistributedParameterPresenter
   {
      private readonly IEditTaskFor<IParameter> _editTasks;
      private IDistributedParameter _distributedParameter;
      private readonly IMoBiContext _context;
      private readonly IDistributedParameterToDistributedParameterDTOMapper _distributedParameterMapper;
      private DistributedParameterDTO _distributedParameterDTO;
      private readonly IDistributionFormulaFactory _distributionFormulaFactory;
      private readonly IQuantityTask _quantityTask;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      private readonly IMoBiFormulaTask _formulaTask;
      private readonly IParameterFactory _parameterFactory;
      private readonly IInteractionTasksForParameter _parameterTask;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditDistributedParameterPresenter(IEditDistributedParameterView view, IEditTaskFor<IParameter> editTasks,
         IMoBiContext context, IDistributedParameterToDistributedParameterDTOMapper distributedParameterMapper,
         IDistributionFormulaFactory distributionFormulaFactory, IQuantityTask quantityTask, IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IMoBiFormulaTask formulaTask, IParameterFactory parameterFactory, IInteractionTasksForParameter parameterTask) : base(view)
      {
         _editTasks = editTasks;
         _distributionFormulaFactory = distributionFormulaFactory;
         _quantityTask = quantityTask;
         _formulaMapper = formulaMapper;
         _formulaTask = formulaTask;
         _parameterFactory = parameterFactory;
         _parameterTask = parameterTask;
         _distributedParameterMapper = distributedParameterMapper;
         _context = context;
      }

      public override void Edit(IDistributedParameter distributedParameter, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _distributedParameter = distributedParameter;
         _distributedParameterDTO = _distributedParameterMapper.MapFrom(_distributedParameter);
         _distributedParameterDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(distributedParameter, existingObjectsInParent));
         _view.BindTo(_distributedParameterDTO);
      }

      public override object Subject => _distributedParameter;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _distributedParameter, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_distributedParameter, BuildingBlock);
      }

      public IReadOnlyList<IDimension> GetDimensions()
      {
         return _context.DimensionFactory.DimensionsSortedByName;
      }

      public override bool CanClose => _distributedParameter == null || base.CanClose;

      public IDimension PercentDimension => _context.DimensionFactory.Dimension(AppConstants.DimensionNames.FRACTION);

      public IDimension NoDimension => _context.DimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS);

      public void SetPercentile(double newValue)
      {
         SetPropertyValueFromView(_distributedParameter.PropertyName(x => x.Percentile), newValue, _distributedParameter.Percentile);
      }

      public void UpdateDistributionFormula()
      {
         IDistributionFormula newFormula;
         switch (_distributedParameterDTO.FormulaType)
         {
            case DistributionFormulaType.DiscreteDistribution:
               checkChildrenForDiscreteDistributionProperties();
               newFormula = _distributionFormulaFactory.CreateDiscreteDistributionFormulaFor(_distributedParameter,
                  getChildParameter(Constants.Distribution.MEAN));
               break;
            case DistributionFormulaType.NormalDistribution:

               checkChildrenForNormalDistributionProperties();
               newFormula = _distributionFormulaFactory.CreateNormalDistributionFormulaFor(_distributedParameter,
                  getChildParameter(Constants.Distribution.MEAN),
                  getChildParameter(Constants.Distribution.DEVIATION));
               break;
            case DistributionFormulaType.UniformDistribution:
               checkChildrenForUniformDistributionProperties();
               newFormula = _distributionFormulaFactory.CreateUniformDistributionFormulaFor(_distributedParameter,
                  getChildParameter(Constants.Distribution.MINIMUM),
                  getChildParameter(Constants.Distribution.MAXIMUM));
               break;
            case DistributionFormulaType.LogNormalDistribution:
               checkChildrenForLogNormalDistributionProperties();
               newFormula = _distributionFormulaFactory.CreateLogNormalDistributionFormulaFor(_distributedParameter,
                  getChildParameter(Constants.Distribution.MEAN),
                  getChildParameter(Constants.Distribution.GEOMETRIC_DEVIATION));
               break;
            default:
               throw new ArgumentOutOfRangeException("distributionFormulaType");
         }
         updateDistributedFormula(newFormula);
         rebind();
      }

      private void updateDistributedFormula(IDistributionFormula newFormula)
      {
         AddCommand(_formulaTask.UpdateDistributedFormula(_distributedParameter, newFormula, DisplayFormulaTypeFor(_distributedParameterDTO.FormulaType), BuildingBlock));
      }

      private IParameter getChildParameter(string parameterName)
      {
         return _distributedParameter.Parameter(parameterName);
      }

      public void SetParameterValue(DistributionParameterDTO distributedParameter, double valueInGuiUnit)
      {
         AddCommand(_quantityTask.SetQuantityDisplayValue(distributedParameter.Parameter, valueInGuiUnit, BuildingBlock));
         updateDistributedParameterPercentile();
      }

      private void updateDistributedParameterPercentile()
      {
         _distributedParameter.RefreshPercentile();
      }

      public void SetParameterUnit(DistributionParameterDTO distributedParameter, Unit unit)
      {
         AddCommand(_quantityTask.SetQuantityDisplayUnit(distributedParameter.Parameter, unit, BuildingBlock));
         updateDistributedParameterPercentile();
      }

      public IEnumerable<DistributionFormulaType> AllFormulaTypes()
      {
         return EnumHelper.AllValuesFor<DistributionFormulaType>();
      }

      public string DisplayFormulaTypeFor(DistributionFormulaType distributionFormulaType)
      {
         return distributionFormulaType.ToString().SplitToUpperCase();
      }

      public void DimensionChanged()
      {
         AddCommand(_quantityTask.SetDistributedParameterDimension(_distributedParameter, _distributedParameterDTO.Dimension, BuildingBlock));
         rebind();
      }

      private void rebind()
      {
         Edit(_distributedParameter);
      }

      private void checkChildrenForDiscreteDistributionProperties()
      {
         addIfNotDefined(Constants.Distribution.MEAN);
      }

      private void checkChildrenForUniformDistributionProperties()
      {
         addIfNotDefined(Constants.Distribution.MINIMUM);
         addIfNotDefined(Constants.Distribution.MAXIMUM);
      }

      private void checkChildrenForNormalDistributionProperties()
      {
         addIfNotDefined(Constants.Distribution.MEAN);
         addIfNotDefined(Constants.Distribution.DEVIATION);
      }

      private void checkChildrenForLogNormalDistributionProperties()
      {
         addIfNotDefined(Constants.Distribution.MEAN);
         addIfNotDefined(Constants.Distribution.GEOMETRIC_DEVIATION, PercentDimension);
      }

      private void addIfNotDefined(string parameterName, IDimension dimension = null)
      {
         if (_distributedParameter.ContainsName(parameterName))
            return;

         var parameter = createDistributionParameter(parameterName, dimension);
         AddCommand(_parameterTask.AddToProject(parameter, _distributedParameter, BuildingBlock));
      }

      private IParameter createDistributionParameter(string name, IDimension dimension = null)
      {
         return _parameterFactory.CreateParameter(name, value: 0, dimension: dimension ?? _distributedParameter.Dimension);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache != null
            ? FormulaCache.MapAllUsing(_formulaMapper)
            : Enumerable.Empty<FormulaBuilderDTO>();
      }

      public IFormulaCache FormulaCache => BuildingBlock != null ? BuildingBlock.FormulaCache : null;
   }
}