using Logging;
using UnityEngine;

namespace Weapons.Components
{
    public class WeaponShootableBox : MonoBehaviour
    {
        [SerializeField] public int health = 3;

        public void Damage(int damageAmount)
        {
            health -= damageAmount;

            if (health <= 0)
            {
                GameLog.LogMessage(
                    $"{nameof(WeaponShootableBox)}.{nameof(Damage)}: {gameObject.name} destroyed");

                gameObject.SetActive(false);
            }
        }
    }
}