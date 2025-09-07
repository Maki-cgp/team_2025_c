using UnityEngine;
using UnityEngine.InputSystem;

public class ActionPlayer : MonoBehaviour
{
    public float distance = 2.7f;
    public float minY = -2.7f; // 最小のY座標
    public float maxY = 0f;  // 最大のY座標

    void Update()
    {
        // 上矢印キーが押された瞬間に上に移動
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
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
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= distance;
            // 新しいY座標が最小値を下回らないように制限
            if (newPosition.y >= minY)
            {
                transform.position = newPosition;
            }
        }
    }
}