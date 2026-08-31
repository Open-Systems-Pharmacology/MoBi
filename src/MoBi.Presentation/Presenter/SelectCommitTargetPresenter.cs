using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public class CommitTarget
   {
      public CommitTarget(ModuleConfiguration moduleConfiguration, ParameterValuesBuildingBlock parameterValues)
      {
         ModuleConfiguration = moduleConfiguration;
         ParameterValues = parameterValues;
      }

      public ModuleConfiguration ModuleConfiguration { get; }

      /// <summary>
      ///    Template building block receiving the parameter value changes. Null when a new building block should be created
      /// </summary>
      public ParameterValuesBuildingBlock ParameterValues { get; }

      public bool CreateNewParameterValues => ParameterValues == null;
   }

   public interface ISelectCommitTargetPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Asks the user to select the module and the parameter values building block (or a new one) where the changes
      ///    of <paramref name="simulation" /> will be committed. The last module is preselected as the default.
      ///    Returns null if the user cancels the selection
      /// </summary>
      CommitTarget SelectCommitTargetFor(IMoBiSimulation simulation);

      void ModuleChanged();
   }

   public class SelectCommitTargetPresenter : AbstractDisposablePresenter<ISelectCommitTargetView, ISelectCommitTargetPresenter>, ISelectCommitTargetPresenter
   {
      private readonly ITemplateResolverTask _templateResolverTask;
      private readonly IItemToListItemMapper<Module> _moduleToListItemMapper;
      private readonly IItemToListItemMapper<ParameterValuesBuildingBlock> _buildingBlockToListItemMapper;
      private readonly ParameterValuesBuildingBlock _newParameterValues;
      private IReadOnlyList<ModuleConfiguration> _moduleConfigurations;

      public SelectCommitTargetPresenter(ISelectCommitTargetView view,
         ITemplateResolverTask templateResolverTask,
         IItemToListItemMapper<Module> moduleToListItemMapper,
         IItemToListItemMapper<ParameterValuesBuildingBlock> buildingBlockToListItemMapper) : base(view)
      {
         _templateResolverTask = templateResolverTask;
         _moduleToListItemMapper = moduleToListItemMapper;
         _moduleToListItemMapper.Initialize(x => x.Name);
         _buildingBlockToListItemMapper = buildingBlockToListItemMapper;
         _buildingBlockToListItemMapper.Initialize(x => x.Name);
         _newParameterValues = new ParameterValuesBuildingBlock().WithName(AppConstants.Captions.NewWindow(ObjectTypes.ParameterValuesBuildingBlock));
      }

      public CommitTarget SelectCommitTargetFor(IMoBiSimulation simulation)
      {
         _moduleConfigurations = simulation.Configuration.ModuleConfigurations;
         var defaultModuleConfiguration = _moduleConfigurations.Last();

         _view.SetDescription(AppConstants.Captions.SelectCommitTargetDescription);
         _view.BindModules(_moduleConfigurations.Select(x => _moduleToListItemMapper.MapFrom(x.Module)), defaultModuleConfiguration.Module);
         bindBuildingBlocksFor(defaultModuleConfiguration);

         _view.Display();

         if (_view.Canceled)
            return null;

         var selectedParameterValues = _view.SelectedParameterValues;
         return new CommitTarget(moduleConfigurationFor(_view.SelectedModule), ReferenceEquals(selectedParameterValues, _newParameterValues) ? null : selectedParameterValues);
      }

      public void ModuleChanged() => bindBuildingBlocksFor(moduleConfigurationFor(_view.SelectedModule));

      private void bindBuildingBlocksFor(ModuleConfiguration moduleConfiguration)
      {
         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);
         var allParameterValues = templateModule.ParameterValuesCollection.Concat(new[] { _newParameterValues });
         var selectedParameterValues = moduleConfiguration.SelectedParameterValues == null
            ? _newParameterValues
            : _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);

         _view.BindParameterValues(allParameterValues.Select(_buildingBlockToListItemMapper.MapFrom), selectedParameterValues);
      }

      private ModuleConfiguration moduleConfigurationFor(Module module) => _moduleConfigurations.First(x => Equals(x.Module, module));
   }
}
