using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // Inspectorで設定する変数
    [SerializeField] private GameObject[] MessageObj; // 判定メッセージのPrefab配列
    [SerializeField] private NotesManager notesManager; // NotesManagerの参照

    void Update()
    {
        // notesManagerが設定されており、かつ処理すべきノーツが1つ以上存在するか確認
        if (GManager.instance.Start && notesManager != null && notesManager.NotesTime.Count > 0)
        {
            // --- プレイヤー入力の処理 ---
            // プレイヤー入力に応じて ProcessInput を呼び出す。
            // ここでノーツが叩かれると、リストからデータが削除される。
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                ProcessInput(0); // レーン0を処理
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                ProcessInput(1); // レーン1を処理
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ProcessInput(2); // レーン2を処理
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ProcessInput(3); // レーン3を処理
            }

            // ★ 修正点: 入力処理後にリストが空になっていないか再チェックする
            if (notesManager.NotesTime.Count == 0)
            {
                return; // 既にノーツが処理されていたら、Miss判定はスキップ
            }

            // --- 時間経過によるMiss判定 (インデックス0のノーツのみ対象) ---
            
            // 現在の時間
            float currentTime = Time.time;
            // リストの先頭にあるノーツの判定時間
            // 41行目: ★ ここでエラーが発生していたため、直前でガード句を追加した
            float noteTiming = notesManager.NotesTime[0] + GManager.instance.StartTime; 

            // 本来ノーツをたたくべき時間から0.2秒過ぎても入力がなかった場合 (Miss判定)
            if (currentTime > noteTiming + 0.2f)
            {
                message(3); // Missメッセージを表示
                deleteData(0); // リストの先頭を削除 (Miss処理)
                Debug.Log("Miss");
                GManager.instance.miss++;
                GManager.instance.combo = 0;
            }
        }
    }
    
    /// <summary>
    /// プレイヤー入力に応じて、該当レーンのノーツを検索し判定を行う
    /// </summary>
    void ProcessInput(int pressedLane)
    {
        // リストの先頭から順に、押されたレーンと一致するノーツを探す
        for (int i = 0; i < notesManager.LaneNum.Count; i++)
        {
            // 押されたレーンとノーツのレーンが一致するか
            if (notesManager.LaneNum[i] == pressedLane)
            {
                // ノーツの時間と入力時間の差を計算
                float noteTiming = notesManager.NotesTime[i] + GManager.instance.StartTime;
                float timeLag = GetABS(Time.time - noteTiming);
                
                // 判定時間内にあるか確認 (0.2秒以内)
                if (timeLag <= 0.20f)
                {
                    // 判定ロジックを呼び出し、判定されたノーツのインデックスを渡す
                    Judgement(timeLag, i);
                }
                
                // 1つのレーンにつき、最も早いノーツ（インデックスが小さいノーツ）だけを判定対象とする
                return; 
            }
        }
    }


    /// <summary>
    /// タイミングのズレに応じて判定を決定する
    /// </summary>
    void Judgement(float timeLag, int noteIndex)
    {
        // Perfect判定
        if (timeLag <= 0.10f)
        {
            Debug.Log("Perfect");
            message(0, notesManager.LaneNum[noteIndex]);
            GManager.instance.perfect++;
            GManager.instance.combo++;
        }
        // Great判定
        else if (timeLag <= 0.15f)
        {
            Debug.Log("Great");
            message(1, notesManager.LaneNum[noteIndex]);
            GManager.instance.great++;
            GManager.instance.combo++;
        }
        // Bad判定
        else // (0.15f < timeLag <= 0.20f)
        {
            Debug.Log("Bad");
            message(2, notesManager.LaneNum[noteIndex]);
            GManager.instance.bad++;
            GManager.instance.combo = 0;
        }

        // 判定が確定したので、ノーツデータを削除
        deleteData(noteIndex);
    }

    /// <summary>
    /// 引数の絶対値を返す
    /// </summary>
    float GetABS(float num)
    {
        return Mathf.Abs(num);
    }

    /// <summary>
    /// 処理したノーツのデータを指定したインデックスから削除する
    /// </summary>
    void deleteData(int indexToRemove)
    {
        // 判定済みのノーツのゲームオブジェクトをシーンから破棄
        Destroy(notesManager.NotesObj[indexToRemove]);

        // 全ての関連リストから指定されたインデックスの要素を削除
        notesManager.NotesObj.RemoveAt(indexToRemove);
        notesManager.NotesTime.RemoveAt(indexToRemove);
        notesManager.LaneNum.RemoveAt(indexToRemove);
        notesManager.NoteType.RemoveAt(indexToRemove);
    }

    /// <summary>
    /// 判定メッセージを生成する (Perfect/Great/Bad用: レーンインデックスを指定)
    /// </summary>
    void message(int judge, int laneIndex)
    {
        if (judge < MessageObj.Length && MessageObj[judge] != null)
        {
            // 判定時のノーツ位置にメッセージを生成
            Instantiate(MessageObj[judge], new Vector3(laneIndex - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
        }
    }
    
    /// <summary>
    /// 判定メッセージを生成する (Miss用: リストの先頭のレーン情報を使用)
    /// </summary>
    void message(int judge)
    {
        // Miss判定のときは、リストの先頭（index 0）のレーン情報を使う
        int laneIndex = notesManager.LaneNum[0];
        
        if (judge < MessageObj.Length && MessageObj[judge] != null)
        {
            // 判定時のノーツ位置にメッセージを生成
            Instantiate(MessageObj[judge], new Vector3(laneIndex - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
        }
    }
}