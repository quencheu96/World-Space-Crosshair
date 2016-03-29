using UnityEngine;
using System.Collections;

public class TargettingBehavior : MonoBehaviour {
    private const int FIXED_DEPTH_CROSSHAIR_TYPE = 0;
    private const int DYNAMIC_CROSSHAIR_TYPE = 1;
    private int crosshairType = 0;
    public float height;
    public bool isHoveringOverObject = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray LandingRay = new Ray(gameObject.transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Renderer rend = this.gameObject.GetComponent<Renderer>();

        if (Physics.Raycast(LandingRay,out hit))
        {
            Debug.Log("Tag: " + hit.collider.tag);
            if(hit.collider.tag == "Sphere")
            {
                hit.transform.SendMessage("HitByRay");
                isHoveringOverObject = true;
                rend.material.mainTexture = Resources.Load("Aiming2") as Texture;           
            }
            else if (hit.collider.tag != "Untagged")
            {
                isHoveringOverObject = false;
                rend.material.mainTexture = Resources.Load("Aiming1") as Texture;
            }
        }
        else
        {
            rend.material.mainTexture = Resources.Load("Aiming1") as Texture;

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
           if (crosshairType == FIXED_DEPTH_CROSSHAIR_TYPE)
           {
               crosshairType = DYNAMIC_CROSSHAIR_TYPE;
           }
           else
           {
                crosshairType = FIXED_DEPTH_CROSSHAIR_TYPE;
           }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(LandingRay, out hit))
            {
                if (hit.collider.tag == "Sphere")
                {
                    hit.transform.SendMessage("HitByClickedRay");
                    isHoveringOverObject = true;
                }
            }
        }
    }

    void OnGUI()
    {
        if (crosshairType == DYNAMIC_CROSSHAIR_TYPE)
        {
            float width = 50f;
            float height = 50f;
            Texture myTexture;
            if (!isHoveringOverObject)
            {
                myTexture = Resources.Load("Aiming1") as Texture;
            }
            else
            {
                myTexture = Resources.Load("Aiming2") as Texture;
            }


            GUI.DrawTexture(new Rect((Screen.width / 2) - (width/2), (Screen.height / 2) - (height/2) , width,height), myTexture);
        }
        
    }
}
