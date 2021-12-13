using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFullnessBar : MonoBehaviour
{
	Slider slider;
	[SerializeField] GameObject rabbit;

	void Start() 
	{
		slider = gameObject.GetComponent<Slider>();
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		slider.value = rabbit.GetComponent<RabbitAgent>().curFullness;
	}
}
