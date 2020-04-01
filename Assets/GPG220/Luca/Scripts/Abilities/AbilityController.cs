using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    /// <summary>
    /// Controls a set of abilities. It can store an unlimited amount of abilities. Abilities are identified by an integer.
    /// </summary>
    public class AbilityController : NetworkBehaviour
    {
        // Automatically loads abilities added to the same gameobject on Start()
        public bool autoLoadAbilities = true;
    
        // Sorted list of all abilities. The key is a unique integer serving as an identifier.
        [Sirenix.OdinInspector.ShowInInspector, OdinSerialize]
        public SortedList<int,AbilityBase> abilities;

        // The int identifier of the default ability
        public int defaultAbilityIndex = 0;
    
        // Start is called before the first frame update
        private void Start()
        {
            // Load Abilities that are added as a component and not in the list
            if (autoLoadAbilities)
            {
                var abilityComponents = GetComponents<AbilityBase>();
                if (abilityComponents != null && abilityComponents.Length > 0)
                {
                    int nextNewIndex = 0;
                    if (abilities == null)
                        abilities = new SortedList<int, AbilityBase>(abilityComponents.Length);
                    else
                        nextNewIndex = abilities.Keys.Max()+1;
                    foreach (var abComp in abilityComponents)
                    {
                        if (!abilities.ContainsValue(abComp))
                        {
                            abilities.Add(nextNewIndex,abComp);
                            nextNewIndex++;
                        }
                    }
                }
            }

            // Make sure defined default index exists (if any abilities defined at all) & fixes it if not to the lowest key.
            if (!(abilities?.ContainsKey(defaultAbilityIndex) ?? true))
            {
                defaultAbilityIndex = abilities.Keys.Min();
            }
        }

        /// <summary>
        /// Executes the default ability.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        /// TODO DEPRECATED; DELETE
        [Obsolete("Use SelectedExecuteDefaultAbility or TargetExecuteDefaultAbility instead.")]
        public bool ExecuteDefaultAbility(GameObject[] targets = null)
        {
            return ExecuteAbility(defaultAbilityIndex);
        }

        /// <summary>
        /// Executes the default ability.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool SelectedExecuteDefaultAbility()
        {
            return SelectedExecuteAbility(defaultAbilityIndex);
        }

        /// <summary>
        /// Executes the default ability.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool TargetExecuteDefaultAbility(GameObject[] targets = null)
        {
            return TargetExecuteAbility(defaultAbilityIndex,targets);
        }
        public bool TargetExecuteDefaultAbility( Vector3 worldPos)
        {
            return TargetExecuteAbility(defaultAbilityIndex,worldPos);
        }

        /// <summary>
        /// Executes an ability of given AbilityBase type.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
        /// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
        /// <returns>Returns true if the ability could be executed.</returns>
        /// TODO DEPRECATED; DELETE
        [Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
        public bool ExecuteAbility<T>(GameObject[] targets = null, bool executeAll = false) where T : AbilityBase
        {
            var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
            // TODO What if there are multiple abilities of the same type, execute all?
            return ability != null && abilities.ContainsValue(ability) && ability.Execute(gameObject, targets);
        }

        /// <summary>
        /// Executes an ability of given AbilityBase type.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
        /// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool SelectedExecuteAbility<T>(bool executeAll = false) where T : AbilityBase
        {
            var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
            // TODO What if there are multiple abilities of the same type, execute all?
            return ability != null && abilities.ContainsValue(ability) && ability.SelectedExecute();
        }

        /// <summary>
        /// Executes an ability of given AbilityBase type.
        /// </summary>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
        /// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool TargetExecuteAbility<T>(GameObject[] targets = null, bool executeAll = false) where T : AbilityBase
        {
            var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
            // TODO What if there are multiple abilities of the same type, execute all?
            return ability != null && abilities.ContainsValue(ability) && ability.TargetExecute(targets);
        }

        /// <summary>
        /// Execute given ability (Actual reference of the ability!)
        /// </summary>
        /// <param name="ability">Ability to execute.</param>
        /// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        /// TODO DEPRECATED; DELETE
        [Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
        public bool ExecuteAbility(AbilityBase ability, GameObject[] targets = null, bool mustContainAbility = true)
        {
            return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) && ability.Execute(gameObject, targets);
        }

        /// <summary>
        /// Execute given ability (Actual reference of the ability!)
        /// </summary>
        /// <param name="ability">Ability to execute.</param>
        /// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool SelectedExecuteAbility(AbilityBase ability, bool mustContainAbility = true)
        {

            return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) && ability.SelectedExecute();
        }

        /// <summary>
        /// Execute given ability (Actual reference of the ability!)
        /// </summary>
        /// <param name="ability">Ability to execute.</param>
        /// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool TargetExecuteAbility(AbilityBase ability, GameObject[] targets = null, bool mustContainAbility = true)
        {
            return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) && ability.TargetExecute(targets);
        }

        /// <summary>
        /// Executes the ability with given ability-index (identifier).
        /// </summary>
        /// <param name="abilityIndex">The index (identifier) of the ability.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        /// TODO DEPRECATED; DELETE
        [Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
        public bool ExecuteAbility(int abilityIndex, GameObject[] targets = null)
        {
            abilities.TryGetValue(abilityIndex, out var ability);

            return ability?.Execute(gameObject, targets) ?? false;
            
        }

        /// <summary>
        /// Executes the ability with given ability-index (identifier).
        /// </summary>
        /// <param name="abilityIndex">The index (identifier) of the ability.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool SelectedExecuteAbility(int abilityIndex)
        {
            
            return CmdSelectedExecuteAbility(abilityIndex);
            
        }

        [Command]
        public bool CmdSelectedExecuteAbility(int abilityIndex)
        {
            Debug.Log(abilityIndex);
            
            abilities.TryGetValue(abilityIndex, out var ability);

            return ability?.SelectedExecute() ?? false;
            
            
        }
        
        
		
        /// <summary>
        /// Executes the ability with given ability-index (identifier).
        /// </summary>
        /// <param name="abilityIndex">The index (identifier) of the ability.</param>
        /// <param name="targets">List of any kind of targets (gameobjects).</param>
        /// <returns>Returns true if the ability could be executed.</returns>
        public bool TargetExecuteAbility(int abilityIndex, GameObject[] targets = null)
        {
            abilities.TryGetValue(abilityIndex, out var ability);

            return ability?.TargetExecute(targets) ?? false;
        }
        public bool TargetExecuteAbility(int abilityIndex, Vector3 worldPos)
        {
            abilities.TryGetValue(abilityIndex, out var ability);

            return ability?.TargetExecute(worldPos) ?? false;
        }
    }
}
