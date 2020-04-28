using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_CheckEnergy : ReGoapAction<string, object>
    {
        public float energyAmount;
        public bool canTeleport;

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            preconditions.Set("energyRequired", true);

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            if (CheckEnergy())
            {
                effects.Set("hasEnergy", true);
                Debug.Log("Teleport Allowed");
            }
            else
            {
                effects.Set("hasEnergy", false);
            }

            return base.GetEffects(stackData);
        }

        public bool CheckEnergy()
        {
            if (energyAmount > 0)
            {
                canTeleport = true;
            }
            else
            {
                canTeleport = false;
            }
            return canTeleport;
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (!settings.HasKey("hasEnergy"))
            {
                failCallback(this);
            }
            else
            {
                doneCallback(this);
            }
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues())
            {
                worldState.Set(pair.Key, pair.Value);
            }
        }
    }
}