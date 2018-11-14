using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    public event EventHandler OnChange;

    private Slider slider;

    public float Value
    {
        get { return slider.value; }
        set { slider.value = value; }
    }

	void Start()
    {
        slider = GetComponent<Slider>();
	}
	
	public void OnValueChanged()
    {
        if (OnChange != null)
        {
            OnChange(this, new EventArgs());
        }
    }
}
