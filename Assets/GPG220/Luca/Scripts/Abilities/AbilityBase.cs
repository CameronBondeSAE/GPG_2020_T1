using System;
using Anthony;
using TMPro;
using UnityEngine;
using AnthonyY;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using NaughtyAttributes;

namespace GPG220.Luca.Scripts.Abilities
{
    public abstract class AbilityBase : NetworkBehaviour,IDescribable
    {
        public float cooldown;
        
        public float currentCooldown; // Info: Public so UI could get current cooldown for example

        /*public delegate void OnAbilityExecutedDel(AbilityBase abilityBase, GameObject executor);
        public event OnAbilityExecutedDel OnAbilityExecuted;*/

        public event Action<AbilityBase, GameObject> AbilityExecutionStartEvent;
        public event Action<AbilityBase, GameObject> AbilityExecutionEndEvent;
        public bool targetRequired;
        public string abilityName;
        [TextArea]
        public string abilityDescription;
		[ShowAssetPreview]
        public Sprite abilityImage;
		
        public string Name 
        {
            get { return abilityName;}
        }

        public string Description
        {
            get { return abilityDescription; }
        }

        public Sprite Image
        {
            get { return abilityImage; }
            set { abilityImage = value; }
        }
        /// <summary>
        /// Sets the cooldown and invokes the OnAbilityExecuted event.
        /// </summary>
        /// <param name="executor"></param>
        protected virtual void NotifyAbilityExecuted(GameObject executor)
        {
            ApplyCooldown();
            AbilityExecutionEndEvent?.Invoke(this, executor);
        }
        
        /// <summary>
        /// Sets the cooldown and invokes the OnAbilityExecuted event.
        /// </summary>
        /// <param name="executor"></param>
        protected virtual void NotifyAbilityStartExecution(GameObject executor)
        {
            AbilityExecutionStartEvent?.Invoke(this, executor);
        }

        void Update()
        {
            if (currentCooldown > 0)
                currentCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// Checks some requirements such as if there is an active cooldown to determine if the ability can be executed.
        /// </summary>
        /// <returns>Returns true if the ability can be executed.</returns>
        public virtual bool CheckRequirements()
        {
            return currentCooldown <= 0;
        }

        protected virtual void ApplyCooldown()
        {
            currentCooldown = cooldown;
        }

        /// <summary>
        /// Function to execute the ability.
        /// </summary>
        /// <param name="executorGameObject">The gameobject that executes the cast.</param>
        /// <param name="targets">Any kind of gameobject targets that might be passed</param>
        /// <returns>Returns true if the ability could be executed, else false.</returns>
        /// TODO Remove
        [Obsolete("Use SelectedExecute or TargetExecute instead.")]
        public virtual bool Execute(GameObject executorGameObject, GameObject targets = null)
        {
            return true;
        }

        // TODO Need 2x CheckRequirements function for Selected/target execute? Need multiple new events? StartSelectedExecute/End... StartTarget.... Maybe add ApplyCooldown function?
        
        /// <summary>
        /// Executes certain functionality when an ability is clicked on/activated
        /// </summary>
        /// <returns>True if the ability could be executed (No cooldown, ...)</returns>
        public virtual bool SelectedExecute()
        {
            return true;
        }

        /// <summary>
        /// Executes certain functionality with a single target
        /// </summary>
        /// <param name="target"></param>
        /// <returns>True if the ability could be executed (No cooldown, ...)</returns>
		public virtual bool TargetExecute(GameObject target = null)
		{
			return true;
		}
		
		/// <summary>
		/// Overload for just right clicking on a world position
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
        public virtual bool TargetExecute(Vector3 worldPos)
        {
            return true;
        }

        /// <summary>
        /// Call this function to cleanup/break/finish the execution of this ability.
        /// </summary>
        public virtual void Finish()
        {
            
        }
        
    }
}
