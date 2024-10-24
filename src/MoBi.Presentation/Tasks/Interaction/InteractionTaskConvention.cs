using System;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Tasks.Interaction
{
   /// <summary>
   ///    Registers components with the following conventions
   ///    Registers all interfaces that begin with "InteractionTasksFor" if
   ///    the interface contains a generic type.
   /// </summary>
   public class InteractionTaskConvention : IRegistrationConvention
   {
      public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         if (concreteType.IsEnum) return;
         if (concreteType.IsNested) return;

         var interactionInterfaces = concreteType.GetInterfaces()
            .Where(i => i.IsGenericType && i.Name.StartsWith("IInteractionTasksFor")).ToList();


         interactionInterfaces.Each(interfaceType => register(concreteType, container, lifeStyle, interfaceType));
      }

      private static void register(Type concreteType, IContainer container, LifeStyle lifeStyle, Type interfaceType)
      {
         container.Register(interfaceType, concreteType, lifeStyle);
      }
   }
}