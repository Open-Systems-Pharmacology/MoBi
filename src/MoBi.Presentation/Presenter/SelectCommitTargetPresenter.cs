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
      public CommitTarget(ModuleConfiguration moduleConfiguration, ParameterValuesBuildingBlock parameterValues, InitialConditionsBuildingBlock initialConditions)
      {
         ModuleConfiguration = moduleConfiguration;
         ParameterValues = parameterValues;
         InitialConditions = initialConditions;
      }

      public ModuleConfiguration ModuleConfiguration { get; }

      /// <summary>
      ///    Template building block receiving the parameter value changes. Null when a new building block should be created
      /// </summary>
      public ParameterValuesBuildingBlock ParameterValues { get; }

      /// <summary>
      ///    Template building block receiving the initial condition changes. Null when a new building block should be created
      /// </summary>
      public InitialConditionsBuildingBlock InitialConditions { get; }

      public bool CreateNewParameterValues => ParameterValues == null;

      public bool CreateNewInitialConditions => InitialConditions == null;
   }

   public interface ISelectCommitTargetPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Asks the user to select the module and, for each type, the building block (or a new one) where the changes
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
      private readonly IItemToListItemMapper<ParameterValuesBuildingBlock> _parameterValuesToListItemMapper;
      private readonly IItemToListItemMapper<InitialConditionsBuildingBlock> _initialConditionsToListItemMapper;
      private readonly ParameterValuesBuildingBlock _newParameterValues;
      private readonly InitialConditionsBuildingBlock _newInitialConditions;
      private IReadOnlyList<ModuleConfiguration> _moduleConfigurations;

      public SelectCommitTargetPresenter(ISelectCommitTargetView view,
         ITemplateResolverTask templateResolverTask,
         IItemToListItemMapper<Module> moduleToListItemMapper,
         IItemToListItemMapper<ParameterValuesBuildingBlock> parameterValuesToListItemMapper,
         IItemToListItemMapper<InitialConditionsBuildingBlock> initialConditionsToListItemMapper) : base(view)
      {
         _templateResolverTask = templateResolverTask;
         _moduleToListItemMapper = moduleToListItemMapper;
         _moduleToListItemMapper.Initialize(x => x.Name);
         _parameterValuesToListItemMapper = parameterValuesToListItemMapper;
         _parameterValuesToListItemMapper.Initialize(x => x.Name);
         _initialConditionsToListItemMapper = initialConditionsToListItemMapper;
         _initialConditionsToListItemMapper.Initialize(x => x.Name);
         _newParameterValues = new ParameterValuesBuildingBlock().WithName(AppConstants.Captions.NewWindow(ObjectTypes.ParameterValuesBuildingBlock));
         _newInitialConditions = new InitialConditionsBuildingBlock().WithName(AppConstants.Captions.NewWindow(ObjectTypes.InitialConditionsBuildingBlock));
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
         var selectedInitialConditions = _view.SelectedInitialConditions;
         return new CommitTarget(moduleConfigurationFor(_view.SelectedModule),
            ReferenceEquals(selectedParameterValues, _newParameterValues) ? null : selectedParameterValues,
            ReferenceEquals(selectedInitialConditions, _newInitialConditions) ? null : selectedInitialConditions);
      }

      public void ModuleChanged() => bindBuildingBlocksFor(moduleConfigurationFor(_view.SelectedModule));

      private void bindBuildingBlocksFor(ModuleConfiguration moduleConfiguration)
      {
         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);

         var allParameterValues = templateModule.ParameterValuesCollection.Concat(new[] { _newParameterValues });
         var selectedParameterValues = moduleConfiguration.SelectedParameterValues == null
            ? _newParameterValues
            : _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);

         _view.BindParameterValues(allParameterValues.Select(_parameterValuesToListItemMapper.MapFrom), selectedParameterValues);

         var allInitialConditions = templateModule.InitialConditionsCollection.Concat(new[] { _newInitialConditions });
         var selectedInitialConditions = moduleConfiguration.SelectedInitialConditions == null
            ? _newInitialConditions
            : _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);

         _view.BindInitialConditions(allInitialConditions.Select(_initialConditionsToListItemMapper.MapFrom), selectedInitialConditions);
      }

      private ModuleConfiguration moduleConfigurationFor(Module module) => _moduleConfigurations.First(x => Equals(x.Module, module));
   }
}
