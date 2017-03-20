using System.Collections;
using System.Linq;
using OSPSuite.Utility.Events;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Helper;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.Converter.v3_3;

using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.ProjectConversion.v3_3
{
   public abstract class concern_for_Converter32To33Specs : ContextSpecification<IMoBiObjectConverter>
   {
      private IMoBiContext _context;
      
      private IDimension _noDimension;
      private IFormulaTask _formulaTask;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
         A.CallTo(() => dimensionFactory.NoDimension).Returns(_noDimension);
         _noDimension = A.Fake<IDimension>();
         _formulaTask = A.Fake<IFormulaTask>();
         sut = new Converter321To331(_context,_formulaTask);
      }
   }

   class When_converting_an_molecule_buidding_block : concern_for_Converter32To33Specs
   {
      private IMoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _moleculeBuildingBlock.Add(new MoleculeBuilder().WithName("Drug"));
         _moleculeBuildingBlock.Add(new MoleculeBuilder().WithName("Transporter"));
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuildingBlock, A.Fake<IMoBiProject>());
      }

      [Observation]
      public void should_add_total_drug_mass_parameter_to_all_molecule_builders()
      {
         foreach (var moleculeBuilder in _moleculeBuildingBlock)
         {
            var parameter = moleculeBuilder.GetSingleChildByName<IParameter>("Total drug mass");
            parameter.ShouldNotBeNull();
            parameter.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Property);
            parameter.Formula.ShouldBeAnInstanceOf<SumFormula>();
         }
      }
   }

   class When_converting_observer_building_block : concern_for_Converter32To33Specs
   {
      private IFormulaUsablePath _totalDrugMassReference;
      private IObserverBuildingBlock _observerBuildingBlock;

      protected override void Context()
      {
         base.Context();
         var monitor = new ExplicitFormula("TotalDrugMass");
         var observer = new ContainerObserverBuilder().WithName("Tralala").WithFormula(monitor);
         _totalDrugMassReference = new FormulaUsablePath(new[] { "Applications", "TotalDrugMass" }).WithAlias("TotalDrugMass");
         monitor.AddObjectPath(_totalDrugMassReference);
         _observerBuildingBlock = new ObserverBuildingBlock();
         _observerBuildingBlock.Add(observer);
      }

      protected override void Because()
      {
         sut.Convert(_observerBuildingBlock, A.Fake<IMoBiProject>());
      }

      [Observation]
      public void should_have_canged_total_drug_mass_parameter_reference()
      {
         _totalDrugMassReference.ShouldOnlyContainInOrder(ObjectPathKeywords.MOLECULE,"Total drug mass");
      }

   }

   class When_converting_an_event_group : concern_for_Converter32To33Specs
   {
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private IParameter _drugMassParameter;
      private IEventGroupBuilder applications;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock();
         var eventGroup = new EventGroupBuilder().WithName("EG");
         _eventGroupBuildingBlock.Add(eventGroup);
         var ehcStartEvent = new EventBuilder().WithName(Converter321To331.EHCStartEvent);
         eventGroup.Add(ehcStartEvent);
         var eabOldStart = new EventAssignmentBuilder().WithName("EAB1").WithFormula(new ExplicitFormula("ln(2) / EHC_Halftime * M * EHC_EjectionFraction"));
         eabOldStart.UseAsValue = false;
         eabOldStart.ObjectPath = new ObjectPath(new[] { "Organism", "Gallbladder", "Gallbladder emptying rate" });
         ehcStartEvent.AddAssignment(eabOldStart);
         var assignmentBuilder = new EventAssignmentBuilder().WithName("EAB2").WithFormula(new ExplicitFormula("DontChange"));
         assignmentBuilder.ObjectPath = new ObjectPath( new[]{"PATH"});
         ehcStartEvent.AddAssignment(assignmentBuilder);
         var ehcStopEvent = new EventBuilder().WithName(Converter321To331.EHCStopEvent);
         var eabOldStop = new EventAssignmentBuilder().WithName("EAB1").WithFormula(new ConstantFormula(0));
         eabOldStop.UseAsValue = false;
         eabOldStop.ObjectPath = new ObjectPath(new[] { "Organism", "Gallbladder", "Gallbladder emptying rate" });
         ehcStopEvent.AddAssignment(eabOldStop);
         assignmentBuilder = new EventAssignmentBuilder().WithName("EAB2").WithFormula(new ExplicitFormula("DontChange"));
         assignmentBuilder.ObjectPath = new ObjectPath(new[] { "PATH" });
         ehcStopEvent.AddAssignment(assignmentBuilder);
         eventGroup.Add(ehcStopEvent);

         var protocolSchemaItem = new Container().WithName("ProtocolSchemaItem").WithParentContainer(eventGroup);
         _drugMassParameter = new Parameter().WithName("DrugMass").WithParentContainer(protocolSchemaItem).WithValue(1);

         applications = new EventGroupBuilder().WithName("Applications");
         applications.Add(new Parameter().WithName("TotalDrugMass"));
         _eventGroupBuildingBlock.Add(applications);
      }

      protected override void Because()
      {
         sut.Convert(_eventGroupBuildingBlock, A.Fake<IMoBiProject>());
      }

      [Observation]
      public void should_change_EHC_Start_event_to_assign_Gallbladder_emptying_active()
      {
         var eventGroup = _eventGroupBuildingBlock.First();
         var ehcStartEvent = eventGroup.GetSingleChildByName<IEventBuilder>(Converter321To331.EHCStartEvent);         
         var assingment = ehcStartEvent.Assignments.FirstOrDefault(a => a.ObjectPath.PathAsString.Equals("Organism|Gallbladder|Gallbladder emptying active"));
         assingment.ShouldNotBeNull();
         assingment.UseAsValue.ShouldBeFalse();
         assingment.Formula.IsConstant().ShouldBeTrue();
         ((ConstantFormula)assingment.Formula).Value.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_change_EHC_Stop_event_to_assign_Gallbladder_emptying_active()
      {
         var eventGroup = _eventGroupBuildingBlock.First();
         var ehcStopEvent= eventGroup.GetSingleChildByName<IEventBuilder>(Converter321To331.EHCStopEvent);
         var assingment = ehcStopEvent.Assignments.FirstOrDefault(a => a.ObjectPath.PathAsString.Equals("Organism|Gallbladder|Gallbladder emptying active"));
         assingment.ShouldNotBeNull();
         assingment.UseAsValue.ShouldBeTrue();
         assingment.Formula.IsConstant().ShouldBeTrue();
         ((ConstantFormula)assingment.Formula).Value.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_leave_other_assignments_unchanged()
      {
         var eventGroup = _eventGroupBuildingBlock.First();
         eventGroup.GetSingleChildByName<IEventBuilder>(Converter321To331.EHCStartEvent);
         foreach (var eventBuilder in eventGroup.GetChildren<IEventBuilder>())
         {
            var assingment = eventBuilder.Assignments.FindByName("EAB2");
            assingment.Formula.IsExplicit().ShouldBeTrue();
            ((ExplicitFormula)assingment.Formula).FormulaString.ShouldBeEqualTo("DontChange");
            assingment.ObjectPath.PathAsString.ShouldBeEqualTo("PATH");
         }
      }

      [Observation]
      public void should_add_molecule_tag_to_drugmass_Parameter()
      {
         var moleculeTag = _drugMassParameter.Tags.FirstOrDefault(t => t.Value.Equals(ObjectPathKeywords.MOLECULE));
         moleculeTag.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_removed_total_drug_mass()
      {
         applications.ContainsName("TotalDrugMass").ShouldBeFalse();
      }
   }

   class When_converting_an_spatial_Structure : concern_for_Converter32To33Specs
   {
      private ISpatialStructure _spatialStructure;
      private Parameter _param;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure();
         var organism = new Container().WithName(Constants.ORGANISM);
         _spatialStructure.AddTopContainer(organism);
         var gallbladder = new Container().WithName(Converter321To331.Gallbladder).WithParentContainer(organism);
         _param = new Parameter().WithName("Volume").WithParentContainer(gallbladder);
         _spatialStructure.GlobalMoleculeDependentProperties = new Container();
         _spatialStructure.NeighborhoodsContainer = new Container();
      }

      protected override void Because()
      {
         sut.Convert(_spatialStructure, A.Fake<IMoBiProject>());
      }

      [Observation]
      public void should_have_added_gallbladderemptying_active_parameter()
      {
         var organism = _spatialStructure.TopContainers.First(c => c.Name.Equals(Constants.ORGANISM));
         var gallbladder = organism.GetSingleChildByName<IContainer>(Converter321To331.Gallbladder);
         var parameter = gallbladder.GetSingleChildByName<IParameter>(Converter321To331.GallbladderEmptyingActive);
         parameter.ShouldNotBeNull();
         parameter.Formula.IsExplicit();
      }

      [Observation]
      public void should_set_group_of_param_to_mobi()
      {
         _param.GroupName.ShouldBeEqualTo(Constants.Groups.MOBI);
      }

   }

   class When_converting_an_GallBladderEmptyingTransportBuilder : concern_for_Converter32To33Specs
   {
      private ITransportBuilder _transportBuilder;
      private IMoBiProject _project;
      private ExplicitFormula _kinetic;
      private IPassiveTransportBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _project= A.Fake<IMoBiProject>();
         _kinetic = new ExplicitFormula("GallbladderEmptyingRate");
         _transportBuilder = new TransportBuilder().WithName("GallbladderEmptying").WithKinetic(_kinetic);
         _buildingBlock  = new PassiveTransportBuildingBlock();
         _buildingBlock.Add(_transportBuilder);
      }

      protected override void Because()
      {
         sut.Convert(_buildingBlock, _project);
      }

      [Observation]
      public void should_change_the_kinetic_to_new_rate()
      {
         _kinetic.FormulaString.ShouldBeEqualTo("EHC_Active ? ln(2) / EHC_Halftime * M * EHC_EjectionFraction : 0");
         var alias = _kinetic.ObjectPaths.Select(op => op.Alias);
         alias.ShouldOnlyContain("EHC_Active", "EHC_Halftime","M","EHC_EjectionFraction");
      }
   }
}	