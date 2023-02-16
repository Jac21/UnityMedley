using Damage.Interfaces;
using Logging;
using UnityEngine;

namespace Damage.Implementations
{
    public class Damageable : MonoBehaviour, IDamageable<float>
    {
        public void Damage(float damage)
        {
            GameLog.LogMessage($"{nameof(Damageable)}.{nameof(Damage)}: Damaged for {damage}");
        }
    }
}