using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwingingVR : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerVRMovement pm;

    [Header("Swinging")]
    public float maxSwingDistance = 15f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;
    public float extendCableMax;


    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Joystick1Button14;


    private void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            StartSwing();
        }
        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
        }

        CheckForSwingPoints();

        if (joint != null) OdmGearMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        // Option 3 - Miss
        else
            realHitPoint = Vector3.zero;

        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        // realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    private void StartSwing()
    {
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        // deactivate active grapple
        if (GetComponent<GrapplingVR>() != null)
            GetComponent<GrapplingVR>().StopGrapple();
        pm.ResetRestrictions();

        pm.swinging = true;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing()
    {
        pm.swinging = false;

        lr.positionCount = 0;

        Destroy(joint);
    }

    private void OdmGearMovement()
    {
        // Get joystick input values for movement
        float horizontalInput = Input.GetAxis("Horizontal"); // Use the same names as defined in the Input Manager
        float verticalInput = Input.GetAxis("Vertical");
        // right
        if (Input.GetKey(KeyCode.D) || horizontalInput> 0.2f)
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        // left
        if (Input.GetKey(KeyCode.A) || horizontalInput < -0.2f)
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        // forward
        if (Input.GetKey(KeyCode.W) || verticalInput > 0.2f)
            rb.AddForce(orientation.forward * horizontalThrustForce * Time.deltaTime);

        // shorten cable
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button1) )
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        // extend cable
        if (Input.GetKey(KeyCode.S) || verticalInput < -0.2f)
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * extendCableMax;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition =
            Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    //joystick Button0 - A
    //joystick Button1 - B
    //joystick Button2 - X
    //joystick Button3 - Y
    //joystick Button4 - PrimaryHandTrigger (밑)
    //joystick Button5 - SecondHandTrigger (밑)
    //joystick Button14 - PrimaryIndexTrigger (맨앞)
    //joystick Button15 - SecondaryIndexTrigger (맨앞)


}
