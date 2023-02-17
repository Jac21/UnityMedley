using Damage.Core;
using Logging;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Inspiration from https://learn.unity.com/tutorial/using-c-to-launch-projectiles#
    /// </summary>
    public class ProjectileWeapon : MonoBehaviour
    {
        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] public float fireRate = 0.5f;
        [SerializeField] public float damage = 10f;
        [SerializeField] public float launchVelocity = 700f;

        [SerializeField] public AudioClip shootsound;
        [SerializeField] public float lowVolumeRange = .25f;
        [SerializeField] public float highVolumeRange = .05f;
        
        private AudioSource _source;
        
        private float _lastShotTime;
        
        // Start is called before the first frame update
        void Start()
        {
            _source = GetComponent<AudioSource>();
        }
        
        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time - _lastShotTime > fireRate)
            {
                var vol = Random.Range(lowVolumeRange, highVolumeRange);
                _source.PlayOneShot(shootsound, vol);
                
                _lastShotTime = Time.time;

                FireProjectile();
            }
        }

        private void FireProjectile()
        {
            // Instantiate the projectile and set its direction
            var transformReference = transform;
            var projectile = Instantiate(projectilePrefab, transformReference.position, transformReference.rotation);

            GameLog.LogMessage($"{nameof(ProjectileWeapon)}.{nameof(FireProjectile)}: {projectile.name} fired");

            var rb = projectile.GetComponent<Rigidbody>();
            rb.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));

            // Add a collider to the projectile to detect hits
            Collider addedCollider = projectile.AddComponent<SphereCollider>();
            addedCollider.isTrigger = true;

            // Attach a script to the projectile to handle damage
            var projectileDamage = projectile.AddComponent<ProjectileDamage>();
            projectileDamage.attackPower = damage;
        }
    }
}