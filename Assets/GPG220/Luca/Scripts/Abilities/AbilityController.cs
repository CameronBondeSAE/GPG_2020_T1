﻿using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Unit;
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
		public static event Action<AbilityController> ClickedLocalAbilityStaticEvent;
		
		// Automatically loads abilities added to the same gameobject on Start()
		public bool autoLoadAbilities = true;

		// Sorted list of all abilities. The key is a unique integer serving as an identifier.
		[Sirenix.OdinInspector.ShowInInspector, OdinSerialize]
		public SortedList<int, AbilityBase> abilities;

		// The int identifier of the default ability
		private int         defaultTargetAbilityIndex = 0;
		public  AbilityBase defaultTargetAbility;
		private int         defaultWorldTargetAbilityIndex = 0;
		public  AbilityBase defaultWorldTargetAbility;

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
						nextNewIndex = abilities.Keys.Max() + 1;
					foreach (var abComp in abilityComponents)
					{
						if (!abilities.ContainsValue(abComp))
						{
							abilities.Add(nextNewIndex, abComp);
							nextNewIndex++;
						}
					}
				}
			}

			// Make sure default abilities are set.
			if (defaultTargetAbility != null)
				defaultTargetAbilityIndex = abilities.Values.IndexOf(defaultTargetAbility);
			else
			{
				// Debug.Log("No default target ability set in " + GetComponent<UnitBase>()?.name);
				defaultTargetAbilityIndex = -1;
			}

			if (defaultWorldTargetAbility != null)
				defaultWorldTargetAbilityIndex = abilities.Values.IndexOf(defaultWorldTargetAbility);
			else
			{
				// Debug.Log("No default target ability set in " + GetComponent<UnitBase>()?.name);
				defaultWorldTargetAbilityIndex = -1;
			}
			
			// abilities[defaultAbilityIndex].TryGetValue(defaultAbilityIndex, out var ability);

			// if (!(abilities?.ContainsKey(defaultAbilityIndex) ?? true))
			// {
			// defaultAbilityIndex = abilities.Keys.Min();
			// }
		}

		
		
		/// <summary>
		/// Executes the default ability.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		public void TargetExecuteDefaultAbility(GameObject target = null)
		{
			if (defaultTargetAbility != null)
			{
				TargetExecuteAbility(defaultTargetAbility, target);
			}
		}

		public void TargetExecuteDefaultAbility(Vector3 worldPos)
		{
			if (defaultWorldTargetAbility != null)
			{
				TargetExecuteAbility(defaultWorldTargetAbility, worldPos);
			}
		}

		
		
		
		
		/// <summary>
		/// Execute given ability (Actual reference of the ability!)
		/// </summary>
		/// <param name="ability">Ability to execute.</param>
		/// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>HACK: NO*** Returns true if the ability could be executed.</returns>
		public void SelectedExecuteAbility(AbilityBase ability, bool mustContainAbility = true)
		{
			ClickedLocalAbilityStaticEvent?.Invoke(this);

			CmdSelectedExecuteAbility(abilities.IndexOfValue(ability));
			// return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) &&
				   // ability.SelectedExecute();
		}

		[Command]
		private void CmdSelectedExecuteAbility(int abilityIndex)
		{
			Debug.Log("Command : "+abilities[abilityIndex].Name);

			RpcSelectedExecuteAbility(abilityIndex);
			//  abilities.TryGetValue(abilityIndex, out var ability);
			//  return ability?.SelectedExecute() ?? false;
		}

		[ClientRpc]
		private void RpcSelectedExecuteAbility(int abilityIndex)
		{
			Debug.Log("RPC: Client->Server : "+abilities[abilityIndex].Name);
			abilities[abilityIndex].SelectedExecute();
		}

		/// <summary>
		/// Execute given ability (Actual reference of the ability!)
		/// </summary>
		/// <param name="ability">Ability to execute.</param>
		/// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		public void TargetExecuteAbility(AbilityBase ability, GameObject target = null)
		{
			ClickedLocalAbilityStaticEvent?.Invoke(this);
			
			var index = abilities.IndexOfValue(ability);
			abilities.TryGetValue(index, out AbilityBase outAbility);

			if (outAbility != null)
			{
				if (target != null)
				{
					CmdTargetExecuteAbility(index, target.GetComponent<NetworkIdentity>());
				}
				else
				{
					Debug.Log("Target null in TargetExecuteAbility");
				}
			}

			//return ability?.TargetExecute(target) ?? false;
		}

		[Command]
		private void CmdTargetExecuteAbility(int abilityIndex, NetworkIdentity target)
		{
			Debug.Log("Command : Called "+abilities.IndexOfKey(abilityIndex));
			if (target != null)
			{
				RpcTargetExecuteAbility(abilityIndex, target);
			}
			else
			{
				Debug.Log("No target. Trying to TargetExecuteAbility");
			}
		}

		[ClientRpc]
		private void RpcTargetExecuteAbility(int abilityIndex, NetworkIdentity target)
		{
			Debug.Log("RPC: Client->Server : Called "+abilities.IndexOfKey(abilityIndex));
			abilities.TryGetValue(abilityIndex, out var ability);

			try
			{
				ability?.TargetExecute(target.gameObject);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}


		public void TargetExecuteAbility(AbilityBase ability, Vector3 worldPos)
		{
			ClickedLocalAbilityStaticEvent?.Invoke(this);
			
			CmdTargetExecuteAbilityWorldPos(abilities.IndexOfValue(ability), worldPos);

			//return ability?.TargetExecute(worldPos) ?? false;
		}

		[Command]
		public void CmdTargetExecuteAbilityWorldPos(int abilityIndex, Vector3 worldPos)
		{
			RpcTargetExecuteAbilityWorldPos(abilityIndex, worldPos);
		}

		[ClientRpc]
		public void RpcTargetExecuteAbilityWorldPos(int abilityIndex, Vector3 worldPos)
		{
			abilities.TryGetValue(abilityIndex, out var ability);

			ability?.TargetExecute(worldPos);
		}

		
		
		
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Executes an ability of given AbilityBase type.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
		/// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
		/// <returns>Returns true if the ability could be executed.</returns>
		// public bool SelectedExecuteAbility<T>(bool executeAll = false) where T : AbilityBase
		// {
		// var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
		// TODO What if there are multiple abilities of the same type, execute all?
		// return ability != null && abilities.ContainsValue(ability) && ability.SelectedExecute();
		// }

		
		
		/// <summary>
		/// Executes the default ability.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		// public bool SelectedExecuteDefaultAbility()
		// {
		// return SelectedExecuteAbility(defaultAbilityIndex);
		// }


		
		/// <summary>
		/// Execute given ability (Actual reference of the ability!)
		/// </summary>
		/// <param name="ability">Ability to execute.</param>
		/// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		// public bool TargetExecuteAbility(AbilityBase ability, GameObject target = null, bool mustContainAbility = true)
		// {
		// 	return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) &&
		// 		   ability.TargetExecute(target);
		// }

		/// <summary>
		/// Executes the ability with given ability-index (identifier).
		/// </summary>
		/// <param name="abilityIndex">The index (identifier) of the ability.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		// public bool SelectedExecuteAbility(int abilityIndex)
		// {
		// CmdSelectedExecuteAbility(abilityIndex);

		// return true;
		// }


		/// <summary>
		/// Executes an ability of given AbilityBase type.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
		/// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
		/// <returns>Returns true if the ability could be executed.</returns>
		/* public bool TargetExecuteAbility<T>(GameObject[] target = null, bool executeAll = false) where T : AbilityBase
		 {
			 var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
			 // TODO What if there are multiple abilities of the same type, execute all?
			 return ability != null && abilities.ContainsValue(ability) && ability.TargetExecute(target);
		 }
		  */


		/// <summary>
		/// Executes the ability with given ability-index (identifier).
		/// </summary>
		/// <param name="abilityIndex">The index (identifier) of the ability.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		/// TODO DEPRECATED; DELETE
		[Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
		public bool ExecuteAbility(int abilityIndex, GameObject target = null)
		{
			abilities.TryGetValue(abilityIndex, out var ability);

			return ability?.Execute(gameObject, target) ?? false;
		}

		/// <summary>
		/// Executes the default ability.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		/// TODO DEPRECATED; DELETE
		// [Obsolete("Use SelectedExecuteDefaultAbility or TargetExecuteDefaultAbility instead.")]
		// public bool ExecuteDefaultAbility(GameObject target = null)
		// {
			// return ExecuteAbility(defaultAbilityIndex);
		// }

		/// <summary>
		/// Executes an ability of given AbilityBase type.
		/// </summary>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <param name="executeAll">If set to true, it will execute all abilities of given type (If there are multiple abilities of the same type). If set to false, it will execute the first available occurence of the given ability type.</param>
		/// <typeparam name="T">Type of AbilityBase to be executed.</typeparam>
		/// <returns>Returns true if the ability could be executed.</returns>
		/// TODO DEPRECATED; DELETE
		[Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
		public bool ExecuteAbility<T>(GameObject target = null, bool executeAll = false) where T : AbilityBase
		{
			var ability = abilities.Values.FirstOrDefault(ab => ab.GetType() == typeof(T) && ab.CheckRequirements());
			// TODO What if there are multiple abilities of the same type, execute all?
			return ability != null && abilities.ContainsValue(ability) && ability.Execute(gameObject, target);
		}

		/// <summary>
		/// Execute given ability (Actual reference of the ability!)
		/// </summary>
		/// <param name="ability">Ability to execute.</param>
		/// <param name="mustContainAbility">If set to true, the given <paramref name="ability"/> must be present in the <see cref="abilities"/> list. Setting it to false allows you to make the AbilityController to execute an ability which it doesn't manage.</param>
		/// <param name="target">List of any kind of target (gameobjects).</param>
		/// <returns>Returns true if the ability could be executed.</returns>
		/// TODO DEPRECATED; DELETE
		[Obsolete("Use SelectedExecuteAbility or TargetExecuteAbility instead.")]
		public bool ExecuteAbility(AbilityBase ability, GameObject target = null, bool mustContainAbility = true)
		{
			return ability != null && (mustContainAbility == false || abilities.ContainsValue(ability)) &&
				   ability.Execute(gameObject, target);
		}
	}
}