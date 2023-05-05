using System.Globalization;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IStartValueToObjectBaseSummaryDTOMapper<in T> : IMapper<T, ObjectBaseSummaryDTO> where T : IStartValue
   {
      
   }

   public abstract class AbstractStartValueToObjectBaseSummaryDTOMapper<T> : IStartValueToObjectBaseSummaryDTOMapper<T> where T : class, IStartValue
   {
      protected ObjectBaseSummaryDTO BaseMapper(T startValue)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            EntityName = startValue.Name,
         };

         dto.AddToDictionary(AppConstants.Captions.Type, new ObjectTypeResolver().TypeFor(startValue));
         dto.AddToDictionary(AppConstants.Captions.Path, startValue.Path.ToString());
         if (startValue.Value.HasValue) dto.AddToDictionary(AppConstants.Captions.Value, startValue.GetStartValueAsDisplayString());
         if (hasValidFormula(startValue)) dto.AddToDictionary(AppConstants.Captions.Formula, startValue.Formula.ToString());
         dto.AddToDictionary(AppConstants.Captions.Dimension, startValue.Dimension.DisplayName);
         dto.AddToDictionary(AppConstants.Captions.Description, startValue.Description);

         return dto;
      }

      private static bool hasValidFormula(IStartValue startValue)
      {
         return startValue.Formula != null && (startValue.Formula.IsExplicit()) || startValue.Formula.IsConstant();
      }

      public abstract ObjectBaseSummaryDTO MapFrom(T startValue);
   }

   public interface IMoleculeStartValueToObjectBaseSummaryDTOMapper : IStartValueToObjectBaseSummaryDTOMapper<InitialCondition>
   {
      
   }

   public class MoleculeStartValueToObjectBaseSummaryDTOMapper : AbstractStartValueToObjectBaseSummaryDTOMapper<InitialCondition>, IMoleculeStartValueToObjectBaseSummaryDTOMapper
   {
      public override ObjectBaseSummaryDTO MapFrom(InitialCondition startValue)
      {
         var dto = BaseMapper(startValue);
         dto.ApplicationIcon = ApplicationIcons.InitialConditions;
         dto.AddToDictionary(AppConstants.Captions.IsPresent, startValue.IsPresent.ToString());
         dto.AddToDictionary(AppConstants.Captions.MoleculeName, startValue.MoleculeName);
         dto.AddToDictionary(AppConstants.Captions.ScaleDivisor, startValue.ScaleDivisor.ToString(CultureInfo.InvariantCulture));
         return dto;
      }
   }

   public interface IParameterStartValueToObjectBaseSummaryDTOMapper : IStartValueToObjectBaseSummaryDTOMapper<ParameterStartValue>
   {
      
   }

   public class ParameterStartValueToObjectBaseSummaryDTOMapper : AbstractStartValueToObjectBaseSummaryDTOMapper<ParameterStartValue>, IParameterStartValueToObjectBaseSummaryDTOMapper
   {
      public override ObjectBaseSummaryDTO MapFrom(ParameterStartValue startValue)
      {
         var dto = BaseMapper(startValue);
         dto.ApplicationIcon = ApplicationIcons.ParameterValues;
         dto.AddToDictionary(AppConstants.Captions.ParameterName, startValue.ParameterName);
         return dto;
      }
   }
}