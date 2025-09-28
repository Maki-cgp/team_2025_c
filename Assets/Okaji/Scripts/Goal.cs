using UnityEngine;

public class Goal : MonoBehaviour
{
    // ResultManagerの参照
    public ResultManager resultManager;
    public TimeManager timeManager;

    // ゴール演出
    private bool goalChecker = false;
    public bool speedChecker = false;
    public GameObject goalScript;
    public AudioClip goalSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 左への移動 (共通の速度を使用)
        transform.position += Vector3.left * GameManager.Instance.currentSpeed * Time.deltaTime;

        // 文字の移動
        if (goalChecker)
        {
            goalScript.transform.position += Vector3.left * 30 * Time.deltaTime;
        }
    }

    public GameObject speedMeter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.currentSpeed >= 50)
            {
                speedChecker = true;
            }
            // 共通速度を0にして、ゲームを停止させる
            GameManager.Instance.currentSpeed = 0;

            // ゴール演出
            audioSource.PlayOneShot(goalSound);
            goalChecker = true;

            // OnGoalメソッドを呼び出す
            timeManager.StopGameTimer();    // タイマー停止
            speedMeter.SetActive(false);
            resultManager.OnGoal();

            GameManager.Instance.isMoving = false;
        }
    }
}