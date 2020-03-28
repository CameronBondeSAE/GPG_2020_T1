using UnityEngine;

namespace SoundManager
{
    public class PlaySound : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SoundManager.PlaySound(SoundManager.Sound.moving, transform.position);
            }
        }
    }
}
