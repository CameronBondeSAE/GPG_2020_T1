using System;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    public class UnitAbilityHandler : MonoBehaviour
    {
        public UnitBase unitBase;

        #region Events

        public delegate void OnAbilityExecutedDel(UnitBase unitBase, Ability ability);

        public event OnAbilityExecutedDel OnAbilityExecuted;

        #endregion
        
        private void Start()
        {
            if (unitBase == null)
                unitBase = GetComponent<UnitBase>();
        }

        public bool ExecuteAbility(Ability ability)
        {
            if(!CheckAbilityRequirements(ability))
                return false;
            
            /*foreach (var instantAction in ability.instantSelfActions)
            {
                instantAction.ExecuteAction(unitBase);
            }*/
            
            OnAbilityExecuted?.Invoke(unitBase, ability);
            return true;
        }

        public bool CheckAbilityRequirements(Ability ability)
        {
            /*if (ability == null || ability.instantSelfActions == null)
                return false;*/
            
            // TODO
            return true;
        }
    }
}