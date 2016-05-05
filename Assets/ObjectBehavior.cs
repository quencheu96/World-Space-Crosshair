using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour {

    void OnEnable()
    {
        CrosshairListener objectCrosshairListener = gameObject.GetComponent<CrosshairListener>();
        objectCrosshairListener.OnClicked += IncreaseSize;
        objectCrosshairListener.OnHoverStarted += ChangeTexture;
        objectCrosshairListener.OnHoverEnded += ResetTexture;
    }

    void OnDisable()
    {
        CrosshairListener objectCrosshairListener = gameObject.GetComponent<CrosshairListener>();
        objectCrosshairListener.OnClicked -= IncreaseSize;
        objectCrosshairListener.OnHoverStarted -= ChangeTexture;
        objectCrosshairListener.OnHoverEnded -= ResetTexture;
    }

    //Increases the object's size if it is clicked on
    public void IncreaseSize()
    {
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }

    //Changes the look of the object if the crosshair goes over the object
    public void ChangeTexture()
    {
        Renderer rend = this.gameObject.GetComponent<Renderer>();
        rend.material.mainTexture = Resources.Load("leopard") as Texture;
    }

    //Changes the object's look back to normal after crosshair exits object
    public void ResetTexture()
    {
        Renderer rend = this.gameObject.GetComponent<Renderer>();
        rend.material.mainTexture = Resources.Load("cow-spot") as Texture;
    }
}
