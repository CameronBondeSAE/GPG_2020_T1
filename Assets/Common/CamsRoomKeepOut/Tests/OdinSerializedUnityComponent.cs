using UnityEngine;

public interface ISomeInterface
{
	string SomeInterfaceProperty { get; set; }
}

public class OdinSerializedUnityComponent : MonoBehaviour
{}
	
	
/*	
	
	: SerializedMonoBehaviour, ISomeInterface
{
	[OdinSerialize]
	public virtual string VirtualProperty { get; set; } // Wil not be shown or serialized, since it is virtual 
    
	[OdinSerialize]
	public string SomeInterfaceProperty { get; set; } // Wil not be shown or serialized, since it is virtual because of the interface
}*/