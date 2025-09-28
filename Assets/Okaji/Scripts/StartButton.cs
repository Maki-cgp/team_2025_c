using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void OnClick()
    {
        StartCoroutine(GameManager.Instance.GameStart());
        this.gameObject.SetActive(false);
    }
}