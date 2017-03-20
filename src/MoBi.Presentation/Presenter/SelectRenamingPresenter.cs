using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Presentation.Settings;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   /// <summary>
   ///    Presenter managing the selecting of dependant renames for an renamed entity.
   /// </summary>
   public interface ISelectRenamingPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Initializes the presenter with possible renamings that could be selected.
      /// </summary>
      /// <param name="possibleRenamings">The possible renamings.</param>
      void InitializeWith(IEnumerable<IStringChange> possibleRenamings);

      /// <summary>
      ///    Gets the commands for the selected renamings.
      /// </summary>
      /// <returns></returns>
      IReadOnlyList<IMoBiCommand> SelectedCommands();

      /// <summary>
      ///    Starts the Select Renaming View.
      /// </summary>
      /// <returns>
      ///    <c>true</c> if renamings are accepted, <c>false</c> id renaming is canceled
      /// </returns>
      bool SelectRenamings();

      /// <summary>
      /// Sets the default checked/unchecked state for all possible renames
      /// </summary>
      /// <param name="checkedState">If true, then all renames are checked by default, otherwise they are not checked</param>
      void SetCheckedStateForAll(bool checkedState);
   }

   internal class SelectRenamingPresenter : AbstractDisposablePresenter<ISelectRenamingView, ISelectRenamingPresenter>, ISelectRenamingPresenter
   {
      private readonly IStringChangeToSelectDTOStringChangeMapper _stringChangeToSelectDTOStringChangeMapper;
      private readonly IUserSettings _userSettings;
      private IEnumerable<SelectStringChangeDTO> _dtos;

      public SelectRenamingPresenter(ISelectRenamingView view, IUserSettings userSettings, IStringChangeToSelectDTOStringChangeMapper stringChangeToSelectDTOStringChangeMapper) : base(view)
      {
         _stringChangeToSelectDTOStringChangeMapper = stringChangeToSelectDTOStringChangeMapper;
         _userSettings = userSettings;
      }

      public void InitializeWith(IEnumerable<IStringChange> possibleRenamings)
      {
         var renameDependentObjectsDefault = _userSettings.RenameDependentObjectsDefault;
         _stringChangeToSelectDTOStringChangeMapper.Initialize(renameDependentObjectsDefault);
         _dtos = possibleRenamings.MapAllUsing(_stringChangeToSelectDTOStringChangeMapper);
         SetCheckedStateForAll(renameDependentObjectsDefault);
         _view.SetData(_dtos, renameDependentObjectsDefault);
      }

      public IReadOnlyList<IMoBiCommand> SelectedCommands()
      {
         var commands = _dtos.Where(dtoStringChange => dtoStringChange.Selected)
            .Select(dtoStringChange => dtoStringChange.Change.ChangeCommand).ToList();

         commands.Each(x => x.Visible = false);
         return commands;
      }

      public bool SelectRenamings()
      {
         _view.Display();
         var accepted = !_view.Canceled;
         if (accepted)
         {
            _userSettings.RenameDependentObjectsDefault = _view.RenameDefault;
         }
         return accepted;
      }

      public void SetCheckedStateForAll(bool checkedState)
      {
         _dtos.Each(dto => dto.Selected = checkedState);
      }
   }
}