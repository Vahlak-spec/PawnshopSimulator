using System.Collections;
using UnityEngine;


namespace PawnshopSimulator.Characters
{
    public class MoveModule : CharacterModuleBase
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _speed = 3;

        private Vector3 _tempVelocity;

        private Coroutine _procces;

        public override void Launch()
        {
            _procces = StartCoroutine(Procces());
        }
        public override void SetModuleActive(bool value)
        {
            if (_procces != null)
                StopCoroutine(_procces);

            if (value)
            {
                _procces = StartCoroutine(Procces());
            }
        }
        public void SetMoveDirection(Vector3 velocity)
        {
            _tempVelocity = velocity * _speed;
        }

        private IEnumerator Procces()
        {
            while (true)
            {
                _body.velocity = _tempVelocity;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
