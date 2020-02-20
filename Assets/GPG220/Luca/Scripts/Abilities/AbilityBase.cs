using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    public abstract class AbilityBase : MonoBehaviour
    {
        public string abilityName;
        public string abilityDescription;
        public Sprite abilityImg;
        public float cooldown;
        
        public float currentCooldown; // Info: Public so UI could get current cooldown for example

        public delegate void OnAbilityExecutedDel(AbilityBase abilityBase, GameObject executor);
        public event OnAbilityExecutedDel OnAbilityExecuted;

        /// <summary>
        /// Sets the cooldown and invokes the OnAbilityExecuted event.
        /// </summary>
        /// <param name="executor"></param>
        protected virtual void NotifyAbilityExecuted(GameObject executor)
        {
            currentCooldown = cooldown;
            OnAbilityExecuted?.Invoke(this, executor);
        }

        void Update()
        {
            if (currentCooldown > 0)
                currentCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// Checks some requirements such as if there is an active cooldown to determine if the ability can be executed.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckRequirements()
        {
            return currentCooldown <= 0;
        }

        /// <summary>
        /// Function to execute the ability.
        /// </summary>
        /// <param name="executorGameObject">The gameobject that executes the cast.</param>
        /// <returns>Returns true if the ability could be executed, else false.</returns>
        protected abstract bool Execute(GameObject executorGameObject);
    }
}
