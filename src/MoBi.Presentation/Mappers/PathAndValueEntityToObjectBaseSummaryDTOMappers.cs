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
   public interface IPathAndValueEntityToObjectBaseSummaryDTOMapper<in T> : IMapper<T, ObjectBaseSummaryDTO> where T : PathAndValueEntity
   {
      
   }

   public abstract class AbstractPathAndValueEntityToObjectBaseSummaryDTOMapper<T> : IPathAndValueEntityToObjectBaseSummaryDTOMapper<T> where T : PathAndValueEntity
   {
      protected ObjectBaseSummaryDTO BaseMapper(T pathAndValueEntity)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            EntityName = pathAndValueEntity.Name,
         };

         dto.AddToDictionary(AppConstants.Captions.Type, new ObjectTypeResolver().TypeFor(pathAndValueEntity));
         dto.AddToDictionary(AppConstants.Captions.Path, pathAndValueEntity.Path.ToString());
         if (pathAndValueEntity.Value.HasValue) dto.AddToDictionary(AppConstants.Captions.Value, pathAndValueEntity.GetValueAsDisplayString());
         if (hasValidFormula(pathAndValueEntity)) dto.AddToDictionary(AppConstants.Captions.Formula, pathAndValueEntity.Formula.ToString());
         dto.AddToDictionary(AppConstants.Captions.Dimension, pathAndValueEntity.Dimension.DisplayName);
         dto.AddToDictionary(AppConstants.Captions.Description, pathAndValueEntity.Description);

         return dto;
      }

      private static bool hasValidFormula(PathAndValueEntity pathAndValueEntity)
      {
         return pathAndValueEntity.Formula != null && (pathAndValueEntity.Formula.IsExplicit()) || pathAndValueEntity.Formula.IsConstant();
      }

      public abstract ObjectBaseSummaryDTO MapFrom(T pathAndValueEntity);
   }

   public interface IInitialConditionToObjectBaseSummaryDTOMapper : IPathAndValueEntityToObjectBaseSummaryDTOMapper<InitialCondition>
   {
      
   }

   public class InitialConditionToObjectBaseSummaryDTOMapper : AbstractPathAndValueEntityToObjectBaseSummaryDTOMapper<InitialCondition>, IInitialConditionToObjectBaseSummaryDTOMapper
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

   public interface IParameterValueToObjectBaseSummaryDTOMapper : IPathAndValueEntityToObjectBaseSummaryDTOMapper<ParameterValue>
   {
      
   }

   public class ParameterValueToObjectBaseSummaryDTOMapper : AbstractPathAndValueEntityToObjectBaseSummaryDTOMapper<ParameterValue>, IParameterValueToObjectBaseSummaryDTOMapper
   {
      public override ObjectBaseSummaryDTO MapFrom(ParameterValue pathAndValueEntity)
      {
         var dto = BaseMapper(pathAndValueEntity);
         dto.ApplicationIcon = ApplicationIcons.ParameterValues;
         dto.AddToDictionary(AppConstants.Captions.ParameterName, pathAndValueEntity.ParameterName);
         return dto;
      }
   }
}