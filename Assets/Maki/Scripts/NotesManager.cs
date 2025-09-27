using System;
using System.Collections.Generic;
using UnityEngine;

// JSONのトップレベル構造に対応するクラス
[Serializable]
public class Data
{
   public string name;
   public int maxBlock;
   public int BPM;
   public int offset;
   public MusicNote[] notes;
}

// JSONのnotes配列の中身に対応するクラス
[Serializable]
public class MusicNote
{
   public int type;
   public int num;
   public int block;
   public int LPB;
    // 循環参照エラーを防ぐため、"notes": [] は削除
}

public class NotesManager : MonoBehaviour
{
   // Inspectorで設定する項目
    [SerializeField] private float NotesSpeed; // ノーツの落下速度
    [SerializeField] private GameObject noteObj; // ノーツのPrefab

   // 内部で管理するデータ
   public int noteNum;
   public List<int> LaneNum = new List<int>();
   public List<int> NoteType = new List<int>();
   public List<float> NotesTime = new List<float>();
   public List<GameObject> NotesObj = new List<GameObject>();
    
    private float startTime = 0; // 曲の開始時間を記録する変数

    /// <summary>
    /// オブジェクトが有効になった時に一度だけ呼ばれる
    /// </summary>
   void OnEnable()
   {
       // 読み込む譜面のファイル名を入力 (拡張子は不要)
       string songName = "Summer Finale!!";
       Load(songName);
   }

    /// <summary>
    /// JSONファイルを読み込み、ノーツ情報をリストに格納する
    /// </summary>
   private void Load(string songName)
   {
        // 念のため変数を初期化
        noteNum = 0;
        LaneNum.Clear();
        NoteType.Clear();
        NotesTime.Clear();
        NotesObj.Clear();

       // ResourcesフォルダからJSONファイルをTextAssetとして読み込む
       TextAsset textAsset = Resources.Load<TextAsset>(songName);
        if (textAsset == null)
        {
            Debug.LogError("譜面ファイルが見つかりません: " + songName);
            return;
        }

       string inputString = textAsset.ToString();
       Data inputJson = JsonUtility.FromJson<Data>(inputString);

       noteNum = inputJson.notes.Length;

       // 各ノーツの情報を計算してリストに追加
       for (int i = 0; i < inputJson.notes.Length; i++)
       {
           // 時間を計算
            float kankaku = 60f / (inputJson.BPM * (float)inputJson.notes[i].LPB);
            float beatSec = kankaku * (float)inputJson.notes[i].LPB;
            float time = (beatSec * inputJson.notes[i].num / (float)inputJson.notes[i].LPB) + inputJson.offset * 0.01f;
           
           // リストに情報を追加
           NotesTime.Add(time);
           LaneNum.Add(inputJson.notes[i].block);
           NoteType.Add(inputJson.notes[i].type);

           // ノーツのゲームオブジェクトを生成
           GameObject obj = Instantiate(noteObj, new Vector3(inputJson.notes[i].block - 1.5f, 20f, 0f), Quaternion.identity);
            obj.SetActive(false); // 生成直後は非表示にしておく
           NotesObj.Add(obj);
       }
   }

    /// <summary>
    /// 毎フレーム呼ばれる処理
    /// </summary>
   void Update()
   {
       // GManagerのStartフラグがtrueになった瞬間の時間を記録
       if (GManager.instance.Start && startTime == 0)
       {
           startTime = Time.time;
       }

       // ゲームが開始されていなければ何もしない
       if (startTime == 0) return;
        
        // 現在の曲の再生時間を計算
        float currentTime = Time.time - startTime;

       // 全てのノーツをチェックし、位置を更新する
       for (int i = 0; i < noteNum; i++)
       {
           // そのノーツが判定ラインに到達すべき時間
           float timing = NotesTime[i];
            
            // ノーツが画面に表示され始める時間
            float appearTime = timing - (10f / NotesSpeed);
            
            // まだ出現時間になっていないノーツは処理しない
            if (appearTime > currentTime)
            {
                continue;
            }

            // 非表示だったノーツを表示する
            if (!NotesObj[i].activeSelf)
            {
                NotesObj[i].SetActive(true);
            }

            // 判定ライン(Y=0)までの残り時間
           float remainingTime = timing - currentTime;

            // 新しいY座標を計算
            float newY = remainingTime * NotesSpeed;
            
            // ノーツのY座標を更新
            Vector3 pos = NotesObj[i].transform.position;
            NotesObj[i].transform.position = new Vector3(pos.x, newY, pos.z);

            // ノーツがアクティブで、かつ画面外（Y=-8より下）に出たらMiss判定とする
            if (NotesObj[i].activeSelf && NotesObj[i].transform.position.y < -8f)
            {
                // オブジェクトを非表示にする
                NotesObj[i].SetActive(false); 
                
                Debug.Log("Note missed!");
                // TODO: GManagerにMiss判定を記録する処理などを追加
            }
       }
   }
}