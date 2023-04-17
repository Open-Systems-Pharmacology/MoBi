using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Serialization.Converter.v10
{
   public class Converter90To100 : IMoBiObjectConverter,
      IVisitor<ParameterStartValuesBuildingBlock>,
      IVisitor<SimulationTransfer>,
      IVisitor<IModelCoreSimulation>

   {
      private readonly OSPSuite.Core.Converters.v10.Converter90To100 _coreConverter;
      private bool _converted;

      public Converter90To100(OSPSuite.Core.Converters.v10.Converter90To100 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V9_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
      {
         _converted = false;
         var (_, coreConversionHappened) = _coreConverter.Convert(objectToUpdate);
         this.Visit(objectToUpdate);
         return (ProjectVersions.V10_0, _converted || coreConversionHappened);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
      {
         return _coreConverter.ConvertXml(element);
      }

      public void Visit(ParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         //we need to update the formula of some predefined expressions parameters from PK-Sim v9 to ensure that a simulation can still be built
         parameterStartValuesBuildingBlock?.FormulaCache.Each(convertFormula);
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         simulation?.Configuration?.All<ParameterStartValuesBuildingBlock>().Each(Visit);
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer?.Simulation);
      }

      private void convertFormula(IFormula formula)
      {
         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null)
            return;

         if (!explicitFormula.NameIsOneOf(
                "RelExpInterstialForInterstitial",
                "RelExpInterstialIntraVascEndoIsInterstitial",
                "RelExpInterstialMembraneExtracellularBasolateral",
                "RelExpPlasmaMembraneExtracellularApicalTissueOrgan"))

            return;

         if (explicitFormula.IsNamed("RelExpInterstialForInterstitial"))
            explicitFormula.FormulaString = "V_int > 0 ? f_cell/ f_int * rel_exp_norm + V_vasend / V_int * rel_exp_vendo_norm : 0";
         else if (explicitFormula.IsNamed("RelExpInterstialIntraVascEndoIsInterstitial"))
            explicitFormula.FormulaString = "V_int > 0 ?  V_vasend/ V_int * rel_exp_vendo_norm : 0";
         else if (explicitFormula.IsNamed("RelExpInterstialMembraneExtracellularBasolateral"))
            explicitFormula.FormulaString = "V_int > 0 ? f_cell/ f_int * rel_exp_norm + V_vasend / V_int * rel_exp_vendo_norm : 0";
         else if (explicitFormula.IsNamed("RelExpPlasmaMembraneExtracellularApicalTissueOrgan"))
            explicitFormula.FormulaString = "V_pls > 0 ? rel_exp_norm + HCT / (1 - HCT) * rel_exp_bc_norm + V_vasend / V_pls * rel_exp_vendo_norm : 0";


         _converted = true;
      }
   }
}