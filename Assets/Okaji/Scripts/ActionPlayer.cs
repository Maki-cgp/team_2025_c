using UnityEngine;
using UnityEngine.InputSystem;

public class ActionPlayer : MonoBehaviour
{
    public float distance = 2.0f;
    public float minY = -2.5f; // 最小のY座標
    public float maxY = 1.5f;  // 最大のY座標

    // アイテム効果が発動しているかどうかのフラグ
    public static bool shield = false;

    // シールド効果発動をわかりやすくする表示
    public GameObject shieldObj;

    void Update()
    {
        // 上矢印キーが押された瞬間に上に移動
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += distance;
            // 新しいY座標が最大値を超えないように制限
            if (newPosition.y <= maxY)
            {
                transform.position = newPosition;
            }
        }

        // 下矢印キーが押された瞬間に下に移動
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= distance;
            // 新しいY座標が最小値を下回らないように制限
            if (newPosition.y >= minY)
            {
                transform.position = newPosition;
            }
        }

        if (shield)
        {
            shieldObj.SetActive(true);
        }
        else
        {
            shieldObj.SetActive(false);
        }
    }
}