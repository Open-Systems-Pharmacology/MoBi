using System;
using System.Drawing;
using MoBi.Core;
using MoBi.Presentation.Settings;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Helpers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using MergeConflictViewSettings = MoBi.Presentation.Settings.MergeConflictViewSettings;

namespace MoBi.Presentation.Presenter
{
   public interface IMergeConflictResolverPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Shows the summary of the merge object and target object as well as indicates the number of remaining conflicts to be
      ///    resolved
      /// </summary>
      /// <param name="merge">The element being merged into the target</param>
      /// <param name="target">The conflicting target element</param>
      /// <param name="remainingConflicts">The number of remaining conflicts that will have to be resolved after the current</param>
      /// <returns>The decision of the user on how to resolve this conflict</returns>
      MergeConflictOptions ShowConflict(IObjectBase merge, IObjectBase target, int remainingConflicts);

      /// <summary>
      ///    Sets the decision of the user in the presenter
      /// </summary>
      /// <param name="option">The option to be used to resolve the current conflict</param>
      void SetConflictResolution(MergeConflictOptions option);

      /// <summary>
      ///    A function that creates a summary dto describing the object being compared.
      /// </summary>
      Func<IObjectBase, ObjectBaseSummaryDTO> ObjectBaseSummaryDTOMapper { get; set; }

      /// <summary>
      ///    Enable the view to show an option for clone
      /// </summary>
      bool CloneOptionEnabled { set; }

      bool MergeOptionEnabled { set; }

      void FormClosing(Point location, Size size);
   }

   internal class MergeConflictResolverPresenter : MoBiDisposablePresenter<IMergeConflictResolverView, IMergeConflictResolverPresenter>, IMergeConflictResolverPresenter
   {
      private MergeConflictOptions _conflictOption;
      private readonly IObjectBaseSummaryPresenter _mergeObjectBaseSummaryPresenter;
      private readonly IObjectBaseSummaryPresenter _targetObjectBaseSummaryPresenter;
      private readonly IUserSettings _userSettings;
      private MergeConflictViewSettings mergeConflictViewSettings => _userSettings.MergeConflictViewSettings;

      public MergeConflictResolverPresenter(
         IMergeConflictResolverView view,
         IObjectBaseSummaryPresenter mergeObjectBaseSummaryPresenter,
         IObjectBaseSummaryPresenter targetObjectBaseSummaryPresenter, IUserSettings userSettings) : base(view)
      {
         _conflictOption = MergeConflictOptions.SkipOnce;

         _mergeObjectBaseSummaryPresenter = mergeObjectBaseSummaryPresenter;
         _targetObjectBaseSummaryPresenter = targetObjectBaseSummaryPresenter;
         _userSettings = userSettings;

         AddSubPresenters(_mergeObjectBaseSummaryPresenter, _targetObjectBaseSummaryPresenter);
      }

      public MergeConflictOptions ShowConflict(IObjectBase merge, IObjectBase target, int remainingConflicts)
      {
         _mergeObjectBaseSummaryPresenter.BindTo(ObjectBaseSummaryDTOMapper(merge));
         _targetObjectBaseSummaryPresenter.BindTo(ObjectBaseSummaryDTOMapper(target));

         _view.AttachSummaryViews(_mergeObjectBaseSummaryPresenter.BaseView, _targetObjectBaseSummaryPresenter.BaseView, remainingConflicts);
         displayView();

         return _conflictOption;
      }

      public void SetConflictResolution(MergeConflictOptions option)
      {
         _conflictOption = option;
         _view.CloseView();
      }

      private void displayView()
      {
         _view.SetFormLayout(mergeConflictViewSettings.Location, mergeConflictViewSettings.Size);
         _view.Display();
      }

      public virtual void FormClosing(Point location, Size size)
      {
         saveFormLayout(location, size);
      }

      private  void saveFormLayout(Point location, Size size)
      {
         mergeConflictViewSettings.Location = location;
         mergeConflictViewSettings.Size = size;
      }
      public Func<IObjectBase, ObjectBaseSummaryDTO> ObjectBaseSummaryDTOMapper { get; set; }

      public bool CloneOptionEnabled
      {
         set { _view.CloneButtonEnabled = value; }
      }

      public bool MergeOptionEnabled
      {
         set { _view.MergeOptionEnabled = value; }
      }
   }
}