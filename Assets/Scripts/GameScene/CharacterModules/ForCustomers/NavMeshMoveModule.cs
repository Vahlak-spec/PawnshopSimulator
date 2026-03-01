using UnityEngine;
using UnityEngine.AI;

namespace PawnshopSimulator.Characters
{
    public class NavMeshMoveModule : CharacterModuleBase
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;

        private static readonly int IsWalkHash = Animator.StringToHash("IsWalk");

        public bool HasReachedDestination =>
            !_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance &&
            (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f);

        public override void Launch()
        {
            _agent.enabled = true;
        }

        public override void SetModuleActive(bool value)
        {
            _agent.enabled = value;
            SetWalkAnimation(false);
        }

        public void SetDestination(Vector3 destination)
        {
            if (!_agent.enabled) return;
            _agent.isStopped = false;
            _agent.SetDestination(destination);
            SetWalkAnimation(true);
        }

        public void Stop()
        {
            if (!_agent.enabled) return;
            _agent.isStopped = true;
            _agent.ResetPath();
            SetWalkAnimation(false);
        }

        private void SetWalkAnimation(bool value)
        {
            Debug.Log("SetWalkAnimation - " + value.ToString());

            if (_animator != null)
                _animator.SetBool(IsWalkHash, value);
        }
    }
}
