using System;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IHistoryPresenter : IMainViewItemPresenter,
      IListener<ProjectLoadedEvent>,
      IListener<ProjectClosedEvent>,
      IListener<ObjectConvertedEvent>

   {
   }

   public class HistoryPresenter : IHistoryPresenter
   {
      private readonly IMoBiContext _context;
      private readonly IHistoryBrowserPresenter _historyBrowserPresenter;
      private readonly IRegionResolver _regionResolver;
      private readonly IApplicationConfiguration _applicationConfiguration;
      private readonly IDisplayNameProvider _displayNameProvider;
      private IRegion _region;
      public event EventHandler StatusChanged = delegate { };

      public HistoryPresenter(IMoBiContext context, IHistoryBrowserPresenter historyBrowserPresenter, IRegionResolver regionResolver,
         IApplicationConfiguration applicationConfiguration, IDisplayNameProvider displayNameProvider)
      {
         _context = context;
         _historyBrowserPresenter = historyBrowserPresenter;
         _regionResolver = regionResolver;
         _applicationConfiguration = applicationConfiguration;
         _displayNameProvider = displayNameProvider;
         _historyBrowserPresenter.EnableHistoryPruning = false;
      }

      public void Initialize()
      {
         _region = _regionResolver.RegionWithName(RegionNames.History);
         _historyBrowserPresenter.Initialize();
         _region.Add(_historyBrowserPresenter.View);
         _historyBrowserPresenter.UpdateHistory();
      }

      public void ToggleVisibility()
      {
         if (!_region.IsVisible)
            _historyBrowserPresenter.UpdateHistory();
         _region.ToggleVisibility();
      }

      public void Handle(ProjectLoadedEvent eventToHandle)
      {
         refreshHistory();
      }

      private void refreshHistory()
      {
         _historyBrowserPresenter.HistoryManager = _context.HistoryManager;
         _historyBrowserPresenter.UpdateHistory();
      }

      public void ViewChanged()
      {
         //nothing to do
      }

      public bool CanClose => true;

      public IView BaseView => null;

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         refreshHistory();
      }

      public void ReleaseFrom(IEventPublisher eventPublisher)
      {
         eventPublisher.RemoveListener(this);
      }

      public void Handle(ObjectConvertedEvent eventToHandle)
      {
         var objectName = _displayNameProvider.DisplayNameFor(eventToHandle.ConvertedObject);
         var objectType = _context.TypeFor(eventToHandle.ConvertedObject);
         var fromVersion = eventToHandle.FromVersion;

         var command = new OSPSuiteInfoCommand
         {
            ObjectType = _context.TypeFor(eventToHandle.ConvertedObject),
            Description = AppConstants.Commands.ObjectConvertedCommand(objectName, objectType, fromVersion.VersionDisplay, _applicationConfiguration.Version),
         };

         command.RunCommand(_context);
         _context.AddToHistory(command);
      }
   }
}