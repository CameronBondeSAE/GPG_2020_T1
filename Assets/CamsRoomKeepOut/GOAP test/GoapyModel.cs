using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a crappy test to manually calculate a new plan
/// </summary>
public class GoapyModel : MonoBehaviour
{
	GoapyAgent goapyAgent;

	// Start is called before the first frame update
    void Start()
    {
		goapyAgent = GetComponent<GoapyAgent>();
	}

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			goapyAgent.CalculateNewGoal(true);
		}
	}
}
