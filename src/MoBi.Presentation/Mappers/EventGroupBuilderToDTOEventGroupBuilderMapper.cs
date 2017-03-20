using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.Mappers
{
   public interface IEventGroupBuilderToDTOEventGroupBuilderMapper : IMapper<IEventGroupBuilder, EventGroupBuilderDTO>
   {
   }

   internal class EventGroupBuilderToDTOEventGroupBuilderMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventGroupBuilderToDTOEventGroupBuilderMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterToDTOParameterMapper;
      private readonly IEventBuilderToDTOEventBuilderMapper _eventBuilderToDTOEventBuilderMapper;
      private readonly IContainerToContainerDTOMapper _containerToDTOContainerMapper;

      /// <summary>
      ///    field has to be initialised in different way in derived Types
      /// </summary>
      protected IApplicationBuilderToDTOApplicationBuilderMapper _applicationBuilderToDTOApplicationBuilderMapper;

      public EventGroupBuilderToDTOEventGroupBuilderMapper(IParameterToParameterDTOMapper parameterToDTOParameterMapper,
         IEventBuilderToDTOEventBuilderMapper eventBuilderToDTOEventBuilderMapper,
         IContainerToContainerDTOMapper containerToDTOContainerMapper)
      {
         _parameterToDTOParameterMapper = parameterToDTOParameterMapper;
         _containerToDTOContainerMapper = containerToDTOContainerMapper;
         _eventBuilderToDTOEventBuilderMapper = eventBuilderToDTOEventBuilderMapper;
      }

      public EventGroupBuilderDTO MapFrom(IEventGroupBuilder input)
      {
         //TODO -This should be refactor to avoid usage of IoC
         _applicationBuilderToDTOApplicationBuilderMapper = IoC.Resolve<IApplicationBuilderToDTOApplicationBuilderMapper>();
         return MapEventGroupProperties(input, new EventGroupBuilderDTO());
      }

      protected T MapEventGroupProperties<T>(IEventGroupBuilder input, T dto) where T : EventGroupBuilderDTO
      {
         MapProperties(input, dto);
         dto.Parameters = input.GetChildrenSortedByName<IParameter>().MapAllUsing(_parameterToDTOParameterMapper).Cast<ParameterDTO>();
         dto.Events = input.Events.OrderBy(x => x.Name).MapAllUsing(_eventBuilderToDTOEventBuilderMapper);
         dto.EventGroups = input.GetChildrenSortedByName<IEventGroupBuilder>(child => !child.IsAnImplementationOf<IApplicationBuilder>()).MapAllUsing(this);
         dto.Applications = input.GetChildrenSortedByName<IApplicationBuilder>().MapAllUsing(_applicationBuilderToDTOApplicationBuilderMapper);
         dto.ChildContainer = input.GetChildrenSortedByName<IContainer>(isPureContainer).MapAllUsing(_containerToDTOContainerMapper);
         return dto;
      }

      private bool isPureContainer(IContainer arg)
      {
         return !arg.IsAnImplementationOf<IEventGroupBuilder>() && !arg.IsAnImplementationOf<IEventBuilder>() && !arg.IsAnImplementationOf<ITransportBuilder>();
      }
   }

   public interface IApplicationBuilderToDTOApplicationBuilderMapper : IMapper<IApplicationBuilder, ApplicationBuilderDTO>
   {
   }

   internal class ApplicationBuilderToDTOApplicationBuilderMapper : EventGroupBuilderToDTOEventGroupBuilderMapper, IApplicationBuilderToDTOApplicationBuilderMapper
   {
      private readonly ITransportBuilderToDTOTransportBuilderMapper _transportBuilderToDTOTransportBuilderToDTOTransportBuilder;
      private readonly IApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder _applicationMoleculeBuilderToDTOApplicationMoleculeBuilderMapper;

      public ApplicationBuilderToDTOApplicationBuilderMapper(IParameterToParameterDTOMapper parameterToDTOParameterMapper, IEventBuilderToDTOEventBuilderMapper eventBuilderToDTOEventBuilderMapper, ITransportBuilderToDTOTransportBuilderMapper transportBuilderToDTOTransportBuilderToDTOTransportBuilder, IApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder applicationMoleculeBuilderToDTOApplicationMoleculeBuilderMapper, IContainerToContainerDTOMapper containerToDTOContainerMapper, IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionMapperToDTODescriptorConditionMapper)
         : base(parameterToDTOParameterMapper, eventBuilderToDTOEventBuilderMapper, containerToDTOContainerMapper)
      {
         _transportBuilderToDTOTransportBuilderToDTOTransportBuilder = transportBuilderToDTOTransportBuilderToDTOTransportBuilder;
         _applicationMoleculeBuilderToDTOApplicationMoleculeBuilderMapper = applicationMoleculeBuilderToDTOApplicationMoleculeBuilderMapper;
      }

      public ApplicationBuilderDTO MapFrom(IApplicationBuilder input)
      {
         _applicationBuilderToDTOApplicationBuilderMapper = this;
         var dto = MapEventGroupProperties(input, new ApplicationBuilderDTO());
         dto.MoleculeName = input.MoleculeName;
         dto.Transports = input.Transports.MapAllUsing(_transportBuilderToDTOTransportBuilderToDTOTransportBuilder);
         dto.Molecules = input.Molecules.MapAllUsing(_applicationMoleculeBuilderToDTOApplicationMoleculeBuilderMapper);
         return dto;
      }
   }

   public interface IApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder : IMapper<IApplicationMoleculeBuilder, ApplicationMoleculeBuilderDTO>
   {
   }

   internal class ApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder : ObjectBaseToObjectBaseDTOMapperBase, IApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaBuilderToDTOFormulaMapper;

      public ApplicationMoleculeBuilderToDTOApplicationMoleculeBuilder(IFormulaToFormulaBuilderDTOMapper formulaBuilderToDTOFormulaMapper)
      {
         _formulaBuilderToDTOFormulaMapper = formulaBuilderToDTOFormulaMapper;
      }

      public ApplicationMoleculeBuilderDTO MapFrom(IApplicationMoleculeBuilder applicationMoleculeBuilder)
      {
         var dto = Map<ApplicationMoleculeBuilderDTO>(applicationMoleculeBuilder);
         dto.RelativeContainerPath = applicationMoleculeBuilder.RelativeContainerPath == null ? string.Empty : applicationMoleculeBuilder.RelativeContainerPath.PathAsString;
         dto.Formula = _formulaBuilderToDTOFormulaMapper.MapFrom(applicationMoleculeBuilder.Formula);
         return dto;
      }
   }

   public interface IEventBuilderToDTOEventBuilderMapper : IMapper<IEventBuilder, EventBuilderDTO>
   {
   }

   internal class EventBuilderToDTOEventBuilderMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventBuilderToDTOEventBuilderMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterToDTOParameterMapper;

      private readonly IEventAssignmentBuilderToDTOEventAssignmentMapper _assignmentToDTOAssignmentMapper;

      public EventBuilderToDTOEventBuilderMapper(IParameterToParameterDTOMapper parameterToDTOParameterMapper, IEventAssignmentBuilderToDTOEventAssignmentMapper assignmentToDTOAssignmentMapper)
      {
         _parameterToDTOParameterMapper = parameterToDTOParameterMapper;
         _assignmentToDTOAssignmentMapper = assignmentToDTOAssignmentMapper;
      }

      public EventBuilderDTO MapFrom(IEventBuilder eventBuilder)
      {
         var dto = Map<EventBuilderDTO>(eventBuilder);
         dto.OneTime = eventBuilder.OneTime;
         dto.Condition = eventBuilder.Formula != null ? eventBuilder.Formula.Name : string.Empty;
         dto.Parameter = eventBuilder.Parameters.MapAllUsing(_parameterToDTOParameterMapper).Cast<ParameterDTO>();
         dto.Assignments = eventBuilder.Assignments.MapAllUsing(_assignmentToDTOAssignmentMapper);
         return dto;
      }
   }

   public interface IEventAssignmentBuilderToDTOEventAssignmentMapper : IMapper<IEventAssignmentBuilder, EventAssignmentBuilderDTO>
   {
   }

   internal class EventAssignmentBuilderToDTOEventAssignmentMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventAssignmentBuilderToDTOEventAssignmentMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;

      public EventAssignmentBuilderToDTOEventAssignmentMapper(IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper)
      {
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
      }

      public EventAssignmentBuilderDTO MapFrom(IEventAssignmentBuilder eventAssignmentBuilder)
      {
         var dto = Map<EventAssignmentBuilderDTO>(eventAssignmentBuilder);
         dto.ChangedEntityPath = eventAssignmentBuilder.ObjectPath == null ? string.Empty : eventAssignmentBuilder.ObjectPath.PathAsString;
         dto.NewFormula = _formulaToDTOFormulaMapper.MapFrom(eventAssignmentBuilder.Formula);
         dto.UseAsValue = eventAssignmentBuilder.UseAsValue;
         return dto;
      }
   }
}