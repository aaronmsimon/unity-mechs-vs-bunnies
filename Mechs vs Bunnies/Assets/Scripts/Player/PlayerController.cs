using UnityEngine;

namespace mvb.player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform torso;

        private Vector2 movement;
        private Vector3 mousePos;

        private PlayerControls playerControls;
        private Rigidbody rb;

        private void Awake()
        {
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            GetPlayerInput();
            GetLookPos();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void GetPlayerInput()
        {
            movement = playerControls.Gameplay.Move.ReadValue<Vector2>();
        }

        private void MovePlayer()
        {
            Vector3 newPos = new Vector3(movement.x, 0f, movement.y);
            rb.MovePosition(rb.position + (newPos * (moveSpeed * Time.fixedDeltaTime)));
        }

        private void GetLookPos()
        {
            mousePos = playerControls.Gameplay.Look.ReadValue<Vector2>();
            // mousePos.z = -Camera.main.transform.position.z;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, pointOfIntersection, Color.red);
                Vector3 heightCorrectedPoint = new Vector3(pointOfIntersection.x, torso.position.y, pointOfIntersection.z);
                torso.LookAt(heightCorrectedPoint);
            }
        }
    }    
}
