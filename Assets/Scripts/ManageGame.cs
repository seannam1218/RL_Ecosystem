using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGame : MonoBehaviour
{
	[SerializeField] GameObject arena;
	[SerializeField] GameObject wall;
	float padding = 3f;
	//[SerializeField] GameObject memoryBeacon;
	//[SerializeField] float beaconGap;

	[SerializeField] GameObject food;
	[SerializeField] int numFoods;
	[SerializeField] GameObject bush;
	[SerializeField] int numBushes;
	[SerializeField] GameObject rabbit;
	[SerializeField] int numRabbits;
	private GameObject[] rabbits;

	//GameObject[][] beaconGrid;
	//private int beaconGridLength;
	//private int beaconGridWidth;
	//private float timer = 0f;
	//private float updateBeaconsTime = 2f;

	


	// Start is called before the first frame update
	void Start()
	{
		// Instantiate wall tiles
		float startX = -arena.transform.localScale.x / 2;
		float endX = arena.transform.localScale.x / 2;
		float startZ = -arena.transform.localScale.z / 2;
		float endZ = arena.transform.localScale.z / 2;
		float x = startX;
		float z = startZ;
		float gapx = wall.transform.localScale.x;
		float gapz = wall.transform.localScale.z;
		while (x < endX) {
			Instantiate(wall, new Vector3(x, 1f, startZ), Quaternion.Euler(new Vector3(0, 0, 0)));
			Instantiate(wall, new Vector3(x, 1f, -startZ), Quaternion.Euler(new Vector3(0, 0, 0)));
			x += gapx;
		}
		while (z < endZ) { 
			Instantiate(wall, new Vector3(startX, 1f, z), Quaternion.Euler(new Vector3(0, 0, 0)));
			Instantiate(wall, new Vector3(-startX, 1f, z), Quaternion.Euler(new Vector3(0, 0, 0)));
			z += gapz;
		}

		// Instantiate memory beacons
		//x = startX;
		//z = startZ;
		//beaconGridLength = (int)System.Math.Ceiling((arena.transform.localScale.x + 0.01) / beaconGap);
		//beaconGridWidth = (int)System.Math.Ceiling((arena.transform.localScale.z + 0.01) / beaconGap);
		//beaconGrid = new GameObject[beaconGridLength][];
		//if (beaconGap < 1) {
		//	Debug.Log("Error: gap value is not set properly");
		//	return;
		//}
		//int i = 0;
		//while (x < endX)
		//{
		//	z = startZ;
		//	int j = 0;
		//	beaconGridWidth = (int)System.Math.Ceiling((arena.transform.localScale.z + 0.01) / beaconGap);
		//	GameObject[] beaconArray = new GameObject[beaconGridWidth];
		//	while (z < endZ)
		//	{
		//		GameObject beacon = Instantiate(memoryBeacon, new Vector3(x, 5f, z), Quaternion.Euler(new Vector3(0, 0, 0)));
		//		beaconArray[j] = beacon;
		//		j++;
		//		z += beaconGap;
		//	}
		//	beaconGrid[i] = beaconArray;
		//	i++;
		//	x += beaconGap;
		//}


		// Instantiate food and water
		float boundaryX = arena.transform.localScale.x / 2 - padding;
		float boundaryZ = arena.transform.localScale.z / 2 - padding;
	
		for (int n=0; n < numFoods; n++) {
			float posX = Random.Range(-boundaryX + padding, boundaryX - padding); 
			float posZ = Random.Range(-boundaryZ + padding, boundaryZ - padding);
			float rotY = Random.Range(0f, 360f);
			Instantiate(food, new Vector3(posX, 1.2f, posZ), Quaternion.Euler(new Vector3(0, rotY, 0)));
		}

		for (int n = 0; n < numBushes; n++) {
			float posX = Random.Range(-boundaryX + padding, boundaryX - padding); 
			float posZ = Random.Range(-boundaryZ + padding, boundaryZ - padding);
			float rotY = Random.Range(0f, 360f);
			Instantiate(bush, new Vector3(posX, 0.5f, posZ), Quaternion.Euler(new Vector3(0, rotY, 0)));
		}

		rabbits = new GameObject[numRabbits];
		for (int n = 0; n < numRabbits; n++) {
			float posX = Random.Range(-boundaryX + padding, boundaryX - padding); 
			float posZ = Random.Range(-boundaryZ + padding, boundaryZ - padding);
			float rotY = Random.Range(0f, 360f);
			GameObject rabbitSpawn = Instantiate(rabbit, new Vector3(posX, 1, posZ), Quaternion.Euler(new Vector3(0, rotY, 0)));
			rabbits[n] = rabbitSpawn;
 		}
	}

	//public GameObject[] CheckBeaconGrid(Vector3 pos)
	//{
	//	GameObject[] ret = new GameObject[4];
	//	int indX = (int)System.Math.Floor((pos.x + arena.transform.localScale.x / 2) / beaconGap);
	//	int indZ = (int)System.Math.Floor((pos.z + arena.transform.localScale.z / 2) / beaconGap);
	//	if (indX < beaconGridLength && indZ < beaconGridWidth) {
	//		//Debug.Log("gameobject:" + beaconGrid[indX][indZ]);
	//		ret[0] = beaconGrid[indX][indZ];
	//	}
	//	if (indX + 1 < beaconGridLength && indZ < beaconGridWidth) {
	//		ret[1] = beaconGrid[indX + 1][indZ];
	//	}
	//	if (indX < beaconGridLength && indZ + 1 < beaconGridWidth) {
	//		ret[2] = beaconGrid[indX][indZ + 1];
	//	}
	//	if (indX + 1 < beaconGridLength && indZ + 1 < beaconGridWidth) {
	//		ret[3] = beaconGrid[indX + 1][indZ + 1];
	//	}

	//	return ret;
	//}

	private void FixedUpdate()
    {
		//timer += Time.deltaTime;
		//if (timer > updateBeaconsTime) {
		//	for (int i=0; i<rabbits.Length; i++) {
		//		Memory rabbitMemory = rabbits[i].transform.GetChild(1).GetComponent<Memory>();
		//		//rabbitMemory.UpdateBeacons(CheckBeaconGrid(rabbits[i].transform.position));
  //          }
		//	timer = 0f;
  //      }
    }

}
