﻿using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTableFormulaWithOffSetFormulaPresenter : IEditTypedFormulaPresenter
   {
      void SetOffsetFormulaPath();
      void SetTableObjectPath();
   }

   public class EditTableFormulaWithOffSetFormulaPresenter : EditTypedFormulaPresenter<IEditTableFormulaWithOffSetFormulaView,
      IEditTableFormulaWithOffSetFormulaPresenter, TableFormulaWithOffset>, IEditTableFormulaWithOffSetFormulaPresenter
   {
      private readonly ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper _mapper;
      private TableFormulaWithOffsetDTO _tableFormulaWithOffsetDTO;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IApplicationController _applicationController;
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      private readonly IDimension _timeDimension;

      public EditTableFormulaWithOffSetFormulaPresenter(IEditTableFormulaWithOffSetFormulaView view, ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper mapper, IMoBiContext context,
         IMoBiFormulaTask moBiFormulaTask, IDisplayUnitRetriever displayUnitRetriever, IApplicationController applicationController, ISelectReferencePresenterFactory selectReferencePresenterFactory) : base(view, displayUnitRetriever)
      {
         _mapper = mapper;
         _moBiFormulaTask = moBiFormulaTask;
         _applicationController = applicationController;
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
         _timeDimension = context.DimensionFactory.Dimension(Constants.Dimension.TIME);
      }

      public override void Edit(TableFormulaWithOffset tableFormulaWithOffset)
      {
         _formula = tableFormulaWithOffset;
         _tableFormulaWithOffsetDTO = _mapper.MapFrom(_formula);
         _view.BindTo(_tableFormulaWithOffsetDTO);
      }

      private void rebind()
      {
         Edit(_formula);
      }

      public void SetOffsetFormulaPath()
      {
         var path = selectFormulaUseablePath(_tableFormulaWithOffsetDTO.OffsetObjectPath, isValidOffsetObject, AppConstants.Captions.OffsetObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeOffsetObject(_formula, path, BuildingBlock));
         rebind();
      }

      private bool isValidOffsetObject(IObjectBase objectBase)
      {
         var parameter = objectBase as IParameter;
         return parameter != null && Equals(parameter.Dimension, _timeDimension);
      }

      public void SetTableObjectPath()
      {
         var path = selectFormulaUseablePath(_tableFormulaWithOffsetDTO.TableObjectPath, isValidTableObject, AppConstants.Captions.TableObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeTableObject(_formula, path, BuildingBlock));
         rebind();
      }

      private bool isValidTableObject(IObjectBase objectBase)
      {
         var parameter = objectBase as IParameter;
         return parameter?.Formula.IsTable() ?? false;
      }

      private IFormulaUsablePath selectFormulaUseablePath(FormulaUsablePathDTO initPath, Func<IObjectBase, bool> predicate, string caption)
      {
         using (var presenter = _applicationController.Start<ISelectFormulaUsablePathPresenter>())
         {
            var referencePresenter = _selectReferencePresenterFactory.ReferenceAtParameterFor(UsingObject.ParentContainer);
            presenter.Init(predicate, UsingObject, new[] {UsingObject.RootContainer}, caption, referencePresenter);
            return presenter.GetSelection();
         }
      }
   }
}