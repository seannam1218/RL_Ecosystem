using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGrid : MonoBehaviour
{
	[SerializeField] GameObject animal;
	Memory memory;
	private enum Object { food, rabbit, wolf, wall, bush, ground }
	[SerializeField]
	private Object RedItem;
	[SerializeField]
	private Object GreenItem;
	[SerializeField]
	private Object BlueItem;

	void Start() 
	{
		memory = transform.parent.parent.gameObject.GetComponent<Memory>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		DrawGrid();
	}

	public void DrawGrid() 
	{
		float[][] memoryArray0 = memory.memoryArray0;
		float[][] memoryArray1 = memory.memoryArray1;
		float[][] memoryArray2 = memory.memoryArray2;
		float[][] memoryArray3 = memory.memoryArray3;
		
		for (int i=0; i < memoryArray0.Length; i++) {
			// layer 0
			float red = memoryArray0[i][Utils.tagToIndex[RedItem.ToString()]];
			float green = memoryArray0[i][Utils.tagToIndex[GreenItem.ToString()]];
			float blue = memoryArray0[i][Utils.tagToIndex[BlueItem.ToString()]];
			int index = i + memoryArray1.Length + memoryArray2.Length + memoryArray3.Length;
			transform.GetChild(index).GetComponent<Image>().color = new Color(red, green, blue);
		}

		for (int i=0; i < memoryArray3.Length; i++) {
			// layer 1
			float red = memoryArray1[i][Utils.tagToIndex[RedItem.ToString()]];
			float green = memoryArray1[i][Utils.tagToIndex[GreenItem.ToString()]];
			float blue = memoryArray1[i][Utils.tagToIndex[BlueItem.ToString()]];
			int index = i + memoryArray2.Length + memoryArray3.Length;
			transform.GetChild(index).GetComponent<Image>().color = new Color(red, green, blue);

			// layer 2
			red = memoryArray2[i][Utils.tagToIndex[RedItem.ToString()]];
			green = memoryArray2[i][Utils.tagToIndex[GreenItem.ToString()]];
			blue = memoryArray2[i][Utils.tagToIndex[BlueItem.ToString()]];
			index = i + memoryArray3.Length;
			transform.GetChild(index).GetComponent<Image>().color = new Color(red, green, blue);

			// layer 3
			red = memoryArray3[i][Utils.tagToIndex[RedItem.ToString()]];
			green = memoryArray3[i][Utils.tagToIndex[GreenItem.ToString()]];
			blue = memoryArray3[i][Utils.tagToIndex[BlueItem.ToString()]];
			transform.GetChild(i).GetComponent<Image>().color = new Color(red, green, blue);
		}
	}
}
