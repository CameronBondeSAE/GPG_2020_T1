using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    public class UnitBase : MonoBehaviour
    {
    
        // TODO Unit Ability Manager
        public UnitStats unitStats;
        //TODO Unit Movement System
    
        // Start is called before the first frame update
        void Start()
        {
            unitStats = GetComponent<UnitStats>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
