using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class ChangePathAndValueEntityNameCommand<TBuildingBlock, TPathAndValueEntity>
      : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, ILookupBuildingBlock<TPathAndValueEntity>
      where TPathAndValueEntity : PathAndValueEntity
   {
      protected string _newValue;
      protected string _oldValue;
      protected TPathAndValueEntity _originalEntity;
      protected IEnumerable<string> _path;

      protected ChangePathAndValueEntityNameCommand(TBuildingBlock buildingBlock, ObjectPath path, string newValue)
         : base(buildingBlock)
      {
         _newValue = newValue;
         _originalEntity = buildingBlock.ByPath(path);
         _oldValue = _originalEntity.Name;

         SetCommandParameters(newValue, _oldValue);
      }

      protected void SetCommandParameters(string newValue, string oldValue)
      {
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(_originalEntity);
         Description = AppConstants.Commands.SetDescription(ObjectType, AppConstants.Commands.Name, newValue, oldValue);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _originalEntity = default;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         _buildingBlock.Remove(_originalEntity);

         _originalEntity.Name = _newValue;

         _buildingBlock.Add(_originalEntity);
         if (_originalEntity.Formula != null)
            _buildingBlock.AddFormula(_originalEntity.Formula);

         _path = _originalEntity.Path.All();
      }
   }
}