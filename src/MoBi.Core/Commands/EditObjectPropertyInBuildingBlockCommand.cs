using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class EditObjectPropertyInBuildingBlockCommand<TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      protected object _newValue;
      protected object _oldValue;
      private object _objectToUpdate;

      public string PropertyName { get; set; }
      public string NewValueSerializationString { get; set; }
      public string OldValueSerializationString { get; set; }

      protected EditObjectPropertyInBuildingBlockCommand(string propertyName, object newValue,
         object oldValue, object objectToUpdate, TBuildingBlock buildingBlock, string objectName)
         : base(buildingBlock)
      {
         _newValue = newValue;
         _oldValue = oldValue;
         _objectToUpdate = objectToUpdate;
         PropertyName = propertyName;
         ObjectType = new ObjectTypeResolver().TypeFor(_objectToUpdate);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, PropertyName.SplitToUpperCase(), _oldValue?.ToString() ?? string.Empty, _newValue?.ToString() ?? string.Empty, objectName);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         UpdatePropertyValue();
         OldValueSerializationString = context.SerializeValue(_oldValue);
         NewValueSerializationString = context.SerializeValue(_newValue);
         context.ProjectChanged();
      }

      protected virtual void UpdatePropertyValue()
      {
         var propertyToUpdate = GetProperty();
         propertyToUpdate?.SetValue(_objectToUpdate, _newValue, null);
      }

      protected PropertyInfo GetProperty()
      {
         return _objectToUpdate.GetType().GetProperty(PropertyName);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _objectToUpdate = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _objectToUpdate = RestoreObjectToUpdate(context);
         var changeProperty = GetProperty();
         var propertyType = changeProperty.PropertyType;
         _oldValue = context.DeserializeValueTo(propertyType, OldValueSerializationString);
         _newValue = context.DeserializeValueTo(propertyType, NewValueSerializationString);
      }

      protected abstract object RestoreObjectToUpdate(IMoBiContext context);
   }
}