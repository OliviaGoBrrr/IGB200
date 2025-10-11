using UnityEngine;

public class Bird : MonoBehaviour
{
    float Speed = 0;
    void Start()
    {
        
        Destroy(gameObject, 20);
    }
    public void SetValues(float speed, bool moveRight, float offset)
    {
        if (!moveRight) 
        {
            transform.rotation = Quaternion.Euler(-30, 180, transform.rotation.z);
        }
        Speed = speed;
        transform.position += new Vector3(0, 0, offset);
    }
    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }
}
