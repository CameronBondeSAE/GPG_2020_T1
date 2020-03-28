using UnityEngine;

namespace DevConsoleTest.Command_Scripts
{
    [CreateAssetMenu(fileName = "New Log Command", menuName = "Developer/ConsoleCommand/LogCommand")]
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
       
            string logTest = string.Join(" ", args);
            Debug.Log(logTest);

            return true;
        }
    }
}
