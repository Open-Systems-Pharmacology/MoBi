using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterReferencesPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Shows all formulas (with the objects using them) in the model of <paramref name="simulation" /> where
      ///    <paramref name="target" /> is referenced
      /// </summary>
      void ShowReferencesTo(IFormulaUsable target, IMoBiSimulation simulation);

      /// <summary>
      ///    Returns <c>true</c> if the object referenced by <paramref name="reference" /> can itself be the target of a
      ///    reference search
      /// </summary>
      bool CanFindReferencesFor(ParameterReferenceDTO reference);

      /// <summary>
      ///    Drills into the references: shows all formulas referencing the object of <paramref name="reference" />
      /// </summary>
      void FindReferencesFor(ParameterReferenceDTO reference);

      /// <summary>
      ///    Closes the view and navigates to the object of <paramref name="reference" /> in the simulation
      /// </summary>
      void GoTo(ParameterReferenceDTO reference);

      /// <summary>
      ///    Shows the references to the previous target in the drill-in trail
      /// </summary>
      void GoBack();
   }

   public class ParameterReferencesPresenter : AbstractDisposablePresenter<IParameterReferencesView, IParameterReferencesPresenter>, IParameterReferencesPresenter
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IFormulaUsableToParameterReferenceDTOMapper _parameterReferenceMapper;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiContext _context;
      private IMoBiSimulation _simulation;
      private IEntity _entityToNavigateTo;
      private readonly Stack<IFormulaUsable> _targetTrail = new Stack<IFormulaUsable>();

      public ParameterReferencesPresenter(
         IParameterReferencesView view,
         IEntityPathResolver entityPathResolver,
         IFormulaUsableToParameterReferenceDTOMapper parameterReferenceMapper,
         IMoBiApplicationController applicationController,
         IMoBiContext context)
         : base(view)
      {
         _entityPathResolver = entityPathResolver;
         _parameterReferenceMapper = parameterReferenceMapper;
         _applicationController = applicationController;
         _context = context;
      }

      public void ShowReferencesTo(IFormulaUsable target, IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _targetTrail.Clear();
         _targetTrail.Push(target);
         showCurrentTarget();
         _view.Display();

         if (_entityToNavigateTo != null)
            _applicationController.Select(_entityToNavigateTo, _simulation, _context.HistoryManager);
      }

      public bool CanFindReferencesFor(ParameterReferenceDTO reference) => reference.UsedByObject is IFormulaUsable;

      public void FindReferencesFor(ParameterReferenceDTO reference)
      {
         if (!CanFindReferencesFor(reference))
            return;

         _targetTrail.Push(reference.UsedByObject.DowncastTo<IFormulaUsable>());
         showCurrentTarget();
      }

      public void GoBack()
      {
         if (_targetTrail.Count <= 1)
            return;

         _targetTrail.Pop();
         showCurrentTarget();
      }

      public void GoTo(ParameterReferenceDTO reference)
      {
         _entityToNavigateTo = reference.UsedByObject;
         _view.CloseView();
      }

      private void showCurrentTarget()
      {
         var target = _targetTrail.Peek();
         _view.Caption = AppConstants.Captions.ReferencesTo(_entityPathResolver.PathFor(target));
         _view.BindTo(_parameterReferenceMapper.MapFrom(target, _simulation));
         _view.UpdateNavigation(breadcrumb(), canGoBack: _targetTrail.Count > 1);
      }

      private string breadcrumb() => string.Join(" → ", _targetTrail.Reverse().Select(x => x.Name));
   }
}
