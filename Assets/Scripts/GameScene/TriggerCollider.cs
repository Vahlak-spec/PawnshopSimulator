using System;
using UnityEngine;

namespace PawnshopSimulator
{
    public class TriggerCollider : MonoBehaviour
    {

        private Collider _collider;

        private Action<GameObject> _enterAction;
        private Action<Collider> _checkEnterCollision;

        private Action<GameObject> _exitAction;
        private Action<Collider> _checkExitCollision;

        private void Start()
        {
            _collider = gameObject.GetComponent<Collider>();
        }

        public void SetActiveCollider(bool value)
        {
            if(_collider == null)
                _collider = gameObject.GetComponent<Collider>();

            _collider.enabled = value;
        } 

        public void SetEnterAction<T>(Action<GameObject> action)
        {
            _checkEnterCollision = CheckEnterCollision<T>;
            _enterAction = action;
        }
        public void SetExitAction<T>(Action<GameObject> action)
        {
            _checkExitCollision = CheckExitCollision<T>;
            _exitAction = action;
        }

        private void CheckEnterCollision<T>(Collider collision)
        {
            if (collision.GetComponent<T>() != null)
            {
                _enterAction.Invoke(collision.gameObject);
            }
        }
        private void CheckExitCollision<T>(Collider collision)
        {
            if (collision.GetComponent<T>() != null)
            {
                _exitAction.Invoke(collision.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _checkEnterCollision?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _checkExitCollision?.Invoke(other);
        }
    }
}
