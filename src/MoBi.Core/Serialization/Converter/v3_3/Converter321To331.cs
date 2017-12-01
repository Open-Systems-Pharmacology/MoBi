using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Serialization.Converter.v3_3
{
   /// <summary>
   ///    Converter For MoBi Files From Version 3.2 To 3.3
   ///    Renaming Elements from Chart To CurveChart
   ///    Change Entities related to EHC in PK-Sim
   ///    New Total Drug Mass Parameter
   /// </summary>
   public class Converter321To331 : IMoBiObjectConverter,
      IVisitor<IEventGroupBuildingBlock>,
      IVisitor<IPassiveTransportBuildingBlock>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<ISpatialStructure>,
      IVisitor<IObserverBuildingBlock>,
      IVisitor<IMoleculeBuildingBlock>,
      IVisitor<IReactionBuildingBlock>,
      IVisitor<SimulationTransfer>

   {
      public static string EHCStartEvent = "EHC_Start_Event";
      public static string EHCStopEvent = "EHC_Stop_Event";
      public static readonly string GallbladderEmptyingRate = "Gallbladder emptying rate";
      public static string GallbladderEmptying = "GallbladderEmptying";
      public static string Gallbladder = "Gallbladder";
      public static string GallbladderEmptyingOldFormula = "GallbladderEmptyingRate";
      public static string GallbladderEmptyingNewFormula = "EHC_Active ? ln(2) / EHC_Halftime * M * EHC_EjectionFraction : 0";
      public static string GallbladderEmptyingActive = "Gallbladder emptying active";
      public static string GallbladderEjectionFraction = "Gallbladder ejection fraction";
      public static string GallbladderEjectionHalfTime = "Gallbladder ejection half-time";
      public static string GallbladderEmptyingActiveAlias = "EHC_Active";
      public static string GallbladderEjectionFractionAlias = "EHC_EjectionFraction";
      public static string GallbladderEjectionHalfTimeAlias = "EHC_Halftime";
      public static ObjectPath GallbladderEmptyingRatePath = new ObjectPath(new[] {"Organism", "Gallbladder", "Gallbladder emptying rate"});
      private readonly IMoBiContext _context;
      public static string TotalDrugmassOld = "TotalDrugMass";
      public static string TotalDrugmassNew = "Total drug mass";
      public static string DrugMass = "DrugMass";
      public static string Applications = "Applications";
      private readonly IFormulaTask _formulaTask;
      private bool _converted;

      public Converter321To331(IMoBiContext context, IFormulaTask formulaTask)
      {
         _context = context;
         _formulaTask = formulaTask;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V3_2_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToConvert, IMoBiProject project)
      {
         _converted = false;
         this.Visit(objectToConvert);
         return (ProjectVersions.V3_3_1, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, IMoBiProject project)
      {
         _converted = false;
         convertChartToCurveChartElement(element);
         return (ProjectVersions.V3_3_1, _converted);
      }

      private void convertChartToCurveChartElement(XElement element)
      {
         var charts = element.DescendantsAndSelf("Chart");
         charts.Each(c =>
         {
            c.Name = "CurveChart";
            _converted = true;
         });
      }

      public void Visit(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         updateEHCEvents(eventGroupBuildingBlock);
         eventGroupBuildingBlock.Each(updateTotalDrugMass);
         updateAllParameters(eventGroupBuildingBlock.SelectMany(c => c.GetAllChildren<IParameter>()));
         _converted = true;
      }

      private void updateTotalDrugMass(IContainer container)
      {
         if (container == null) return;
         var allProtocolSchemaItems = container.GetAllContainersAndSelf<IContainer>(c => c.Name.Equals("ProtocolSchemaItem"));
         foreach (var protocolSchemaItem in allProtocolSchemaItems)
         {
            var drugMassParameter = protocolSchemaItem.GetSingleChildByName<IParameter>(DrugMass);
            if (drugMassParameter != null) drugMassParameter.AddTag(ObjectPathKeywords.MOLECULE);
         }
         var parameter = container.GetSingleChildByName<IParameter>(TotalDrugmassOld);
         if (parameter != null)
            container.RemoveChild(parameter);
      }

      private void updateEHCEvents(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         var startEvents =
            eventGroupBuildingBlock.SelectMany(eg => eg.GetAllChildren<IEventBuilder>(eb => eb.Name.Equals(EHCStartEvent)));
         startEvents.Each(se => changeAssignments(se, false, 1));
         var stopEvents =
            eventGroupBuildingBlock.SelectMany(eg => eg.GetAllChildren<IEventBuilder>(eb => eb.Name.Equals(EHCStopEvent)));
         stopEvents.Each(se => changeAssignments(se, true, 0));
      }

      private void changeAssignments(IEventBuilder startEvent, bool useAsValue, double value)
      {
         var assignment =
            startEvent.Assignments.FirstOrDefault(
               a => a.ObjectPath.Equals(GallbladderEmptyingRatePath));
         changeAssignment(useAsValue, value, assignment);
      }

      private void changeAssignments(IEvent startEvent, bool useAsValue, double value, IObjectPath gallbladderEmptyingRatePath)
      {
         var assignment =
            startEvent.Assignments.FirstOrDefault(
               a => a.ObjectPath.Equals(gallbladderEmptyingRatePath));

         changeAssignment(useAsValue, value, assignment);
      }

      private void changeAssignment(bool useAsValue, double value, IAssignment assignment)
      {
         if (assignment == null) return;
         assignment.ObjectPath.Replace(GallbladderEmptyingRate, GallbladderEmptyingActive);
         assignment.UseAsValue = useAsValue;
         assignment.Formula = _context.Create<ConstantFormula>().WithValue(value);
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         var transpotBuilder = passiveTransportBuildingBlock.FirstOrDefault(isOldGallbladderEmptyingTransport);

         if (transpotBuilder != null)
         {
            var kinetic = ((ExplicitFormula) transpotBuilder.Formula);
            updateEHCKinetic(kinetic, ObjectPathKeywords.MOLECULE);
         }
         updateAllParameters(passiveTransportBuildingBlock.SelectMany(c => c.GetAllChildren<IParameter>()));
         _converted = true;
      }

      private void updateEHCKinetic(ExplicitFormula kinetic, string moleculeName)
      {
         kinetic.FormulaString = GallbladderEmptyingNewFormula;
         var amount = _context.DimensionFactory.Dimension(Constants.Dimension.AMOUNT);
         var noDimension = _context.DimensionFactory.NoDimension;
         var time = _context.DimensionFactory.Dimension(Constants.Dimension.TIME);
         kinetic.ClearObjectPaths();
         kinetic.AddObjectPath(
            _context.ObjectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Gallbladder,
                  moleculeName)
               .WithAlias("M")
               .WithDimension(amount));
         kinetic.AddObjectPath(
            _context.ObjectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Gallbladder,
                  GallbladderEmptyingActive)
               .WithAlias(GallbladderEmptyingActiveAlias)
               .WithDimension(noDimension));
         kinetic.AddObjectPath(
            _context.ObjectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Gallbladder,
                  GallbladderEjectionFraction)
               .WithAlias(GallbladderEjectionFractionAlias)
               .WithDimension(amount));
         kinetic.AddObjectPath(
            _context.ObjectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Gallbladder,
                  GallbladderEjectionHalfTime)
               .WithAlias(GallbladderEjectionHalfTimeAlias)
               .WithDimension(time));
      }

      private static bool isOldGallbladderEmptyingTransport(ITransportBuilder transpotBuilder)
      {
         return transpotBuilder.Name.Equals(GallbladderEmptying) && ((ExplicitFormula) transpotBuilder.Formula).FormulaString.Equals(GallbladderEmptyingOldFormula);
      }

      private static bool isOldGallbladderEmptyingTransport(ITransport transport)
      {
         return transport.Name.Equals(GallbladderEmptying) && ((ExplicitFormula) transport.Formula).FormulaString.Equals(GallbladderEmptyingOldFormula);
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         simulation.BuildConfiguration.AllBuildingBlocks.Each(this.Visit);

         var root = simulation.Model.Root;
         var organism = root.GetSingleChildByName<IContainer>(Constants.ORGANISM);
         if (organism == null) return;
         //Add new parameter gall bladder emptying active
         var parameter = addGallBladderEmptyingActiveParameter(organism);
         if (parameter == null) return;

         // Genearate EHC_Event assingment path in simulaton
         var gallbladderEmptyingRatePathSimulation = GallbladderEmptyingRatePath.Clone<IObjectPath>();
         gallbladderEmptyingRatePathSimulation.AddAtFront(root.Name);

         // change  ehc events
         var startEvents = root.GetAllChildren<IEvent>(eb => eb.Name.Equals(EHCStartEvent));
         startEvents.Each(se => changeAssignments(se, false, 1, gallbladderEmptyingRatePathSimulation));
         startEvents.Each(se => (se.Assignments).Each(a => ((EventAssignment) a).ChangedEntity = null));

         var stopEvents = root.GetAllChildren<IEvent>(eb => eb.Name.Equals(EHCStopEvent));
         stopEvents.Each(se => changeAssignments(se, true, 0, gallbladderEmptyingRatePathSimulation));
         stopEvents.Each(se => se.Assignments.Each(a => ((EventAssignment) a).ChangedEntity = null));

         // Update gall bladder emptying kinetics
         var tranports = simulation.Model.Neighborhoods.GetAllChildren<ITransport>(isOldGallbladderEmptyingTransport);
         tranports.Each(t => updateGallBladderEmptyingInSimulation(t, root.Name));

         updateTotalDrugMassHandling(simulation, root);

         updateAllParameters(root.GetAllChildren<IParameter>());
         _formulaTask.ExpandDynamicFormulaIn(simulation.Model);
         _converted = true;
      }

      private void updateTotalDrugMassHandling(IModelCoreSimulation simulation, IContainer root)
      {
         var amountDimension = _context.DimensionFactory.Dimension(Constants.Dimension.AMOUNT);
         updateTotalDrugMass(root.GetSingleChildByName<IContainer>(Applications));
         var oldDrugMassPath = new ObjectPath(new[] {simulation.Model.Root.Name, Applications, TotalDrugmassOld});
         var allObserver = root.GetAllChildren<IObserver>().ToList();
         updateTotalDrugMassReferences(allObserver, oldDrugMassPath);
         replaceMoleculeKeyword(allObserver);
         var allMoleculeNames = simulation.BuildConfiguration.Molecules.Select(mb => mb.Name).ToArray();
         var allMoleculeContainer = root.GetChildren<IContainer>().Where(c => allMoleculeNames.Contains(c.Name));
         allMoleculeContainer.Each(mc => mc.Add(createTotalDrugMassParameter(createTotalDrugMassFormula(amountDimension), amountDimension)));
      }

      private void replaceMoleculeKeyword(IEnumerable<IObserver> allObserver)
      {
         foreach (var observer in allObserver)
         {
            observer.Formula.ObjectPaths.Each(op => op.Replace(ObjectPathKeywords.MOLECULE, observer.ParentContainer.Name));
         }
      }

      private void updateGallBladderEmptyingInSimulation(ITransport transport, string rootName)
      {
         var kinetic = (ExplicitFormula) transport.Formula;
         var moleculeName = transport.ParentContainer.Name;
         updateEHCKinetic(kinetic, moleculeName);
         //Add Root Name to all Paths
         kinetic.ObjectPaths.Each(ob => ob.AddAtFront(rootName));
      }

      public void Visit(ISpatialStructure spatialStructure)
      {
         var organism = spatialStructure.TopContainers.FirstOrDefault(tc => tc.Name.Equals(Constants.ORGANISM));
         if (organism == null) return;
         var param = addGallBladderEmptyingActiveParameter(organism);
         if (param != null)
            spatialStructure.FormulaCache.Add(param.Formula);


         updateAllParameters(spatialStructure.SelectMany(c => c.GetAllChildren<IParameter>()));
         _converted = true;
      }

      private IParameter addGallBladderEmptyingActiveParameter(IContainer organism)
      {
         var gallbladder = organism.GetSingleChildByName<IContainer>(Gallbladder);
         if (gallbladder == null) return null;
         var noDimension = _context.DimensionFactory.NoDimension;

         var formula =_context.Create<ExplicitFormula>()
               .WithName($"PARAM_{GallbladderEmptyingActive}")
               .WithFormulaString("0")
               .WithDimension(noDimension);

         var parameter =_context.Create<IParameter>()
               .WithName(GallbladderEmptyingActive)
               .WithFormula(formula)
               .WithDimension(noDimension);
         gallbladder.Add(parameter);
         gallbladder.RemoveChild(gallbladder.GetSingleChildByName<IParameter>(GallbladderEmptyingRate));
         return parameter;
      }

      public void Visit(IObserverBuildingBlock objToVisit)
      {
         var oldDrugMassPath = new ObjectPath(new[] {Applications, TotalDrugmassOld});
         updateTotalDrugMassReferences(objToVisit, oldDrugMassPath);
         _converted = true;
      }

      private void updateTotalDrugMassReferences(IEnumerable<IUsingFormula> objToVisit, IObjectPath oldDrugMassPath)
      {
         foreach (var observerBuilder in objToVisit)
         {
            var pathsToChange =
               observerBuilder.Formula.ObjectPaths.Where(op => op.PathAsString.Equals(oldDrugMassPath.PathAsString))
                  .ToList();
            pathsToChange.Each(p => p.Replace(Applications, ObjectPathKeywords.MOLECULE));
            pathsToChange.Each(p => p.Replace(TotalDrugmassOld, TotalDrugmassNew));
         }
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var amountDimension = _context.DimensionFactory.Dimension(Constants.Dimension.AMOUNT);
         var formula = createTotalDrugMassFormula(amountDimension);
         foreach (var moleculeBuilder in moleculeBuildingBlock)
         {
            moleculeBuilder.Add(
               createTotalDrugMassParameter(formula, amountDimension));
         }
         updateAllParameters(moleculeBuildingBlock.SelectMany(mb => mb.GetAllChildren<IParameter>()));
         moleculeBuildingBlock.AddFormula(formula);
         _converted = true;
      }

      private SumFormula createTotalDrugMassFormula(IDimension amountDimension)
      {
         var formula =
            _context.Create<SumFormula>()
               .WithName("PARAM_TotalDrugMass")
               .WithDimension(amountDimension);
         formula.Criteria.Add(new MatchTagCondition(ObjectPathKeywords.MOLECULE));
         formula.Criteria.Add(new MatchTagCondition(DrugMass));
         return formula;
      }

      private IParameter createTotalDrugMassParameter(SumFormula formula, IDimension amountDimension)
      {
         return _context.Create<IParameter>()
            .WithName(TotalDrugmassNew)
            .WithMode(ParameterBuildMode.Property)
            .WithFormula(formula)
            .WithDimension(amountDimension)
            .WithGroup(Constants.Groups.MOBI);
      }

      private void updateAllParameters(IEnumerable<IParameter> allParameter)
      {
         allParameter.Each(p => p.GroupName = Constants.Groups.MOBI);
      }

      public void Visit(IReactionBuildingBlock reactionBuildingBlock)
      {
         updateAllParameters(reactionBuildingBlock.SelectMany(c => c.GetAllChildren<IParameter>()));
         _converted = true;
      }

      public void Visit(SimulationTransfer objToVisit)
      {
         Visit(objToVisit.Simulation);
         _converted = true;
      }
   }
}