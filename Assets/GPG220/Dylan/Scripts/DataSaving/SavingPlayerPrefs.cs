using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DataSaving
{
    public class SavingPlayerPrefs : MonoBehaviour
    {
        public float playerLives = 1;

        public InputField playerLivesStringText;
        public InputField playerLivesIntText;

        public string playerlivesString;

        public void SaveLives()
        {
            playerlivesString = playerLivesStringText.text;

            playerLives = int.Parse(playerLivesIntText.text);
            
            PlayerPrefs.SetFloat("Lives to save float",playerLives);
            
            PlayerPrefs.SetString("Lives to save string", playerlivesString);
         
        }

        public void LoadLives()
        {
            PlayerPrefs.GetString("Lives to save string");
            Debug.Log(PlayerPrefs.GetString("Lives to save string"));
            
            PlayerPrefs.GetFloat("Lives to save float");
            Debug.Log(PlayerPrefs.GetFloat("Lives to save float"));
        }
    }
}
