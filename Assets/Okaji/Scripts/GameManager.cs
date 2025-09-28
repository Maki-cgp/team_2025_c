using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float currentSpeed = 0f;              // 速度
    public float maxSpeed = 50f;            // 最高速
    public float accelerationRate = 2f;   // 1秒あたりの加速率
    public float decelerationRate = 10f;     // 減速率

    public GameObject startButton;
    public GameObject tutorial;
    public GameObject speedMeter;
    public GameObject poseButton;

    // カウントダウン画像
    public GameObject go;
    public GameObject one;
    public GameObject two;
    public GameObject three;

    public PoseManager poseManager;

    private void Awake()
    {
        // インスタンスがまだ存在しない場合、このインスタンスを設定
        if (Instance == null)
        {
            Instance = this;
            // シーンを切り替えてもこのオブジェクトを破棄しないようにする
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既にインスタンスが存在する場合は、新しいインスタンスを破棄
            // Destroy(gameObject);
        }
    }

    // 速度
    public bool isMoving = false;   // 動いているフラグ

    // BGM
    public AudioClip countDown;
    public AudioClip bgm;
    public AudioClip startbuttonSound;

    public AudioSource audioSource;
    private bool isPlayingFirstSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
         //StartCoroutine(GameStart()); 
    }

    public void OnButtonClick()
    {
        audioSource.PlayOneShot(startbuttonSound);   // ボタン効果音
        StartCoroutine(GameStart());
        startButton.SetActive(false);
    }

    // カウントダウン画像表示
    public IEnumerator GameStart()
    {
        if (tutorial != null)
        {
            tutorial.SetActive(false);
        }

        speedMeter.SetActive(true);
        PlayCountDown();

        yield return new WaitForSeconds(0.2f);
        three.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        three.SetActive(false);
        two.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        two.SetActive(false);
        one.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        one.SetActive(false);
        go.SetActive(true);
        isMoving = true;    // ゲームを動かす
        currentSpeed = poseManager.speedMemory;
        poseButton.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        go.SetActive(false);
    }

    void Update()
    {
        // 最初の効果音が再生中であれば
        if (isPlayingFirstSound)
        {
            // 再生が終了したかどうかをチェック
            if (!audioSource.isPlaying)
            {
                // 最初の効果音のフラグをオフにする
                isPlayingFirstSound = false;
                // BGMを再生
                PlayBGM();
            }
        }

        if (isMoving)
        {
            // 共通の速度を加速させる
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerationRate * Time.deltaTime;
            }
        }
    }

    void PlayCountDown()
    {
        audioSource.clip = countDown;
        audioSource.loop = false;
        audioSource.Play();
        isPlayingFirstSound = true;
    }

    void PlayBGM()
    {
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
    }

    // スコア関連の変数と関数
    public int itemCount = 0;    // アイテム取得数

    // アイテム取得数を加算する関数
    public void ItemCounter()
    {
        itemCount++;
    }
}
