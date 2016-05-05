using UnityEngine;
using System.Collections;

public class CrosshairListener : MonoBehaviour {

    public delegate void ClickAction();
    public event ClickAction OnClicked;

    public delegate void OnHoverStartAction();
    public event OnHoverStartAction OnHoverStarted;

    public delegate void OnHoverEndAction();
    public event OnHoverEndAction OnHoverEnded;

	public void OnClick()
    {
        OnClicked();
    }

    public void OnHoverStart()
    {
        OnHoverStarted();
    }

    public void OnHoverEnd()
    {
        OnHoverEnded();
    }
}
