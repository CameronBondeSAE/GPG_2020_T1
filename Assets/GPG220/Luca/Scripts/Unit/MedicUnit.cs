using GPG220.Luca.Scripts.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    [RequireComponent(typeof(CharacterController))]
    public class MedicUnit : MovableUnit
    {
        public AbilityBase ability = null;
        
        // Start is called before the first frame update
        void Start()
        {
            Initialize();    
        }

        

        protected override void Initialize()
        {
            base.Initialize();

            if (ability == null)
                ability = GetComponent<AbilityBase>();
        }

        public bool HACKExecuteAbility = false;
        
        // Update is called once per frame
        void Update()
        {
            if (HACKExecuteAbility)
            {
                OnExecuteAction(transform.position, null);
                HACKExecuteAbility = false;
            }
            
            HandleMovement();
        }
        
        public override bool Selectable()
        {
            return true;
        }

        public override bool GroupSelectable()
        {
            return true;
        }

        public override void OnSelected()
        {
            
        }

        public override void OnDeSelected()
        {
            
        }

        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            Debug.Log("SWUSH!");
            base.OnExecuteAction(worldPosition, g);
            ability?.Execute(gameObject);
            
        }
        
    }
}
