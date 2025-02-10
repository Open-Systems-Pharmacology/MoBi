using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface INewNameForExpressionProfileBuildingBlockPresenter : IDisposablePresenter
   {
      string NewNameFrom(string molecule, string species, string category, ExpressionType expressionType, IEnumerable<string> forbiddenNames, bool allowRename);
   }

   public class NewNameForExpressionProfileBuildingBlockPresenter : AbstractDisposablePresenter<INewNameExpressionProfileBuildingBlockView, INewNameForExpressionProfileBuildingBlockPresenter>, INewNameForExpressionProfileBuildingBlockPresenter
   {
      private readonly IRenameExpressionProfileDTOCreator _renameExpressionProfileDTOCreator;
      private IReadOnlyList<string> _forbiddenNames;
      private RenameExpressionProfileDTO _dto;

      public NewNameForExpressionProfileBuildingBlockPresenter(INewNameExpressionProfileBuildingBlockView view, IRenameExpressionProfileDTOCreator renameExpressionProfileDTOCreator) : base(view)
      {
         _renameExpressionProfileDTOCreator = renameExpressionProfileDTOCreator;
         _view.Caption = Captions.NewName;
      }

      public string NewNameFrom(string molecule, string species, string category, ExpressionType expressionType, IEnumerable<string> forbiddenNames, bool allowRename)
      {
         _forbiddenNames = (forbiddenNames ?? Enumerable.Empty<string>()).ToList();
         _dto = _renameExpressionProfileDTOCreator.Create(molecule, species, category, expressionType);
         _dto.AddForbiddenNames(_forbiddenNames);

         if (allowRename)
         {
            _dto.AllowCaseOnlyChangesFor(_dto.Name);
            _view.Caption = Captions.Rename;
         }

         _view.BindTo(_dto);
         _view.Display();

         return !_view.Canceled ? _dto.Name : string.Empty;
      }
   }

   public interface INewNameExpressionProfileBuildingBlockView : IModalView<INewNameForExpressionProfileBuildingBlockPresenter>
   {
      void BindTo(RenameExpressionProfileDTO dto);
   }
}