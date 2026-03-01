using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PawnshopSimulator.UI
{
    public class RecursesUI : MonoBehaviour
    {
        [field: SerializeField] public RecursesTypeUI[] recursesTypeUI { get; private set; }

        [System.Serializable]
        public class RecursesTypeUI
        {
            [SerializeField] private GameObject _holder;
            [Space]
            [SerializeField] private TextMeshProUGUI _value;
            [SerializeField] private Image _logo;

            public void SetSprite(Sprite sprite) => _logo.sprite = sprite;
            public void OnChangeValue(int value) => _value.text = value.ToString();
            public void SetActive(bool value) => _holder.SetActive(value);
        }
    }
}
