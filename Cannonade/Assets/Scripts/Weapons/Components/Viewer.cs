using UnityEngine;

namespace Weapons.Components
{
    public class Viewer : MonoBehaviour
    {
        [SerializeField] public float weaponRange = 50f;

        private UnityEngine.Camera _fpsCam;

        private void Start()
        {
            _fpsCam = GetComponentInParent<UnityEngine.Camera>();
        }

        private void Update()
        {
            // Create a vector at the center of our camera's viewport
            var lineOrigin = _fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
            Debug.DrawRay(lineOrigin, _fpsCam.transform.forward * weaponRange, Color.green);
        }
    }
}