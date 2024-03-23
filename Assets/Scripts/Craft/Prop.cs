using MicroJam10.SO;
using UnityEngine;

namespace MicroJam10.Craft
{
    public class Prop : MonoBehaviour
    {
        [SerializeField]
        private PropInfo _info;

        private GameObject _selectionHint;
        private Rigidbody _rb;

        public PentacleSpot Spot { set; get; }

        public PropInfo Info => _info;

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
