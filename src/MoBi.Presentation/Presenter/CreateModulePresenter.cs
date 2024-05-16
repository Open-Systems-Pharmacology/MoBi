using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
  
   public interface ICreateModulePresenter : IAddContentToModulePresenter
   {
      Module CreateModule();
   }

   public class CreateModulePresenter : AddContentToModulePresenter<ICreateModuleView, ICreateModulePresenter>, ICreateModulePresenter
   {
      private readonly IMoBiContext _context;
      private readonly ICreateModuleDTOToModuleMapper _createModuleDTOToModuleMapper;
      private Module _module;
      private ModuleContentDTO _createModuleDTO;

      public CreateModulePresenter(ICreateModuleView view, IMoBiContext context, ICreateModuleDTOToModuleMapper createModuleDTOToModuleMapper) : base(view)
      {
         _context = context;
         _createModuleDTOToModuleMapper = createModuleDTOToModuleMapper;
      }

      public Module CreateModule()
      {
         _module = _context.Create<Module>();
         _view.Caption = AppConstants.Captions.NewWindow(_context.TypeFor(_module));

         _createModuleDTO = new ModuleContentDTO();
         _createModuleDTO.AddUsedNames(_context.CurrentProject.Modules.AllNames());
         _view.BindTo(_createModuleDTO);
         _view.Display();
         
         return _view.Canceled ? null : _createModuleDTOToModuleMapper.MapFrom(_createModuleDTO);
      }

      protected override Module Module => _module;
      protected override ModuleContentDTO ContentDTO => _createModuleDTO;
   }
}