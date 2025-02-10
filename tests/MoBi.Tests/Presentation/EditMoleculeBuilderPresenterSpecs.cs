using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public class concern_for_EditMoleculeBuilderPresenter : ContextSpecification<EditMoleculeBuilderPresenter>
   {
      protected IEditMoleculeBuilderView _view;
      protected IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderDTOMapper;
      protected IEditParametersInContainerPresenter _editMoleculeParameters;
      protected IEditTaskFor<MoleculeBuilder> _editTasks;
      protected IEditFormulaInContainerPresenter _editFormulaPresenter;
      protected IMoBiContext _context;
      protected ISelectReferenceAtMoleculePresenter _selectReferencePresenter;
      protected IReactionDimensionRetriever _dimensionRetriever;
      protected ICoreCalculationMethodRepository _calculationMethodsRepository;

      protected override void Context()
      {
         _view = A.Fake<IEditMoleculeBuilderView>();
         _moleculeBuilderDTOMapper = A.Fake<IMoleculeBuilderToMoleculeBuilderDTOMapper>();
         _editMoleculeParameters = A.Fake<IEditParametersInContainerPresenter>();
         _editTasks = A.Fake<IEditTaskFor<MoleculeBuilder>>();
         _editFormulaPresenter = A.Fake<IEditFormulaInContainerPresenter>();
         _context = A.Fake<IMoBiContext>();
         _selectReferencePresenter = A.Fake<ISelectReferenceAtMoleculePresenter>();
         _dimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _calculationMethodsRepository = A.Fake<ICoreCalculationMethodRepository>();
         
         sut = new EditMoleculeBuilderPresenter(_view, _moleculeBuilderDTOMapper, _editMoleculeParameters, _editTasks, _editFormulaPresenter, _context, _selectReferencePresenter, _dimensionRetriever, _calculationMethodsRepository);
      }
   }

   public class When_initializing_command_collector : concern_for_EditMoleculeBuilderPresenter
   {
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _commandCollector = new MoBiMacroCommand();
      }

      protected override void Because()
      {
         sut.InitializeWith(_commandCollector);
      }

      [Observation]
      public void the_sub_presenter_should_be_initialized()
      {
         A.CallTo(() => _editMoleculeParameters.InitializeWith(_commandCollector)).MustHaveHappened();
      }
   }

   public class When_editing_a_molecule_builder_without_parent_container : concern_for_EditMoleculeBuilderPresenter
   {
      private MoleculeBuilder _moleculeBuilder;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = new MoleculeBuilder();
      }

      protected override void Because()
      {
         sut.Edit(_moleculeBuilder);
      }

      [Observation]
      public void forbidden_names_should_be_resolved()
      {
         A.CallTo(() => _editTasks.GetForbiddenNamesWithoutSelf(_moleculeBuilder, null)).MustHaveHappened();
      }

      [Observation]
      public void the_sub_presenter_should_edit_the_same_molecule_builder()
      {
         A.CallTo(() => _editFormulaPresenter.Init(_moleculeBuilder, A<IBuildingBlock>._, A<FormulaDecoder<MoleculeBuilder>>._)).MustHaveHappened();
         A.CallTo(() => _editMoleculeParameters.Edit(_moleculeBuilder)).MustHaveHappened();
      }
   }
}
