﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrosshairBehavior : MonoBehaviour
{
    [SerializeField]
    private Camera CameraFacing;

    public enum CameraMode
    {
        DYNAMIC_MODE,
        FIXED_DEPTH_MODE
    };

    [SerializeField]
    private CameraMode mCameraMode = CameraMode.FIXED_DEPTH_MODE;

    private bool mIsObjectTargetted;
    private CrosshairListener mLastObjectHit;
    private RaycastHit mCurrentHit;

    //Sets the default mode to FIXED_DEPTH_MODE
    void Start()
    {
        mIsObjectTargetted = false;
    }

    // Update is called once per frame
    void Update()
    {
        mCurrentHit = LaunchRaycast();
        CheckModeToggle();
        SetCrosshairPosition();
        CrosshairEvents();
    }

    //Use T to toggle between FIXED_DEPTH_MODE and DYNAMIC_MODE
    void CheckModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (mCameraMode == CameraMode.FIXED_DEPTH_MODE)
            {
                mCameraMode = CameraMode.DYNAMIC_MODE;
            }
            else
            {
                mCameraMode = CameraMode.FIXED_DEPTH_MODE;
            }
        }
    }

    //Sets the crosshair to the appropiate depth
    void SetCrosshairPosition()
    {
        //Sets crosshair quad to always face towards camera and fixes the depth
        if (mCameraMode == CameraMode.FIXED_DEPTH_MODE)
        {
            SetFixedDepth();
        }
        //Determines distance from object and renders crosshair quad at that distance
        else if (mCameraMode == CameraMode.DYNAMIC_MODE)
        {
            //If object is untagged, it defaults to the Fixed depth
            if (mIsObjectTargetted == true && mCurrentHit.collider.tag != "Untagged")
            {
                SetDynamicDepth(mCurrentHit);
            }
            else
            {
                SetFixedDepth();
            }
        }
    }

    //Shoots out a raycast coming from the center of the crosshair
    RaycastHit LaunchRaycast()
    {
        RaycastHit hit;
        Ray LandingRay = new Ray(gameObject.transform.parent.gameObject.transform.position, transform.forward);
        LandingRay.direction = LandingRay.direction;
        if (Physics.Raycast(LandingRay, out hit))
        {
            mIsObjectTargetted = true;

            //Help keeps track of the last object which was hovered over so that it may reset to original state after crosshair leaves it
            if (IsObject(hit.collider.tag))
            {
                //Deals with the case in which the crosshair hovers from one object to another so that the first object hit will reset upon hovering over second crosshair
                if (mLastObjectHit != null && mLastObjectHit.GetComponent<Collider>().tag != hit.collider.tag)
                {
                    mLastObjectHit.OnHoverEnd();
                }
                mLastObjectHit = hit.collider.gameObject.GetComponent<CrosshairListener>();
            }
            return hit;
        }
        mIsObjectTargetted = false;
        return hit;
    }

    //Determines all the crosshair actions, including clicking on the object and hovering over it.
    void CrosshairEvents()
    {
        CrosshairListener objectHit;
        
        //If the user is clicking on the object
        if (mIsObjectTargetted == true && Input.GetMouseButtonDown(0) && IsObject(mCurrentHit.collider.tag))
        {
            objectHit = mCurrentHit.collider.gameObject.GetComponent<CrosshairListener>();
            objectHit.OnClick();
        }

        //When the crosshair hovers above the object
        if (mIsObjectTargetted == true && IsObject(mCurrentHit.collider.tag))
        {
            objectHit = mCurrentHit.collider.gameObject.GetComponent<CrosshairListener>();
            objectHit.OnHoverStart();
        }

        //Resets object texture after crosshair leaves it
        if (mIsObjectTargetted  && mLastObjectHit != null && mLastObjectHit.GetComponent<Collider>().tag != mCurrentHit.collider.tag)
        {
            mLastObjectHit.OnHoverEnd();
        } 
    }

    //Sets the crosshair quad's depth dynamically
    void SetDynamicDepth(RaycastHit hit)
    {
        float distance = hit.distance;
        //Adjusts the distance so that at extremely close distance (directly touching object) it'll look approximately same size as in FIXED_DEPTH_MODE
        if (distance < 10.0f)
            distance *= 1 + 4.55f * Mathf.Exp(-distance);

        transform.localPosition = Vector3.forward * distance;
    }

    //Sets the crosshair quad to a fixed depth
    void SetFixedDepth()
    {
        transform.LookAt(CameraFacing.transform.position);
        transform.Rotate(0.0f, 180.0f, 0.0f);
        transform.position = CameraFacing.transform.position +
                    CameraFacing.transform.rotation * Vector3.forward * 2;
    }

    //Determines if the Raycasthit is an object
    bool IsObject(string tag)
    {
        if (tag == "Sphere" || tag == "Cube")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
