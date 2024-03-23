using UnityEngine;

namespace MicroJam10.Craft
{
    public class Prop : MonoBehaviour
    {
        private GameObject _selectionHint;

        private void Awake()
        {
            _selectionHint = transform.GetChild(0).gameObject;
        }

        public void ToggleSelectionHint(bool value)
        {
            _selectionHint.SetActive(value);
        }
    }
}
