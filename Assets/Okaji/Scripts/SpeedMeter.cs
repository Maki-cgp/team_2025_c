using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    public GameObject meter1;
    public GameObject meter2;
    public GameObject meter3;
    public GameObject meter4;
    public GameObject meter5;
    public GameObject metermax;

    void Update()
    {
        //メーターを表示させる処理
        if (GameManager.Instance.currentSpeed >= 5.0f)  //速度に応じて表示される
        {
            meter1.SetActive(true);
            if (GameManager.Instance.currentSpeed >= 15.0f)
            {
                meter2.SetActive(true);
                if (GameManager.Instance.currentSpeed >= 25.0f)
                {
                    meter3.SetActive(true);
                    if (GameManager.Instance.currentSpeed >= 35.0f)
                    {
                        meter4.SetActive(true);
                        if (GameManager.Instance.currentSpeed >= 45.0f)
                        {
                            meter5.SetActive(true);
                            if (GameManager.Instance.currentSpeed >= 50.0f)
                            {
                                metermax.SetActive(true);
                            }
                        }
                    }
                }
            }
        }

        //メーターを非表示にする処理
        if (GameManager.Instance.currentSpeed < 50.0f)  //速度に応じて非表示に
        {
            metermax.SetActive(false);
            if (GameManager.Instance.currentSpeed < 45.0f)
            {
                meter5.SetActive(false);
                if (GameManager.Instance.currentSpeed < 35.0f)
                {
                    meter4.SetActive(false);
                    if (GameManager.Instance.currentSpeed < 25.0f)
                    {
                        meter3.SetActive(false);
                        if (GameManager.Instance.currentSpeed < 15.0f)
                        {
                            meter2.SetActive(false);
                            if (GameManager.Instance.currentSpeed < 5.0f)
                            {
                                meter1.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
