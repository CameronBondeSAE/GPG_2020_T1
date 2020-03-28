using UnityEngine;

namespace DevConsoleTest.Command_Scripts
{
    [CreateAssetMenu(fileName = "Quit Command", menuName = "Developer/ConsoleCommand/QuitGame")]
    public class QuitCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            if (args.Length != 0)
            {
                return false;
            }

            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                return true;
            }
            else
            {
                Application.Quit();
                return true;
            }
            
            return true;
        }
    }
}
