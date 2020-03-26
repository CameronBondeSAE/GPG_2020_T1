using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetworkBehaviour : MonoBehaviour
{
	public Light light;

	public CamMonster camMonster;

	// Start is called before the first frame update
	void Start()
	{
		camMonster = FindObjectOfType<CamMonster>();

		light = FindObjectOfType<Light>();
	}

	// Update is called once per frame
	void Update()
	{
	}
}