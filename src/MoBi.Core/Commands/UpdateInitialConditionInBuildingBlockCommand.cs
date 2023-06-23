using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateInitialConditionInBuildingBlockCommand : BuildingBlockChangeCommandBase<ILookupBuildingBlock<InitialCondition>>
   {
      private readonly ObjectPath _path;
      private readonly double? _value;
      private readonly double? _originalValue;
      private readonly bool _present;
      private readonly bool _originalPresent;
      private readonly double _scaleDivisor;
      private readonly bool _negativeValuesAllowed;
      private readonly double _originalScaleDivisor;
      private readonly bool _originalNegativeValuesAllowed;

      public UpdateInitialConditionInBuildingBlockCommand(
         ILookupBuildingBlock<InitialCondition> initialConditionsBuildingBlock,
         ObjectPath path,
         double? value, bool present, double scaleDivisor, bool negativeValuesAllowed) : base(initialConditionsBuildingBlock)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.InitialCondition;
         _path = path;
         _value = value;
         _present = present;
         _scaleDivisor = scaleDivisor;
         _negativeValuesAllowed = negativeValuesAllowed;

         var initialCondition = _buildingBlock.ByPath(_path);
         if (initialCondition == null)
            return;

         _originalValue = initialCondition.Value;
         _originalPresent = initialCondition.IsPresent;
         _originalScaleDivisor = initialCondition.ScaleDivisor;
         _originalNegativeValuesAllowed = initialCondition.NegativeValuesAllowed;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var initialCondition = _buildingBlock.ByPath(_path);
         if (initialCondition == null)
            return;

         initialCondition.Value = _value;
         initialCondition.IsPresent = _present;
         initialCondition.ScaleDivisor = _scaleDivisor;
         initialCondition.NegativeValuesAllowed = _negativeValuesAllowed;
         Description = AppConstants.Commands.UpdateInitialCondition(_path, _value, _present, initialCondition.DisplayUnit, _scaleDivisor, _negativeValuesAllowed);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateInitialConditionInBuildingBlockCommand(_buildingBlock, _path, _originalValue, _originalPresent, _originalScaleDivisor, _originalNegativeValuesAllowed).AsInverseFor(this);
      }
   }
}