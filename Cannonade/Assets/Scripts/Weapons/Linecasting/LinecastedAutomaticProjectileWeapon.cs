using System.Collections;
using Logging;
using UnityEngine;

namespace Weapons.Linecasting
{
    public class LinecastedAutomaticProjectileWeapon : MonoBehaviour
    {
        /// <summary>
        /// Set the number of hitpoints that this gun will take away from shot objects with a health script
        /// </summary>
        [SerializeField] public int gunDamage = 1;

        /// <summary>
        /// Number in seconds which controls how often the player can fire
        /// </summary>
        [SerializeField] public float fireRate = 0.15f;

        /// <summary>
        /// Distance in Unity units over which the player can fire
        /// </summary>
        [SerializeField] public int weaponRange = 50;

        /// <summary>
        /// Amount of force which will be added to objects with a rigidbody shot by the player
        /// </summary>
        [SerializeField] public float hitForce = 100f;

        [SerializeField] public Transform gunEnd;

        private UnityEngine.Camera _fpsCam;
        private readonly WaitForSeconds _shotDuration = new(0.07f);
        private LineRenderer _laserLine;
        private float _nextFire;

        private AudioSource _gunAudio;

        // Start is called before the first frame update
        void Start()
        {
            _laserLine = GetComponent<LineRenderer>();

            _gunAudio = GetComponent<AudioSource>();

            _fpsCam = GetComponentInParent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > _nextFire)
            {
                _nextFire = Time.time + fireRate;

                StartCoroutine(ShotEffect());

                // create a vector at the center of our camera's viewport
                var origin = _fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                var gunEndPosition = gunEnd.position;

                _laserLine.SetPosition(0, gunEndPosition);
                var endPoint = gunEndPosition + new Vector3(0, 0, weaponRange);

                // check if our linecast has hit anything
                if (Physics.Linecast(origin, endPoint, out var hit))
                {
                    GameLog.LogMessage(
                        $"{nameof(LinecastedAutomaticProjectileWeapon)}.{nameof(Update)}: {hit.collider.name} hit!");

                    // Set the end position for our laser line 
                    _laserLine.SetPosition(1, hit.point);

                    // Get a reference to a health script attached to the collider we hit
                    var health = hit.collider.GetComponent<Components.WeaponShootableBox>();

                    // If there was a health script attached
                    if (health != null)
                    {
                        // Call the damage function of that script, passing in our gunDamage variable
                        health.Damage(gunDamage);

                        GameLog.LogMessage(
                            $"{nameof(LinecastedAutomaticProjectileWeapon)}.{nameof(Update)}: {health.name} damaged for {gunDamage}");
                    }

                    // Check if the object we hit has a rigidbody attached
                    if (hit.rigidbody != null)
                    {
                        // Add force to the rigidbody we hit, in the direction from which it was hit
                        hit.rigidbody.AddForce(-hit.normal * hitForce);
                    }
                }
                else
                {
                    // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                    _laserLine.SetPosition(1, origin + _fpsCam.transform.forward * weaponRange);
                }
            }
        }

        private IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            _gunAudio.Play();

            // Turn on our line renderer
            _laserLine.enabled = true;

            //Wait for .07 seconds
            yield return _shotDuration;

            // Deactivate our line renderer after waiting
            _laserLine.enabled = false;
        }
    }
}