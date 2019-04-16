﻿using System;
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
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaPathListPresenter :
      ICommandCollectorPresenter,
      IPresenter<IEditFormulaPathListView>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<FormulaChangedEvent>
   {
      IBuildingBlock BuildingBlock { set; }

      /// <summary>
      ///    Sets to <c>true</c> (default), circular reference check will be performed every time a path is added or edited.
      /// </summary>
      bool CheckCircularReference { get; set; }

      /// <summary>
      ///    Sets the alias of the <paramref name="formulaUsablePath" /> being edited to <paramref name="newAlias" /> from
      ///    <paramref name="oldAlias" />
      /// </summary>
      void SetAlias(string newAlias, string oldAlias, IFormulaUsablePath formulaUsablePath);

      void SetFormulaUsablePath(string newPath, FormulaUsablePathDTO dto);

      /// <summary>
      ///    Sets the formula dimension from <paramref name="oldValue" /> to <paramref name="newValue" />
      /// </summary>
      void SetFormulaPathDimension(IDimension newValue, IDimension oldValue, FormulaUsablePathDTO formulaUsablePathDTO);

      void RemovePath(FormulaUsablePathDTO formulaUsablePathDTO);

      IEnumerable<IDimension> GetDimensions();

      /// <summary>
      ///    Clones the path of <paramref name="formulaUsablePathToClone" /> and adds it to the formula with a unique alias
      /// </summary>
      void ClonePath(FormulaUsablePathDTO formulaUsablePathToClone);

      /// <summary>
      ///    Creates a new empty path in the formula
      /// </summary>
      void CreateNewPath();

      Func<ReferenceDTO, bool> DragDropAllowedFor { get; set; }

      void AddPathToFormula(IFormulaUsablePath path);

      bool ReadOnly { set; get; }

      void Drop(ReferenceDTO referenceDTO);

      void Edit(IFormula formula, IUsingFormula formulaOwner);
   }

   public class EditFormulaPathListPresenter : AbstractEditPresenter<IEditFormulaPathListView, IEditFormulaPathListPresenter, FormulaWithFormulaString>, IEditFormulaPathListPresenter
   {
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IMoBiContext _context;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      private readonly ICircularReferenceChecker _circularReferenceChecker;
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathDTOMapper;
      private readonly IUserSettings _userSettings;
      public bool CheckCircularReference { get; set; } = true;
      private FormulaWithFormulaString _formula;
      private IUsingFormula _formulaOwner;
      public IBuildingBlock BuildingBlock { set; protected get; }
      public Func<ReferenceDTO, bool> DragDropAllowedFor { get; set; } = x => false;

      public EditFormulaPathListPresenter(
         IEditFormulaPathListView view,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiContext context,
         IDimensionFactory dimensionFactory,
         IUserSettings userSettings,
         IViewItemContextMenuFactory contextMenuFactory,
         ICircularReferenceChecker circularReferenceChecker,
         IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathDTOMapper) : base(view)
      {
         _moBiFormulaTask = moBiFormulaTask;
         _context = context;
         _dimensionFactory = dimensionFactory;
         _userSettings = userSettings;
         _contextMenuFactory = contextMenuFactory;
         _circularReferenceChecker = circularReferenceChecker;
         _formulaUsablePathDTOMapper = formulaUsablePathDTOMapper;
      }

      public void SetAlias(string newAlias, string oldAlias, IFormulaUsablePath formulaUsablePath)
      {
         if (string.Equals(newAlias, oldAlias))
            return;

         AddCommand(_moBiFormulaTask.EditAliasInFormula(_formula, newAlias, oldAlias, formulaUsablePath, BuildingBlock));
      }

      public void SetFormulaUsablePath(string newPath, FormulaUsablePathDTO dto)
      {
         var path = new ObjectPath(newPath.ToPathArray());
         var formulaUsablePath = _formula.FormulaUsablePathBy(dto.Alias);

         AddCommand(_moBiFormulaTask.ChangePathInFormula(_formula, path, formulaUsablePath, BuildingBlock));
      }

      public void SetFormulaPathDimension(IDimension newValue, IDimension oldValue, FormulaUsablePathDTO formulaUsablePathDTO)
      {
         AddCommand(_moBiFormulaTask.SetFormulaPathDimension(_formula, newValue, formulaUsablePathDTO.Alias, BuildingBlock));
      }

      public void RemovePath(FormulaUsablePathDTO formulaUsablePathDTO)
      {
         var path = getPathFrom(formulaUsablePathDTO);
         AddCommand(_moBiFormulaTask.RemoveFormulaUsablePath(_formula, path, BuildingBlock));
      }

      public void ClonePath(FormulaUsablePathDTO formulaUsablePathToClone)
      {
         var path = getPathFrom(formulaUsablePathToClone);
         AddPathToFormula(path.Clone<IFormulaUsablePath>());
      }

      public void CreateNewPath()
      {
         var path = new FormulaUsablePath().WithDimension(_dimensionFactory.TryGetDimension(_userSettings.ParameterDefaultDimension));
         AddPathToFormula(path);
      }

      private IFormulaUsablePath getPathFrom(FormulaUsablePathDTO dto)
      {
         return _formula.ObjectPaths.FirstOrDefault(usablePath => string.Equals(usablePath.Alias, dto.Alias));
      }

      public void Edit(IFormula formula, IUsingFormula formulaOwner)
      {
         _formulaOwner = formulaOwner;
         Edit(formula);
      }

      public override void Edit(FormulaWithFormulaString formula)
      {
         _formula = formula;
         _view.BindTo(_formulaUsablePathDTOMapper.MapFrom(formula, _formulaOwner));
      }

      public override object Subject => _formula;

      public void AddPathToFormula(IFormulaUsablePath path)
      {
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
         return CheckCircularReference && _formulaOwner != null && _circularReferenceChecker.HasCircularReference(path, _formulaOwner);
      }

      public bool ReadOnly
      {
         get => _view.ReadOnly;
         set => _view.ReadOnly = value;
      }

      public void Drop(ReferenceDTO droppedReferenceDTO)
      {
         if (droppedReferenceDTO == null) return;
         AddPathToFormula(droppedReferenceDTO.Path);
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

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         if (_formula == null) return;
         if (ReadOnly) return;

         var contextMenu = _contextMenuFactory.CreateFor(viewItem ?? new EmptyFormulaUsableDTO(), this);
         contextMenu.Show(View, popupLocation);
      }

      public IEnumerable<IDimension> GetDimensions()
      {
         return _context.DimensionFactory.Dimensions;
      }

      public void Handle(FormulaChangedEvent eventToHandle)
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
   }
}