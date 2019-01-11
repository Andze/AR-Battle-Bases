using System;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class CustomVuforiaButton : MonoBehaviour, IVirtualButtonEventHandler
{

    public VirtualButtonBehaviour Button_Ref;

    public UnityEvent onPress;
	public UnityEvent onHold;
	public UnityEvent onRelease;

	bool isPressed;

	void Start()
	{
        Button_Ref = GetComponent<VirtualButtonBehaviour>();
        Button_Ref.RegisterEventHandler(this);
	}

	void Update()
	{
		if (isPressed)
			onHold.Invoke();
	}

	public void OnButtonPressed(VirtualButtonBehaviour vb)
	{
		//if (onPress != null)
			onPress.Invoke();
		isPressed = true;
	}

	public void OnButtonReleased(VirtualButtonBehaviour vb)
	{
		//if (onRelease != null)
			onRelease.Invoke();
		isPressed = false;
	}
}
