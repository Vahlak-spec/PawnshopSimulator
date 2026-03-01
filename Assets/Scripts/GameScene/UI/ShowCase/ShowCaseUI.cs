using UnityEngine;
using UnityEngine.UI;

namespace PawnshopSimulator.UI
{
    public class ShowCaseUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiRoot;


        [field: SerializeField] public ShowCaseInventoryItemUI[] InventoryItemUIs { get; private set; }


        [field: SerializeField] public ShowCaseDisplayUI DisplayUI { get; private set; }


        [field: SerializeField] public Button TakeButton { get; private set; }

        public void SetActiveUI(bool value) => _uiRoot.SetActive(value);
    }
}
