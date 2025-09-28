using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public GameObject speedSelectorPanel; // 速度調整画面のパネル
    public static GManager instance = null;

    public int songID;
    public float noteSpeed;

    public bool Start;
    public float StartTime;

    public int combo;
    public int score;

    public int perfect;
    public int great;
    public int bad;
    public int miss;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // 速度調整画面に戻るメソッド
    public void ReturnToSpeedSelector()
    {
        Time.timeScale = 0f; // ゲーム一時停止
        if (speedSelectorPanel != null)
        {
            speedSelectorPanel.SetActive(true); // 速度調整画面を表示
        }
        // 必要ならゲームUIを非表示にする処理も追加
    }
}