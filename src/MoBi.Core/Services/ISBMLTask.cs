using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Services
{
   public interface ISbmlTask
   {
      IMoBiCommand ImportModelFromSbml(string filename, MoBiProject project);
   }
}