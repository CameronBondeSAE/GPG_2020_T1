using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    public class UnitStats : MonoBehaviour
    {
        // public Health health // Reference to health script? Or add Health
        
        // TODO TEMPORARY STAT; Used until theres a health script from GPG210
        [SerializeField]
        private float health = 0;
        public float Health
        {
            get => health;
            set => health = value;
        }
        
        [SerializeField]
        private float strength = 0;
        public float Strength
        {
            get => strength;
            set => strength = value;
        }

        [SerializeField]
        private Vector3 validMovement = Vector3.one; // Axis on which the unit may move (0 = mustn't move, 1 = can move)
        public Vector3 ValidMovement
        {
            get => validMovement;
            set => validMovement = value;
        }

        [SerializeField]
        private Vector3 validRotation = Vector3.one; // Axis on which the unit may rotate (0 = mustn't rotate, 1 = can rotate)
        public Vector3 ValidRotation
        {
            get => validRotation;
            set => validRotation = value;
        }

    
    
    }
}
