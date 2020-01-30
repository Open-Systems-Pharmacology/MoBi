using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaPresenter : ICommandCollectorPresenter, ISubjectPresenter
   {
      /// <summary>
      ///    Get the available formula types that can be edited by the presenter. By default, only Constant, Table and
      ///    Explicit
      /// </summary>
      IEnumerable<Type> AllFormulaTypes();

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

      /// <summary>
      ///    Initializes the editor with <paramref name="formulaOwner" /> and a method to retrieve the formula
      ///    <paramref name="formulaDecoder" />
      /// </summary>
      /// <typeparam name="TObjectWithFormula"></typeparam>
      /// <param name="formulaOwner">The object which contains the reference to the formula to be edited</param>
      /// <param name="buildingBlock">The building block that the formula belongs in</param>
      /// <param name="formulaDecoder">The decoder which can retrieve the formula from the owner</param>
      void Init<TObjectWithFormula>(TObjectWithFormula formulaOwner, IBuildingBlock buildingBlock, FormulaDecoder<TObjectWithFormula> formulaDecoder)
         where TObjectWithFormula : IEntity, IWithDimension;

      /// <summary>
      ///    Initializes the editor with <paramref name="parameter" />
      /// </summary>
      void Init(IParameter parameter, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Initializes the editor with the <paramref name="formulaOwner" />
      /// </summary>
      /// <param name="formulaOwner">The object which contains the reference to the formula to be edited</param>
      /// <param name="buildingBlock">The building block that the formula belongs in</param>
      void Init(IUsingFormula formulaOwner, IBuildingBlock buildingBlock);

      bool BlackBoxAllowed { set; }
      string DisplayFor(Type formulaType);
      void FormulaSelectionChanged(string formulaName);
      void NamedFormulaSelectionChanged();
      ISelectReferencePresenter ReferencePresenter { get; set; }

      /// <summary>
      ///    Triggers the use case to create a new formula
      /// </summary>
      void AddNewFormula();

      /// <summary>
      ///    set the default formula type to be used when creating a formula for the first time If the value is not set
      ///    explicitely, the first value in the list of available types will be used
      /// </summary>
      void SetDefaultFormulaType<TFormulaType>();

      IEnumerable<string> DisplayFormulaNames();
      bool IsRHS { set; get; }
   }

   public class EditFormulaPresenter : AbstractCommandCollectorPresenter<IEditFormulaView, IEditFormulaPresenter>, IEditFormulaPresenter,
      IListener<RemovedEvent>,
      IListener<FormulaChangedEvent>
   {
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private IFormula _formula;
      private IEditTypedFormulaPresenter _formulaPresenter;
      private IEntity _formulaOwner;
      private readonly IMoBiContext _context;
      private IBuildingBlock _buildingBlock;
      private readonly IFormulaToFormulaInfoDTOMapper _formulaDTOMapper;
      private IFormula _constantFormula;
      private FormulaInfoDTO _formulaDTO;
      private ISelectReferencePresenter _referencePresenter;
      private readonly HashSet<Type> _allFormulaType;
      private Type _defaultFormulaType;
      private readonly FormulaTypeCaptionRepository _formulaTypeCaptionRepository;
      private bool _isRHS;
      private readonly IMoBiFormulaTask _formulaTask;
      private readonly ICircularReferenceChecker _circularReferenceChecker;
      private FormulaDecoder _formulaDecoder;

      public EditFormulaPresenter(IEditFormulaView view, IFormulaPresenterCache formulaPresenterCache, IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper, FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask, ICircularReferenceChecker circularReferenceChecker) : base(view)
      {
         _formulaDTOMapper = formulaDTOMapper;
         _formulaTypeCaptionRepository = formulaTypeCaptionRepository;
         _formulaTask = formulaTask;
         _circularReferenceChecker = circularReferenceChecker;
         _context = context;
         _formulaPresenterCache = formulaPresenterCache;
         _allFormulaType = new HashSet<Type> {typeof(ConstantFormula), typeof(TableFormula), typeof(ExplicitFormula), typeof(TableFormulaWithOffset), typeof(TableFormulaWithXArgument), typeof(SumFormula)};
         _defaultFormulaType = _allFormulaType.First();
      }

      public void RemoveAllFormulaTypes()
      {
         _allFormulaType.Clear();
      }

      public void Init<TObjectWithFormula>(TObjectWithFormula formulaOwner, IBuildingBlock buildingBlock, FormulaDecoder<TObjectWithFormula> formulaDecoder)
         where TObjectWithFormula : IEntity, IWithDimension
      {
         _formulaOwner = formulaOwner;
         _formulaDecoder = formulaDecoder;
         _formula = formulaDecoder.GetFormula(formulaOwner);
         _buildingBlock = buildingBlock;
         _constantFormula = null;
         updateFormula();
      }

      public void Init(IParameter parameter, IBuildingBlock buildingBlock)
      {
         if (IsRHS)
            Init(parameter, buildingBlock, new RHSFormulaDecoder());
         else
            Init(parameter.DowncastTo<IUsingFormula>(), buildingBlock);
      }

      public void Init(IUsingFormula formulaOwner, IBuildingBlock buildingBlock)
      {
         Init(formulaOwner, buildingBlock, new UsingFormulaDecoder());
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.Each(p => p.StatusChanged -= formulaPresenterChanged);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
      }

      private void updateFormula()
      {
         //this call creates a default formula and set it in _formula
         if (_formula == null)
         {
            selectFormulaByTypeAndName(defaultFormulaType(), string.Empty);
            _view.ClearFormulaView();
         }
         if (_formula == null) return;

         _formulaDTO = _formulaDTOMapper.MapFrom(_formula);
         rebind();
         _view.BindTo(_formulaDTO);
         _view.IsComplexFormulaView = isComplexFormula(_formula);
         _view.IsNamedFormulaView = isNamedFormula(_formula);
      }

      private Type defaultFormulaType()
      {
         if (_allFormulaType.Contains(_defaultFormulaType))
            return _defaultFormulaType;
         return _allFormulaType.First();
      }

      private IDimension formulaDimension
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

      private IDimension rhsDimensionFor(IWithDimension withDimension)
      {
         return _context.DimensionFactory.GetOrAddRHSDimensionFor(withDimension.Dimension);
      }

      private void selectFormulaByTypeAndName(Type formulaType, string formulaName)
      {
         var newFormula = needsName(formulaType)
            ? getFormulaFromFormulaCache(formulaType, formulaName)
            : getConstantFormula();

         //only the first time that a new formula is created from an empty formula cache
         if (newFormula == null)
            _formula = _formulaTask.CreateNewFormula(formulaType, formulaDimension);
         else
            selectFormula(newFormula);
      }

      private void selectFormula(IFormula formula)
      {
         if (formula != null)
         {
            if (formula.ObjectPaths.Any(hasCircularReference))
               throw new OSPSuiteException(AppConstants.Exceptions.CircularReferenceFormulaException(formula));
         }

         if (!formulaIsDefined())
            _formula = null;

         setFormulaInOwner(formula);
         _formula = formula;
      }

      private bool hasCircularReference(IFormulaUsablePath path)
      {
         return _formulaOwner != null && !IsRHS && _circularReferenceChecker.HasCircularReference(path, _formulaOwner);
      }

      private void setFormulaInOwner(IFormula newFormula)
      {
         AddCommand(_formulaTask.UpdateFormula(_formulaOwner, _formula, newFormula, _formulaDecoder, _buildingBlock));
      }

      private bool formulaIsDefined()
      {
         return _formula != null && _context.ObjectRepository.ContainsObjectWithId(_formula.Id);
      }

      private bool needsName(Type type)
      {
         return !(type == typeof(ConstantFormula) || type == typeof(DistributionFormula));
      }

      private IFormula getConstantFormula()
      {
         if (_constantFormula == null)
         {
            _constantFormula = _formulaTask.CreateNewFormula<ConstantFormula>(formulaDimension);
            //it is important to register the constant formula in the repository here otherwise it won't be found
            //when rollbacking commands
            _context.ObjectRepository.Register(_constantFormula);
         }
         return _constantFormula;
      }

      public IEnumerable<string> DisplayFormulaNames()
      {
         //Do not use .ToList() as we want the lazy evaluation allowing the list of names to be always updated
         return _buildingBlock.FormulaCache
            .Where(formula => formula.IsAnImplementationOf(_formulaDTO.Type))
            .Where(formula => formula.Dimension != null)
            .Where(formula => formula.Dimension.IsEquivalentTo(formulaDimension))
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

      public void FormulaSelectionChanged(string formulaName)
      {
         selectFormulaByTypeAndName(_formulaDTO.Type, formulaName);
         updateFormula();
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
         selectFormulaByTypeAndName(_formulaDTO.Type, _formulaDTO.Name);
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

      public void AddNewFormula()
      {
         var formulaType = _formulaDTO == null ? defaultFormulaType() : _formulaDTO.Type;
         (var command, var formula) = _formulaTask.CreateNewFormulaInBuildingBlock(formulaType, formulaDimension, allFormulaNames(), _buildingBlock);
         if (formula == null)
            return;

         AddCommand(command);

         selectFormula(formula);
         updateFormula();

         //once setup has been performed, raise the change event to notify presenters that formula was added
         OnStatusChanged();
      }

      private IEnumerable<string> allFormulaNames()
      {
         return _buildingBlock.FormulaCache.Where(isNamedFormula)
            .Select(formula => formula.Name)
            .OrderBy(x => x);
      }

      public void SetDefaultFormulaType<TFormulaType>()
      {
         _defaultFormulaType = typeof(TFormulaType);
      }

      private IFormula getFormulaFromFormulaCache(Type type, string formulaName)
      {
         return _buildingBlock.FormulaCache
            .Where(f => f.IsAnImplementationOf(type))
            .FindByName(formulaName);
      }

      public void AddFormulaType<TFormulaType>()
      {
         _allFormulaType.Add(typeof(TFormulaType));
      }

      private bool isComplexFormula(IFormula formula)
      {
         return (formula.IsExplicit() || formula.IsBlackBox());
      }

      private bool isNamedFormula(IFormula formula)
      {
         return !(formula.IsConstant() || formula.IsDistributed());
      }

      private void rebind()
      {
         if (!formulaIsDefined())
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
         _view.SetEditFormualInstanceView(_formulaPresenter.BaseView);
         _formulaPresenter.Edit(_formula, _formulaOwner);
      }

      private void formulaPresenterChanged(object sender, EventArgs e)
      {
         ViewChanged();
      }

      public IEnumerable<Type> AllFormulaTypes()
      {
         return _allFormulaType;
      }

      public void RemoveFormulaType<TFormulaType>()
      {
         _allFormulaType.Remove(typeof(TFormulaType));
      }

      public string DisplayFor(Type formulaType)
      {
         return _formulaTypeCaptionRepository[formulaType];
      }

      public object Subject => _formula;

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_formula == null) return;
         var formulas = eventToHandle.RemovedObjects.Where(x => x.IsAnImplementationOf<IFormula>()).Cast<IFormula>();
         var formulasToRemove = formulas.Where(_buildingBlock.FormulaCache.Contains).ToList();
         if (!formulasToRemove.Contains(_formula)) return;

         _formula = null;
         updateFormula();
      }

      public void Handle(FormulaChangedEvent formulaChangedEvent)
      {
         if (!canHandle(formulaChangedEvent))
            return;

         updateFormula();
      }

      private bool canHandle(FormulaChangedEvent formulaChangedEvent)
      {
         return Equals(formulaChangedEvent.Formula, _formula);
      }
   }
}