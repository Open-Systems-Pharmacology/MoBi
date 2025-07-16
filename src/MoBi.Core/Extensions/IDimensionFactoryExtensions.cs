using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Extensions;

public static class IDimensionFactoryExtensions
{
   public static void SetupDimensionMerging(this IDimensionFactory factory)
   {
      var concentrationDimension = factory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
      var molarConcentrationDimension = factory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);

      factory.AddMergingInformation(new MoBiDimensionMergingInformation<IQuantity>(concentrationDimension, molarConcentrationDimension,
         new MolWeightDimensionConverterForFormulaUsable(concentrationDimension, molarConcentrationDimension)));

      factory.AddMergingInformation(new MoBiDimensionMergingInformation<DataColumn>(concentrationDimension, molarConcentrationDimension,
         new ConcentrationToMolarConcentrationConverterForDataColumn(concentrationDimension, molarConcentrationDimension)));

      factory.AddMergingInformation(new MoBiDimensionMergingInformation<DataColumn>(molarConcentrationDimension, concentrationDimension,
         new MolarConcentrationToConcentrationConverterForDataColumn(molarConcentrationDimension, concentrationDimension)));
   }
}