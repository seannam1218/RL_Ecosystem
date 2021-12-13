using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memory : MonoBehaviour
{
	int gridCount = 12;
	int gridCount0 = 4;
	GameObject parent;
	//GameObject beaconsManager;
	public Dictionary<GameObject, float[]> rememberedThings;

	private int POSX_IND = 0;
	private int POSZ_IND = 1;
	private int TAG = 2;
	private int STR = 3;

	public float[][] memoryArray0;
	public float[][] memoryArray1;
	public float[][] memoryArray2;
	public float[][] memoryArray3;
	private int cutoff0 = 4;
	private int cutoff1 = 12;
	private int cutoff2 = 22;
	private int cutoff3 = 40;

	void Start() 
	{ 
		parent = transform.parent.gameObject;
		//beaconsManager = parent.GetComponent<RabbitAgent>().beaconsManager;
		rememberedThings = new Dictionary<GameObject, float[]>();
		memoryArray0 = new float[gridCount0][];
		for (int i=0; i < gridCount0; i++) {
			memoryArray0[i] = new float[Utils.numChannels];
		}
		memoryArray1 = new float[gridCount][];
		for (int i=0; i < gridCount; i++) {
			memoryArray1[i] = new float[Utils.numChannels];
		}
		memoryArray2 = new float[gridCount][];
		for (int i=0; i < gridCount; i++) {
			memoryArray2[i] = new float[Utils.numChannels];
		}
		memoryArray3 = new float[gridCount][];
		for (int i=0; i < gridCount; i++) {
			memoryArray3[i] = new float[Utils.numChannels];
		}
	}

	public void ClearMemory() 
	{
		rememberedThings = new Dictionary<GameObject, float[]>();
	}

	public void CommitToMemory(RayHit rayHit)
	{
		if (rememberedThings.ContainsKey(rayHit.hitTarget)) {
			rememberedThings[rayHit.hitTarget][POSX_IND] = rayHit.x;
			rememberedThings[rayHit.hitTarget][POSZ_IND] = rayHit.z;
			rememberedThings[rayHit.hitTarget][STR] += 0.03f;
			rememberedThings[rayHit.hitTarget][STR] = Math.Min(1f, rememberedThings[rayHit.hitTarget][STR]);
		} else {
			float[] content = new float[4]; // xpos, ypos, tag, memory strength
			content[POSX_IND] = rayHit.x;
			content[POSZ_IND] = rayHit.z;
			content[TAG] = Utils.tagToIndex[rayHit.hitTarget.tag];
			content[STR] = 0.2f;
			rememberedThings.Add(rayHit.hitTarget, content);
		}
	}

	public void UpdateMemoryArray()
	{
		// clear memoryArrays
		for (int j=0; j < gridCount0; j++) {
			memoryArray0[j] = new float[Utils.numChannels];
		}
		for (int i=0; i < gridCount; i++) {
			memoryArray1[i] = new float[Utils.numChannels];
			memoryArray2[i] = new float[Utils.numChannels];
			memoryArray3[i] = new float[Utils.numChannels];
		}

		foreach (KeyValuePair<GameObject, float[]> item in rememberedThings)
		{
			GameObject parent = transform.parent.gameObject;
			Vector3 itemPos = new Vector3(item.Value[POSX_IND], 1, item.Value[POSZ_IND]);
			Vector3 targetDir = itemPos - parent.transform.position;
			float angle = Vector3.SignedAngle(parent.transform.forward, targetDir, Vector3.up);
			if (angle < 0) {
				angle = angle + 360;
			}
			//Debug.Log("angle:" + angle);

			float distance = Vector3.Distance(parent.transform.position, itemPos);
			int hour = (int)System.Math.Floor(System.Math.Abs(angle + 15) / 30) % 12;
			int quarter = (int)System.Math.Floor(System.Math.Abs(angle + 45) / 90) % 4;
			float[] newArray = new float[Utils.numChannels];
			newArray[(int) item.Value[TAG]] = item.Value[STR];

			if (distance > 0 && distance <= cutoff0) {
				memoryArray0[quarter] = (float[]) memoryArray0[quarter].Zip(newArray, (x,y) => x + y).ToArray();
			} else if (distance > cutoff0 && distance <= cutoff1) {
				memoryArray1[hour] = (float[]) memoryArray1[hour].Zip(newArray, (x,y) => x + y).ToArray();
			} else if (distance > cutoff1 && distance <= cutoff2) {
				memoryArray2[hour] = (float[]) memoryArray2[hour].Zip(newArray, (x,y) => x + y).ToArray();
			} else if (distance > cutoff2 && distance <= cutoff3) {
				memoryArray3[hour] = (float[]) memoryArray3[hour].Zip(newArray, (x,y) => x + y).ToArray();
			}
		}

		// log the addition of items.
		for (int j = 0; j < gridCount0; j++) {
			memoryArray0[j] = (float[]) memoryArray0[j].Select(x => (float) Math.Log10(x + 1f) * 1.8f).ToArray();
		}
		for (int i = 0; i < gridCount; i++)	{
			memoryArray1[i] = (float[]) memoryArray1[i].Select(x => (float)Math.Log10(x + 1f) * 1.8f).ToArray();
			memoryArray2[i] = (float[]) memoryArray2[i].Select(x => (float)Math.Log10(x + 1f) * 1.8f).ToArray();
			memoryArray3[i] = (float[]) memoryArray3[i].Select(x => (float)Math.Log10(x + 1f) * 1.8f).ToArray();
		}
	}

	void FixedUpdate() 
	{
		// decrement memory strength for all items in the list and clean up.
		ArrayList toRemove = new ArrayList();
		foreach(KeyValuePair<GameObject, float[]> item in rememberedThings) {
			//Debug.Log(String.Join<float>(",", item.Value));
			float decayRate = Utils.tagToDecayRate[Utils.indexToTag[(int)item.Value[TAG]]];
			float newValue = item.Value[STR] * decayRate - 0.0005f;
			rememberedThings[item.Key][STR] = newValue;
			if (newValue < 0.05f) {
				toRemove.Add(item.Key);
			}
		}
		for (int i=0; i < toRemove.Count; i++)	{
			rememberedThings.Remove((GameObject) toRemove[i]);
		}
		UpdateMemoryArray();
	}

	//public void UpdateBeacons(GameObject[] nearbyBeacons)
	//   {
	//	//update beacons
	//	for (int i = 0; i < nearbyBeacons.Length; i++) {
	//		if (nearbyBeacons[i] == null) { continue; }
	//		// TODO: need to rebuild the memory content array. currently, the beacons default to the wall channel.
	//		float[] content = new float[Utils.numChannels + 2] { nearbyBeacons[i].transform.position.x, nearbyBeacons[i].transform.position.z, 0f,0f,0f,0f,0f };
	//		if (!rememberedThings.ContainsKey(nearbyBeacons[i])) {
	//			rememberedThings.Add(nearbyBeacons[i], content);
	//		}
	//	}
	//}

	public float[][] GetMemoryArray() 
	{
		//Debug.Log(string.Join(", ", memoryArray0));
		float[][] ret = new float[memoryArray0.Length + memoryArray1.Length + memoryArray2.Length + memoryArray3.Length][];
		memoryArray0.CopyTo(ret, 0);
		memoryArray1.CopyTo(ret, memoryArray0.Length);
		memoryArray2.CopyTo(ret, memoryArray0.Length + memoryArray1.Length);
		memoryArray3.CopyTo(ret, memoryArray0.Length + memoryArray1.Length + memoryArray2.Length);
		return ret;
	}

	public void DebugMemory()
	{
		foreach(KeyValuePair<GameObject, float[]> item in rememberedThings)
		{
			Debug.Log("Key:" + item.Key + "Value:" + string.Join(",", item.Value));
		}
	}
}
