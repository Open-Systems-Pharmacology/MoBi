using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditExplicitFormulaPresenter :
      IEditTypedFormulaPresenter, IListener<AddedFormulaUsablePathEvent>,
      IListener<RemovedFormulaUsablePathEvent>,
      IListener<FormulaChangedEvent>,
      IPresenterWithContextMenu<IViewItem>
   {
      void SetFormulaUsablePath(string newPath, FormulaUsablePathDTO dto);
      IEnumerable<string> AllFormulaNames();
      bool DragDropAllowedFor(ReferenceDTO droppedReferenceDTO);
      void Drop(ReferenceDTO referenceDTO);
      void Validate(string formulaString);
      IEnumerable<IDimension> GetDimensions();
      void RemovePath(FormulaUsablePathDTO formulaUsablePathDTO);

      /// <summary>
      ///    Sets the formula string to a new value from the old value
      /// </summary>
      /// <param name="newFormulaString">The new value</param>
      /// <param name="oldFormulaString">The old value</param>
      void SetFormulaString(string newFormulaString, string oldFormulaString);

      /// <summary>
      ///    Sets the alias of the <paramref name="formulaUsablePath" /> being edited to <paramref name="newAlias" /> from
      ///    <paramref name="oldAlias" />
      /// </summary>
      void SetAlias(string newAlias, string oldAlias, IFormulaUsablePath formulaUsablePath);

      /// <summary>
      ///    Sets the formula dimension from <paramref name="oldValue" /> to <paramref name="newValue" />
      /// </summary>
      void SetFormulaPathDimension(IDimension newValue, IDimension oldValue, FormulaUsablePathDTO formulaUsablePathDTO);

      /// <summary>
      ///    Clones the path of <paramref name="formulaUsablePathToClone" /> and adds it to the formula with a unique alias
      /// </summary>
      void ClonePath(FormulaUsablePathDTO formulaUsablePathToClone);

      /// <summary>
      ///    Creates a new empty path in the formula
      /// </summary>
      void CreateNewPath();
   }

   public class EditExplicitFormulaPresenter : EditTypedFormulaPresenter<IEditExplicitFormulaView, IEditExplicitFormulaPresenter, ExplicitFormula>, IEditExplicitFormulaPresenter
   {
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly ICircularReferenceChecker _circularReferenceChecker;
      private readonly IMoBiContext _context;
      private readonly IExplicitFormulaToExplicitFormulaDTOMapper _explicitFormulaMapper;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      private readonly IUserSettings _userSettings;
      private readonly IDimensionFactory _dimensionFactory;

      public EditExplicitFormulaPresenter(IEditExplicitFormulaView view, IExplicitFormulaToExplicitFormulaDTOMapper explicitFormulaMapper, IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiContext context, ICircularReferenceChecker circularReferenceChecker,
         IMoBiFormulaTask moBiFormulaTask, IReactionDimensionRetriever reactionDimensionRetriever, IDisplayUnitRetriever displayUnitRetriever,
         IViewItemContextMenuFactory contextMenuFactory, IUserSettings userSettings, IDimensionFactory dimensionFactory) : base(view, displayUnitRetriever)
      {
         _explicitFormulaMapper = explicitFormulaMapper;
         _circularReferenceChecker = circularReferenceChecker;
         _moBiFormulaTask = moBiFormulaTask;
         _reactionDimensionRetriever = reactionDimensionRetriever;
         _contextMenuFactory = contextMenuFactory;
         _userSettings = userSettings;
         _dimensionFactory = dimensionFactory;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      private void updateFormulaCaption()
      {
         var reactionMode = _reactionDimensionRetriever.SelectedDimensionMode;
         var caption = _moBiFormulaTask.GetFormulaCaption(UsingObject, reactionMode, IsRHS);

         if (string.IsNullOrEmpty(caption))
            _view.HideFormulaCaption();
         else
            _view.SetFormulaCaption(caption);
      }

      public override void Edit(ExplicitFormula objectToEdit)
      {
         if (objectToEdit.IsExplicit())
         {
            _view.Enabled = true;
            _formula = objectToEdit;
            _view.Show(_explicitFormulaMapper.MapFrom(_formula, UsingObject));
            Validate(_formula.FormulaString);
         }

         if (objectToEdit.IsBlackBox())
            _view.Enabled = false;

         updateFormulaCaption();
      }

      public void SetFormulaString(string newFormulaString, string oldFormulaString)
      {
         if (string.Equals(newFormulaString, oldFormulaString)) return;
         AddCommand(_moBiFormulaTask.SetFormulaString(_formula, newFormulaString, oldFormulaString, BuildingBlock));

         notifyFormulaChanged();
      }

      public void SetAlias(string newAlias, string oldAlias, IFormulaUsablePath formulaUsablePath)
      {
         if (string.Equals(newAlias, oldAlias))
            return;

         AddCommand(_moBiFormulaTask.EditAliasInFormula(_formula, newAlias, oldAlias, formulaUsablePath, BuildingBlock));
         notifyFormulaChanged();
      }

      public void SetFormulaPathDimension(IDimension newValue, IDimension oldValue, FormulaUsablePathDTO formulaUsablePathDTO)
      {
         AddCommand(_moBiFormulaTask.SetFormulaPathDimension(_formula, newValue, formulaUsablePathDTO.Alias, BuildingBlock));
         notifyFormulaChanged();
      }

      private void notifyFormulaChanged()
      {
         Validate(_formula.FormulaString);
         _context.PublishEvent(new FormulaChangedEvent(_formula));
      }

      public void SetFormulaUsablePath(string newPath, FormulaUsablePathDTO dto)
      {
         var path = new ObjectPath(newPath.ToPathArray());
         var formulaUsablePath = _formula.FormulaUsablePathBy(dto.Alias);

         AddCommand(_moBiFormulaTask.ChangePathInFormula(_formula, path, formulaUsablePath, BuildingBlock));
      }

      public IEnumerable<string> AllFormulaNames()
      {
         var formulaCache = getBuldingBlock().FormulaCache;
         return formulaCache.Select(formula => formula.Name).OrderBy(x => x);
      }

      public bool DragDropAllowedFor(ReferenceDTO droppedReferenceDTO)
      {
         if (ReadOnly) return false;
         if (droppedReferenceDTO == null || droppedReferenceDTO.Path == null) return false;
         if (droppedReferenceDTO.Path.IsAnImplementationOf<TimePath>()) return true;
         if (droppedReferenceDTO.BuildMode.Equals(ParameterBuildMode.Local) && referencesToLocalForbidden) return false;
         return true;
      }

      private bool referencesToLocalForbidden
      {
         get
         {
            var parameter = UsingObject as IParameter;
            if (parameter == null) return false;
            return !parameter.BuildMode.Equals(ParameterBuildMode.Local);
         }
      }

      public void Drop(ReferenceDTO droppedReferenceDTO)
      {
         if (droppedReferenceDTO == null) return;
         addPathToFormula(droppedReferenceDTO.Path);
      }

      public void Validate(string formulaString)
      {
         try
         {
            _formula.Validate(formulaString);
            _view.SetParserError(null);
            _context.PublishEvent(new FormulaValidEvent(_formula, BuildingBlock));
         }
         catch (OSPSuiteException parserException)
         {
            _view.SetParserError(parserException.Message);
            _context.PublishEvent(new FormulaInvalidEvent(_formula, BuildingBlock, parserException.Message));
         }
      }

      public IEnumerable<IDimension> GetDimensions()
      {
         return _context.DimensionFactory.Dimensions;
      }

      public void CreateNewPath()
      {
         var path = new FormulaUsablePath().WithDimension(_dimensionFactory.TryGetDimension(_userSettings.ParameterDefaultDimension));
         addPathToFormula(path);
      }

      public void ClonePath(FormulaUsablePathDTO formulaUsablePathToClone)
      {
         var path = getPathFrom(formulaUsablePathToClone);
         addPathToFormula(path.Clone<IFormulaUsablePath>());
      }

      public void RemovePath(FormulaUsablePathDTO formulaUsablePathDTO)
      {
         var path = getPathFrom(formulaUsablePathDTO);
         AddCommand(_moBiFormulaTask.RemoveFormulaUsablePath(_formula, path, BuildingBlock));
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         if (_formula == null) return;
         if (ReadOnly) return;

         var contextMenu = _contextMenuFactory.CreateFor(viewItem ?? new EmptyFormulaUsableDTO(), this);
         contextMenu.Show(View, popupLocation);
      }

      private IFormulaUsablePath getPathFrom(FormulaUsablePathDTO dto)
      {
         return _formula.ObjectPaths.FirstOrDefault(usablePath => string.Equals(usablePath.Alias, dto.Alias));
      }

      public void Handle(AddedFormulaUsablePathEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      public void Handle(FormulaChangedEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      public void Handle(RemovedFormulaUsablePathEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      private void handle(FormulaEvent eventToHandle)
      {
         if (!canHandle(eventToHandle)) return;
         Edit(_formula, _formulaOwner);
      }

      private bool canHandle(FormulaEvent formulaEvent)
      {
         return Equals(formulaEvent.Formula, _formula);
      }

      private IBuildingBlock getBuldingBlock()
      {
         return _activeSubjectRetriever.Active<IBuildingBlock>();
      }

      private void addPathToFormula(IFormulaUsablePath path)
      {
         if (ReadOnly) return;
         if (path == null) return;

         checkForCircularReference(path);

         makeAliasUnique(path, _formula.ObjectPaths);
         AddCommand(_moBiFormulaTask.AddFormulaUsablePath(_formula, path, BuildingBlock));
      }

      private void checkForCircularReference(IFormulaUsablePath path)
      {
         if (hasCircularReference(path))
            throw new OSPSuiteException(AppConstants.Exceptions.CircularReferenceException(path, _formula));
      }

      private bool hasCircularReference(IFormulaUsablePath path)
      {
         return UsingObject != null && !IsRHS && _circularReferenceChecker.HasCircularReference(path, UsingObject);
      }

      private void makeAliasUnique(IFormulaUsablePath path, IEnumerable<IFormulaUsablePath> objectPaths)
      {
         var aliases = objectPaths.Select(usedPath => usedPath.Alias).ToList();
         var alias = path.Alias;
         int i = 1;
         while (aliases.Contains(alias))
         {
            alias = $"{path.Alias}{i}";
            i++;
         }

         path.Alias = alias;
      }
   }
}