using UnityEngine;

namespace PawnshopSimulator.Characters
{
    public abstract class CharacterModuleBase : MonoBehaviour
    {
        public abstract void Launch();
        public abstract void SetModuleActive(bool value);
    }
}
