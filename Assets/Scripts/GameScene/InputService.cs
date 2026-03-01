using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Services;

namespace PawnshopSimulator
{
    public class InputService : IGameService
    {
        public Action<float> onChangeXMouseAxis;
        public Action<float> onChangeYMouseAxis;

        public Action<Vector3> onChangeMoveVelocity;

        private Coroutine _procces;

        private Vector3 _tempMoveVelocity;

        private Ticker _ticker;

        private readonly Dictionary<KeyCode, Action> _keyDownBindings = new Dictionary<KeyCode, Action>();
        private readonly Dictionary<KeyCode, Action> _keyUpBindings = new Dictionary<KeyCode, Action>();
        private readonly Dictionary<KeyCode, Action> _keyHoldBindings = new Dictionary<KeyCode, Action>();

        public void Bind(ServicesProvider componentProvider)
        {
            _ticker = componentProvider.Ticker;
        }

        public void OnLaunchGame()
        {
            _ticker.onUpdate += Tick;
        }

        public void SetActiveCursor(bool value)
        {
            if (value)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = value;
        }

        public void BindKey(KeyCode keyCode, Action callback)
        {
            if (!_keyHoldBindings.ContainsKey(keyCode))
                _keyHoldBindings[keyCode] = null;

            _keyHoldBindings[keyCode] += callback;
        }

        public void UnbindKey(KeyCode keyCode, Action callback)
        {
            if (_keyHoldBindings.ContainsKey(keyCode))
                _keyHoldBindings[keyCode] -= callback;
        }

        public void BindDownKey(KeyCode keyCode, Action callback)
        {
            if (!_keyDownBindings.ContainsKey(keyCode))
                _keyDownBindings[keyCode] = null;

            _keyDownBindings[keyCode] += callback;
        }

        public void UnbindDownKey(KeyCode keyCode, Action callback)
        {
            if (_keyDownBindings.ContainsKey(keyCode))
                _keyDownBindings[keyCode] -= callback;
        }

        public void BindUpKey(KeyCode keyCode, Action callback)
        {
            if (!_keyUpBindings.ContainsKey(keyCode))
                _keyUpBindings[keyCode] = null;

            _keyUpBindings[keyCode] += callback;
        }

        public void UnbindUpKey(KeyCode keyCode, Action callback)
        {
            if (_keyUpBindings.ContainsKey(keyCode))
                _keyUpBindings[keyCode] -= callback;
        }

        private void Tick()
        {
            onChangeXMouseAxis?.Invoke(Input.GetAxis("Mouse X"));
            onChangeYMouseAxis?.Invoke(Input.GetAxis("Mouse Y"));

            _tempMoveVelocity = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical"));

            onChangeMoveVelocity?.Invoke(_tempMoveVelocity);

            foreach (var binding in _keyDownBindings)
                if (Input.GetKeyDown(binding.Key))
                    binding.Value?.Invoke();

            foreach (var binding in _keyUpBindings)
                if (Input.GetKeyUp(binding.Key))
                    binding.Value?.Invoke();

            foreach (var binding in _keyHoldBindings)
                if (Input.GetKey(binding.Key))
                    binding.Value?.Invoke();
        }
        private IEnumerator Procces()
        {
            while (true)
            {
                onChangeXMouseAxis?.Invoke(Input.GetAxis("Mouse X"));
                onChangeYMouseAxis?.Invoke(Input.GetAxis("Mouse Y"));

                _tempMoveVelocity = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical"));

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Space");
                }

                onChangeMoveVelocity?.Invoke(_tempMoveVelocity);

                foreach (var binding in _keyDownBindings)
                    if (Input.GetKeyDown(binding.Key))
                        binding.Value?.Invoke();

                foreach (var binding in _keyUpBindings)
                    if (Input.GetKeyUp(binding.Key))
                        binding.Value?.Invoke();

                foreach (var binding in _keyHoldBindings)
                    if (Input.GetKey(binding.Key))
                        binding.Value?.Invoke();

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
