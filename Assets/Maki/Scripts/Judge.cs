using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // Inspectorで設定する変数
    [SerializeField] private GameObject[] MessageObj; // 判定メッセージのPrefab配列 (Sizeを4に設定)
    [SerializeField] private NotesManager notesManager; // NotesManagerの参照

    void Update()
    {
        // ゲームが開始されているか確認
        if (GManager.instance.Start)
        {
            // notesManagerが設定されており、かつ処理すべきノーツが1つ以上存在するか確認
            if (notesManager != null && notesManager.NotesTime.Count > 0)
            {
                // Dキーが押されたとき (レーン0)
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (notesManager.LaneNum[0] == 0)
                    {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
                    }
                }
                // Fキーが押されたとき (レーン1)
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (notesManager.LaneNum[0] == 1)
                    {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
                    }
                }
                // Jキーが押されたとき (レーン2)
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (notesManager.LaneNum[0] == 2)
                    {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
                    }
                }
                // Kキーが押されたとき (レーン3)
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (notesManager.LaneNum[0] == 3)
                    {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
                    }
                }

                // 本来ノーツをたたくべき時間から0.2秒過ぎても入力がなかった場合 (Miss判定)
                if (Time.time > notesManager.NotesTime[0] + 0.2f + GManager.instance.StartTime)
                {
                    message(3); // Missメッセージを表示
                    deleteData(); // ノーツ情報を削除
                    Debug.Log("Miss");
                    GManager.instance.miss++;
                    GManager.instance.combo = 0;
                }
            }
        }
    }

    /// <summary>
    /// タイミングのズレに応じて判定を決定する
    /// </summary>
    void Judgement(float timeLag)
    {
        // Perfect判定
        if (timeLag <= 0.10f)
        {
            Debug.Log("Perfect");
            message(0);
            GManager.instance.perfect++;
            GManager.instance.combo++;
            deleteData();
        }
        // Great判定
        else if (timeLag <= 0.15f)
        {
            Debug.Log("Great");
            message(1);
            GManager.instance.great++;
            GManager.instance.combo++;
            deleteData();
        }
        // Bad判定
        else if (timeLag <= 0.20f)
        {
            Debug.Log("Bad");
            message(2);
            GManager.instance.bad++;
            GManager.instance.combo = 0;
            deleteData();
        }
    }

    /// <summary>
    /// 引数の絶対値を返す
    /// </summary>
    float GetABS(float num)
    {
        return Mathf.Abs(num); // Unity標準の絶対値関数を使う方がシンプルです
    }

    /// <summary>
    /// 処理したノーツのデータを全てのリストから削除する
    /// </summary>
    void deleteData()
    {
        // 判定済みのノーツのゲームオブジェクトをシーンから破棄
        Destroy(notesManager.NotesObj[0]);

        // 全ての関連リストから先頭の要素を削除して、情報のズレを防ぐ
        notesManager.NotesObj.RemoveAt(0);
        notesManager.NotesTime.RemoveAt(0);
        notesManager.LaneNum.RemoveAt(0);
        notesManager.NoteType.RemoveAt(0);
    }

    /// <summary>
    /// 判定メッセージを生成する
    /// </summary>
    void message(int judge)
    {
        // MessageObjの範囲外にアクセスしないように安全確認
        if (judge < MessageObj.Length && MessageObj[judge] != null)
        {
            // 判定時のノーツ位置にメッセージを生成
            Instantiate(MessageObj[judge], new Vector3(notesManager.LaneNum[0] - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
        }
    }
}