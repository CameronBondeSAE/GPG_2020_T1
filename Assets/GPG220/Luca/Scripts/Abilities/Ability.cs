using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName = "";
    public string abilityDescription = "";
    public Sprite abilityImg;

    public List<ActionBase> instantSelfActions;
    public List<ActionBase> instantTargetActions;
    
    // TODO add requirements
    
}
