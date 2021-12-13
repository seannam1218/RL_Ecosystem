using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class RabbitAgent : Agent
{
	private enum UseVision {Observe, LeaveBlank, Disuse}
	[SerializeField]
	private UseVision useVision;

	private enum UseMemory {Use, Disuse}
	[SerializeField] 
	private UseMemory useMemory;

	[SerializeField] private float moveSpeed;
	[SerializeField] private float turnSpeed;
	// Vision variables
	[SerializeField] private int visionRayCount;	// must be an even number
	[SerializeField] private float visionMaxDistance;
	[SerializeField] private int decisionPeriod;
	[SerializeField] private GameObject memory;

	[SerializeField] private GameObject arena;
	private float boundaryX;
	private float boundaryZ;
	private float padding = 2f;

	public System.Random r;


	//STATUS
	private float rotY;
	public float saveRotAction;
	public Vector3 saveMoveAction;

	private static float maxFullness = 100f;
	public float curFullness = maxFullness;
	private float deltaFullness = 0f;
	private float DEFAULT_FULLNESS_DECREMENT = -0.2f;


	//REWARDS
	//private float RWD_WALLHIT = -300f;
	private float RWD_STARVE = -300f;
	private float RWD_EAT = 1f;
	private float RWD_FULLNESS = 3f;
	private float RWD_MOVEMENT = -0.1f;
	
	void Start() 
	{
		r = new System.Random();
		boundaryX = arena.transform.position.x + arena.transform.localScale.x/2 - padding;
		boundaryZ = arena.transform.position.y + arena.transform.localScale.z/2 - padding;
	}

	// void OnTriggerEnter(Collider other) 
	// {
	// 	if (other.gameObject.CompareTag("wall")) {
	// 		AddReward(RWD_WALLHIT);
	// 		EndEpisode();
	// 	} 
	// }

	void FixedUpdate() 
	{
		// finish episode when rabbit starves
		if (curFullness <= 0) {
			AddReward(RWD_STARVE);
			EndEpisode();
		}
		if (transform.position.y < -4f) {
			AddReward(RWD_STARVE*2);
			EndEpisode();
		}
	}

	public override void OnEpisodeBegin() 
	{
		float posX = UnityEngine.Random.Range(-boundaryX, boundaryX); 
		float posZ = UnityEngine.Random.Range(-boundaryZ, boundaryZ);
		float rotY = UnityEngine.Random.Range(0f, 360f);
		transform.position = new Vector3(posX, 1, posZ);
		transform.rotation = Quaternion.Euler(new Vector3(0, rotY, 0));
		
		memory.GetComponent<Memory>().ClearMemory();
		curFullness = maxFullness;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		deltaFullness = DEFAULT_FULLNESS_DECREMENT * decisionPeriod / 5; // initialize to default curFullness decrement

		// vision observation
		int countRaysCloseToFood = 0;
		GameObject hitFood = null;
		Vector3[] rayDirections = Utils.GetRayAngles(1, 70, visionRayCount, transform.eulerAngles.y);

		// have two numbers iterating in opposite directions. 
		int j = visionRayCount - 1;
		for (int i = 0; i < visionRayCount / 2; i++)
		{
			RayHit rayHit1 = ShootRay(rayDirections[i]);
			if (useMemory == UseMemory.Use && rayHit1.hitTarget != null) {
				memory.GetComponent<Memory>().CommitToMemory(rayHit1);
			}
			RayHit rayHit2 = ShootRay(rayDirections[j]);
			if (useMemory == UseMemory.Use && rayHit2.hitTarget != null) {
				memory.GetComponent<Memory>().CommitToMemory(rayHit2);
			}

			float[] rayHitArray1 = rayHit1.GetVisionArray(visionMaxDistance);
			float[] rayHitArray2 = rayHit2.GetVisionArray(visionMaxDistance);
			if (useVision == UseVision.Observe) {
				sensor.AddObservation(rayHitArray1);
				sensor.AddObservation(rayHitArray2);
			} else if (useVision == UseVision.LeaveBlank) {
				float[] blank = new float[rayHitArray1.Length];
				sensor.AddObservation(blank);
				sensor.AddObservation(blank);
			}

			// count how many rays are hitting food near.
			if (rayHit1.tag == "food" && rayHit1.distance < 4) {
				countRaysCloseToFood += 1;
				hitFood = rayHit1.hitTarget;
			}
			if (rayHit2.tag == "food" && rayHit2.distance < 4) {
				countRaysCloseToFood += 1;
				hitFood = rayHit2.hitTarget;
			}

			j--;
		}


		// memory observation
		//Debug.Log("count:" + memory.GetComponent<Memory>().rememberedThings.Count);
		if (useMemory == UseMemory.Use) {
			float[][] memoryArray = memory.GetComponent<Memory>().GetMemoryArray();
			for (int i = 0; i < memoryArray.Length; i++) {
				sensor.AddObservation(memoryArray[i]);
			}
			//memory.GetComponent<Memory>().DebugMemory();
		}

		// action observation
		sensor.AddObservation(saveMoveAction.x);
		sensor.AddObservation(saveMoveAction.z);
		sensor.AddObservation(saveRotAction);
		// ApplyMovementPunishment();


		// status observation update
		if (IsEatingFood(countRaysCloseToFood)) {
			sensor.AddObservation(hitFood.GetComponent<Traits>().curFoodAmount);
			// ApplyEatFoodReward(sensor);
			hitFood.GetComponent<Traits>().DecrementFoodAmount();
			if (hitFood.GetComponent<Traits>().curFoodAmount > 0.15) {
				deltaFullness += 2f * decisionPeriod / 5;
				AddReward(RWD_EAT * decisionPeriod / 5);
			}
		} else {
			sensor.AddObservation(0f);
		}

		//directional embedding. rotation.y ranges from [-1, 1]
		float y = transform.localEulerAngles.y;
		float sin = (float) Math.Sin(y * Math.PI / 180);
		float cos = (float) Math.Cos(y * Math.PI / 180);
		float[] dirEmbed = new float[] {sin, cos};
		sensor.AddObservation(dirEmbed);


		curFullness += deltaFullness;
		curFullness = Math.Min(maxFullness, curFullness);
		sensor.AddObservation(curFullness);
		sensor.AddObservation(deltaFullness);

		float rewardFullness = ((curFullness-maxFullness/4)*RWD_FULLNESS / maxFullness) * decisionPeriod/5;
		AddReward(rewardFullness);
	}

	private RayHit ShootRay(Vector3 rayDirection)
	{
		RaycastHit hit;
		RayHit rayHit = new RayHit();
		if (Physics.Raycast(transform.position, rayDirection, out hit, visionMaxDistance))
		{
			// create a rayhit object that stores direction of the ray, distance, and tag information.
			rayHit.hitTarget = hit.transform.gameObject;
			rayHit.x = hit.point.x;
			rayHit.z = hit.point.z;
			rayHit.distance = hit.distance;
			rayHit.tag = hit.transform.tag;
			//rayHit.memoryChannelInd = Utils.tagToIndex[hit.transform.tag];
			rayHit.foodAmount = (hit.transform.tag == "food"? 
								hit.transform.gameObject.GetComponent<Traits>().traitsDict["food amount"]:
								0f);
			DebugRays(rayHit.tag, rayDirection, rayHit.distance);
		}
		else {	// when raycast hit nothing
			rayHit.distance = visionMaxDistance;
			rayHit.tag = "none";
			rayHit.foodAmount = 0f;
		}
		return rayHit;
	}
	

	private void DebugRays(string targetTag, Vector3 rayDirection, float distance) {
		if (targetTag == "food") {
			Debug.DrawRay(transform.position, rayDirection*distance, Color.red);
		}
		else {	// when raycast hit something other than food
			// Debug.DrawRay(transform.position, rayDirection * visionMaxDistance, Color.black);
		}
	}

	private bool IsEatingFood(int countRays) 
	{
		if (countRays > 2 && saveMoveAction == Vector3.zero && saveRotAction == 0f) {
			return true;
		}
		return false;
	}

	public override void OnActionReceived(ActionBuffers actions) 
	{
/*		Vector3 moveX = transform.forward * (((actions.ContinuousActions[0] + 1f) / 2f) * 1.3f - 0.3f);	// maps to [-0.3, 1]
		Vector3 moveZ = transform.right * (actions.ContinuousActions[1] / 2f);
		saveMoveAction = moveX + moveZ;
		transform.position += saveMoveAction*moveSpeed*Time.deltaTime;

		saveRotAction = actions.ContinuousActions[2];
		rotY += saveRotAction*turnSpeed*Time.deltaTime;
		transform.localEulerAngles = new Vector3(0, rotY, 0);*/

		int moveAction = actions.DiscreteActions[0];
		if (moveAction == 0) {
			// move forward
			saveMoveAction = transform.forward * moveSpeed * Time.deltaTime;
		} else if (moveAction == 1) {
			// move right
			saveMoveAction = transform.right * moveSpeed/2 * Time.deltaTime;
		} else if (moveAction == 2) {
			// move left
			saveMoveAction = -transform.right * moveSpeed/2 * Time.deltaTime;
		} else if (moveAction == 3) {
			// stay still
			saveMoveAction = Vector3.zero;
		}
		transform.position += saveMoveAction;

		int turnAction = actions.DiscreteActions[1];
		if (turnAction == 0) {
			// turn right
			saveRotAction = turnSpeed * Time.deltaTime;
		} else if (turnAction == 1) {
			// turn left
			saveRotAction = -turnSpeed * Time.deltaTime;
		} else if (turnAction == 2) {
			saveRotAction = 0f;
		}
		rotY += saveRotAction;
		transform.localEulerAngles = new Vector3(0, rotY, 0);
	}


/*	public override void Heuristic(in ActionBuffers actionsOut) 
	{
		Debug.Log(String.Join(", ", actionsOut.DiscreteActions));
		if (Input.GetKey(KeyCode.UpArrow))
		{
			actionsOut.DiscreteActions = [0,0];
		}
	}*/

	//public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
	//{
	//	actionMask.SetActionEnabled(branch, actionIndex, isEnabled);
	//}


	/*private void ApplyMovementPunishment()
	{
		if (saveMoveAction.magnitude > 0 || saveRotAction > 0) {
			// curFullness = curFullness - (saveMoveAction.magnitude + Math.Abs(saveRotAction))*0.05f;
			AddReward(RWD_MOVEMENT * (saveMoveAction.magnitude + Math.Abs(saveRotAction)));
		}
	}*/
}
