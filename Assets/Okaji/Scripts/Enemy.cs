using System.Collections; // Coroutineを使うために必要
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;

    // 敵が動いているかどうかのフラグ
    private bool isMoving = true;

    // 点滅処理に必要な設定
    public float blinkDuration = 0.5f; // 点滅にかける時間
    public int blinkCount = 5;         // 点滅させる回数

    void Start()
    {
        // 必要なコンポーネントを取得
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMoving)
        {
            // 左への移動 (共通の速度を使用)
            transform.position += Vector3.left * GameManager.Instance.currentSpeed * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに触れたかチェック
        if (collision.gameObject.CompareTag("Player"))
        {
            // 共通の速度を減速させる
            GameManager.Instance.currentSpeed -= GameManager.Instance.decelerationRate;

            // 速度が0を下回らないようにする
            if (GameManager.Instance.currentSpeed < 0)
            {
                GameManager.Instance.currentSpeed = 0;
            }

            isMoving = false;

            // 衝突判定を無効にする
            enemyCollider.enabled = false;

            // 点滅処理を開始
            StartCoroutine(BlinkAndFadeOut());
        }
    }

    // 点滅と消滅を制御するコルーチン
    IEnumerator BlinkAndFadeOut()
    {
        // 点滅のインターバルを計算
        float blinkInterval = blinkDuration / (2 * blinkCount);

        for (int i = 0; i < blinkCount; i++)
        {
            // 不透明にする
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);

            // 透明にする
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(blinkInterval);
        }

        // コルーチン終了後、完全にオブジェクトを破棄
        Destroy(gameObject);
    }
}
