using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Core
{
   public abstract class concern_for_ObjectTypeResolver: ContextSpecification<IObjectTypeResolver>
   {
      protected override void Context()
      {
         sut = new ObjectTypeResolver();
      }
   }

   public class When_resolving_the_type_for_known_object : concern_for_ObjectTypeResolver
   {
      [Observation]
      public void should_return_then_expected_type()
      {
         sut.TypeFor<MoleculeBuildingBlock>().ShouldBeEqualTo(ObjectTypes.MoleculeBuildingBlock);
         sut.TypeFor<ReactionBuildingBlock>().ShouldBeEqualTo(ObjectTypes.ReactionBuildingBlock);
         sut.TypeFor<SpatialStructure>().ShouldBeEqualTo(ObjectTypes.SpatialStructure);
         sut.TypeFor<PassiveTransportBuildingBlock>().ShouldBeEqualTo(ObjectTypes.PassiveTransportBuildingBlock);
         sut.TypeFor<ObserverBuildingBlock>().ShouldBeEqualTo(ObjectTypes.ObserverBuildingBlock);
         sut.TypeFor<EventGroupBuildingBlock>().ShouldBeEqualTo(ObjectTypes.EventGroupBuildingBlock);
         sut.TypeFor<InitialConditionsBuildingBlock>().ShouldBeEqualTo(ObjectTypes.InitialConditionsBuildingBlock);
         sut.TypeFor<ParameterValuesBuildingBlock>().ShouldBeEqualTo(ObjectTypes.ParameterValuesBuildingBlock);
         sut.TypeFor<MoleculeBuilder>().ShouldBeEqualTo(ObjectTypes.Molecule);
         sut.TypeFor<TransporterMoleculeContainer>().ShouldBeEqualTo(ObjectTypes.TransporterMoleculeContainer);
         sut.TypeFor<TransportBuilder>().ShouldBeEqualTo(ObjectTypes.TransportBuilder);
         sut.TypeFor<Parameter>().ShouldBeEqualTo(ObjectTypes.Parameter);
         sut.TypeFor<DistributedParameter>().ShouldBeEqualTo(ObjectTypes.DistributedParameter);
         sut.TypeFor<Container>().ShouldBeEqualTo(ObjectTypes.Container);
         sut.TypeFor<ReactionBuilder>().ShouldBeEqualTo(ObjectTypes.Reaction);
         sut.TypeFor<AmountObserverBuilder>().ShouldBeEqualTo(ObjectTypes.AmountObserverBuilder);
         sut.TypeFor<ContainerObserverBuilder>().ShouldBeEqualTo(ObjectTypes.ContainerObserverBuilder);
         sut.TypeFor<EventGroupBuilder>().ShouldBeEqualTo(ObjectTypes.EventGroupBuilder);
         sut.TypeFor<EventBuilder>().ShouldBeEqualTo(ObjectTypes.EventBuilder);
         sut.TypeFor<ApplicationBuilder>().ShouldBeEqualTo(ObjectTypes.Application);
         sut.TypeFor<EventAssignmentBuilder>().ShouldBeEqualTo(ObjectTypes.EventAssignmentBuilder);
         sut.TypeFor<ApplicationMoleculeBuilder>().ShouldBeEqualTo(ObjectTypes.ApplicationMoleculeBuilder);
         sut.TypeFor<Formula>().ShouldBeEqualTo(ObjectTypes.Formula);
         sut.TypeFor<ReactionPartnerBuilder>().ShouldBeEqualTo(ObjectTypes.ReactionPartnerBuilder);
         sut.TypeFor<FormulaUsablePath>().ShouldBeEqualTo(ObjectTypes.Reference);
         sut.TypeFor<ConstantFormula>().ShouldBeEqualTo(ObjectTypes.ConstantFormula);
         sut.TypeFor<ExplicitFormula>().ShouldBeEqualTo(ObjectTypes.ExplicitFormula);
         sut.TypeFor<BlackBoxFormula>().ShouldBeEqualTo(ObjectTypes.BlackBoxFormula);
         sut.TypeFor<MoBiSimulation>().ShouldBeEqualTo(ObjectTypes.Simulation);
         sut.TypeFor<DataRepository>().ShouldBeEqualTo(ObjectTypes.ObservedData);
         sut.TypeFor<SimulationSettings>().ShouldBeEqualTo(ObjectTypes.SimulationSettings);
         sut.TypeFor<CurveChartTemplate>().ShouldBeEqualTo(ObjectTypes.ChartTemplate);
         sut.TypeFor<InitialCondition>().ShouldBeEqualTo(ObjectTypes.InitialCondition);
         sut.TypeFor<ParameterValue>().ShouldBeEqualTo(ObjectTypes.ParameterValue);
         sut.TypeFor<ObserverBuilder>().ShouldBeEqualTo(ObjectTypes.ObserverBuilder);
         sut.TypeFor<TimePath>().ShouldBeEqualTo(ObjectTypes.Reference);
         sut.TypeFor<ObjectPath>().ShouldBeEqualTo(ObjectTypes.Reference);
         sut.TypeFor<ObjectReference>().ShouldBeEqualTo(ObjectTypes.Reference);
         sut.TypeFor<MoBiHistoryManager>().ShouldBeEqualTo(ObjectTypes.History);
         sut.TypeFor<MoBiProject>().ShouldBeEqualTo(ObjectTypes.Project);
         sut.TypeFor<IDiagramModel>().ShouldBeEqualTo(ObjectTypes.DiagramModel);
      }
   }
}	