using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomTester : NetworkBehaviour
{
	// public override void OnStartServer()
	// {
	//     base.OnStartServer();
	private void Start()
	{
		color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

		if (isServer)
		{
			InvokeRepeating("RandomColour", 3f, 3f);
		}
	}

	public void RandomColour()
	{
		color = Random.ColorHSV();
	}

	// Color32 packs to 4 bytes
	[SyncVar(hook = nameof(SetColor))]
	public Color32 color = Color.black;

	// Unity clones the material when GetComponent<Renderer>().material is called
	// Cache it here and destroy it in OnDestroy to prevent a memory leak
	Material cachedMaterial;

	void SetColor(Color32 oldColor, Color32 newColor)
	{
		if (cachedMaterial == null) cachedMaterial = GetComponentInChildren<Renderer>().material;
		cachedMaterial.color = newColor;
	}

	void OnDestroy()
	{
		Destroy(cachedMaterial);
	}
}