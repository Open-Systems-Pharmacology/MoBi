using System.Collections;
using System.Collections.Generic;
using MoBi.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain
{
   public class StringChanges : IEnumerable<IStringChange>
   {
      private readonly IBuildingBlock _buildingBlock;
      private readonly IList<IStringChange> _list = new List<IStringChange>();

      public StringChanges(IBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
      }

      public IEnumerator<IStringChange> GetEnumerator()
      {
         return _list.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void Add<T>(T entityToEdit, IMoBiCommand changeCommand, string description)
      {
         _list.Add(new StringChange<T>
            {
               EntityToEdit = entityToEdit,
               ChangeCommand = changeCommand,
               ChangeDescription = description,
               BuildingBlock = _buildingBlock
            }
         );
      }

      public void Add<T>(T entityToEdit, IMoBiCommand changeCommand) => Add(entityToEdit, changeCommand, changeCommand.Description);

      public void Clear()
      {
         _list.Clear();
      }
   }
}