namespace PawnshopSimulator.Services
{
    public interface IGameService
    {
        public void Bind(ServicesProvider componentProvider);
        public void OnLaunchGame();
    }
}
