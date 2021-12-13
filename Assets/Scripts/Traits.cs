using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traits : MonoBehaviour
{
	[SerializeField] private Renderer model;
	private Color originalColor;
	public Dictionary<string, float> traitsDict = new Dictionary<string, float>();

	[System.NonSerialized] public float maxFoodAmount = 1f;
	[System.NonSerialized] public float curFoodAmount;
	[SerializeField] public float foodAmountIncrement; // 0.0006 is a good number
	

	[System.NonSerialized] public float maxAge = 1f;
	[System.NonSerialized] public float curAge;
	[SerializeField] public float ageIncrement;
	


	// Start is called before the first frame update
	void Start()
	{
		originalColor = model.material.color;

		curFoodAmount = maxFoodAmount;
		curAge = 0f;

		traitsDict.Add("food amount", curFoodAmount);
		traitsDict.Add("age", curAge);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		curFoodAmount += foodAmountIncrement;
		curFoodAmount = Math.Min(curFoodAmount, maxFoodAmount);
		// for debugging purposes, bushes lose color when eaten.
		model.material.color = new Color(1 - (1-originalColor.r)*curFoodAmount, 1 - (1-originalColor.g)*curFoodAmount, 1 - (1-originalColor.b)*curFoodAmount, originalColor.a);

		curAge += ageIncrement;
		curAge = Math.Min(curAge, maxAge);

		traitsDict["food amount"] = curFoodAmount;
		traitsDict["age"] = curAge;
	}

	public void DecrementFoodAmount() {
		curFoodAmount -= 20*foodAmountIncrement;
		curFoodAmount = Math.Max(curFoodAmount, 0);
	}
}
