using UnityEngine;

namespace CustomEditorDemo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]
        public new Rigidbody rigidbody;

        public float damageRadius = 1;
    
        //the reset is called every time this script is added as a component
        void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}
