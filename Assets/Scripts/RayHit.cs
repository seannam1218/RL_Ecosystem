using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHit
{
	public GameObject hitTarget;
	public float x;
	public float z;
	public float distance;
	public string tag;
	public int memoryChannelInd;
	public float foodAmount;	// receives food amount from Traits class (traitsDict)
	float[] traitsVector;
	float[] tagVector;
	int numChannels = 10;

	public float[] GetVisionArray(float maxDistance) 
	{
		tagVector = ConvertTagToVector(tag, 3, numChannels, maxDistance);
		traitsVector = new float[1]{foodAmount};	// insert traits here
		float[] combined = new float[tagVector.Length + traitsVector.Length];
		tagVector.CopyTo(combined, 0);
		traitsVector.CopyTo(combined, tagVector.Length);
		return combined;
	}


	// public float[] GetVisionArray_backup (float maxDistance) 
	// {
	// 	// tagVector = ConvertToVector(tag, 3, 10);
	// 	tagVector = ConvertTagToVector(tag, 3, numChannels, maxDistance);
	// 	traitsVector = new float[1]{foodAmount};	// insert traits here
	// 	float[] angle = new float[2]{x, y};
	// 	float[] combined = new float[angle.Length + tagVector.Length + traitsVector.Length];
	// 	angle.CopyTo(combined, 0);
	// 	tagVector.CopyTo(combined, angle.Length);
	// 	traitsVector.CopyTo(combined, angle.Length + tagVector.Length);
	// 	return combined;
	// }


	// Converts a string to a vector form of numChannels length.
	public float[] ConvertTagToVector(string s, int numSignals, int channels, float maxDistance) 
	{
		float[] vector = new float[channels];
		if (tag=="none") {
			return vector;
		}
		for (int i=0; i<numSignals; i++) {
			string input = i.ToString() + s;
			int hash = System.Math.Abs(input.GetHashCode());
			int location = hash % channels;
			float value = (hash % 100) * (maxDistance - distance) / maxDistance;
			vector[location] = value/100;
		}
		return vector;
	}

	// public float[] ConvertTraitsToVector(string s, int numSignals, int channels, float amount) 
	// {
	// 	float[] vector = new float[channels];
	// 	if (tag=="none") {
	// 		return vector;
	// 	}
	// 	for (int i=0; i<numSignals; i++) {
	// 		string input = i.ToString() + s;
	// 		int hash = System.Math.Abs(input.GetHashCode());
	// 		int location = hash % channels;
	// 		float value = (hash % 100) * amount;
	// 		vector[location] = value/100;
	// 	}
	// 	return vector;
	// }

	public string GetString() 
	{
		string tagVecString = "[";
		for (int i=0; i<tagVector.Length; i++) {
			tagVecString = tagVecString + ", " + tagVector[i].ToString();
		};
		tagVecString += "]";

		string foodAmountVecString = "[";
		for (int i=0; i<traitsVector.Length; i++) {
			foodAmountVecString = foodAmountVecString + ", " + traitsVector[i].ToString();
		};
		foodAmountVecString += "]";

		return(tag + ": " + tagVecString + foodAmountVecString);

	}
}