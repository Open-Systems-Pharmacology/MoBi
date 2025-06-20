using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Snapshots.Mappers;

public class TableFormulaMapper : OSPSuite.Core.Snapshots.Mappers.TableFormulaMapper
{
   private readonly IDimensionFactory _dimensionFactory;
   private readonly IObjectBaseFactory _objectBaseFactory;

   public TableFormulaMapper(IDimensionFactory dimensionFactory, IObjectBaseFactory objectBaseFactory)
   {
      _dimensionFactory = dimensionFactory;
      _objectBaseFactory = objectBaseFactory;
   }
   protected override TableFormula CreateNewTableFormula()
   {
      return _objectBaseFactory.Create<TableFormula>().WithName(AppConstants.TableFormula);
   }

   protected override IDimension DimensionByName(string dimensionName)
   {
      if (!_dimensionFactory.Has(dimensionName))
         return _dimensionFactory.NoDimension;

      return _dimensionFactory.Dimension(dimensionName);
   }
}