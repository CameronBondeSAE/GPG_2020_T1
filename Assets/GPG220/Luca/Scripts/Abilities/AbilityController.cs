using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    /// <summary>
    /// Controls a set of abilities. It can store an unlimited amount of abilities. Abilities are identified by an integer.
    /// </summary>
    public class AbilityController : SerializedMonoBehaviour
    {
        // Automatically loads abilities added to the same gameobject on Start()
        public bool autoLoadAbilities = true;
    
        // Sorted list of all abilities. The key is a unique integer serving as an identifier.
        [ShowInInspector, OdinSerialize]
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

            if (!(abilities?.ContainsKey(defaultAbilityIndex) ?? true))
            {
                defaultAbilityIndex = abilities.Keys.Min();
            }
        }

        public bool ExecuteDefaultAbility(GameObject[] targets = null)
        {
            return ExecuteAbility(defaultAbilityIndex);
        }
    
        public bool ExecuteAbility<T>(GameObject[] targets = null) where T : AbilityBase
        {
            var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T));
        
            return ability != null && abilities.ContainsValue(ability) && ability.Execute(gameObject, targets);
        }

        public bool ExecuteAbility(AbilityBase ability, GameObject[] targets = null)
        {
            return ability != null && abilities.ContainsValue(ability) && ability.Execute(gameObject, targets);
        }

        public bool ExecuteAbility(int abilityIndex, GameObject[] targets = null)
        {
            abilities.TryGetValue(abilityIndex, out var ability);

            return ability?.Execute(gameObject, targets) ?? false;
        }
    }
}
