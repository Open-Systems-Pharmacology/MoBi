using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
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
      ///    Asks the user to select the module and, for each type with changes, the building block (or a new one) where
      ///    the changes of <paramref name="simulation" /> will be committed. The last module is preselected as the default
      ///    and the building block selection is hidden for a type without changes.
      ///    Returns null if the user cancels the selection
      /// </summary>
      CommitTarget SelectCommitTargetFor(IMoBiSimulation simulation, bool hasParameterChanges, bool hasMoleculeChanges);

      void ModuleChanged();
      IReadOnlyList<Module> AllModules { get; }
      IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFor(Module module);
      IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFor(Module module);
   }

   public class SelectCommitTargetPresenter : AbstractDisposablePresenter<ISelectCommitTargetView, ISelectCommitTargetPresenter>, ISelectCommitTargetPresenter
   {
      private readonly ITemplateResolverTask _templateResolverTask;
      private readonly ParameterValuesBuildingBlock _newParameterValues;
      private readonly InitialConditionsBuildingBlock _newInitialConditions;
      private IReadOnlyList<ModuleConfiguration> _moduleConfigurations;
      private CommitTargetDTO _commitTargetDTO;

      public SelectCommitTargetPresenter(ISelectCommitTargetView view, ITemplateResolverTask templateResolverTask) : base(view)
      {
         _templateResolverTask = templateResolverTask;
         _newParameterValues = new ParameterValuesBuildingBlock().WithName(AppConstants.Captions.NewWindow(ObjectTypes.ParameterValuesBuildingBlock));
         _newInitialConditions = new InitialConditionsBuildingBlock().WithName(AppConstants.Captions.NewWindow(ObjectTypes.InitialConditionsBuildingBlock));
      }

      public CommitTarget SelectCommitTargetFor(IMoBiSimulation simulation, bool hasParameterChanges, bool hasMoleculeChanges)
      {
         _moduleConfigurations = simulation.Configuration.ModuleConfigurations;
         _commitTargetDTO = new CommitTargetDTO { Module = _moduleConfigurations.Last().Module };
         setDefaultBuildingBlocks();

         if (!hasParameterChanges)
            _view.HideParameterValuesSelection();

         if (!hasMoleculeChanges)
            _view.HideInitialConditionsSelection();

         _view.SetDescription(AppConstants.Captions.SelectCommitTargetDescription);
         _view.BindTo(_commitTargetDTO);
         _view.Display();

         if (_view.Canceled)
            return null;

         return new CommitTarget(moduleConfigurationFor(_commitTargetDTO.Module),
            ReferenceEquals(_commitTargetDTO.ParameterValues, _newParameterValues) ? null : _commitTargetDTO.ParameterValues,
            ReferenceEquals(_commitTargetDTO.InitialConditions, _newInitialConditions) ? null : _commitTargetDTO.InitialConditions);
      }

      public void ModuleChanged()
      {
         setDefaultBuildingBlocks();
         _view.BindTo(_commitTargetDTO);
      }

      public IReadOnlyList<Module> AllModules => _moduleConfigurations.Select(x => x.Module).ToList();

      public IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFor(Module module)
      {
         return _templateResolverTask.TemplateModuleFor(module).ParameterValuesCollection.Concat(new[] { _newParameterValues }).ToList();
      }

      public IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFor(Module module)
      {
         return _templateResolverTask.TemplateModuleFor(module).InitialConditionsCollection.Concat(new[] { _newInitialConditions }).ToList();
      }

      /// <summary>
      ///    Preselects the template of the building block used by the module configuration, or the create-new entry
      ///    when none is used, so that the selection is never empty
      /// </summary>
      private void setDefaultBuildingBlocks()
      {
         var moduleConfiguration = moduleConfigurationFor(_commitTargetDTO.Module);

         var templateParameterValues = moduleConfiguration.SelectedParameterValues == null ? null : _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);
         _commitTargetDTO.ParameterValues = templateParameterValues ?? _newParameterValues;

         var templateInitialConditions = moduleConfiguration.SelectedInitialConditions == null ? null : _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);
         _commitTargetDTO.InitialConditions = templateInitialConditions ?? _newInitialConditions;
      }

      private ModuleConfiguration moduleConfigurationFor(Module module) => _moduleConfigurations.First(x => Equals(x.Module, module));
   }
}
