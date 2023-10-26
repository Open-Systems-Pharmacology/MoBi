using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public abstract class StartValueDTO<T> : PathAndValueEntityDTO<T> where T : PathAndValueEntity
   {
      private readonly IBuildingBlock<T> _buildingBlock;

      /// <summary>
      ///    Updates the name of the start value
      /// </summary>
      /// <param name="newName">The new name for the start value</param>
      public abstract void UpdateName(string newName);

      protected StartValueDTO(T startValueObject, IBuildingBlock<T> buildingBlock)
         : base(startValueObject)
      {
         _buildingBlock = buildingBlock;

         Rules.AddRange(AllRules.All());
      }

      protected override ObjectPath GetContainerPath()
      {
         return PathWithValueObject.ContainerPath;
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All()
         {
            yield return renameMustNotCauseCollision();
            yield return mustNotAlreadyContainStartValue(x => x.PathElement0);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement1);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement2);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement3);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement4);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement5);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement6);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement7);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement8);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement9);


            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement1);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement2);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement3);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement4);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement5);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement6);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement7);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement8);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement9);
         }

         private static IBusinessRule renameMustNotCauseCollision()
         {
            return CreateRule.For<StartValueDTO<T>>()
               .Property(x => x.Name).WithRule(namedOnlyElementDoesNotExist)
               .WithError((dto, name) => AppConstants.Validation.NameIsAlreadyUsedInThisContainer(dto.ContainerPath.ToString(), name));
         }

         private static bool namedOnlyElementDoesNotExist(StartValueDTO<T> dto, string name)
         {
            return !dto._buildingBlock.Any(sv =>
               string.Equals(sv.Name, name) && sv.ContainerPath.Equals(dto.ContainerPath) && sv != dto.PathWithValueObject);
         }

         private static IBusinessRule mustHaveAllPreviousPathElementsSet(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var index = getPathIndex(propertyToCheck);
            return CreateRule.For<StartValueDTO<T>>()
               .Property(propertyToCheck).WithRule((dto, pathElement) => areAllPreviousPathElementsNonEmpty(dto, pathElement, index))
               .WithError((dto, pathElement) => AppConstants.Validation.CannotSetPathElementWhenPreviousElementsAreEmpty);
         }

         private static bool areAllPreviousPathElementsNonEmpty(StartValueDTO<T> dto, string pathElement, int index)
         {
            // to set a path element to empty, there is no requirement for the preceding elements
            if (string.IsNullOrEmpty(pathElement))
               return true;

            for (var i = index - 1; i >= 0; i--)
            {
               if (string.IsNullOrEmpty(dto.PathElementByIndex(i)))
                  return false;
            }

            return true;
         }

         private static IBusinessRule mustNotAlreadyContainStartValue(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var index = getPathIndex(propertyToCheck);
            return CreateRule.For<StartValueDTO<T>>()
               .Property(propertyToCheck)
               .WithRule((dto, pathElement) => noDuplicatesInBuildingBlockRule(dto, pathElement, index))
               .WithError((dto, pathElement) => AppConstants.Validation.PathIsIdenticalToExistingPath(newPathWithReplacement(dto, pathElement, index)));
         }

         private static int getPathIndex(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var propertyName = propertyToCheck.PropertyInfo().Name;
            var index = int.Parse(propertyName.Last().ToString(CultureInfo.InvariantCulture));
            return index;
         }

         private static bool noDuplicatesInBuildingBlockRule(StartValueDTO<T> dto, string pathElement, int index)
         {
            var containerPath = newPathWithReplacement(dto, pathElement, index);

            var proposedNewPath = containerPath.Clone<ObjectPath>().AndAdd(dto.Name);

            // if the path being validated is the same as the original path of the object then 
            // we can assume the path is valid as no changes will take place
            // This prevents us from having to evaluate path equals checks against all start values when
            // the validation is not caused by user edit
            if (Equals(proposedNewPath, dto.PathWithValueObject.Path))
               return true;

            if (dto._buildingBlock.Any(sv => sv.Path.Equals(proposedNewPath) && sv != dto.PathWithValueObject))
               return false;

            return true;
         }

         private static ObjectPath newPathWithReplacement(StartValueDTO<T> dto, string pathElement, int index)
         {
            var containerPath = dto.ContainerPath.Clone<ObjectPath>();

            // This means we don't have enough elements to do a replacement
            if (containerPath.Count > index)
               if (!pathElement.IsNullOrEmpty())
                  containerPath[index] = pathElement;
               else
                  containerPath.RemoveAt(index);

            // We are appending to the end of the list of elements
            else if (containerPath.Count == index && !string.IsNullOrEmpty(pathElement))
               containerPath.Add(pathElement);
            return containerPath;
         }
      }
   }
}