using System;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Repositories
{
   public interface IIconRepository
   {
      string IconFor<T>(T objectBase) where T : IObjectBase;
   }

   public class IconRepository : IIconRepository
   {
      public string IconFor<T>(T objectBase) where T : IObjectBase
      {
         if (!string.IsNullOrEmpty(objectBase.Icon))
            return objectBase.Icon;

         return iconFor(objectBase).IconName;
      }

      private ApplicationIcon iconFor<T>(T objectBase)
      {
         if (objectBase.IsAnImplementationOf<IMoleculeBuilder>())
            return getMoleculeBuilderIconFor(objectBase.DowncastTo<IMoleculeBuilder>());

         if (objectBase.IsAnImplementationOf<IContainer>())
            return getContainerIconFor(objectBase.DowncastTo<IContainer>());

         if (objectBase.IsAnImplementationOf<IObserverBuilder>())
            return ApplicationIcons.Observer;

         return getIconByTypeName(objectBase);
      }

      private ApplicationIcon getMoleculeBuilderIconFor(IMoleculeBuilder objectBase)
      {
         var iconName = Enum.GetName(typeof (QuantityType), objectBase.QuantityType);
         return iconByName(iconName) ?? ApplicationIcons.Drug;
      }

      private ApplicationIcon getIconByTypeName(object objectBase)
      {
         var typeName = objectBase.GetType().Name;

         if (ApplicationIcons.HasIconNamed(typeName))
            return ApplicationIcons.IconByName(typeName);

         if (typeName.StartsWith("I"))
            typeName = typeName.Substring(1);

         if (!ApplicationIcons.HasIconNamed(typeName))
            typeName = typeName.Remove(typeName.Length - "Builder".Length);
         
         return ApplicationIcons.IconByName(typeName);
      }

      private ApplicationIcon getContainerIconFor(IContainer container)
      {
         return iconByName(container.Name) ??
                iconByName(Enum.GetName(typeof (ContainerType), container.ContainerType)) ??
                getIconByTypeName(container);
      }

      private ApplicationIcon iconByName(string iconName)
      {
         return ApplicationIcons.HasIconNamed(iconName)
            ? ApplicationIcons.IconByName(iconName)
            : null;
      }
   }
}