using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float currentSpeed;              // 速度
    public float maxSpeed = 50f;            // 最高速
    public float accelerationRate = 2f;   // 1秒あたりの加速率
    public float decelerationRate = 10f;     // 減速率

    private void Awake()
    {
        // インスタンスがまだ存在しない場合、このインスタンスを設定
        if (Instance == null)
        {
            Instance = this;
            // シーンを切り替えてもこのオブジェクトを破棄しないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既にインスタンスが存在する場合は、新しいインスタンスを破棄
            Destroy(gameObject);
        }
    }
    // ゴールUIを管理するGameObject
    public GameObject goalUI;

    // ゴール時に呼び出されるメソッド
    public void OnGoal()
    {
        // 共通速度を0にして、ゲームを停止させる
        currentSpeed = 0;

        // ゴールUIを表示
        goalUI.SetActive(true);
    }

    // 再挑戦ボタンが押された時に呼び出されるメソッド
    public void Retry()
    {
        // 現在のシーンを再ロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // UIを非表示にする
        goalUI.SetActive(false);
    }
}
