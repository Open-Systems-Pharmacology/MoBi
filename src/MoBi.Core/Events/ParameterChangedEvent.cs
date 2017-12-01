using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class ParameterChangedEvent
   {
      public IReadOnlyList<IParameter> Parameters { get; }

      public ParameterChangedEvent(IParameter parameter) : this(new List<IParameter> { parameter })
      {
         ;
      }

      public ParameterChangedEvent(IReadOnlyList<IParameter> parameters)
      {
         Parameters = parameters;
      }

   }
}