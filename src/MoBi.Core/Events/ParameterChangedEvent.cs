using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class ParameterChangedEvent
   {
      public ParameterChangedEvent(IParameter parameter)
      {
         Parameter = parameter;
      }

      public IParameter Parameter { get; private set; }
   }
}