using System.Linq;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface INewFormulaPresenter : IPresenter<INewFormulaView>, IDisposablePresenter, ICommandCollectorPresenter
   {
      bool Edit(IFormula formula, IBuildingBlock buildingBlock, IParameter parameter);
   }

   internal class NewFormulaPresenter : AbstractDisposableCommandCollectorPresenter<INewFormulaView, INewFormulaPresenter>, INewFormulaPresenter
   {
      private readonly IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private readonly ISelectReferenceAtParameterPresenter _referencePresenter;

      public NewFormulaPresenter(INewFormulaView view, IObjectBaseToObjectBaseDTOMapper objectBaseMapper, IFormulaPresenterCache formulaPresenterCache,
         ISelectReferenceAtParameterPresenter referencePresenter) : base(view)
      {
         _objectBaseMapper = objectBaseMapper;
         _formulaPresenterCache = formulaPresenterCache;
         _referencePresenter = referencePresenter;
         _view.AddReferenceView(_referencePresenter.View);
         AddSubPresenters(_referencePresenter);
      }

      public bool Edit(IFormula formula, IBuildingBlock buildingBlock, IParameter parameter)
      {
         var editSubPresenter = _formulaPresenterCache.PresenterFor(formula);
         editSubPresenter.BuildingBlock = buildingBlock;
         _subPresenterManager.Add(editSubPresenter);
         _subPresenterManager.InitializeWith(this);
         editSubPresenter.Edit(formula);
         _view.AddFormulaView(editSubPresenter.BaseView);
         var dto = _objectBaseMapper.MapFrom(formula);
         var formulaCache = buildingBlock.FormulaCache;
         dto.AddUsedNames(formulaCache.Select(f => f.Name));
         _view.BindTo(dto);
         _referencePresenter.Init(parameter, Enumerable.Empty<IObjectBase>().ToList(), parameter);
         _view.Display();
         formula.Name = dto.Name;
         return !_view.Canceled;
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
      }
   }
}