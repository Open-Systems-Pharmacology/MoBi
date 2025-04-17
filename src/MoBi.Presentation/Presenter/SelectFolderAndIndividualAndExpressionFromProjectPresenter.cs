using System.Collections.Generic;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectFolderAndIndividualAndExpressionFromProjectPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Opens a dialog where the user will select an IndividualBuildingBlock from the project and a file path to export
      /// </summary>
      (string path, IndividualBuildingBlock individual, IReadOnlyList<ExpressionProfileBuildingBlock> expressions) GetPathIndividualAndExpressionsForExport(IContainer container);

      /// <summary>
      ///    Returns a list of all IndividualBuildingBlocks in the project
      /// </summary>
      IReadOnlyList<IndividualBuildingBlock> AllIndividuals { get; }

      /// <summary>
      ///    Opens a dialog for the user to select file path
      /// </summary>
      /// <returns>The path if dialog is dismissed with ok, empty string if canceled</returns>
      string BrowseFilePath();

      void SelectionChanged();
   }

   public class SelectFolderAndIndividualAndExpressionFromProjectPresenter : MoBiDisposablePresenter<ISelectFolderAndIndividualAndExpressionFromProjectView, ISelectFolderAndIndividualAndExpressionFromProjectPresenter>, ISelectFolderAndIndividualAndExpressionFromProjectPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IEditTaskForContainer _editTaskForContainer;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IIndividualExpressionAndFilePathDTOMapper _mapper;
      private IndividualExpressionAndFilePathDTO _dto;

      public SelectFolderAndIndividualAndExpressionFromProjectPresenter(
         ISelectFolderAndIndividualAndExpressionFromProjectView view, 
         IBuildingBlockRepository buildingBlockRepository, 
         IEditTaskForContainer editTaskForContainer, 
         IObjectPathFactory objectPathFactory,
         IIndividualExpressionAndFilePathDTOMapper mapper) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _editTaskForContainer = editTaskForContainer;
         _objectPathFactory = objectPathFactory;
         _mapper = mapper;
      }

      public (string path, IndividualBuildingBlock individual, IReadOnlyList<ExpressionProfileBuildingBlock> expressions) GetPathIndividualAndExpressionsForExport(IContainer container)
      {
         _dto = mapFrom(container);
         _view.BindTo(_dto);
         _view.Display();

         return _view.Canceled ? (string.Empty, null, null) : (_dto.FilePath, _dto.IndividualBuildingBlock, ExpressionProfileBuildingBlocks: _dto.SelectedExpressionProfileBuildingBlocks);
      }

      private IndividualExpressionAndFilePathDTO mapFrom(IContainer container)
      {
         var dto = _mapper.MapFrom(_buildingBlockRepository.ExpressionProfileCollection).WithName(container.Name);
         dto.ContainerPath = _objectPathFactory.CreateAbsoluteObjectPath(container);
         return dto;
      }

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals => _buildingBlockRepository.IndividualsCollection;

      public string BrowseFilePath() => _editTaskForContainer.BrowseSavePathFor(_dto.Name);
      public void SelectionChanged() => OnStatusChanged();
   }
}