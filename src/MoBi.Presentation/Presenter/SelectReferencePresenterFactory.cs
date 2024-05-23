using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferencePresenterFactory
   {
      ISelectReferenceAtParameterPresenter ReferenceAtParameterFor(IContainer container);
      ISelectReferenceAtParameterPresenter ReferenceAtParameterFor(Type parentType);
   }

   public class SelectReferencePresenterFactory : ISelectReferencePresenterFactory
   {
      private readonly IMoBiApplicationController _applicationController;

      public SelectReferencePresenterFactory(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public ISelectReferenceAtParameterPresenter ReferenceAtParameterFor(IContainer container)
      {
         return ReferenceAtParameterFor(container?.GetType());
      }

      public ISelectReferenceAtParameterPresenter ReferenceAtParameterFor(Type parentType)
      {
         if (parentType.IsAnImplementationOf<ReactionBuilder>())
            return _applicationController.Start<ISelectReferenceAtReactionParameterPresenter>();

         if (parentType.IsAnImplementationOf<MoleculeBuilder>())
            return _applicationController.Start<ISelectReferenceAtMoleculeParameterPresenter>();

         return _applicationController.Start<ISelectReferenceAtParameterPresenter>();
      }
   }
}