using UnityEngine;
using System.Collections;

public class TutrialManager : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    private AudioSource audioSource;
    public AudioClip changeSound;
    
    // スムーズな移動のための速度
    public float moveSpeed = 5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ボタンのクリックイベントなどで呼び出すメソッド
    // ボタンクリックなどで実行するメソッド
    public void NextPanelMovement()
    {
        audioSource.PlayOneShot(changeSound);   // ボタン効果音

        // Panel 1のRectTransformを取得し、コルーチンに渡す
        RectTransform rect1 = panel1.GetComponent<RectTransform>();
        if (rect1 != null)
        {
            // rect1を引数として渡す
            StartCoroutine(MovePanelCoroutine(rect1, -800f, 1200f));
        }

        // Panel 2のRectTransformを取得し、コルーチンに渡す
        RectTransform rect2 = panel2.GetComponent<RectTransform>();
        if (rect2 != null)
        {
            // rect2を引数として渡す
            StartCoroutine(MovePanelCoroutine(rect2, 200f, 200f));
        }
    }
    
    public void BackPanelMovement()
    {
        audioSource.PlayOneShot(changeSound);

        // Panel 1のRectTransformを取得し、コルーチンに渡す
        RectTransform rect1 = panel1.GetComponent<RectTransform>();
        if (rect1 != null)
        {
            // rect1を引数として渡す
            StartCoroutine(MovePanelCoroutine(rect1, 200f, 200f));
        }

        // Panel 2のRectTransformを取得し、コルーチンに渡す
        RectTransform rect2 = panel2.GetComponent<RectTransform>();
        if (rect2 != null)
        {
            // rect2を引数として渡す
            StartCoroutine(MovePanelCoroutine(rect2, 1200f, -800f));
        }
    }

        private IEnumerator MovePanelCoroutine(RectTransform rect, float targetL, float targetR)
    {
        // これでコルーチン内では常に引数で渡された rect を操作できる
        
        float duration = 0.5f; // 移動時間
        float elapsedTime = 0f;

        float startLeft = rect.offsetMin.x;
        float startRight = -rect.offsetMax.x; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            
            // Left (offsetMin.x) の設定
            float newLeft = Mathf.Lerp(startLeft, targetL, t);
            rect.offsetMin = new Vector2(newLeft, rect.offsetMin.y);

            // Right (-offsetMax.x) の設定
            float newRight = Mathf.Lerp(startRight, targetR, t);
            rect.offsetMax = new Vector2(-newRight, rect.offsetMax.y);

            elapsedTime += Time.deltaTime;
            
            yield return null; 
        }

        // 最後に目標値を正確に設定
        rect.offsetMin = new Vector2(targetL, rect.offsetMin.y);
        rect.offsetMax = new Vector2(-targetR, rect.offsetMax.y);
    }
}
