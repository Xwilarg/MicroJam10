using UnityEngine;

namespace MicroJam10.Craft
{
    public class PentacleSpot : MonoBehaviour
    {
        private Light _light;
        private Prop _prop;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        public void ToggleLight(bool value)
        {
            _light.enabled = value;
        }

        public void SetProp(Prop p)
        {
            _prop = p;
        }

        public bool IsBusy => _prop != null;
    }
}
