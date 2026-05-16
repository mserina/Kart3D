using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIKartController : MonoBehaviour
{
    [Header("Waypoints")]
    public WaypointPath waypointPath;
    public float waypointReachDistance = 3f;

    [Header("Velocidad")]
    public float maxSpeed = 17f;
    public float acceleration = 2f;
    public float brakeForce = 5f;

    [Header("Giro")]
    public float turnSpeed = 60f;
    public float steeringSensitivity = 1.5f;

    [Header("Suelo")]
    public float gravityMultiplier = 3f;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private float currentSpeed;
    private bool isGrounded;
    private RaycastHit groundHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        CheckGround();
        NavigateToWaypoint();
        ApplyGravity();
        AlignToGround();
        ClampSpeed();
    }

    void NavigateToWaypoint()
    {
        if (waypointPath == null) return;

        Debug.Log("WayPointActual --> " + currentWaypointIndex);
        Transform target = waypointPath.GetWaypoint(currentWaypointIndex);

        // Dirección hacia el waypoint en espacio local
        Vector3 localTarget = transform.InverseTransformPoint(target.position);

        // Cuánto hay que girar: valor entre -1 y 1
        float steerInput = Mathf.Clamp(localTarget.x / localTarget.magnitude * steeringSensitivity, -1f, 1f);

        // Aceleración: reduce velocidad si la curva es muy cerrada
        float cornerFactor = 1f - Mathf.Abs(steerInput) * 0.4f;
        float targetSpeed = maxSpeed * cornerFactor;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        // Movimiento
        Vector3 move = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // Giro
        float turn = steerInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));

        // Comprueba si ha llegado al waypoint
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < waypointReachDistance)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointPath.GetWaypointCount();
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, groundCheckDistance, groundLayer);
    }

    void AlignToGround()
    {
        if (!isGrounded) return;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
    }

    void ApplyGravity()
    {
        if (!isGrounded)
            rb.AddForce(Vector3.down * gravityMultiplier * 9.81f, ForceMode.Acceleration);
    }

    void ClampSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            flatVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(flatVel.x, rb.linearVelocity.y, flatVel.z);
        }
    }

    // Para que el sistema de stun del plátano pueda pararlo
    public void SetStunned(bool stunned)
    {
        enabled = !stunned;
        if (stunned) rb.linearVelocity = Vector3.zero;
    }

    public float CurrentSpeed => currentSpeed;
    public int CurrentWaypointIndex => currentWaypointIndex;
}