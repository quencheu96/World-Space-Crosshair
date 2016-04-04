using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrosshairBehavior : MonoBehaviour
{
    public Camera CameraFacing;
    public int DYNAMIC_MODE;
    public int FIXED_DEPTH_MODE;
    public int cameraMode;
    public bool isObjectTargetted;

    //Sets the default mode to FIXED_DEPTH_MODE
    void Start()
    {
        DYNAMIC_MODE = 0;
        FIXED_DEPTH_MODE = 1;
        cameraMode = FIXED_DEPTH_MODE;
        isObjectTargetted = false;
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
            if (cameraMode == FIXED_DEPTH_MODE)
            {
                cameraMode = DYNAMIC_MODE;
            }
            else
            {
                cameraMode = FIXED_DEPTH_MODE;
            }
        }
    }

    //Sets the crosshair to the appropiate depth
    void SetCrosshairPosition()
    {
        //Sets crosshair quad to always face towards camera and fixes the depth
        if (cameraMode == FIXED_DEPTH_MODE)
        {
            SetFixedDepth();
        }
        //Determines distance from object and renders crosshair quad at that distance
        else if (cameraMode == DYNAMIC_MODE)
        {
            RaycastHit hit = LaunchRaycast();

            //If object is untagged, it defaults to the F
            if (isObjectTargetted == true && hit.collider.tag != "Untagged")
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
            isObjectTargetted = true;
            return hit;
        }
        isObjectTargetted = false;
        return hit;
    }

    //Determines all the crosshair actions, including clicking on the object and hovering over it.
    void CrosshairEvents()
    {
        RaycastHit hit = LaunchRaycast();       
        if (isObjectTargetted == true && Input.GetMouseButtonDown(0) && hit.collider.tag == "Object")
        {
            ObjectBehavior objectHit = hit.collider.gameObject.GetComponent<ObjectBehavior>();
            objectHit.IncreaseSize();
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
