using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditObjectBasePropertyInBuildingBlockCommand<T> : EditObjectPropertyInBuildingBlockCommand<T> where T : class, IBuildingBlock
   {
      protected readonly string _objectBaseId;
      protected IObjectBase _objectBase;

      public EditObjectBasePropertyInBuildingBlockCommand(string propertyName, object newValue, object oldValue, IObjectBase objectBase, T buildingBlock)
         : this(propertyName, newValue, oldValue, objectBase, buildingBlock, objectBase.Name)
      {
      }

      public EditObjectBasePropertyInBuildingBlockCommand(string propertyName, object newValue, object oldValue, IObjectBase objectBase, T buildingBlock, string objectName)
         : base(propertyName, newValue, oldValue, objectBase, buildingBlock, objectName)
      {
         _objectBaseId = objectBase.Id;
         _objectBase = objectBase;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditObjectBasePropertyInBuildingBlockCommand<T>(PropertyName, _oldValue, _newValue, _objectBase, _buildingBlock) {Visible = Visible}
            .AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _objectBase = null;
      }

      protected override object RestoreObjectToUpdate(IMoBiContext context)
      {
         _objectBase = context.Get<IObjectBase>(_objectBaseId);
         return _objectBase;
      }
   }

   public class EditObjectBasePropertyInBuildingBlockCommand : EditObjectBasePropertyInBuildingBlockCommand<IBuildingBlock>
   {
      public EditObjectBasePropertyInBuildingBlockCommand(string propertyName, object newValue, object oldValue, IObjectBase objectBase, IBuildingBlock buildingBlock) : base(propertyName, newValue, oldValue, objectBase, buildingBlock)
      {
      }
   }
}