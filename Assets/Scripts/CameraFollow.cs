using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    private Vector3 objectToFollowOffset;
    private Vector3 localObjectToFollowOffset;
    private Vector3 startingPosition;
    private Vector3 lastLRMiddle;
    private float lastLRWidth;
    private float distanceFromFollowing;

    private Vector3 lastObjToFollowPosition;
    private float previousEulerY;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        objectToFollowOffset = transform.position - objectToFollow.transform.position;
        localObjectToFollowOffset = objectToFollow.transform.InverseTransformVector(objectToFollowOffset);
        //Debug.Log(objectToFollowOffset.Equals(objectToFollow.transform.InverseTransformVector(objectToFollowOffset)));
        distanceFromFollowing = objectToFollowOffset.magnitude;
        lastObjToFollowPosition = objectToFollow.transform.position;

        Application.targetFrameRate = 60;
        previousEulerY = this.transform.rotation.eulerAngles.y;
    }


    //todo do something when the car flips
    // Update is called once per frame
    void LateUpdate()
    {

        float maxDistance = 100f;


        var carPosition = objectToFollow.transform.position;

        Vector3 position = transform.position;

        RaycastHit left, right, up, down;

        bool castSuccessLeft, castSuccessRight;
        SafeTwoCast(transform.position, -transform.right, out left, out castSuccessLeft, transform.right, out right, out castSuccessRight, maxDistance, false);

        // Rotation

        RaycastHit rotL, rotR;
        bool rotLS, rotRS;

        Vector3 rotationEuler = transform.rotation.eulerAngles;
        var previousEulerX = rotationEuler.x;
        rotationEuler.x = 0f;
        this.transform.rotation = Quaternion.Euler(rotationEuler);

        Vector3 rotPos = this.transform.position + this.transform.forward * 0.3f;
        Debug.DrawLine(this.transform.position, rotPos, Color.blue);
        SafeTwoCast(rotPos, -transform.right, out rotL, out rotLS, transform.right, out rotR, out rotRS, maxDistance, false);

        Vector3 betweenSamplePoints = ((rotL.point + rotR.point) / 2f) - ((left.point + right.point) / 2f);
        float angle = Vector3.SignedAngle(Vector3.forward, betweenSamplePoints, Vector3.up);

        // So, in general, for the rotation to be smooth the camera must not be tilted downwars, to prevent this, I guess we could change some values lol;

        rotationEuler.x = previousEulerX;
        transform.rotation = Quaternion.Euler(rotationEuler);
        rotationEuler.y = angle;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationEuler), 0.15f);


        //End rotation



        RaycastHit carBack;
        Vector3 transformedFollowOffset = objectToFollow.transform.TransformVector(localObjectToFollowOffset);
        if (Physics.Raycast(objectToFollow.transform.position, transformedFollowOffset, out carBack, maxDistance))
        {
            Debug.DrawLine(objectToFollow.transform.position, carBack.point);
        }

        var followOffsetCollision = carBack.point - objectToFollow.transform.position;
        if (followOffsetCollision.sqrMagnitude >= objectToFollowOffset.sqrMagnitude)
        {
            this.transform.position = objectToFollow.transform.position + transformedFollowOffset;
        }
        else
        {
            Debug.Log("Would go out of bounds, fixing that!");
            this.transform.position = objectToFollow.transform.position + (followOffsetCollision * 0.5f);
        }

        SafeTwoCast(transform.position, -transform.right, out left, out castSuccessLeft, transform.right, out right, out castSuccessRight, maxDistance);

        bool castSuccessUp, castSuccessDown;
        SafeTwoCast(transform.position, Vector3.up, out up, out castSuccessUp, -Vector3.up, out down, out castSuccessDown, maxDistance);

        if (!(castSuccessLeft && castSuccessRight))
        {
            SafeTwoCast(transform.position, -transform.right, out left, out castSuccessLeft, transform.right, out right, out castSuccessRight, maxDistance);
        }
        
        this.lastObjToFollowPosition = carPosition;
    }

    private void SafeTwoCast(Vector3 origin, Vector3 firstDirection, out RaycastHit firstRaycastHit, out bool firstHit,
        Vector3 secondDirection, out RaycastHit secondRaycastHit, out bool secondHit, float maxDistance, bool setPos = true)
    {
        firstHit = Physics.Raycast(origin, firstDirection, out firstRaycastHit, maxDistance);
        secondHit = Physics.Raycast(origin, secondDirection, out secondRaycastHit, maxDistance);


        if (!firstHit && secondHit)
        {
            secondHit = Physics.Raycast(firstRaycastHit.point, secondDirection, out secondRaycastHit, maxDistance);
        }
        else if (firstHit & !secondHit)
        {
            firstHit = Physics.Raycast(secondRaycastHit.point, firstDirection, out firstRaycastHit, maxDistance);
        }

        if (firstHit && secondHit && setPos)
        {
            transform.position = (firstRaycastHit.point + secondRaycastHit.point) / 2f;
        }
    }

    float AngleBetweenVectors(Vector3 v1, Vector3 v2, Vector3 up)
    {
        // provides a cleaner result at low angles than Vector3.Angle()
        var cross = Vector3.Cross(v1, v2);
        var dot = Vector3.Dot(v1, v2);
        var angle = Mathf.Atan2(cross.magnitude, dot);
        var test = Vector3.Dot(up, cross);
        if (test < 0.0) angle = -angle;
        return angle * Mathf.Rad2Deg; 
    }

}
