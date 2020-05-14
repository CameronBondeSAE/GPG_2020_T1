using UnityEngine;

public class PlayMenu : MonoBehaviour
{
	public GameObject PlayMenuUI;
    public MultiplayerMenu multiplayerMenu;
    public OptionsMenu optionsMenu;

	public GameManager gameManager;
	public RTSNetworkManager rtsNetworkManager;

    public void Awake()
    {
        PlayMenuUI.SetActive(true);
        multiplayerMenu = GetComponent<MultiplayerMenu>();
    }


    public void Play()
    {
       PlayMenuUI.SetActive(false);
	   rtsNetworkManager.StartHost();
	   gameManager.OnStartGameEventInvocation();
	}

    public void Options()
    {
        PlayMenuUI.SetActive(false);
        optionsMenu.optionsMenu.SetActive(true);
    }

    public void Setup()
    {
        multiplayerMenu.multiplayerMenu.SetActive(true);
        PlayMenuUI.SetActive(false);
    }
    

    public void Exit()
    {
        Application.Quit();
    }
}
