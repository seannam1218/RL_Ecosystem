using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCamController : MonoBehaviour
{
	[SerializeField] private Transform rabbit;

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector3(rabbit.position.x, rabbit.position.y + 40, rabbit.position.z);
		transform.LookAt(rabbit);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rabbit.localEulerAngles.y, transform.localEulerAngles.z);
	}

	
	
}
