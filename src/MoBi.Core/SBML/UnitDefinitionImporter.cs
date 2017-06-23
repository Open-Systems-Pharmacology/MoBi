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

namespace MoBi.Core.SBML
{
    public class UnitDefinitionImporter : SBMLImporter
    {
        private readonly IMoBiDimensionFactory _moBiDimensionFactory;
        private readonly IDictionary<int,Unit> _baseUnitsDictionary;

        public UnitDefinitionImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, IMoBiDimensionFactory mobiDimensionFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
        {
            _moBiDimensionFactory = mobiDimensionFactory;
            _baseUnitsDictionary = new Dictionary<int, Unit>();
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
            _baseUnitsDictionary.Add(libsbml.UNIT_KIND_GRAM,_moBiDimensionFactory.Dimension("Mass").Unit("g"));
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

            var newFactor = 1.0;
            var unitsList = new List<Unit>();
            var brList = new List<BaseDimensionRepresentation>();
            for (long i = 0; i < unitDefinition.getNumUnits(); i++)
            {
                var mobiUnit = SBMLBaseUnitToMoBi(unitDefinition.getUnit(i));
                if (mobiUnit == null) continue;
                var unitDimension = _moBiDimensionFactory.DimensionForUnit(mobiUnit.Name);
                var unitBr = unitDimension.BaseRepresentation;
                unitBr = SetExponents(unitDefinition, unitBr, i);
                brList.Add(unitBr);
                    
                if (Math.Abs(newFactor) < 0.0001)
                {
                    newFactor =  unitDefinition.getUnit(i).getMultiplier() * Math.Pow(10, unitDefinition.getUnit(i).getScale()) * Math.Pow(mobiUnit.Factor, unitDefinition.getUnit(i).getExponent());
                }
                else
                {
                    newFactor = newFactor * unitDefinition.getUnit(i).getMultiplier() * Math.Pow(10, unitDefinition.getUnit(i).getScale()) * Math.Pow(mobiUnit.Factor, unitDefinition.getUnit(i).getExponent());
                }
                unitsList.Add(mobiUnit);
            }

            var baseDimensionRepresentation = CreateBaseDimRepresentationExponents(brList);

            IDimension newDim = new Dimension(baseDimensionRepresentation, String.Format("SBML_{0}",unitDefinition.getId()), String.Format("SBML_{0}_BaseUnit",unitDefinition.getId()));

            var newBaseUnit = newDim.BaseUnit;

            newBaseUnit.Visible = false;
            var newUnit = new Unit(unitDefinition.getId(), newFactor, 0);
            newDim.AddUnit(newUnit);
            newDim.DefaultUnit = newUnit;

            _sbmlInformation.MobiDimension[sbmlUnit] = newDim;
            if (_moBiDimensionFactory.Dimensions.All(dim => dim.Name != newDim.Name))
                _moBiDimensionFactory.AddDimension(newDim);
            return newDim;
        }

        /// <summary>
        ///     Sets the exponents of a BaseDimensionRepresentation.
        /// </summary>
        private BaseDimensionRepresentation SetExponents(UnitDefinition unitDefinition, BaseDimensionRepresentation unitBr, long i)
        {
            unitBr.AmountExponent = unitBr.AmountExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.ElectricCurrentExponent = unitBr.ElectricCurrentExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.LuminousIntensityExponent = unitBr.LuminousIntensityExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.MassExponent = unitBr.MassExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.LengthExponent = unitBr.LengthExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.TimeExponent = unitBr.TimeExponent*unitDefinition.getUnit(i).getExponent();
            unitBr.TemperatureExponent = unitBr.TemperatureExponent*unitDefinition.getUnit(i).getExponent();
            return unitBr;
        }

        private BaseDimensionRepresentation CreateBaseDimRepresentationExponents(IEnumerable<BaseDimensionRepresentation> brList)
        {
            var baseDimensionRepresentation = new BaseDimensionRepresentation();
            foreach (var br in brList)
            {
                baseDimensionRepresentation.AmountExponent += br.AmountExponent;
                baseDimensionRepresentation.ElectricCurrentExponent += br.ElectricCurrentExponent;
                baseDimensionRepresentation.LengthExponent += br.LengthExponent;
                baseDimensionRepresentation.LuminousIntensityExponent += br.LuminousIntensityExponent;
                baseDimensionRepresentation.MassExponent += br.MassExponent;
                baseDimensionRepresentation.TemperatureExponent += br.TemperatureExponent;
                baseDimensionRepresentation.TimeExponent += br.TimeExponent;
            }
            return baseDimensionRepresentation;
        }

        public Unit SBMLBaseUnitToMoBi(libsbmlcs.Unit sbmlBaseUnit)
        {
            if (sbmlBaseUnit == null) return null;
            return _baseUnitsDictionary.ContainsKey(sbmlBaseUnit.getKind()) ? _baseUnitsDictionary[sbmlBaseUnit.getKind()] : null;
        }

        public override void AddToProject(){}
    }

}