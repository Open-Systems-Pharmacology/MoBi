using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditDistributedParameterPresenter : ContextSpecification<IEditDistributedParameterPresenter>
   {
      protected IEditDistributedParameterView _view;
      protected IEditTaskFor<IParameter> _editTask;
      protected IMoBiContext _context;
      protected IDistributedParameterToDistributedParameterDTOMapper _distributedParameterMapper;
      protected IDistributionFormulaFactory _distributionFormulaFactory;
      protected IQuantityTask _quantityTask;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IMoBiFormulaTask _formulaTask;
      protected IParameterFactory _parameterFactory;
      protected IInteractionTasksForParameter _parameterTask;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         _commandCollector= A.Fake<ICommandCollector>();
         _view = A.Fake<IEditDistributedParameterView>();
         _editTask = A.Fake<IEditTaskFor<IParameter>>();
         _context = A.Fake<IMoBiContext>();
         _distributedParameterMapper = A.Fake<IDistributedParameterToDistributedParameterDTOMapper>();
         _distributionFormulaFactory = A.Fake<IDistributionFormulaFactory>();
         _quantityTask = A.Fake<IQuantityTask>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _parameterFactory = A.Fake<IParameterFactory>();
         _parameterTask = A.Fake<IInteractionTasksForParameter>();

         sut = new EditDistributedParameterPresenter(_view, _editTask, _context, _distributedParameterMapper, _distributionFormulaFactory, _quantityTask, _formulaMapper,
            _formulaTask, _parameterFactory, _parameterTask);

         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_setting_the_display_unit_of_a_distrubtion_parameter : concern_for_EditDistributedParameterPresenter
   {
      private IDistributedParameter _distributedParameter;
      private Unit _unit;
      private DistributionParameterDTO _distributedParmaeterDTO;
      private IParameter _meanParameter;

      protected override void Context()
      {
         base.Context();
         _unit= A.Fake<Unit>();
         _distributedParameter = A.Fake<IDistributedParameter>();
         _meanParameter = A.Fake<IParameter>();
         _distributedParmaeterDTO = new DistributionParameterDTO(_meanParameter);
         sut.Edit(_distributedParameter);
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_distributedParmaeterDTO, _unit);
      }

      [Observation]
      public void should_also_update_the_percentile_of_the_distributed_parameter()
      {
         A.CallTo(()=>_distributedParameter.RefreshPercentile()).MustHaveHappened();
      }
   }
}