using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class ChangeStartValueNameCommand<TBuildingBlock, TStartValue> 
      : BuildingBlockChangeCommandBase<TBuildingBlock> 
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TStartValue>, IBuildingBlock<TStartValue>
      where TStartValue : PathAndValueEntity
   {
      protected string _newValue;
      protected string _oldValue;
      protected TStartValue _originalStartValue;
      protected IEnumerable<string> _path;

      protected ChangeStartValueNameCommand(TBuildingBlock buildingBlock, ObjectPath path, string newValue)
         : base(buildingBlock)
      {
         _newValue = newValue;
         _originalStartValue = buildingBlock[path];
         _oldValue = _originalStartValue.Name;

         SetCommandParameters(newValue, _oldValue);
      }

      protected void SetCommandParameters(string newValue, string oldValue)
      {
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor<TStartValue>();
         Description = AppConstants.Commands.SetDescription(ObjectType, AppConstants.Commands.Name, newValue, oldValue);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _originalStartValue = default(TStartValue);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         _buildingBlock.Remove(_originalStartValue);

         _originalStartValue.Name = _newValue;

         _buildingBlock.Add(_originalStartValue);
         if (_originalStartValue.Formula != null)
            _buildingBlock.AddFormula(_originalStartValue.Formula);

         _path = _originalStartValue.Path.All();
      }
   }
}