using UnityEngine;

public class FireWarning : MonoBehaviour
{
    private float warningTimer;
    private float warningDuration = 2f;
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (warningTimer > warningDuration)
            {
                gameObject.SetActive(false);

            }
            else
            {
                warningTimer += Time.deltaTime;
            }
        }
    }

    public void ShowWarning() { gameObject.SetActive(true); warningTimer = 0; }
}
