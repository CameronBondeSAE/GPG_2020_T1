using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnthonyY
{
    public class StateBase : MonoBehaviour
    {
        public virtual void Enter()
        {
      
        }

        public virtual void Execute()
        {
            Debug.Log("Base Executed!");
        }

        public virtual void Exit()
        {
      
        }
    }

}


