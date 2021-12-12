using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public static readonly float DISPLAY_SPEED_MULTIPLIER = 5f;

    public static Car instance;
    public Transform centerOfMass;

    public WheelCollider[] frontWheels;
    public WheelCollider[] backWheels;

    public AnimationCurve turnCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve throttleCurve = AnimationCurve.EaseInOut(0f, 600f, 100f, 0f);
    
    [HideInInspector]
    public Rigidbody rb;
    public float downforce = 100000f;
    public float turningCoeff = 0.5f;
    public float turningAngle = 35f;
    private readonly Vector3 vectorSpeed = new Vector3(25f, 0f, 15f);

    private bool trackNotLoaded;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        rb = GetComponent<Rigidbody>();   
        if(rb != null){
            rb.centerOfMass = centerOfMass.localPosition;
        }
        rb.isKinematic = true;
    }

    void Start()
    {
        GameManager.instance.loadTrackEvents.Add(TrackLoaded);
    }

    public void TrackLoaded(){
        trackNotLoaded = false;
        rb.isKinematic = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(trackNotLoaded) return;
        var speed = transform.InverseTransformDirection(rb.velocity).z; 

        if(GameManager.instance.gameInfo.gameState == GameState.PLAYING){
            var turnInput = Input.GetAxis("Horizontal");
            var turnValue = turnCurve.Evaluate(Mathf.Abs(turnInput)) * Mathf.Sign(turnInput) * turningAngle;

            foreach(var wheel in frontWheels){
                var angle = Mathf.Lerp(wheel.steerAngle, turnValue, turningCoeff);
                wheel.steerAngle = angle;
                if(Mathf.Abs(angle) > turningAngle){
                    Debug.LogWarning("WTF");
                }
            }

            var throttleInput = Input.GetAxisRaw("Vertical");
            var convertedTorque = throttleCurve.Evaluate(speed) * throttleInput;

            foreach(var wheel in backWheels){
                wheel.motorTorque = convertedTorque;
            }
        }
        rb.AddForce(-transform.up * downforce * speed);
    }   
}
