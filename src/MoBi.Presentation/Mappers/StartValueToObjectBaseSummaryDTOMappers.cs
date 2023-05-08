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
   public interface IStartValueToObjectBaseSummaryDTOMapper<in T> : IMapper<T, ObjectBaseSummaryDTO> where T : PathAndValueEntity
   {
      
   }

   public abstract class AbstractStartValueToObjectBaseSummaryDTOMapper<T> : IStartValueToObjectBaseSummaryDTOMapper<T> where T : PathAndValueEntity
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

      private static bool hasValidFormula(PathAndValueEntity startValue)
      {
         return startValue.Formula != null && (startValue.Formula.IsExplicit()) || startValue.Formula.IsConstant();
      }

      public abstract ObjectBaseSummaryDTO MapFrom(T startValue);
   }

   public interface IInitialConditionToObjectBaseSummaryDTOMapper : IStartValueToObjectBaseSummaryDTOMapper<InitialCondition>
   {
      
   }

   public class InitialConditionToObjectBaseSummaryDTOMapper : AbstractStartValueToObjectBaseSummaryDTOMapper<InitialCondition>, IInitialConditionToObjectBaseSummaryDTOMapper
   {
      public override ObjectBaseSummaryDTO MapFrom(InitialCondition initialCondition)
      {
         var dto = BaseMapper(initialCondition);
         dto.ApplicationIcon = ApplicationIcons.InitialConditions;
         dto.AddToDictionary(AppConstants.Captions.IsPresent, initialCondition.IsPresent.ToString());
         dto.AddToDictionary(AppConstants.Captions.MoleculeName, initialCondition.MoleculeName);
         dto.AddToDictionary(AppConstants.Captions.ScaleDivisor, initialCondition.ScaleDivisor.ToString(CultureInfo.InvariantCulture));
         return dto;
      }
   }

   public interface IParameterValueToObjectBaseSummaryDTOMapper : IStartValueToObjectBaseSummaryDTOMapper<ParameterValue>
   {
      
   }

   public class ParameterValueToObjectBaseSummaryDTOMapper : AbstractStartValueToObjectBaseSummaryDTOMapper<ParameterValue>, IParameterValueToObjectBaseSummaryDTOMapper
   {
      public override ObjectBaseSummaryDTO MapFrom(ParameterValue startValue)
      {
         var dto = BaseMapper(startValue);
         dto.ApplicationIcon = ApplicationIcons.ParameterValues;
         dto.AddToDictionary(AppConstants.Captions.ParameterName, startValue.ParameterName);
         return dto;
      }
   }
}