using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Repository
{
   public class GroupRepository : IGroupRepository
   {
      private readonly Cache<string, IGroup> _allGroups;
      private readonly Cache<string, IGroup> _allGroupsById;
      private readonly Group _defaulfGroup;

      public GroupRepository()
      {
         _defaulfGroup = new Group {Name = Constants.Groups.MOBI};
         _allGroups = new Cache<string, IGroup>(x => x.Name, x => _defaulfGroup);
         _allGroupsById = new Cache<string, IGroup>(x => x.Id, x => _defaulfGroup);
      }

      public IEnumerable<IGroup> All()
      {
         return _allGroups;
      }

      public IGroup GroupByName(string groupName)
      {
         return _allGroups[groupName];
      }

      public IGroup GroupById(string groupId)
      {
         return _allGroupsById[groupId];
      }

      public void Clear()
      {
         _allGroups.Clear();
         _allGroupsById.Clear();
      }

      public void AddGroup(IGroup group)
      {
         _allGroups.Add(group);
         _allGroupsById.Add(group);
      }
   }
}