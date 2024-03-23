using UnityEngine;

namespace MicroJam10.Craft
{
    public class Prop : MonoBehaviour
    {
        private GameObject _selectionHint;
        private Rigidbody _rb;

        private void Awake()
        {
            _selectionHint = transform.GetChild(0).gameObject;
            _rb = GetComponent<Rigidbody>();
        }

        public void ToggleSelectionHint(bool value)
        {
            _selectionHint.SetActive(value);
        }

        public void ToggleStatic(bool value)
        {
            _rb.isKinematic = value;
        }
    }
}
