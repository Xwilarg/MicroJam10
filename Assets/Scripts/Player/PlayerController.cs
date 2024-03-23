using MicroJam10.Craft;
using MicroJam10.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MicroJam10.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private Transform _cam;

        [SerializeField]
        private Transform _hands, _flashlight;

        private Vector2 _mov;
        private float _verticalSpeed;
        private CharacterController _controller;

        private int _mapLayer, _propSelectionLayer, _pentacleSelectionLayer;

        private Prop _interactionTarget;
        private PentacleSpot _spotTarget;
        private Prop _carriedProp;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _mapLayer = 1 << LayerMask.GetMask("Map");
            _propSelectionLayer = ~(1 << LayerMask.GetMask("Map", "Prop"));
            _pentacleSelectionLayer = ~(1 << LayerMask.GetMask("Map", "Spot"));

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            var pos = _mov;
            Vector3 desiredMove = _cam.transform.forward * pos.y + _cam.transform.right * pos.x;

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform.position, _controller.radius, Vector3.down, out RaycastHit hitInfo,
                               _controller.height / 2f, _mapLayer, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            Vector3 moveDir = Vector3.zero;
            moveDir.x = desiredMove.x * _info.ForceMultiplier;
            moveDir.z = desiredMove.z * _info.ForceMultiplier;

            if (_controller.isGrounded && _verticalSpeed < 0f) // We are on the ground and not jumping
            {
                moveDir.y = -.1f; // Stick to the ground
                _verticalSpeed = -_info.GravityMultiplicator;
            }
            else
            {
                // We are currently jumping, reduce our jump velocity by gravity and apply it
                _verticalSpeed += Physics.gravity.y * _info.GravityMultiplicator;
                moveDir.y += _verticalSpeed;
            }

            _controller.Move(moveDir);
        }

        private void Update()
        {
            var target = GetInteractionTarget(_carriedProp == null ? _propSelectionLayer : _pentacleSelectionLayer);

            if (_carriedProp == null)
            {
                var havePropTarget = target != null && target.Value.collider.CompareTag("Prop");

                if (!havePropTarget && _interactionTarget != null)
                {
                    _interactionTarget.ToggleSelectionHint(false);
                    _interactionTarget = null;
                }
                else if (havePropTarget && (_interactionTarget == null || _interactionTarget.gameObject.GetInstanceID() != target.Value.collider.gameObject.GetInstanceID()))
                {
                    if (_interactionTarget != null)
                    {
                        _interactionTarget.ToggleSelectionHint(false);
                    }
                    _interactionTarget = target.Value.collider.GetComponent<Prop>();
                    _interactionTarget.ToggleSelectionHint(true);
                }
            }
            else
            {
                var haveSpotTarget = target != null && target.Value.collider.CompareTag("Spot");


                if (!haveSpotTarget && _spotTarget != null)
                {
                    _spotTarget.ToggleLight(false);
                    _spotTarget = null;
                }
                else if (haveSpotTarget && _spotTarget == null)
                {
                    _spotTarget = target.Value.collider.GetComponent<PentacleSpot>();
                    _spotTarget.ToggleLight(true);
                }


            }

            var forward = _cam.transform.forward.normalized * 2f;
            _hands.transform.position = new(transform.position.x + forward.x, _hands.transform.position.y, transform.position.z + forward.z);
            _hands.transform.rotation = _cam.transform.rotation;
            _flashlight.transform.rotation = Quaternion.Euler(_flashlight.transform.rotation.eulerAngles.x, _cam.transform.rotation.eulerAngles.y, _flashlight.transform.rotation.eulerAngles.z);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (_carriedProp == null)
                {
                    if (_interactionTarget != null)
                    {
                        _interactionTarget.ToggleSelectionHint(false);
                        _interactionTarget.ToggleStatic(true);
                        _interactionTarget.transform.parent = _hands.transform;
                        _interactionTarget.transform.localPosition = Vector3.zero;
                        _carriedProp = _interactionTarget;
                        _interactionTarget = null;
                    }
                }
                else
                {
                    _carriedProp.ToggleStatic(false);
                    _carriedProp.transform.parent = null;
                    _carriedProp = null;
                }
            }
        }

        public RaycastHit? GetInteractionTarget(int targetLayer)
        {
            if (Physics.Raycast(new Ray(_cam.position, _cam.forward), out RaycastHit interInfo, 100f, targetLayer))
            {
                return interInfo;
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            var target = GetInteractionTarget(_propSelectionLayer);
            if (target != null)
            {
                Gizmos.color = target.Value.collider.CompareTag("Prop") ? Color.blue : Color.red;
                Gizmos.DrawLine(_cam.transform.position, target.Value.point);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_cam.transform.position, _cam.transform.position + _cam.transform.forward * 10f);
            }
        }
    }
}