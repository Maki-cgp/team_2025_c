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
}

public class NotesManager : MonoBehaviour
{
    // Inspectorで設定する項目
    public float NotesSpeed; // ノーツの落下速度 (例: 10.0f)
    [SerializeField] private GameObject noteObj; // ノーツのPrefab
    [SerializeField] private float offset; //オフセット

    // 内部で管理するデータ
    public int noteNum; // JSON読み込み時の総ノーツ数（参考用）
    public List<int> LaneNum = new List<int>();
    public List<int> NoteType = new List<int>();
    public List<float> NotesTime = new List<float>();
    public List<GameObject> NotesObj = new List<GameObject>();

    private float startTime = 0; // 曲の開始時間を記録する変数 (GManagerから取得される)

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
        // 変数を初期化
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
            float time = (beatSec * inputJson.notes[i].num / (float)inputJson.notes[i].LPB) + offset * 0.01f;

            // リストに情報を追加
            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            // ノーツのゲームオブジェクトを生成
            // 初期Y座標は判定ライン(0)にNotesSpeedで到達する時間に合わせて適当に高く設定（ここでは20fを仮定）
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
            GManager.instance.StartTime = startTime; // GManagerにstartTimeを記録
        }

        // ゲームが開始されていなければ何もしない
        if (startTime == 0) return;

        // 現在の曲の再生時間を計算
        float currentTime = Time.time - startTime;

        // 全てのノーツをチェックし、位置を更新する
        // ★ 修正点: NotesTime.Count を使用し、削除されたノーツの影響を受けないようにする
        for (int i = 0; i < NotesTime.Count; i++)
        {
            // そのノーツが判定ラインに到達すべき時間
            float timing = NotesTime[i];

            // 画面上にノーツが表示されるべき時間（例: 判定ラインのY=0からY=20の距離をNotesSpeedで割る）
            // Load時の初期Y座標と合わせる
            float distanceToLine = 20f; 
            float appearTime = timing - (distanceToLine / NotesSpeed);

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

            // 新しいY座標を計算 (remainingTime * NotesSpeed)
            float newY = remainingTime * NotesSpeed;

            // ノーツのY座標を更新
            Vector3 pos = NotesObj[i].transform.position;
            NotesObj[i].transform.position = new Vector3(pos.x, newY, pos.z);

            /*
            // Note: Judge.csがノーツの削除を担当するため、
            // ここでの画面外判定による削除ロジックはJudge.csのMiss判定と重複し、
            // 処理が複雑になるためコメントアウトします。
            // Miss判定はJudge.csの Time.time > NotesManager.NotesTime[0] + 0.2f + GManager.instance.StartTime
            // の部分に任せるのが安全です。
            */
        }
    }
}