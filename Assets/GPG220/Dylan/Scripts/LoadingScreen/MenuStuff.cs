using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingScreen
{
    public class MenuStuff : MonoBehaviour
    {
        public void LoadLevel()
        {
            LoadingScreen.instance.LoadGame();
        }
    }
}
