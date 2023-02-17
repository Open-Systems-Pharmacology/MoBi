using System;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTableFormulaWithXArgumentFormulaPresenter : IEditTypedFormulaPresenter
   {
      void SetXArgumentFormulaPath();
      void SetTableObjectPath();
   }

   public class EditTableFormulaWithXArgumentFormulaPresenter :
      EditTypedFormulaPresenter<IEditTableFormulaWithXArgumentFormulaView, IEditTableFormulaWithXArgumentFormulaPresenter, TableFormulaWithXArgument>,
      IEditTableFormulaWithXArgumentFormulaPresenter
   {
      private readonly ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper _mapper;
      private TableFormulaWithXArgumentDTO _tableFormulaWithXArgumentDTO;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IApplicationController _applicationController;
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;

      public EditTableFormulaWithXArgumentFormulaPresenter(
         IEditTableFormulaWithXArgumentFormulaView view,
         ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper mapper,
         IMoBiFormulaTask moBiFormulaTask,
         IDisplayUnitRetriever displayUnitRetriever,
         IApplicationController applicationController,
         ISelectReferencePresenterFactory selectReferencePresenterFactory) : base(view, displayUnitRetriever)
      {
         _mapper = mapper;
         _moBiFormulaTask = moBiFormulaTask;
         _applicationController = applicationController;
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
      }

      public override void Edit(TableFormulaWithXArgument tableFormulaWithXArgument)
      {
         _formula = tableFormulaWithXArgument;
         bindToView();
      }

      private void bindToView()
      {
         _tableFormulaWithXArgumentDTO = _mapper.MapFrom(_formula);
         _view.BindTo(_tableFormulaWithXArgumentDTO);
      }

      public void SetXArgumentFormulaPath()
      {
         var path = selectFormulaUsablePath(isValidXArgumentObject, AppConstants.Captions.OffsetObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeXArgumentObject(_formula, path, BuildingBlock));
         bindToView();
      }

      private bool isValidXArgumentObject(IObjectBase objectBase)
      {
         var parameter = objectBase as IParameter;
         return parameter != null;
      }

      public void SetTableObjectPath()
      {
         var path = selectFormulaUsablePath(isValidTableObject, AppConstants.Captions.TableObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeTableObject(_formula, path, BuildingBlock));
         bindToView();
      }

      private bool isValidTableObject(IObjectBase objectBase)
      {
         var parameter = objectBase as IParameter;
         return parameter?.Formula.IsTable() ?? false;
      }

      private FormulaUsablePath selectFormulaUsablePath(Func<IObjectBase, bool> predicate, string caption)
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