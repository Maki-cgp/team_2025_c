using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject titlepanel;
    public GameObject gameselect;
    public GameObject modeselect;
    public void GoToGameSelectPanel()
    {
        titlepanel.SetActive(false);
        gameselect.SetActive(true);
    }
    public void GoToModeSelectPanel()
    {
        gameselect.SetActive(false);
        modeselect.SetActive(true);
    }

    public void BackToTitlePanel()
    {
        titlepanel.SetActive(true);
        gameselect.SetActive(false);
        modeselect.SetActive(false);
    }
}
