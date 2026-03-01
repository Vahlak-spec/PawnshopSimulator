
namespace PawnshopSimulator.Services
{
    public abstract class ExclusiveServiceBase : IGameService
    {
        protected ExclusiveServicesGroup _exclusiveServicesController;

        public void SetController(ExclusiveServicesGroup exclusiveServicesController)
        {
            _exclusiveServicesController = exclusiveServicesController;
        }
        public abstract void OnLaunchGame();
        public abstract void EndServiceProcess();
        public abstract void Bind(ServicesProvider componentProvider);
    }
}
