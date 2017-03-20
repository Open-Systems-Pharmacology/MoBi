using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_PassiveTransportConverter : ContextSpecification<IPassiveTransportConverter>
   {
      protected IContainerTask _containerTask;
      private IObjectBaseFactory _objectBaseFactory;
      private IMoBiProjectRetriever _projectRetriever;
      protected IMoleculeBuilder _moleculeWithPassiveTransport;
      protected TransportBuilder _passiveTransport;
      protected IPassiveTransportBuildingBlock _passiveTransports;
      protected IMoleculeBuildingBlock _molecules;
      protected IMoBiProject _project;
      private IEventPublisher _eventPublisher;
      protected ShowNotificationsEvent _showNotificationEvent;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IFormulaTask _formulaTask;
      protected IFormula _passiveTransportKinetic;

      protected override void Context()
      {
         _containerTask = A.Fake<IContainerTask>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _formulaTask = A.Fake<IFormulaTask>();

         sut = new PassiveTransportConverter(_objectBaseFactory, _projectRetriever, _containerTask, _eventPublisher, _cloneManagerForModel, _formulaTask);

         A.CallTo(() => _containerTask.CreateUniqueName(A<IEnumerable<IWithName>>._, A<string>._, true))
            .ReturnsLazily(x => x.Arguments[1].ToString());

         _passiveTransportKinetic = new ExplicitFormula("1+2");
         _moleculeWithPassiveTransport = new MoleculeBuilder().WithName("MOLECULE");
         _passiveTransport = new TransportBuilder().WithName("PASSIVE TRANSPORT").WithFormula(_passiveTransportKinetic);
         _moleculeWithPassiveTransport.Add(_passiveTransport);
         _passiveTransports = new PassiveTransportBuildingBlock();
         _molecules = new MoleculeBuildingBlock {_moleculeWithPassiveTransport}.WithName("MBB");
         _project = new MoBiProject();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         A.CallTo(() => _eventPublisher.PublishEvent(A<ShowNotificationsEvent>._))
            .Invokes(x => _showNotificationEvent = x.GetArgument<ShowNotificationsEvent>(0));
      }
   }

   public class When_converting_a_simulation_using_passive_transports_defined_in_molecule_builer : concern_for_PassiveTransportConverter
   {
      private IMoBiSimulation _simulation;
      private TransportBuilder _clonedPassiveTransport;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _simulation.BuildConfiguration.Molecules = _molecules;
         _simulation.BuildConfiguration.PassiveTransports = _passiveTransports;
         _clonedPassiveTransport = new TransportBuilder().WithName("PASSIVE TRANSPORT");
         A.CallTo(() => _cloneManagerForModel.Clone<ITransportBuilder>(_passiveTransport)).Returns(_clonedPassiveTransport);
      }

      protected override void Because()
      {
         sut.Convert(_simulation);
      }

      [Observation]
      public void should_add_the_passive_transport_in_the_passive_transport_building_block()
      {
         _passiveTransports.ShouldContain(_clonedPassiveTransport);
      }

      [Observation]
      public void should_have_renamed_the_passive_transport_to_contain_the_name_of_the_molecule()
      {
         _clonedPassiveTransport.Name.ShouldBeEqualTo(AppConstants.CompositeNameFor(_passiveTransport.Name, _moleculeWithPassiveTransport.Name));
      }

      [Observation]
      public void the_passive_transport_should_only_transport_the_molecule_builder_it_was_attached_to()
      {
         _clonedPassiveTransport.ForAll.ShouldBeFalse();
         _clonedPassiveTransport.MoleculeNames().ShouldOnlyContain(_moleculeWithPassiveTransport.Name);
      }

      [Observation]
      public void should_not_notify_any_warning_or_info()
      {
         _showNotificationEvent.ShouldBeNull();
      }
   }

   public class When_converting_a_molecule_building_block_using_a_molecule_with_a_passive_transport : concern_for_PassiveTransportConverter
   {
      protected override void Because()
      {
         sut.Convert(_molecules);
      }

      [Observation]
      public void should_have_added_a_new_passive_transport_to_the_project()
      {
         _project.PassiveTransportCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_have_named_the_newly_added_passive_transport_after_the_molecule_building_block()
      {
         _project.PassiveTransportCollection.ElementAt(0).Name.ShouldBeEqualTo(_molecules.Name);
      }

      [Observation]
      public void should_notify_an_info_to_the_user()
      {
         _showNotificationEvent.ShouldNotBeNull();
         _showNotificationEvent.NotificationMessages.Count.ShouldBeEqualTo(1);
         _showNotificationEvent.NotificationMessages[0].BuildingBlock.ShouldBeEqualTo(_project.PassiveTransportCollection.ElementAt(0));
      }
   }

   public class When_converting_a_project : concern_for_PassiveTransportConverter
   {
      private IPassiveTransportBuildingBlock _passiveTransports2;
      protected override void Context()
      {
         base.Context();
         _passiveTransports2 = new PassiveTransportBuildingBlock();
         _project.AddBuildingBlock(_passiveTransports);
         _project.AddBuildingBlock(_passiveTransports2);
         _project.AddBuildingBlock(_molecules);

         var anotherMoleculeWithPassiveTransport = new MoleculeBuilder().WithName("MOLECULE");
         var molecules2 = new MoleculeBuildingBlock {anotherMoleculeWithPassiveTransport}.WithName("MBB2");
         var samePassiveTransport = new TransportBuilder().WithName("PASSIVE TRANSPORT").WithFormula(A.Fake<IFormula>());
         anotherMoleculeWithPassiveTransport.Add(samePassiveTransport);

         A.CallTo(() => _formulaTask.FormulasAreTheSame(_passiveTransportKinetic, samePassiveTransport.Formula)).Returns(true);

         A.CallTo(() => _cloneManagerForModel.Clone<ITransportBuilder>(_passiveTransport)).ReturnsLazily(x => new TransportBuilder().WithFormula(_passiveTransportKinetic));
         A.CallTo(() => _containerTask.CreateUniqueName(_passiveTransports, A<string>._, true)).ReturnsLazily(x => x.Arguments[1].ConvertedTo<string>());
         _project.AddBuildingBlock(molecules2);
      }

      protected override void Because()
      {
         sut.Convert(_project);
      }

      [Observation]
      public void should_have_added_all_passive_transport_of_all_molecules_in_all_passive_transport_building_blocks()
      {
         _passiveTransports.Count().ShouldBeEqualTo(1);
         _passiveTransports2.Count().ShouldBeEqualTo(1);
         Assert.AreNotSame(_passiveTransports.First(),_passiveTransports2.First());
      }
   }
}