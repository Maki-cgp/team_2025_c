using UnityEngine;
using UnityEngine.SceneManagement;

public class PoseManager : MonoBehaviour
{
    public GameObject poseUI;
    public GameObject poseButton;
    public GameObject retryMenuButton;
    public GameObject selectMenuButton;
    public float speedMemory = 10f;

    // ポーズボタンが押されたときに呼び出されるメソッド
    public void Pose()
    {
        // 速度を記録し、ゲームを停止
        poseButton.SetActive(false);
        speedMemory = GameManager.Instance.currentSpeed; 
        GameManager.Instance.currentSpeed = 0; 
        GameManager.Instance.isMoving = false;

        poseUI.SetActive(true);
        retryMenuButton.SetActive(true);
        selectMenuButton.SetActive(true);
    }

    // 再挑戦ボタンが押された時に呼び出されるメソッド
    public void PoseRetry()
    {
        // シールド無効
        ActionPlayer.shield = false;
        
        // 現在のシーンを再ロード
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.name);

        // UIを非表示にする
        poseUI.SetActive(false);
    }

    // ステージ選択に戻るボタンが押されたときに呼び出されるメソッド
    public void PoseStage()
    {
        SceneManager.LoadScene(7);

        // UIを非表示にする
        poseUI.SetActive(false);
    }
}
