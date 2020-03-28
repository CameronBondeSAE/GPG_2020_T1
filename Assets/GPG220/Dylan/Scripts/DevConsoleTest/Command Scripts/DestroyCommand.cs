using UnityEngine;

namespace DevConsoleTest.Command_Scripts
{
    [CreateAssetMenu(fileName = "DestroyObject", menuName = "Developer/ConsoleCommand/DestroyObject")]
    public class DestroyCommand : ConsoleCommand
    {
        public LayerMask destoryLayer;
        
        public override bool Process(string[] args)
        {
            if (args.Length != 0)
            {
                return false;
            }

            RaycastHit hit = DeveloperConsoleBehaviour.instance.ShootRaycast(destoryLayer);

            if (hit.collider == null)
            {
                return false;
            }
            Destroy(hit.collider.gameObject);

            return true;
        }
    }
}
