using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public delegate void CamsCallBackDelegate(string[] strings);

public class ThreadTests : MonoBehaviour
{
	public async void RequestPathFind(int rnd, CamsCallBackDelegate callback)
	{
		// Manually create the delegate (Func is just a convenience wrapper around a normal delegate). Just like Action, but with a return type 
		// var function = new Func<string[]>(() => DoSlowStuff(rnd));
		string[] res = await Task.Run<string[]>(()=>DoSlowStuff(rnd));
		callback(res);
	}

	public string[] DoSlowStuff(int rnd)
	{
		Debug.Log("Start : Thread ID = " + Thread.CurrentThread.ManagedThreadId);
		Thread.Sleep(rnd * 1000);
		Debug.Log("Done");

		return new string[] {"ARSE:" + rnd, "Cam", "THINGS"};
	}
}