using System.Collections;
using System.Collections.Generic;
using MoBi.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain
{
   public class StringChanges : IEnumerable<IStringChange>
   {
      private readonly IList<IStringChange> _list = new List<IStringChange>();

      public IEnumerator<IStringChange> GetEnumerator()
      {
         return _list.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void Add<T>(T entitiyToEdit, IBuildingBlock containingBuildingBlock, IMoBiCommand changeCommand, string description)
      {
         _list.Add(new StringChange<T>
         {
            EntityToEdit = entitiyToEdit,
            ChangeCommand = changeCommand,
            ChangeDescription = description,
            BuildingBlock = containingBuildingBlock
         }
            );
      }

      public void Add<T>(T entitiyToEdit, IBuildingBlock buildingBlock, IMoBiCommand changeCommand)
      {
         Add(entitiyToEdit, buildingBlock, changeCommand, changeCommand.Description);
      }

      public void Clear()
      {
         _list.Clear();
      }
   }
}