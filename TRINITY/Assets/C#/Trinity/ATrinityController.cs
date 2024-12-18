using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ATrinityController : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] 
    public LayerMask GroundLayer;

    [SerializeField] 
    public float Gravity = 9.81f;
    
    [SerializeField] 
    public float GroundDistance = .1f;
    
    [SerializeField]
    public float Height; // => Collider.bounds.extents.y;

    [SerializeField] public float RotationSpeed = 5f;

    [HideInInspector]
    public CapsuleCollider Collider;
    
    [HideInInspector]
    public Rigidbody Rigidbody;

    // Movement Variables
    [HideInInspector]
    public Vector3 MoveDirection;
    [HideInInspector] public Vector3 Forward => transform.forward;
    [HideInInspector] public Vector3 Right => transform.right;
    [HideInInspector] public Vector3 Rotation => transform.rotation.eulerAngles;
    [HideInInspector]
    public float VerticalVelocity;
    [HideInInspector]
    public float PlanarVelocity;


    private APlayerInput InputReference;
    
    

    private void Awake()
    {
        InputReference = FindObjectOfType<APlayerInput>();
        // Ensure required components are assigned
        Collider = GetComponent<CapsuleCollider>();
        Rigidbody = GetComponent<Rigidbody>();

        if (Collider == null)
        {
            Debug.LogError("CapsuleCollider is missing!", this);
        }
        else
        {
            Collider.radius = 0.5f;
            Collider.height = 1.7f;
            Collider.center = new Vector3(0f, .8f, 0f);
        }

        if (Rigidbody == null)
        {
            Debug.LogError("Rigidbody is missing!", this);
        }
        else
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Update()
    {
        ApplyRotation();
    }

    public RaycastHit CheckGround()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, GroundDistance, GroundLayer);

        return hit;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position;

        // Draw the ground-check raycast
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * GroundDistance);
        Gizmos.DrawLine(rayOrigin, rayOrigin + Forward * 2f);
        Gizmos.DrawSphere(rayOrigin + Vector3.down * GroundDistance, 0.01f);
    }

    void ApplyRotation()
    {
        transform.Rotate(Vector3.up, InputReference.CameraInput.x * RotationSpeed * Time.deltaTime);
    }
}
