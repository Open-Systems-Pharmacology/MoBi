using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.MenusAndBars
{
   public static class MenuBarItemExtensions
   {
      public static IMenuBarButton WithRemoveCommand<TParent, TChild>(this IMenuBarButton menuBarItem, TParent parent, TChild child)
         where TParent : class
         where TChild : class
      {
         return menuBarItem.WithCommand(IoC.Resolve<RemoveCommandFor<TParent, TChild>>().For(parent, child));
      }
   }

   public static class EntityUICommandExpressions
   {
      public static TCommand For<TCommand, T>(this TCommand command, T objectBase) where TCommand : IObjectUICommand<T> where T : class
      {
         command.Subject = objectBase;
         return command;
      }
   }

   public static class RemoveCommandExpressions
   {
      public static TCommand For<TCommand, TParent, TChild>(this TCommand command, TParent parent, TChild child)
         where TCommand : RemoveCommandFor<TParent, TChild>
         where TParent : class
         where TChild : class
      {
         command.Parent = parent;
         command.Child = child;
         return command;
      }
   }
}