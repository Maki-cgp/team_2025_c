using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    // UIのTextコンポーネント
    public GameObject time;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeScore;

    public GameObject item;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemScore;

    public GameObject shield;
    public TextMeshProUGUI shieldScore;

    public GameObject noDamage;
    public TextMeshProUGUI noDamageScore;

    public GameObject sum;
    public TextMeshProUGUI sumScore;

    public GameObject fukidashi;
    public TextMeshProUGUI message;

    public GameObject retryButton;
    public GameObject endButton;

    // ゴールUIを管理するGameObject
    public GameObject resultUI;

    // TimeManagerの参照
    public TimeManager timeManager;
    private float timeInSeconds;
    private float sumMath;

    // リザルト音声
    private AudioSource audioSource;
    public AudioClip drumRoll;
    public AudioClip fanfare;
    public AudioClip resultSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ゴール時に呼び出されるメソッド
    public void OnGoal()
    {
        // タイムスコア
        timeInSeconds = timeManager.GetTimeElapsed();   // 経過時間を取得
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);   // クリアタイムを表示

        timeScore.text = string.Format(30000 - 100 * Mathf.Ceil(timeInSeconds) + "点");   // スコアを表示

        // アイテムボーナス
        itemText.text = string.Format("×" + GameManager.Instance.itemCount);    //取得アイテム数を表示
        itemScore.text = string.Format(1000 + "点 ×" + GameManager.Instance.itemCount);    //アイテムのスコアを表示

        // シールドボーナス
        if (ActionPlayer.shield)    // シールドの有無
        {
            shieldScore.text = string.Format(3000 + "点");
        }

        // ノーダメージ
        if (Enemy.nodamage)
        {
            noDamageScore.text = string.Format(5000 + "点");
        }

        // 合計得点
        sumMath = (30000 - 100 * Mathf.Ceil(timeInSeconds)) + (1000 * GameManager.Instance.itemCount);
        if (ActionPlayer.shield)
        {
            sumMath += 3000;
        }
        if (Enemy.nodamage)
        {
            sumMath += 5000;
        }
        sumScore.text = string.Format(sumMath + "点");

        // コルーチンを使って遅延処理
        StartCoroutine(DelayedAction());
    }

    IEnumerator DelayedAction()
    {
        // ゴール演出中待機
        yield return new WaitForSeconds(2.5f);

        resultUI.SetActive(true);   // リザルト表示
        GameManager.Instance.audioSource.clip = drumRoll;
        GameManager.Instance.audioSource.Play();

        yield return new WaitForSeconds(0.5f);  // 時間
        time.SetActive(true);
        audioSource.PlayOneShot(resultSound);

        yield return new WaitForSeconds(0.5f);  // アイテム
        item.SetActive(true);
        audioSource.PlayOneShot(resultSound);

        yield return new WaitForSeconds(0.5f);  // シールド
        shield.SetActive(true);
        audioSource.PlayOneShot(resultSound);

        yield return new WaitForSeconds(0.5f);  // ノーダメ
        noDamage.SetActive(true);
        audioSource.PlayOneShot(resultSound);

        yield return new WaitForSeconds(1.4f);  // 合計
        sum.SetActive(true);

        yield return new WaitForSeconds(1.0f);  // 最後にファンファーレ
        GameManager.Instance.audioSource.clip = fanfare;
        GameManager.Instance.audioSource.loop = true;
        GameManager.Instance.audioSource.Play();
        fukidashi.SetActive(true);
        retryButton.SetActive(true);
        endButton.SetActive(true);

        // スコアごとに演出分岐
        if (sumMath >= 30000)
        {
            message.text = string.Format("スゴイ!");
        }
        else if (20000 <= sumMath && sumMath < 30000)
        {
            message.text = string.Format("ソコソコ!");
        }
        else
        {
            message.text = string.Format("ガンバロウ!");
        }
    }

    // 再挑戦ボタンが押された時に呼び出されるメソッド
    public void Retry()
    {
        // 現在のシーンを再ロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // UIを非表示にする
        resultUI.SetActive(false);
    }
}
