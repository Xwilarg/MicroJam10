using UnityEngine;

namespace MicroJam10.Craft
{
    public class ExecutionSpot : MonoBehaviour
    {
        private Light _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _light.enabled = true;
                GameManager.Instance.CheckVictory();
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
