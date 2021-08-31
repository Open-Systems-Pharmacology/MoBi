using System;
using System.Collections.Generic;
using System.Linq;
using libsbmlcs;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using Model = libsbmlcs.Model;
using Unit = OSPSuite.Core.Domain.UnitSystem.Unit;

namespace MoBi.Engine.Sbml
{
   internal class UnitConvertionInfo
   {
      public IDimension Dimension { get; set; }
      public Unit Unit { get; set; }
      public double Rate { get; set; }
   }

   public class UnitDefinitionImporter : SBMLImporter
   {
      private readonly IMoBiDimensionFactory _moBiDimensionFactory;
      private readonly IDictionary<int, Unit> _baseUnitsDictionary;
      private readonly IDictionary<string, UnitConvertionInfo> _unitConvertionDictionary;

      public UnitDefinitionImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, IMoBiDimensionFactory mobiDimensionFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _moBiDimensionFactory = mobiDimensionFactory;
         _baseUnitsDictionary = new Dictionary<int, Unit>();
         _unitConvertionDictionary = new Dictionary<string, UnitConvertionInfo>();
         InitBaseUnitsDictionary();
      }

      private void InitBaseUnitsDictionary()
      {
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_AMPERE, _moBiDimensionFactory.Dimension("Ampere").Unit("A"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_BECQUEREL, _moBiDimensionFactory.Dimension("Becquerel").Unit("Bq"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_CANDELA, _moBiDimensionFactory.Dimension("Candela").Unit("cd"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_COULOMB, _moBiDimensionFactory.Dimension("Coulomb").Unit("C"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_DIMENSIONLESS, _moBiDimensionFactory.NoDimension.DefaultUnit);
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_FARAD, _moBiDimensionFactory.Dimension("Becquerel").Unit("Bq"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_GRAM, _moBiDimensionFactory.Dimension("Mass").Unit("g"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_GRAY, _moBiDimensionFactory.Dimension("Gray").Unit("Gy"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_HENRY, _moBiDimensionFactory.Dimension("Henry").Unit("H"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_HERTZ, _moBiDimensionFactory.Dimension("Hertz").Unit("Hz"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_JOULE, _moBiDimensionFactory.Dimension("Joule").Unit("J"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_KATAL, _moBiDimensionFactory.Dimension("Katal").Unit("kat"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_KELVIN, _moBiDimensionFactory.Dimension("Kelvin").Unit("K"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_KILOGRAM, _moBiDimensionFactory.Dimension("Mass").Unit("kg"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_LITER, _moBiDimensionFactory.Dimension("Volume").Unit("l"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_LITRE, _moBiDimensionFactory.Dimension("Volume").Unit("l"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_LUMEN, _moBiDimensionFactory.Dimension("Lumen").Unit("lm"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_LUX, _moBiDimensionFactory.Dimension("Lux").Unit("lx"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_METER, _moBiDimensionFactory.Dimension("Length").Unit("m"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_METRE, _moBiDimensionFactory.Dimension("Length").Unit("m"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_MOLE, _moBiDimensionFactory.Dimension("Amount").Unit("mol"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_NEWTON, _moBiDimensionFactory.Dimension("Newton").Unit("N"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_OHM, _moBiDimensionFactory.Dimension("Ohm").Unit("Ohm"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_PASCAL, _moBiDimensionFactory.Dimension("Pressure").Unit("Pa"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_RADIAN, _moBiDimensionFactory.Dimension("Radian").Unit("rad"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_SECOND, _moBiDimensionFactory.Dimension("Time").Unit("s"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_SECOND * -1, _moBiDimensionFactory.Dimension("Inversed time").Unit("1/s"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_SIEMENS, _moBiDimensionFactory.Dimension("Siemens").Unit("S"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_SIEVERT, _moBiDimensionFactory.Dimension("Sievert").Unit("Sv"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_STERADIAN, _moBiDimensionFactory.Dimension("Steradian").Unit("sr"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_TESLA, _moBiDimensionFactory.Dimension("Tesla").Unit("T"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_VOLT, _moBiDimensionFactory.Dimension("Volt").Unit("V"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_WATT, _moBiDimensionFactory.Dimension("Watt").Unit("W"));
         _baseUnitsDictionary.Add(libsbml.UNIT_KIND_WEBER, _moBiDimensionFactory.Dimension("Weber").Unit("Wb"));
      }

      protected override void Import(Model model)
      {
         if (model == null) return;
         if (_sbmlProject == null) return;

         for (long i = 0; i < model.getNumUnitDefinitions(); i++)
         {
            ConvertUnit(model.getUnitDefinition(i));
         }
      }

      /// <summary>
      ///     Converts a SBML UnitDefinition into a MoBi Unit.
      /// </summary>
      public IDimension ConvertUnit(UnitDefinition unitDefinition)
      {
         var sbmlUnit = unitDefinition.getId();
         var dimension = _moBiDimensionFactory.TryGetDimensionCaseInsensitive(sbmlUnit);

         if (dimension != Constants.Dimension.NO_DIMENSION)
         {
            _sbmlInformation.MobiDimension[sbmlUnit] = dimension;
            return dimension;
         }

         for (long i = 0; i < unitDefinition.getNumUnits(); i++)
         {
            var sbmlUnitDefinition = unitDefinition.getUnit(i);
            var mobiUnit = SBMLBaseUnitToMoBi(sbmlUnitDefinition);
            if (mobiUnit == null) continue;
            var unitDimension = _moBiDimensionFactory.DimensionForUnit(mobiUnit.Name);
            if (unitDimension != Constants.Dimension.NO_DIMENSION)
            {
               _sbmlInformation.MobiDimension[sbmlUnit] = unitDimension;
               _unitConvertionDictionary.Add(sbmlUnit, new UnitConvertionInfo() { Dimension = unitDimension, Unit = mobiUnit, Rate = Math.Pow(sbmlUnitDefinition.getMultiplier(), sbmlUnitDefinition.getExponent()) });
               return unitDimension;
            }
         }
         return dimension;
      }

      public Unit SBMLBaseUnitToMoBi(libsbmlcs.Unit sbmlBaseUnit)
      {
         if (sbmlBaseUnit == null) return null;
         var kind = (sbmlBaseUnit.getExponent() != 0 ? sbmlBaseUnit.getExponent() : 1) * sbmlBaseUnit.getKind();
         return _baseUnitsDictionary.ContainsKey(kind) ? _baseUnitsDictionary[kind] : null;
      }

      public override void AddToProject() { }

      public IDimension DimensionFor(string sbmlUnit)
      {
         return _unitConvertionDictionary[sbmlUnit].Dimension;
      }

      public double[] ToMobiBaseUnit(string unit, IEnumerable<double> value)
      {
         var convertionData = _unitConvertionDictionary[unit];
         return convertionData.Dimension.UnitValuesToBaseUnitValues(convertionData.Unit, value.Select(v => v * convertionData.Rate));
      }
   }

}