using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour {

    //Changes the look of the object if the crosshair goes over the object
    void OnMouseOver()
    {
        Renderer rend = this.gameObject.GetComponent<Renderer>();
        rend.material.mainTexture = Resources.Load("leopard") as Texture;
    }

    //Changes the object's look back to normal after crosshair exits object
    void OnMouseExit()
    {
        Renderer rend = this.gameObject.GetComponent<Renderer>();
        rend.material.mainTexture = Resources.Load("cow-spot") as Texture;
    }

    //Increases the object's size after clicking on it
    void OnMouseDown()
    {
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }
}
