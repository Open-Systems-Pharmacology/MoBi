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
      ///    Opens a dialog where the user will select an IndividualBuildingBlock from the project and a file path to export
      /// </summary>
      (string, IndividualBuildingBlock) GetPathAndIndividualForExport(IContainer container);

      /// <summary>
      ///    Returns a list of all IndividualBuildingBlocks in the project
      /// </summary>
      IReadOnlyList<IndividualBuildingBlock> AllIndividuals { get; }

      /// <summary>
      ///    Opens a dialog for the user to select file path
      /// </summary>
      /// <returns>The path if dialog is dismissed with ok, empty string if canceled</returns>
      string BrowseFilePath();
   }

   public class SelectFolderAndIndividualFromProjectPresenter : MoBiDisposablePresenter<ISelectFolderAndIndividualFromProjectView, ISelectFolderAndIndividualFromProjectPresenter>, ISelectFolderAndIndividualFromProjectPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IEditTaskForContainer _editTaskForContainer;
      private readonly IObjectPathFactory _objectPathFactory;
      private IndividualAndFilePathDTO _dto;

      public SelectFolderAndIndividualFromProjectPresenter(ISelectFolderAndIndividualFromProjectView view, IBuildingBlockRepository buildingBlockRepository, IEditTaskForContainer editTaskForContainer, IObjectPathFactory objectPathFactory) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _editTaskForContainer = editTaskForContainer;
         _objectPathFactory = objectPathFactory;
      }

      public (string, IndividualBuildingBlock) GetPathAndIndividualForExport(IContainer container)
      {
         _dto = new IndividualAndFilePathDTO
         {
            ContainerPath = _objectPathFactory.CreateAbsoluteObjectPath(container)
         }.WithName(container.Name);
         _view.BindTo(_dto);
         _view.Display();

         return _view.Canceled ? (string.Empty, null) : (_dto.FilePath, _dto.IndividualBuildingBlock);
      }

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals => _buildingBlockRepository.IndividualsCollection;

      public string BrowseFilePath() => _editTaskForContainer.BrowseSavePathFor(_dto.Name);
   }
}