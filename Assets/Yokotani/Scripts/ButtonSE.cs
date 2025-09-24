using UnityEngine;
using UnityEngine.UI;

public class ButtonSE : MonoBehaviour
{
    public AudioClip clickSound;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySE);
    }
    void PlaySE()
    {
        SEManager.Instance.PlaySE(clickSound);
    }
}
