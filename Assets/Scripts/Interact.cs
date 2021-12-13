using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: remove this script

public class Interact : MonoBehaviour
{
	[SerializeField] GameObject rabbit;
	public float saveRotAction;
	public Vector3 saveMoveAction;


	void OnTriggerStay(Collider other)
	{
		Debug.Log(gameObject.tag);
		Debug.Log("triggerd " + other.gameObject.tag);
		if (other.gameObject.CompareTag("food")) {
			Debug.Log("triggerd - tag is food");
		}
		if (other.gameObject.CompareTag("food") && saveMoveAction.magnitude == 0 && saveRotAction == 0) {
			Debug.Log("triggerd and no movement");
			other.gameObject.GetComponent<Traits>().DecrementFoodAmount();
		}
	}

	void FixedUpdate() {
		saveMoveAction = rabbit.GetComponent<RabbitAgent>().saveMoveAction;
		saveRotAction = rabbit.GetComponent<RabbitAgent>().saveRotAction;
	}
}
