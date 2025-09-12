using UnityEngine;

public class Goal : MonoBehaviour
{
    // ゲームが進行中かどうかのフラグ
    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            // 共通の速度を加速させる
            if (GameManager.Instance.currentSpeed < GameManager.Instance.maxSpeed)
            {
                GameManager.Instance.currentSpeed += GameManager.Instance.accelerationRate * Time.deltaTime;
            }
        }

        // 左への移動 (共通の速度を使用)
            transform.position += Vector3.left * GameManager.Instance.currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // GameManagerのOnGoalメソッドを呼び出す
            GameManager.Instance.OnGoal();

            isMoving = false;
        }
    }
}