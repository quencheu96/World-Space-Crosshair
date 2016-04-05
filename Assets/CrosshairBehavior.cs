using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrosshairBehavior : MonoBehaviour
{
    public Camera CameraFacing;
    public int DYNAMIC_MODE;
    public int FIXED_DEPTH_MODE;
    public int mCameraMode;
    public bool mIsObjectTargetted;
    ObjectBehavior mLastObjectHit;

    //Sets the default mode to FIXED_DEPTH_MODE
    void Start()
    {
        DYNAMIC_MODE = 0;
        FIXED_DEPTH_MODE = 1;
        mCameraMode = FIXED_DEPTH_MODE;
        mIsObjectTargetted = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckModeToggle();
        SetCrosshairPosition();
        CrosshairEvents();
    }

    //Use T to toggle between FIXED_DEPTH_MODE and DYNAMIC_MODE
    void CheckModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (mCameraMode == FIXED_DEPTH_MODE)
            {
                mCameraMode = DYNAMIC_MODE;
            }
            else
            {
                mCameraMode = FIXED_DEPTH_MODE;
            }
        }
    }

    //Sets the crosshair to the appropiate depth
    void SetCrosshairPosition()
    {
        //Sets crosshair quad to always face towards camera and fixes the depth
        if (mCameraMode == FIXED_DEPTH_MODE)
        {
            SetFixedDepth();
        }
        //Determines distance from object and renders crosshair quad at that distance
        else if (mCameraMode == DYNAMIC_MODE)
        {
            RaycastHit hit = LaunchRaycast();

            //If object is untagged, it defaults to the Fixed depth
            if (mIsObjectTargetted == true && hit.collider.tag != "Untagged")
            {
                SetDynamicDepth(hit);
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
            if (hit.collider.tag == "Object")
            {
                mLastObjectHit = hit.collider.gameObject.GetComponent<ObjectBehavior>();
            }
            return hit;
        }
        mIsObjectTargetted = false;
        return hit;
    }

    //Determines all the crosshair actions, including clicking on the object and hovering over it.
    void CrosshairEvents()
    {
        RaycastHit hit = LaunchRaycast();
        ObjectBehavior objectHit;
        
        //If the user is clicking on the object
        if (mIsObjectTargetted == true && Input.GetMouseButtonDown(0) && hit.collider.tag == "Object")
        {
            objectHit = hit.collider.gameObject.GetComponent<ObjectBehavior>();
            objectHit.IncreaseSize();
        }

        //When the crosshair hovers above the object
        if (mIsObjectTargetted == true && hit.collider.tag == "Object")
        {
            objectHit = hit.collider.gameObject.GetComponent<ObjectBehavior>();
            objectHit.ChangeTexture();
        }

        //Resets object texture after crosshair leaves it
        if (mIsObjectTargetted && hit.collider.tag != "Object" && mLastObjectHit != null && mLastObjectHit.tag != hit.collider.tag)
        {
            mLastObjectHit.ResetTexture();
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

}
