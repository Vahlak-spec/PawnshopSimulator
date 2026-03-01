using UnityEngine;

namespace PawnshopSimulator.UI
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiHolder;

        public void SetActiveUIHolder(bool value) => _uiHolder.SetActive(value);
    }
}
