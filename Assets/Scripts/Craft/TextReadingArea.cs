using UnityEngine;

namespace MicroJam10.Craft
{
    public class TextReadingArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject _worldCamContainer, _textCamContainer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _worldCamContainer.SetActive(false);
                _textCamContainer.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _worldCamContainer.SetActive(true);
                _textCamContainer.SetActive(false);
            }
        }
    }
}
