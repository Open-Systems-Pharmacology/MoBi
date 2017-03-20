using System;
using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public interface IDimensionToDTODimensionMapper : IMapper<IDimension, DimensionDTO>
   {
   }

   public class DimensionToDTODimensionMapper : IDimensionToDTODimensionMapper
   {
      private readonly IUnitToDTOUnitMapper _unitToDTOUnitMapper;

      public DimensionToDTODimensionMapper(IUnitToDTOUnitMapper unitToDtoUnitMapper)
      {
         _unitToDTOUnitMapper = unitToDtoUnitMapper;
      }

      public DimensionDTO MapFrom(IDimension input)
      {
         var dto = new DimensionDTO
            {
               Name = input.Name,
               BaseUnit = input.BaseUnit != null ? input.BaseUnit.Name : String.Empty,
               Units = new List<Unit>(input.Units.MapAllUsing(_unitToDTOUnitMapper))
            };

         if (input.BaseRepresentation != null)
         {
            dto.Amount = input.BaseRepresentation.AmountExponent;
            dto.ElectricCurrent = input.BaseRepresentation.ElectricCurrentExponent;
            dto.Length = input.BaseRepresentation.LengthExponent;
            dto.LuminousIntensity = input.BaseRepresentation.LuminousIntensityExponent;
            dto.Mass = input.BaseRepresentation.MassExponent;
            dto.Temperature = input.BaseRepresentation.TemperatureExponent;
            dto.Time = input.BaseRepresentation.TimeExponent;
         }

         return dto;
      }
   }
}