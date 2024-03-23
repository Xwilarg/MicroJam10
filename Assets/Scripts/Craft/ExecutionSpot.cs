using UnityEngine;

namespace MicroJam10.Craft
{
    public class ExecutionSpot : MonoBehaviour
    {
        private Light _light;

        [SerializeField]
        private GameObject _bloodMiddle;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _light.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _light.enabled = false;
            }
        }
    }
}
