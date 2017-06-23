using System;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
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
   public interface IEditTableFormulaWithOffSetFormulaPresenter : IEditTypedFormulaPresenter
   {
      void SetOffsetFormulaPath(FormulaUsablePathDTO formulaUsablePath);
      void SetTableObjectPath(FormulaUsablePathDTO formulaUsablePath);
   }

   public class EditTableFormulaWithOffSetFormulaPresenter : EditTypedFormulaPresenter<IEditTableFormulaWithOffSetFormulaView,
      IEditTableFormulaWithOffSetFormulaPresenter, TableFormulaWithOffset>, IEditTableFormulaWithOffSetFormulaPresenter
   {
      private readonly ITableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper _mapper;
      private readonly IMoBiContext _context;
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathMapper;
      private TableFormulaWithOffsetDTO _dto;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IApplicationController _applicationController;
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;

      public EditTableFormulaWithOffSetFormulaPresenter(IEditTableFormulaWithOffSetFormulaView view, ITableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper mapper, IMoBiContext context,
         IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathMapper, IMoBiFormulaTask moBiFormulaTask, IDisplayUnitRetriever displayUnitRetriever,
         IApplicationController applicationController, ISelectReferencePresenterFactory selectReferencePresenterFactory) : base(view, displayUnitRetriever)
      {
         _mapper = mapper;
         _context = context;
         _formulaUsablePathMapper = formulaUsablePathMapper;
         _moBiFormulaTask = moBiFormulaTask;
         _applicationController = applicationController;
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
      }

      public override void Edit(TableFormulaWithOffset objectToEdit)
      {
         _formula = objectToEdit;
         _dto = _mapper.MapFrom(_formula);
         _view.Show(_dto);
      }

      public void SetOffsetFormulaPath(FormulaUsablePathDTO formulaUsablePath)
      {
         var path = selectFormulaUseablePath(formulaUsablePath, isValideOffsetObject, AppConstants.Captions.OffsetObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeOffsetObject(_formula, path, BuildingBlock));
         _view.ShowOffsetObjectPath(_formulaUsablePathMapper.MapFrom(path, _formula));
      }

      private bool isValideOffsetObject(IObjectBase para)
      {
         var time = _context.DimensionFactory.Dimension(Constants.Dimension.TIME);
         return para.IsAnImplementationOf<IParameter>() && ((IParameter) para).Dimension.Equals(time);
      }

      public void SetTableObjectPath(FormulaUsablePathDTO formulaUsablePath)
      {
         var path = selectFormulaUseablePath(formulaUsablePath, isValideTableObject, AppConstants.Captions.TableObjectPath);
         if (path == null) return;

         AddCommand(_moBiFormulaTask.ChangeTableObject(_formula, path, BuildingBlock));
         _view.ShowTableObjectPath(_formulaUsablePathMapper.MapFrom(path, _formula));
      }

      private bool isValideTableObject(IObjectBase para)
      {
         if (!para.IsAnImplementationOf<IParameter>()) return false;
         return ((IParameter) para).Formula.IsTable();
      }

      private IFormulaUsablePath selectFormulaUseablePath(FormulaUsablePathDTO initPath, Func<IObjectBase, bool> predicate, string caption)
      {
         using (var presenter = _applicationController.Start<ISelectFormulaUsablePathPresenter>())
         {
            var refererncePresneter = _selectReferencePresenterFactory.ReferenceAtParameterFor(UsingObject.ParentContainer);
            presenter.Init(predicate, UsingObject, new[] { UsingObject.RootContainer }, caption, refererncePresneter);
            return presenter.GetSelection();
         }
      }
   }
}