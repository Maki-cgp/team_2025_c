using UnityEngine;

public class Item : MonoBehaviour
{
    private Collider2D itemCollider;

    // アイテムが動いているかどうかのフラグ
    private bool isMoving = true;

    // アイテム取得音
    private AudioSource audioSource;
    public AudioClip getSound;

    void Start()
    {
        // 必要なコンポーネントを取得
        itemCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
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
            GameManager.Instance.ItemCounter(); // アイテムカウント増加

            audioSource.PlayOneShot(getSound);
            isMoving = false;

            // 衝突判定を無効にする
            itemCollider.enabled = false;

            // アイテムを破棄
            Destroy(gameObject);
        }
    }
}
