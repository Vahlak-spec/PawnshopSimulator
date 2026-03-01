using System;


namespace PawnshopSimulator.Characters
{
    public abstract class InteractibleModuleBase : CharacterModuleBase
    {
        public Action onInteractObjectExit;
        public Action onInteractObjectEnter;
        public abstract bool HasInteractObject();
        public abstract void TryStartInteract();
        public abstract void TryStopInteract();
        public abstract void OpenMenu();
        public abstract void CloseMenu();
        public abstract void TryDestroy();
    }
}
