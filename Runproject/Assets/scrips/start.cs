using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
   
    private const string GameSceneName = "Game";

 
    public void StartGame()
    {
        Debug.Log("Starting Game Scene: " + GameSceneName);

        SceneManager.LoadScene(GameSceneName);
    }
}