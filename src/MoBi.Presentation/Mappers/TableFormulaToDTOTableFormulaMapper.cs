using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public interface ITableFormulaToDTOTableFormulaMapper : IMapper<TableFormula, TableFormulaBuilderDTO>
   {
   }

   internal class TableFormulaToDTOTableFormulaMapper : ObjectBaseToObjectBaseDTOMapperBase, ITableFormulaToDTOTableFormulaMapper
   {
      private readonly IValuePointToDTOValuePointMapper _valuePointToDTOValuePointMapperMapper;

      public TableFormulaToDTOTableFormulaMapper(IValuePointToDTOValuePointMapper valuePointToDTOValuePointMapperMapper)
      {
         _valuePointToDTOValuePointMapperMapper = valuePointToDTOValuePointMapperMapper;
      }

      public TableFormulaBuilderDTO MapFrom(TableFormula tableFormula)
      {
         var dto = Map(new TableFormulaBuilderDTO(tableFormula));
         dto.Dimension = tableFormula.Dimension;
         dto.UseDerivedValues = tableFormula.UseDerivedValues;
         dto.XDisplayName = Constants.NameWithUnitFor(tableFormula.XName, tableFormula.XDisplayUnit);
         dto.YDisplayName = Constants.NameWithUnitFor(tableFormula.YName, tableFormula.YDisplayUnit);
         _valuePointToDTOValuePointMapperMapper.Initialise(tableFormula.XDimension, tableFormula.Dimension, tableFormula.XDisplayUnit, tableFormula.YDisplayUnit, dto);
         dto.ValuePoints = tableFormula.AllPoints().MapAllUsing(_valuePointToDTOValuePointMapperMapper);
         return dto;
      }
   }

   internal interface IValuePointToDTOValuePointMapper : IMapper<ValuePoint, DTOValuePoint>
   {
      void Initialise(IDimension xDimension, IDimension yDimension, Unit xDisplayUnit, Unit yDisplayUnit, TableFormulaBuilderDTO tableFormulaDTO);
   }

   internal class ValuePointToDTOValuePointMapper : IValuePointToDTOValuePointMapper
   {
      private IDimension _xDimension;
      private IDimension _yDimension;
      private Unit _xDisplayUnit;
      private Unit _yDisplayUnit;
      private TableFormulaBuilderDTO _tableFormulaDTO;

      public void Initialise(IDimension xDimension, IDimension yDimension, Unit xDisplayUnit, Unit yDisplayUnit, TableFormulaBuilderDTO tableFormulaDTO)
      {
         _xDimension = xDimension;
         _yDimension = yDimension;
         _xDisplayUnit = xDisplayUnit;
         _yDisplayUnit = yDisplayUnit;
         _tableFormulaDTO = tableFormulaDTO;
      }

      public DTOValuePoint MapFrom(ValuePoint input)
      {
         var dto = new DTOValuePoint(_tableFormulaDTO)
         {
            X = new ValuePointParameterDTO {Value = input.X, Dimension = _xDimension, DisplayUnit = _xDisplayUnit},
            Y = new ValuePointParameterDTO {Value = input.Y, Dimension = _yDimension, DisplayUnit = _yDisplayUnit},
            RestartSolver = input.RestartSolver,
            ValuePoint = input
         };
         return dto;
      }
   }
}