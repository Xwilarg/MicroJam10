using UnityEngine;

namespace MicroJam10.Craft
{
    public class ExecutionSpot : MonoBehaviour
    {
        public static ExecutionSpot Instance { private set; get; }

        public bool IsPlayerInside { private set; get; }

        private Light _light;

        private void Awake()
        {
            Instance = this;
            _light = GetComponent<Light>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerInside = true;
                _light.enabled = true;
                GameManager.Instance.CheckVictory();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerInside = false;
                _light.enabled = false;
            }
        }
    }
}
