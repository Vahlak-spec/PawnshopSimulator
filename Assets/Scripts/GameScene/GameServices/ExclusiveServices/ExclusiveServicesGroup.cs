using System.Collections.Generic;


namespace PawnshopSimulator.Services
{
    public class ExclusiveServicesGroup : IGameService
    {
        private List<ExclusiveServiceBase> _exclusiveServices = new List<ExclusiveServiceBase>();

        public void AddService(ExclusiveServiceBase exclusiveServiceBase) => _exclusiveServices.Add(exclusiveServiceBase);

        public void Bind(ServicesProvider componentProvider)
        {
            for (int i = 0; i < _exclusiveServices.Count; i++)
                _exclusiveServices[i].SetController(this);
        }
        public void OnLaunchGame() { }
        public void OnServiceStartWork(ExclusiveServiceBase service)
        {
            for(int i = 0; i < _exclusiveServices.Count; i++)
            {
                if (_exclusiveServices[i] != service)
                    _exclusiveServices[i].EndServiceProcess();
            }
        }
    }
}
