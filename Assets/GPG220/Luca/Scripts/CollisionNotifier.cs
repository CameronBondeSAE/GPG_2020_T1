using System;
using UnityEngine;

namespace GPG220.Luca.Scripts
{
    public class CollisionNotifier : MonoBehaviour
    {
        public event Action<Collision> CollisionEnterEvent;
        public event Action<Collision> CollisionExitEvent;
        public event Action<Collider> TriggerEnterEvent;
        public event Action<Collider> TriggerExitEvent;
    
        private void OnCollisionEnter(Collision other)
        {
            CollisionEnterEvent?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionExitEvent?.Invoke(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExitEvent?.Invoke(other);
        }
    }
}
