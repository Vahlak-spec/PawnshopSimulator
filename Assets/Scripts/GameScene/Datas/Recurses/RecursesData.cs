using UnityEngine;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "RecursesData", menuName = "Data/RecursesData")]
    public class RecursesData : ScriptableObject
    {
        [field: SerializeField] public RecursData[] recursDatas { get; private set; }

        [System.Serializable]
        public class RecursData
        {
            [field: SerializeField] public int DefaultValue { get; private set; }
            [field: SerializeField] public RecursType RecursType { get; private set; }
            [field: SerializeField] public Sprite Logo { get; private set; }
        }
    }
}
