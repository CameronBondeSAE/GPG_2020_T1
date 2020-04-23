using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PretendUnit : MonoBehaviour
{
	public ThreadTests threadTests;
	
	void Start()
	{
		threadTests = FindObjectOfType<ThreadTests>();

		// onCamsAction = Update;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			threadTests.RequestPathFind(Random.Range(2,10), OnCallBack);
		}
	}

	private void OnCallBack(string[] strings)
	{
		foreach (var s in strings)
		{
			Debug.Log("Main: "+s+": ID = "+Thread.CurrentThread.ManagedThreadId);
		}
		transform.Rotate(10f,400f,0);
	}
}