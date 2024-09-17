using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaPresenter
   {
      /// <summary>
      ///    Remove a formula type that should be used in the context of the container presenter (for instance table and constant
      ///    for observer builder)
      /// </summary>
      void RemoveFormulaType<TFormulaType>();

      /// <summary>
      ///    Add a formula type that should be used in the context of the container presenter (for instance table and constant
      ///    for observer builder)
      /// </summary>
      void AddFormulaType<TFormulaType>();

      /// <summary>
      ///    Remove all allowed formula type. Caller is responsible to add at least one formula type using AddFormulaType
      /// </summary>
      void RemoveAllFormulaTypes();

      bool BlackBoxAllowed { set; }

      ISelectReferencePresenter ReferencePresenter { get; set; }

      /// <summary>
      ///    set the default formula type to be used when creating a formula for the first time If the value is not set
      ///    explicitly, the first value in the list of available types will be used
      /// </summary>
      void SetDefaultFormulaType<TFormulaType>();

      bool IsRHS { set; get; }

      IEnumerable<string> DisplayFormulaNames();

      void NamedFormulaSelectionChanged();

      /// <summary>
      ///    Get the available formula types that can be edited by the presenter. By default, only Constant, Table and
      ///    Explicit
      /// </summary>
      IEnumerable<Type> AllFormulaTypes();

      string DisplayFor(Type formulaType);

      void FormulaTypeSelectionChanged(string formulaName);
   }

   public abstract class EditFormulaPresenter<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter> where TView : IView<TPresenter>, IFormulaEditView where TPresenter : IPresenter
   {
      protected IFormulaPresenterCache _formulaPresenterCache;
      protected IFormula _formula;
      protected IEditTypedFormulaPresenter _formulaPresenter;
      protected IEntity _formulaOwner;
      protected IMoBiContext _context;
      protected IBuildingBlock _buildingBlock;
      protected IFormulaToFormulaInfoDTOMapper _formulaDTOMapper;
      protected FormulaInfoDTO _formulaDTO;
      protected ISelectReferencePresenter _referencePresenter;
      protected HashSet<Type> _allFormulaType;
      protected FormulaTypeCaptionRepository _formulaTypeCaptionRepository;
      protected readonly IMoBiFormulaTask _formulaTask;
      protected readonly ICircularReferenceChecker _circularReferenceChecker;
      protected Type _defaultFormulaType;
      private IFormula _constantFormula;
      private bool _isRHS;
      private FormulaDecoder _formulaDecoder;

      protected EditFormulaPresenter(TView view, IFormulaPresenterCache formulaPresenterCache, IMoBiContext context, IFormulaToFormulaInfoDTOMapper formulaDTOMapper, IMoBiFormulaTask formulaTask, FormulaTypeCaptionRepository formulaTypeCaptionRepository, ICircularReferenceChecker circularReferenceChecker) : base(view)
      {
         _formulaPresenterCache = formulaPresenterCache;
         _context = context;
         _formulaDTOMapper = formulaDTOMapper;
         _formulaTypeCaptionRepository = formulaTypeCaptionRepository;
         _formulaTask = formulaTask;
         _circularReferenceChecker = circularReferenceChecker;
         _allFormulaType = new HashSet<Type> { typeof(ConstantFormula), typeof(TableFormula), typeof(ExplicitFormula), typeof(TableFormulaWithOffset), typeof(TableFormulaWithXArgument), typeof(SumFormula) };
         _defaultFormulaType = _allFormulaType.First();
      }

      public object Subject => _formula;

      public void RemoveAllFormulaTypes() => _allFormulaType.Clear();

      public IEnumerable<Type> AllFormulaTypes() => _allFormulaType;

      public void RemoveFormulaType<TFormulaType>() => _allFormulaType.Remove(typeof(TFormulaType));

      public string DisplayFor(Type formulaType) => _formulaTypeCaptionRepository[formulaType];

      protected void Initialize<TObjectWithFormula>(TObjectWithFormula formulaOwner, IBuildingBlock buildingBlock, FormulaDecoder<TObjectWithFormula> formulaDecoder) where TObjectWithFormula : IEntity, IWithDimension
      {
         _formulaOwner = formulaOwner;
         _formulaDecoder = formulaDecoder;
         _formula = formulaDecoder.GetFormula(formulaOwner);
         _buildingBlock = buildingBlock;
         _constantFormula = null;
         UpdateFormula();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.Each(p => p.StatusChanged -= formulaPresenterChanged);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
      }

      protected void UpdateFormula()
      {
         //this call creates a default formula and set it in _formula
         if (_formula == null)
         {
            SelectFormulaByTypeAndName(DefaultFormulaType, string.Empty);
            _view.ClearFormulaView();
         }
         if (_formula == null) return;

         _formulaDTO = _formulaDTOMapper.MapFrom(_formula);
         rebind();
         _view.BindTo(_formulaDTO);
         _view.IsComplexFormulaView = isComplexFormula(_formula);
         _view.IsNamedFormulaView = isNamedFormula(_formula);

         ViewChanged();
      }

      protected Type DefaultFormulaType => 
         _allFormulaType.Contains(_defaultFormulaType) ? _defaultFormulaType : _allFormulaType.First();

      protected IDimension FormulaDimension
      {
         get
         {
            var withDimension = _formulaOwner.DowncastTo<IWithDimension>();
            if (!IsRHS)
               return withDimension.Dimension;

            var parameter = withDimension as IParameter;
            if (parameter?.RHSFormula == null)
               return rhsDimensionFor(withDimension);

            return parameter.RHSFormula.Dimension ?? rhsDimensionFor(withDimension);
         }
      }

      private IDimension rhsDimensionFor(IWithDimension withDimension) => _context.DimensionFactory.GetOrAddRHSDimensionFor(withDimension.Dimension);

      protected void SelectFormulaByTypeAndName(Type formulaType, string formulaName)
      {
         var newFormula = needsName(formulaType)
            ? getFormulaFromFormulaCache(formulaType, formulaName)
            : getConstantFormula();

         //only the first time that a new formula is created from an empty formula cache
         if (newFormula == null)
            _formula = _formulaTask.CreateNewFormula(formulaType, FormulaDimension);
         else
            SelectFormula(newFormula);
      }

      protected void SelectFormula(IFormula formula)
      {
         if (formula != null)
         {
            if (formula.ObjectPaths.Any(hasCircularReference))
               throw new OSPSuiteException(AppConstants.Exceptions.CircularReferenceFormulaException(formula));
         }

         if (!ShouldUpdateOwner())
            _formula = null;

         setFormulaInOwner(formula);
         _formula = formula;
      }

      private bool hasCircularReference(FormulaUsablePath path) => 
         _formulaOwner != null && !IsRHS && _circularReferenceChecker.HasCircularReference(path, _formulaOwner);

      private void setFormulaInOwner(IFormula newFormula) => 
         AddCommand(_formulaTask.UpdateFormula(_formulaOwner, _formula, newFormula, _formulaDecoder, _buildingBlock));

      protected abstract bool ShouldUpdateOwner();

      private bool needsName(Type type) => !(type == typeof(ConstantFormula) || type == typeof(DistributionFormula));

      private IFormula getConstantFormula()
      {
         return _constantFormula ?? (_constantFormula = CreateNewConstantFormula());
      }

      protected abstract ConstantFormula CreateNewConstantFormula();

      public IEnumerable<string> DisplayFormulaNames()
      {
         //Do not use .ToList() as we want the lazy evaluation allowing the list of names to be always updated
         return _buildingBlock.FormulaCache
            .Where(formula => formula.IsAnImplementationOf(_formulaDTO.Type))
            .Where(formula => formula.Dimension != null)
            .Where(formula => formula.Dimension.IsEquivalentTo(FormulaDimension))
            .Select(formula => formula.Name)
            .OrderBy(x => x);
      }

      public bool IsRHS
      {
         set
         {
            if (_formulaPresenter != null)
               _formulaPresenter.IsRHS = value;

            _isRHS = value;
         }
         get => _isRHS;
      }

      public override bool CanClose
      {
         get
         {
            if (_formulaPresenter == null)
               return base.CanClose;

            return base.CanClose && _formulaPresenter.CanClose;
         }
      }

      public bool BlackBoxAllowed
      {
         set
         {
            if (value)
               AddFormulaType<BlackBoxFormula>();
            else
               RemoveFormulaType<BlackBoxFormula>();
         }
      }

      public void NamedFormulaSelectionChanged()
      {
         SelectFormulaByTypeAndName(_formulaDTO.Type, _formulaDTO.Name);
         rebind();
      }

      public ISelectReferencePresenter ReferencePresenter
      {
         get => _referencePresenter;
         set
         {
            _referencePresenter = value;
            _view.SetReferenceView(_referencePresenter.View);
         }
      }

      protected IEnumerable<string> AllFormulaNames =>
         _buildingBlock.FormulaCache.Where(isNamedFormula)
            .Select(formula => formula.Name)
            .OrderBy(x => x);

      public void SetDefaultFormulaType<TFormulaType>() => _defaultFormulaType = typeof(TFormulaType);

      private IFormula getFormulaFromFormulaCache(Type type, string formulaName)
      {
         return _buildingBlock.FormulaCache
            .Where(f => f.IsAnImplementationOf(type))
            .FindByName(formulaName);
      }

      public void AddFormulaType<TFormulaType>() => _allFormulaType.Add(typeof(TFormulaType));

      private bool isComplexFormula(IFormula formula) => (formula.IsExplicit() || formula.IsBlackBox());

      private bool isNamedFormula(IFormula formula) => !(formula.IsConstant() || formula.IsDistributed());

      private void rebind()
      {
         if (!ShouldUpdateOwner())
         {
            _view.ClearFormulaView();
            return;
         }

         if (_formulaPresenterCache.HasPresenterFor(_formulaDTO.Type))
            _formulaPresenter = _formulaPresenterCache.PresenterFor(_formulaDTO.Type);
         else
         {
            //create and setup presenter once that will be added to cache
            _formulaPresenter = _formulaPresenterCache.PresenterFor(_formulaDTO.Type);
            _formulaPresenter.StatusChanged += formulaPresenterChanged;
            _formulaPresenter.BuildingBlock = _buildingBlock;
            _formulaPresenter.InitializeWith(CommandCollector);
         }
         _formulaPresenter.IsRHS = IsRHS;
         _view.SetEditFormulaInstanceView(_formulaPresenter.BaseView);
         _formulaPresenter.Edit(_formula, _formulaOwner);
      }

      private void formulaPresenterChanged(object sender, EventArgs e) => ViewChanged();

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_formula == null) return;
         var formulas = eventToHandle.RemovedObjects.Where(x => x.IsAnImplementationOf<IFormula>()).Cast<IFormula>();
         var formulasToRemove = formulas.Where(_buildingBlock.FormulaCache.Contains).ToList();
         if (!formulasToRemove.Contains(_formula))
            return;

         _formula = null;
         UpdateFormula();
      }

      public void Handle(FormulaChangedEvent formulaChangedEvent)
      {
         if (!canHandle(formulaChangedEvent))
            return;

         UpdateFormula();
      }

      private bool canHandle(FormulaChangedEvent formulaChangedEvent) => Equals(formulaChangedEvent.Formula, _formula);
   }
}