using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
  
   public interface ICreateModulePresenter : IDisposablePresenter
   {
      Module CreateModule();
   }

   public class CreateModulePresenter : AbstractDisposablePresenter<ICreateModuleView, ICreateModulePresenter>, ICreateModulePresenter
   {
      private readonly IMoBiContext _context;
      private readonly ICreateModuleDTOToModuleMapper _createModuleDTOToModuleMapper;

      public CreateModulePresenter(ICreateModuleView view, IMoBiContext context, ICreateModuleDTOToModuleMapper createModuleDTOToModuleMapper) : base(view)
      {
         _context = context;
         _createModuleDTOToModuleMapper = createModuleDTOToModuleMapper;
      }

      public Module CreateModule()
      {
         var module = _context.Create<Module>();
         _view.Caption = AppConstants.Captions.NewWindow(_context.TypeFor(module));

         var createModuleDTO = new CreateModuleDTO();
         createModuleDTO.AddUsedNames(_context.CurrentProject.Modules.AllNames());
         _view.BindTo(createModuleDTO);
         _view.Display();
         
         return _view.Canceled ? null : _createModuleDTOToModuleMapper.MapFrom(createModuleDTO);
      }
   }
}