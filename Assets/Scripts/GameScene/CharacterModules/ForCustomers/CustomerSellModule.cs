using System;
using PawnshopSimulator;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Services;
using UnityEngine;

public class CustomerSellModule : CharacterModuleBase
{
    [SerializeField] private TriggerCollider _triggerCollider;

    private CashRegister _nearestRegister;

    public override void Launch()
    {
        _triggerCollider.SetEnterAction<CashRegister>(OnEnterRegister);
        _triggerCollider.SetExitAction<CashRegister>(OnExitRegister);
    }

    public override void SetModuleActive(bool value) { }

    public bool TrySellToRegister(CashRegister register, InventoryItem inventoryItem, Action<bool> onResolved)
    {
        return register.TrySetOffer(inventoryItem, onResolved);
    }

    private void OnEnterRegister(GameObject obj)
    {
        _nearestRegister = obj.GetComponent<CashRegister>();
    }

    private void OnExitRegister(GameObject obj)
    {
        if (obj.GetComponent<CashRegister>() == _nearestRegister)
            _nearestRegister = null;
    }
}
