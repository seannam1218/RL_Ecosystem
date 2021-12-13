using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static double SampleGaussian(System.Random r, double mean, double stddev)
	{
		// The method requires sampling from a uniform random of (0,1]
		// but Random.NextDouble() returns a sample of [0,1).
		double x1 = 1 - r.NextDouble();
		double x2 = 1 - r.NextDouble();

		double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
		return y1 * stddev + mean;
	}

	public static Vector3[] GetRayAngles(float initInterval, float maxInterval, int numRays, float curRotation)
	{
		Vector3[] ret = new Vector3[numRays];
		float b = (float)Math.Pow(maxInterval / initInterval, (double)1 / (numRays / 2 - 1));

		float min = 0;
		float max = initInterval;
		for (int i = 0; i < numRays / 2; i++) {
			float positiveAngle = (float)(Math.PI / 180) * (curRotation + UnityEngine.Random.Range(min, max));
			Vector3 rightDirVector = new Vector3(Mathf.Sin(positiveAngle), 0f, Mathf.Cos(positiveAngle));

			float negativeAngle = (float)(Math.PI / 180) * (curRotation - UnityEngine.Random.Range(min, max));
			Vector3 leftDirVector = new Vector3(Mathf.Sin(negativeAngle), 0f, Mathf.Cos(negativeAngle));

			ret[numRays / 2 + i] = leftDirVector.normalized;
			ret[numRays / 2 - i - 1] = rightDirVector.normalized;
			min = max;
			max = max * (float)b;
		}

		return ret;
	}


	// memory related variables and methods.
/*	public static Dictionary<string, int> memoryContentChannelsLookUp = new Dictionary<string, int>() {
		{"food", 0},
		{"friend", 1},
		{"enemy", 2},
		{"transparent", 3},
		{"solid", 4},
		{"ground", 5}
	};*/

	//public static int numChannels = memoryContentChannelsLookUp.Count;

/*	public static Dictionary<string, int> tagToIndex = new Dictionary<string, int>() { 
		{"food", memoryContentChannelsLookUp["food"]},
		{"rabbit", memoryContentChannelsLookUp["friend"]},
		{"wolf", memoryContentChannelsLookUp["enemy"]},
		{"bush", memoryContentChannelsLookUp["transparent"]},
		{"wall", memoryContentChannelsLookUp["solid"]},
		{"ground", memoryContentChannelsLookUp["ground"]}
	};*/

	public static Dictionary<string, int> tagToIndex = new Dictionary<string, int>() {
		{"food", 0},
		{"rabbit", 1},
		{"wolf", 2},
		{"bush", 3},
		{"wall", 4},
		{"ground", 5}
	};

	public static int numChannels = tagToIndex.Count;

	public static Dictionary<int, string> indexToTag = tagToIndex.ToDictionary(kp => kp.Value, kp => kp.Key);

	public static Dictionary<string, float> tagToDecayRate = new Dictionary<string, float>() {
		{"food", 0.999f},
		{"rabbit", 0.994f},
		{"wolf", 0.994f},
		{"bush", 0.999f},
		{"wall", 0.999f},
		{"ground", 0.999f}
	};

}

