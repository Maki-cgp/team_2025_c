using System.Collections;
using UnityEngine;

// 判定メッセージオブジェクトにアタッチして、自動的に破棄させるためのスクリプト
public class JudgeMessageDestructor : MonoBehaviour
{
    // Inspectorから設定する表示時間
    [SerializeField]
    private float displayDuration = 0.5f; // 例: 0.5秒後に消える

    void Start()
    {
        // オブジェクトが生成されたら、自動で破棄するコルーチンを開始
        StartCoroutine(DestroyAfterDelay(displayDuration));
    }

    /// <summary>
    /// 指定された時間後にオブジェクトを破棄するコルーチン
    /// </summary>
    IEnumerator DestroyAfterDelay(float delay)
    {
        // 指定された時間（フレームではない）待機する
        yield return new WaitForSeconds(delay);

        // 待機時間が経過したら、このゲームオブジェクト（メッセージ）を破棄
        Destroy(gameObject);
    }
}