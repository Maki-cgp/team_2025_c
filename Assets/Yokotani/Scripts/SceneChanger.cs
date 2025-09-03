using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButtonScript : MonoBehaviour 
{
    [SerializeField] private string sceneName;
    public void OnClickStartButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}