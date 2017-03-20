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

      public IMoBiProject Current
      {
         get { return _moBiContext.CurrentProject; }
      }

      public void AddToHistory(ICommand command)
      {
         _moBiContext.AddToHistory(command); 
      }

      public IProject CurrentProject
      {
         get { return Current; }
      }

      public string ProjectName
      {
         get { return Current.Name; }
      }

      public string ProjectFullPath
      {
         get { return Current.FilePath; }
      }
   }
}