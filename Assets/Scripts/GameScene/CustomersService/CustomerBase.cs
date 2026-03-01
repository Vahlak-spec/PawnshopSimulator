using System;
using UnityEngine;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Customers
{
    public abstract class CustomerBase
    {
        public abstract void Bind(
            CharacterModulesController characterController,
            Ticker ticker,
            Transform start,
            Transform end,
            BuidRegisterService buidRegisterService);

        public abstract void LaunchProcces(Action<CustomerBase> onComplete);
    }
}
