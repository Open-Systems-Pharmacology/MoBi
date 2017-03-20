using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_ReactionEductsPresenter : ContextSpecification<ReactionEductsPresenter>
   {
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IMoBiContext _context;
      protected IReactionPartnerView _view;
      protected IReadOnlyList<ReactionPartnerBuilderDTO> _partnerBuilders;
      protected IMoBiReactionBuildingBlock _reactionBuildingBlock;
      protected ReactionBuilderDTO _reactionBuilderDTO;
      protected IInteractionTasksForReactionBuilder _interactionTaskForReactionBuilder;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _context = A.Fake<IMoBiContext>();
         _view = A.Fake<IReactionPartnerView>();
         _interactionTaskForReactionBuilder = A.Fake<IInteractionTasksForReactionBuilder>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new ReactionEductsPresenter(_view, _context, _viewItemContextMenuFactory, _interactionTaskForReactionBuilder);
         sut.InitializeWith(_commandCollector);
         _reactionBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>();
         A.CallTo(() => _reactionBuildingBlock.DiagramManager).Returns(A.Fake<IMoBiReactionDiagramManager>());
         _reactionBuilderDTO = new ReactionBuilderDTO(new ReactionBuilder() {Formula = A.Fake<IFormula>()}) { Educts = new BindingList<ReactionPartnerBuilderDTO>() };
      }
   }

   public class When_editing_a_reaction_builder : concern_for_ReactionEductsPresenter
   {
      protected override void Because()
      {
         sut.Edit(_reactionBuilderDTO, _reactionBuildingBlock);
      }

      [Observation]
      public void the_view_should_bind_to_the_partner_list()
      {
         A.CallTo(() => _view.BindTo(_reactionBuilderDTO.Educts)).MustHaveHappened();
      }
   }

   public class When_setting_the_stoichiometric_coefficient : concern_for_ReactionEductsPresenter
   {

      private ReactionPartnerBuilderDTO _reactionPartnerDTO;
      private IReactionPartnerBuilder _reactionPartnerBuilder;

      protected override void Context()
      {
         base.Context();
         
         _reactionPartnerBuilder = new ReactionPartnerBuilder("moleculeName", 2.0);
         _reactionPartnerDTO = new ReactionPartnerBuilderDTO(_reactionPartnerBuilder);

         sut.Edit(_reactionBuilderDTO, _reactionBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetStochiometricCoefficient(1.0, _reactionPartnerDTO);
      }

      [Observation]
      public void the_command_should_be_added_to_the_command_collector()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<ICommand>.That.Matches(x => x.IsAnImplementationOf<EditReactionPartnerStoichiometricCoefficientCommand>()))).MustHaveHappened();
      }

      [Observation]
      public void the_coefficient_should_be_changed()
      {
         _reactionPartnerBuilder.StoichiometricCoefficient.ShouldBeEqualTo(1.0);
      }
   }

   public class When_setting_the_partner_molecule_name : concern_for_ReactionEductsPresenter
   {
      private ReactionPartnerBuilderDTO _reactionPartnerDTO;
      private IReactionPartnerBuilder _reactionPartnerBuilder;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = new ReactionPartnerBuilder("moleculeName", 1.0);
         _reactionPartnerDTO = new ReactionPartnerBuilderDTO(_reactionPartnerBuilder);
         sut.InitializeWith(_commandCollector);
         sut.Edit(_reactionBuilderDTO, _reactionBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetPartnerMoleculeName("newMoleculeName", _reactionPartnerDTO);
      }

      [Observation]
      public void the_command_should_be_added_to_the_command_collector()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<ICommand>.That.Matches(x => x.IsAnImplementationOf<EditReactionPartnerMoleculeNameCommand>()))).MustHaveHappened();
      }

      [Observation]
      public void the_partner_molecule_name_must_have_changed()
      {
         _reactionPartnerBuilder.MoleculeName.ShouldBeEqualTo("newMoleculeName");
      }
   }

   public abstract class When_adding_or_removing_an_educt : concern_for_ReactionEductsPresenter
   {
      protected ReactionPartnerBuilder _reactionPartnerBuilder;
      protected ReactionPartnerBuilderDTO _reactionPartnerDTO;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = new ReactionPartnerBuilder("moleculeName", 2.0);
         _reactionPartnerDTO = new ReactionPartnerBuilderDTO(_reactionPartnerBuilder);
         _reactionBuilderDTO.Educts.Add(_reactionPartnerDTO);
         _reactionBuilderDTO.ReactionBuilder.AddEduct(_reactionPartnerBuilder);
         _reactionBuildingBlock.DiagramModel = A.Fake<IDiagramModel>();

         A.CallTo(() => _interactionTaskForReactionBuilder.SelectMoleculeNames(_reactionBuildingBlock, A<IEnumerable<string>>._, A<string>._, AppConstants.Captions.Educts)).Returns(new List<string> { "moleculeName" });

         sut.Edit(_reactionBuilderDTO, _reactionBuildingBlock);
      }
   }

   public class When_removing_an_educt : When_adding_or_removing_an_educt
   {
      protected override void Because()
      {
         sut.Remove(_reactionPartnerDTO);
      }

      [Observation]
      public void the_correct_command_must_be_used_to_remove_the_educt()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<ICommand>.That.Matches(x => x.IsAnImplementationOf<RemoveReactionPartnerFromEductCollection>()))).MustHaveHappened();
      }
   }

   public class When_adding_a_new_educt_ : When_adding_or_removing_an_educt
   {
      protected override void Because()
      {
         sut.AddNewReactionPartnerBuilder();
      }

      [Observation]
      public void the_list_of_molecules_that_could_be_added_must_be_retrieved()
      {
         A.CallTo(() => _interactionTaskForReactionBuilder.SelectMoleculeNames(_reactionBuildingBlock, A<IEnumerable<string>>.That.Matches(x => x.Contains("moleculeName")), _reactionBuilderDTO.Name, AppConstants.Captions.Educts)).MustHaveHappened();
      }

      [Observation]
      public void the_correct_command_must_be_used_to_create_the_educt()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<ICommand>.That.Matches(x => x.IsAnImplementationOf<AddReactionPartnerToEductCollection>()))).MustHaveHappened();
      }
   }
}
