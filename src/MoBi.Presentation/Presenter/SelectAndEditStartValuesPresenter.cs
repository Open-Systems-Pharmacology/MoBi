using System.Drawing;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Collections;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectAndEditStartValuesPresenter<out TBuildingBlock> : ISimulationConfigurationItemPresenter, ISelectAndEditPresenter
   {
      void Refresh();
      TBuildingBlock StartValues { get; }
   }

   public abstract class SelectAndEditStartValuesPresenter<TBuildingBlock, TStartValue> : AbstractSubPresenter<ISelectAndEditContainerView, ISelectAndEditPresenter>, ISelectAndEditStartValuesPresenter<TBuildingBlock>
      where TBuildingBlock : class, IStartValuesBuildingBlock<TStartValue>
      where TStartValue : class, IStartValue
   {
      private readonly IStartValuesTask<TBuildingBlock, TStartValue> _startValuesTask;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected IMoBiBuildConfiguration _buildConfiguration;
      public TBuildingBlock StartValues { get; private set; }
      protected ICache<string, TStartValue> _templateStartValues;
      private readonly IStartValuesPresenter _editPresenter;

      protected SelectAndEditStartValuesPresenter(
         ISelectAndEditContainerView view,
         IStartValuesTask<TBuildingBlock, TStartValue> startValuesTask,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IObjectTypeResolver objectTypeResolver,
         IStartValuesPresenter editPresenter,
         ILegendPresenter legendPresenter) : base(view)
      {
         _editPresenter = editPresenter;
         _startValuesTask = startValuesTask;
         _objectTypeResolver = objectTypeResolver;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _editPresenter.CanCreateNewFormula = false;
         legendPresenter.AddLegendItems(new []{ new LegendItemDTO {Description = AppConstants.Captions.AutomaticallyGeneratedValues, Color = MoBiColors.Extended} });
         View.AddLegendView(legendPresenter.View);
         AddSubPresenters(_editPresenter, legendPresenter);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _buildConfiguration = simulation.MoBiBuildConfiguration;
         Refresh();
      }

      public void AddToProject()
      {
         AddCommand(_startValuesTask.AddToProject(_cloneManagerForBuildingBlock.CloneBuildingBlock(StartValues)));
      }

      public void ValidateStartValues()
      {
         if (bothEmpty()) return;
         var msvCache = StartValues.ToCache();
         if (_templateStartValues.Keys.Any(msvCache.Contains))
            return;

         throw new InvalidBuildConfigurationException(AppConstants.Exceptions.InvalidStartValuesConfiguration(StartValues.Name, _objectTypeResolver.TypeFor(StartValues)));
      }

      public object Subject => _editPresenter.Subject;

      private bool bothEmpty()
      {
         return !_templateStartValues.Any() && !StartValues.Any();
      }

      protected virtual Color DisplayColorFor(TStartValue startValue)
      {
         if (IsTemplate(startValue))
            return Color.White;

         return MoBiColors.Extended;
      }

      protected bool IsTemplate(TStartValue startValue)
      {
         return _templateStartValues.Contains(startValue.Path.ToString());
      }

      public virtual void Refresh()
      {
         _editPresenter.OnlyShowFilterSelection();
      }

      protected void Refresh(IBuildingBlockInfo<TBuildingBlock> startValueBuildingBlockInfo)
      {
         if (startValueBuildingBlockInfo.TemplateBuildingBlock == null) return;
         _templateStartValues = startValueBuildingBlockInfo.TemplateBuildingBlock.ToCache();

         StartValues = _startValuesTask.CreateStartValuesForSimulation(_buildConfiguration);
         StartValues.Version = startValueBuildingBlockInfo.TemplateBuildingBlock.Version;
         _view.Description = AppConstants.Captions.TemporaryStartValuesBasedOn(_objectTypeResolver.TypeFor(StartValues), StartValues.Name);
      }
   }
}