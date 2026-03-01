using System.Collections;
using UnityEngine;


namespace PawnshopSimulator.Characters
{
    public class CameraFollowModule : CharacterModuleBase
    {
        public Vector3 EyePos => _cameraPos.position;
        public Vector3 CameraDirection => _camera.transform.forward;
        public Vector3 CameraDirectionFlat => new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z).normalized;

        [SerializeField] private Transform _cameraPos;
        [Space]
        [SerializeField] private float sensitivityX = 2f;
        [SerializeField] private float sensitivityY = 2f;
        [Space]
        [SerializeField] private float minY = -80f;
        [SerializeField] private float maxY = 80f;

        private Camera _camera;

        private float _rotationX;
        private float _rotationY;

        private const float _followSpeed = 10f;
        private const float _rotationSpeed = 10f;

        private Coroutine _procces;

        private Quaternion _targetRotation;
        private Vector3 _targetPos;

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

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

        public void SetMouseXAxis(float xAxis)
        {
            _rotationY += xAxis * sensitivityX;
        }
        public void SetMouseYAxis(float yAxis)
        {
            _rotationX -= yAxis * sensitivityY;
            _rotationX = Mathf.Clamp(_rotationX, minY, maxY);
        }

        private IEnumerator Procces()
        {
            while (true)
            {
                _targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                _targetPos = _cameraPos.position;
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPos, _followSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
