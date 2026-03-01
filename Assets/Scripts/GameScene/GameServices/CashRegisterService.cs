using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Services
{
    public class CashRegisterService : IGameService
    {
        private const float BUY_PRICE_MULTIPLIER = 0.8f;

        private CashRegisterUI _ui;
        private InventoryService _inventoryService;
        private RecursesManagerService _recursesManager;
        private PlayerControllerService _playerController;
        private InputService _inputService;

        private CashRegister _currentRegister;

        public CashRegisterService(UIHolder uiHolder)
        {
            _ui = uiHolder.CashRegisterUI;

            _ui.BuyButton.onClick.AddListener(TryBuy);
            _ui.DeclineButton.onClick.AddListener(Decline);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _inventoryService = componentProvider.GetService<InventoryService>();
            _recursesManager = componentProvider.GetService<RecursesManagerService>();
            _playerController = componentProvider.GetService<PlayerControllerService>();
            _inputService = componentProvider.GetService<InputService>();

            _inputService.BindDownKey(KeyCode.Mouse1, CloseCashRegisterMenu);
        }

        public void OnLaunchGame()
        {
            _ui.SetActiveUI(false);
        }

        public void OpenCashRegisterMenu(CashRegister register)
        {
            _currentRegister = register;

            _ui.SetActiveUI(true);
            _playerController.SetPlayerActivity(false);
            _inputService.SetActiveCursor(true);

            RefreshUI();
        }

        public void CloseCashRegisterMenu()
        {
            if (_currentRegister == null) return;

            _inputService.SetActiveCursor(false);
            _ui.SetActiveUI(false);
            _playerController.SetPlayerActivity(true);
            _currentRegister = null;
        }


        public void NotifyOfferChanged(CashRegister register)
        {
            if (_currentRegister == register)
                RefreshUI();
        }

        private void RefreshUI()
        {
            InventoryItem offer = _currentRegister?.GetCurrentOffer();
            bool hasOffer = offer != null;

            _ui.SetOfferVisible(hasOffer);

            if (!hasOffer) return;

            int price = Mathf.RoundToInt(offer.Price * BUY_PRICE_MULTIPLIER);

            _ui.ItemSprite.sprite = offer.Sprite;
            _ui.ItemNameText.text = offer.ItemName;
            _ui.ItemStateText.text = offer.ItemState.ToString();
            _ui.ItemPriceText.text = price.ToString();

            _ui.BuyButton.interactable = _recursesManager.GetAmount(RecursType.Money) >= price;
        }

        private void TryBuy()
        {
            if (_currentRegister == null) return;

            InventoryItem offer = _currentRegister.GetCurrentOffer();
            if (offer == null) return;

            int price = Mathf.RoundToInt(offer.Price * BUY_PRICE_MULTIPLIER);

            if (!_recursesManager.TrySpend(RecursType.Money, price)) return;

            _inventoryService.AddItem(offer);
            _currentRegister.AcceptOffer();

            RefreshUI();
        }

        private void Decline()
        {
            if (_currentRegister == null) return;

            _currentRegister.DeclineOffer();
            RefreshUI();
        }
    }
}
