using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PawnshopSimulator;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Customers;
using PawnshopSimulator.Data;
using PawnshopSimulator.MainMenu;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class SetDirty : MonoBehaviour {
    	public Graphic m_graphic;

    	void Reset () {
    		m_graphic = GetComponent<Graphic>();
    	}


    	void Update () {
    		m_graphic.SetVerticesDirty();
    	}
    }
}
