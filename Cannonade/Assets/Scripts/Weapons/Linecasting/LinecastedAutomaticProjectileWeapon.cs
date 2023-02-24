using System.Collections;
using Logging;
using UnityEngine;

namespace Weapons.Raycasting
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
        [SerializeField] public float fireRate = 0.25f;

        /// <summary>
        /// Distance in Unity units over which the player can fire
        /// </summary>
        [SerializeField] public int weaponRange = 50;

        /// <summary>
        /// Amount of force which will be added to objects with a rigidbody shot by the player
        /// </summary>
        [SerializeField] public float hitForce = 100f;

        /// <summary>
        /// Amount of force which will be added to objects with a rigidbody shot by the player
        /// </summary>
        [SerializeField] public Transform gunEnd;

        private UnityEngine.Camera _fpsCam;
        private readonly WaitForSeconds _shotDuration = new(0.07f);
        private LineRenderer _laserLine;
        private float _nextFire;

        private AudioSource gunAudio;

        // Start is called before the first frame update
        void Start()
        {
            _laserLine = GetComponent<LineRenderer>();

            gunAudio = GetComponent<AudioSource>();

            _fpsCam = GetComponentInParent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > _nextFire)
            {
                _nextFire = Time.time + fireRate;

                StartCoroutine(ShotEffect());

                var rayOrigin = _fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                _laserLine.SetPosition(0, gunEnd.position);

                // Check if our raycast has hit anything
                if (Physics.Linecast(rayOrigin, _fpsCam.transform.forward, out var hit, weaponRange))
                {
                    // Set the end position for our laser line 
                    _laserLine.SetPosition(1, hit.point);

                    // Get a reference to a health script attached to the collider we hit
                    var health = hit.collider.GetComponent<LinecastingShootableBox>();

                    // If there was a health script attached
                    if (health != null)
                    {
                        // Call the damage function of that script, passing in our gunDamage variable
                        health.Damage(gunDamage);

                        GameLog.LogMessage(
                            $"{nameof(RaycastedAutomaticProjectileWeapon)}.{nameof(Update)}: {health.name} damaged for {gunDamage}");
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
                    _laserLine.SetPosition(1, rayOrigin + _fpsCam.transform.forward * weaponRange);
                }
            }
        }

        private IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            gunAudio.Play();

            // Turn on our line renderer
            _laserLine.enabled = true;

            //Wait for .07 seconds
            yield return _shotDuration;

            // Deactivate our line renderer after waiting
            _laserLine.enabled = false;
        }
    }
}