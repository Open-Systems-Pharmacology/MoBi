using System.Collections.Generic;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectFolderAndIndividualFromProjectPresenter : IDisposablePresenter
   {
      /// <summary>
      /// Opens a dialog where the user will select an IndividualBuildingBlock from the project and a file path to export
      /// </summary>
      /// <param name="name"></param>
      void GetPathAndIndividualForExport(string name);

      /// <summary>
      /// Returns a list of all IndividualBuildingBlocks in the project
      /// </summary>
      IReadOnlyList<IndividualBuildingBlock> AllIndividuals { get; }

      /// <summary>
      /// Opens a dialog for the user to select file path
      /// </summary>
      /// <returns>The path if dialog is dismissed with ok, empty string if canceled</returns>
      string BrowseFilePath();

      /// <summary>
      /// After dismissing the dialog, this returns the selected IndividualBuildingBlock or null if the dialog was canceled
      /// </summary>
      IndividualBuildingBlock SelectedIndividual { get; }
      
      /// <summary>
      /// After dismissing the dialog, this returns the selected file path for export, or an empty string if the dialog was canceled
      /// </summary>
      string SelectedFilePath { get; }
   }
   
   public class SelectFolderAndIndividualFromProjectPresenter : MoBiDisposablePresenter<ISelectFolderAndIndividualFromProjectView, ISelectFolderAndIndividualFromProjectPresenter>, ISelectFolderAndIndividualFromProjectPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IEditTaskForContainer _editTaskForContainer;
      private IndividualAndFilePathDTO _dto;

      public SelectFolderAndIndividualFromProjectPresenter(ISelectFolderAndIndividualFromProjectView view, IBuildingBlockRepository buildingBlockRepository, IEditTaskForContainer editTaskForContainer) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _editTaskForContainer = editTaskForContainer;
      }

      public void GetPathAndIndividualForExport(string name)
      {
         _dto = new IndividualAndFilePathDTO().WithName(name);
         _view.BindTo(_dto);
         _view.Display();
         
         if (!_view.Canceled)
            return;
         
         _dto.IndividualBuildingBlock = null;
         _dto.FilePath = string.Empty;
      }

      public string SelectedFilePath => _dto.FilePath;
      
      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals => _buildingBlockRepository.IndividualsCollection;
      
      public IndividualBuildingBlock SelectedIndividual => _dto.IndividualBuildingBlock;
      
      public string BrowseFilePath() => _editTaskForContainer.BrowseSavePathFor(_dto.Name);
   }
}