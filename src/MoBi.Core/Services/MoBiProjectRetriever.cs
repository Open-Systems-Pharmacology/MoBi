using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IMoBiProjectRetriever : IProjectRetriever
   {
      IMoBiProject Current { get; }
   }

   public class MoBiProjectRetriever : IMoBiProjectRetriever
   {
      private readonly IMoBiContext _moBiContext;

      public MoBiProjectRetriever(IMoBiContext moBiContext)
      {
         _moBiContext = moBiContext;
      }

      public IMoBiProject Current => _moBiContext.CurrentProject;

      public void AddToHistory(ICommand command)
      {
         _moBiContext.AddToHistory(command); 
      }

      public IProject CurrentProject => Current;

      public string ProjectName => Current.Name;

      public string ProjectFullPath => Current.FilePath;
   }
}