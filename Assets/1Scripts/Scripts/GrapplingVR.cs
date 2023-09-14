using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplingVR : MonoBehaviour
{
    [Header("References")]
    private PlayerVRMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    public bool grappling;

    //Joohyuk's Added
    public Transform predictionPoint;
    public bool isExecuted = false;

    private void Start()
    {
        pm = GetComponent<PlayerVRMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        // if (grappling)
        //    lr.SetPosition(0, gunTip.position);
    }

    public void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        pm.freeze = true;

        if (predictionPoint.gameObject.activeSelf)
        {
            grapplePoint = predictionPoint.position;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);

        }
        else
        {
            StopGrapple();
        }

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        isExecuted = true;
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        isExecuted = false;
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
