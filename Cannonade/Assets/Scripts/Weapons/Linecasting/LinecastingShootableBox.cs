using Logging;
using UnityEngine;

namespace Weapons.Raycasting
{
    public class LinecastingShootableBox : MonoBehaviour
    {
        [SerializeField] public int health = 3;

        public void Damage(int damageAmount)
        {
            health -= damageAmount;

            if (health <= 0)
            {
                GameLog.LogMessage(
                    $"{nameof(LinecastingShootableBox)}.{nameof(Damage)}: {gameObject.name} destroyed");

                gameObject.SetActive(false);
            }
        }
    }
}