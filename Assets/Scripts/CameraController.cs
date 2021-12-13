using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float scrollSpeed;
	[SerializeField] private float turnSensitivity;

	private float minY = -60f;
	private float maxY = 60f;
	private float rotY = 0f;
	private float rotX = 0f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		//update the position
		Vector3 forward = Input.GetAxis("Vertical")*(transform.forward-new Vector3(0, transform.forward.y)).normalized*movementSpeed*Time.deltaTime;
		Vector3 right = Input.GetAxis("Horizontal")*transform.right*movementSpeed*Time.deltaTime;
		Vector3 up = new Vector3(0, Input.mouseScrollDelta.y*scrollSpeed*Time.deltaTime, 0);
		transform.position += forward + right + up;
		
		//update the rotation
		rotX += Input.GetAxis("Mouse X")*turnSensitivity*100*Time.deltaTime;
		rotY += Input.GetAxis("Mouse Y")*turnSensitivity*100*Time.deltaTime;
		rotY = Mathf.Clamp(rotY, minY, maxY);
		transform.localEulerAngles = new Vector3(-rotY, rotX, 0);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (Cursor.visible && Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	
	
}
