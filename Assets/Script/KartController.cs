using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour
{
    [Header("Velocity")]
    public float maxSpeed = 20f;
    public float acceleration = 10f;
    public float brakeForce = 15f;   //Stop the car
    private float speedMultiplier = 1f;

    
    [Header("Turn")]
    public float turnSpeed = 80f;
    
    [Header("Wheels")]
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelRL;
    public Transform wheelRR;
    public float wheelRadius = 0.3f;

    
    public AnimationCurve accelerationCurve = AnimationCurve.Linear(0, 1, 1, 0);
    private Rigidbody rb;
    private float currentSpeed;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        RotateWheels();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
    }

   
    
    // To control move to the car (to accelerate or to brake)
    void HandleMovement()
    {
        
        float driveinput = Input.GetAxis("Vertical"); // W/S or arrows
        bool isBraking = Input.GetKey(KeyCode.Space);   // Space — brake
        
        if (isBraking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakeForce * Time.fixedDeltaTime);
        }
        
        if (driveinput > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * driveinput, acceleration * Time.fixedDeltaTime);
        }
            
        else if (driveinput < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * 0.5f * driveinput, brakeForce * Time.fixedDeltaTime);
        }

        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakeForce * 0.5f * Time.fixedDeltaTime);
        }
            

        Vector3 move = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        
    }

    
    // To control direction to the car (Right or Left)
    void HandleSteering()
    {
        if (Mathf.Abs(currentSpeed) < 0.5f) return;

        float steerInput = Input.GetAxis("Horizontal"); // A/D o flechas
        float turn = steerInput * turnSpeed * Time.fixedDeltaTime;
        if (currentSpeed < 0) turn = -turn;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
    
    
    // To move the wheels of the car 
    void RotateWheels()
    {
        //This line calculate how many gradious must spin the wheel
        float rpm = (currentSpeed / (2f * Mathf.PI * wheelRadius)) * 360f * Time.deltaTime;

        // The move to the back wheels
        wheelRL.Rotate(rpm, 0f, 0f);
        wheelRR.Rotate(rpm, 0f, 0f);

        // The move to the front wheels
        float steer = Input.GetAxis("Horizontal") * 25f;
        wheelFL.localRotation = Quaternion.Euler(wheelFL.localRotation.eulerAngles.x + rpm, steer, 0f);
        wheelFR.localRotation = Quaternion.Euler(wheelFR.localRotation.eulerAngles.x + rpm, steer, 0f);
    }
    
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }
    
    IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }
    
    
    
    // The current speed value is public, for others methods can take it
    public float CurrentSpeed => currentSpeed;

}