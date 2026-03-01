using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Customers;
using PawnshopSimulator.MainMenu;
using PawnshopSimulator.Services;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "RecurseGroupData", menuName = "Data/RecurseGroupData")]
    public class RecurseGroupData : ScriptableObject
    {
        [field: SerializeField] public RecurseRequest[] RecurseRequests { get; private set; }

        [System.Serializable]
        public class RecurseRequest
        {
            [field: SerializeField] public int Value { get; private set; }
            [field: SerializeField] public RecursType RecursType { get; private set; }
        }
    }
}
