using UnityEngine;

public class Item : MonoBehaviour
{
    private Collider2D itemCollider;

    // アイテムが動いているかどうかのフラグ
    private bool isMoving = true;

    void Start()
    {
        // 必要なコンポーネントを取得
        itemCollider = GetComponent<Collider2D>();
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
            ActionPlayer.shield = true;   // シールド付与

            isMoving = false;

            // 衝突判定を無効にする
            itemCollider.enabled = false;

            // アイテムを破棄
            Destroy(gameObject);
        }
    }
}
