using UnityEngine;

public class Note : MonoBehaviour
{
    // ノーツの落下速度
    public float speed = 5f;

    void Update()
    {
        // 毎フレーム下方向に移動
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // 一定位置より下に落ちたら削除
        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }
}
