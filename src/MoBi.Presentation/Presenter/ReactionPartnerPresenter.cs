using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IReactionPartnerPresenter<TReactionPartnerBuilder> : IPresenter<IReactionBuilderView<TReactionPartnerBuilder>>, IPresenterWithContextMenu<IViewItem>, ISubjectPresenter
   {
      bool HasError { get; }
      void AddNewReactionPartnerBuilder();
      void SetStoichiometricCoefficient(double newCoefficient, ReactionPartnerBuilderDTO reactionPartnerDTO);
      void SetPartnerMoleculeName(string newMoleculeName, ReactionPartnerBuilderDTO reactionPartnerDTO);
      void Remove(TReactionPartnerBuilder reactionPartnerBuilderDTO);
      void Edit(ReactionBuilderDTO reactionBuilderDTO, IBuildingBlock buildingBlock);
   }

   public abstract class ReactionPartnerPresenter<TReactionPartnerBuilder> : AbstractCommandCollectorPresenter<IReactionBuilderView<TReactionPartnerBuilder>, IReactionPartnerPresenter<TReactionPartnerBuilder>>, IReactionPartnerPresenter<TReactionPartnerBuilder>
   {
      private readonly IMoBiContext _context;
      protected ReactionBuilderDTO _reactionBuilderDTO;
      private IBuildingBlock _buildingBlock;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IInteractionTasksForReactionBuilder _reactionBuilderTask;

      protected ReactionPartnerPresenter(IReactionBuilderView<TReactionPartnerBuilder> view, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory, IInteractionTasksForReactionBuilder reactionBuilderTask) : base(view)
      {
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _reactionBuilderTask = reactionBuilderTask;
      }

      public void SetStoichiometricCoefficient(double newCoefficient, ReactionPartnerBuilderDTO reactionPartnerDTO)
      {
         var partner = reactionPartnerDTO.PartnerBuilder;
         AddCommand(new EditReactionPartnerStoichiometricCoefficientCommand(newCoefficient, _reactionBuilderDTO.ReactionBuilder, partner, ReactionBuildingBlock).RunCommand(_context));
      }

      public void SetPartnerMoleculeName(string newMoleculeName, ReactionPartnerBuilderDTO reactionPartnerDTO)
      {
         var partner = reactionPartnerDTO.PartnerBuilder;
         AddCommand(new EditReactionPartnerMoleculeNameCommand(newMoleculeName, _reactionBuilderDTO.ReactionBuilder, partner, ReactionBuildingBlock).RunCommand(_context));
      }

      protected MoBiReactionBuildingBlock ReactionBuildingBlock => _buildingBlock.DowncastTo<MoBiReactionBuildingBlock>();

      public virtual void Edit(ReactionBuilderDTO reactionBuilderDTO, IBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
         _reactionBuilderDTO = reactionBuilderDTO;
         _view.BindTo(PartnerBuilders());
      }

      protected abstract IReadOnlyList<TReactionPartnerBuilder> PartnerBuilders();

      public bool HasError => _view.HasError;

      public void AddNewReactionPartnerBuilder()
      {
         var moleculeNames = _reactionBuilderTask.SelectMoleculeNames(ReactionBuildingBlock, ExistingPartners(), _reactionBuilderDTO.Name, PartnerType);
         moleculeNames.Each(moleculeName => AddCommand(AddCommandFor(moleculeName).RunCommand(_context)));
      }

      public abstract string PartnerType { get; }

      protected abstract IEnumerable<string> ExistingPartners();

      protected abstract ICommand<IMoBiContext> AddCommandFor(string moleculeName);

      public void Remove(TReactionPartnerBuilder reactionPartnerBuilderDTO)
      {
         AddCommand(RemoveCommandFor(reactionPartnerBuilderDTO).RunCommand(_context));
      }

      protected abstract ICommand<IMoBiContext> RemoveCommandFor(TReactionPartnerBuilder reactionPartnerBuilderDTO);

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public object Subject => _reactionBuilderDTO.ReactionBuilder;
   }
}