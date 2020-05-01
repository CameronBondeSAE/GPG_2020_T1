using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;

namespace GPG220.Dylan.King_Scripts
{
    public class AddAbility : MonoBehaviour
    {
        //adds all the units to spawn to the spawn unit ability
        private PlayerBase player;
        public UnitSpawner unitSpawner;
        private GameManager gameManager;
        public LayerMask groundMask;

        //get all units from gamemanager and unit spawner
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            unitSpawner = gameManager.unitSpawner;
            player = gameManager.localPlayer;
            // player = FindObjectOfType<HumanPlayer>();

            for (int i = 0; i < unitSpawner.unitBases.Count; i++)
            {
                UnitSpawnerAbility unitSpawnerAbility = gameObject.AddComponent<UnitSpawnerAbility>();
                unitSpawnerAbility.unitPrefabIndex = i;
                unitSpawnerAbility.groundMask = groundMask;
                unitSpawnerAbility.abilityName = unitSpawner.unitBases[i].unitName;
                unitSpawnerAbility.abilityDescription = "Build " + unitSpawner.unitBases[i].Name + " next to King";
                unitSpawnerAbility.player = player;
            }
        }
    }
}