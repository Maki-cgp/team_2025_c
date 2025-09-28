using UnityEngine;
using UnityEngine.UI;

public class SpeedSelector : MonoBehaviour
{
    public GameObject speedPanel; // 速度調整UIの親Panel
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedValueText;
    [SerializeField] private NotesManager notesManager; // NotesManager参照

    void Update()
    {
        // 速度調整画面が表示中のみSpaceキーで非表示
        if (speedPanel != null && speedPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            speedPanel.SetActive(false);
            // 必要ならゲーム開始フラグをON
            // GManager.instance.Start = true;
        }
    }

    void Start()
    {
        // Sliderの範囲を1～10に設定
        speedSlider.minValue = 1f;
        speedSlider.maxValue = 10f;
        // 初期値表示
        speedSlider.value = notesManager.NotesSpeed;
        speedValueText.text = speedSlider.value.ToString("0.0");
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);
    }

    void OnSpeedChanged(float value)
    {
        notesManager.NotesSpeed = value;
        speedValueText.text = value.ToString("0.0");
    }
}
