using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    [RequireComponent(typeof(CharacterController))]
    public class MedicUnit : MovableUnit
    {
        
        // Start is called before the first frame update
        void Start()
        {
            Initialize();    
        }

        

        protected override void Initialize()
        {
            base.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            
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
            base.OnExecuteAction(worldPosition, g);
        }
        
    }
}
