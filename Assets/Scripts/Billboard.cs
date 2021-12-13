using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera cam;

	// Update is called once per frame
	void FixedUpdate()
	{
		cam = Camera.main;
		transform.LookAt(cam.transform.position);
	}
}
