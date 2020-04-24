using System;
using GPG220.Dylan.Scripts.GOAP.Goals;
using GPG220.Dylan.Scripts.GOAPFirstTry;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;


namespace GPG220.Dylan.Scripts.GOAPFirstTry
{
    public class Test : MonoBehaviour
    {
    
    
        public GoapAgentDylan goapAgentDylan;
        public GoalTargetReached goal;

        public void Awake()
        {
            goapAgentDylan = GetComponent<GoapAgentDylan>();
            goal = GetComponent<GoalTargetReached>();
        }
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                goapAgentDylan.CalculateNewGoal(true);
            }
        }
    }
}
