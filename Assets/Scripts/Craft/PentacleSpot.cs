using UnityEngine;

namespace MicroJam10.Craft
{
    public class PentacleSpot : MonoBehaviour
    {
        private Light _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        public void ToggleLight(bool value)
        {
            _light.enabled = value;
        }
    }
}
