using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface INewNameForExpressionProfileBuildingBlockPresenter : IDisposablePresenter
   {
      string NewNameFrom(string molecule, string species, string category, ExpressionType expressionType, IEnumerable<string> forbiddenNames);
   }

   public class NewNameForExpressionProfileBuildingBlockPresenter : AbstractDisposablePresenter<INewNameExpressionProfileBuildingBlockView, INewNameForExpressionProfileBuildingBlockPresenter>, INewNameForExpressionProfileBuildingBlockPresenter
   {
      private readonly IRenameExpressionProfileDTOCreator _expressionProfileDTOCreator;
      private IReadOnlyList<string> _forbiddenNames;
      private RenameExpressionProfileDTO _dto;

      public NewNameForExpressionProfileBuildingBlockPresenter(INewNameExpressionProfileBuildingBlockView view, IRenameExpressionProfileDTOCreator expressionProfileDTOCreator) : base(view)
      {
         _expressionProfileDTOCreator = expressionProfileDTOCreator;
         _view.Caption = Captions.Rename;
      }

      public string NewNameFrom(string molecule, string species, string category, ExpressionType expressionType, IEnumerable<string> forbiddenNames)
      {
         _forbiddenNames = (forbiddenNames ?? Enumerable.Empty<string>()).ToList();
         _dto = _expressionProfileDTOCreator.Create(molecule, species, category, expressionType);
         _dto.AddForbiddenNames(_forbiddenNames);
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
