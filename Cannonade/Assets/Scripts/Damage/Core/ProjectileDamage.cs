using Damage.Interfaces;
using Logging;
using UnityEngine;

namespace Damage.Core
{
    public class ProjectileDamage : MonoBehaviour
    {
        [SerializeField] public float attackPower;

        private bool _canAttack;

        private void OnEnable()
        {
            _canAttack = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            GameLog.LogMessage($"{nameof(ProjectileDamage)}.{nameof(OnTriggerEnter)}: Collider {other.name} triggered");

            var hit = other.GetComponent<IDamageable<float>>();

            if (hit != null && _canAttack)
            {
                hit.Damage(attackPower);
                _canAttack = false;
            }
        }
    }
}