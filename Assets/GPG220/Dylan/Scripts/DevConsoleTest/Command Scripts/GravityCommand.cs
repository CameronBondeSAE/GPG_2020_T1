using UnityEngine;
using System;
using System.Collections.Generic;

namespace DevConsoleTest.Command_Scripts
{
    [CreateAssetMenu(fileName = "New Gravity Command", menuName = "Developer/ConsoleCommand/GravityCommmand")]
    public class GravityCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                return false;
            }

            //check for a float and pass out value
            if (!float.TryParse(args[0], out float value))
            {
                return false;
            }
            
            Physics.gravity = new Vector3(Physics.gravity.x, value,Physics.gravity.z);

            return true;
        }
    }
}
