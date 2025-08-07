using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Burnable : MonoBehaviour
{
    float burnTime = 3.0f;
    [SerializeField]
    private float burnInterval = 0.3f;
    public GameObject BurntTile;
    public BoxCollider tileCollider;
    public bool isBurning;
    public Vector3 spreadDirection;

    private void Awake()
    {
        tileCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
        if (isBurning)
        {
            burnInterval -= Time.deltaTime;

            if (burnInterval <= 0)
            {
                burnInterval = 0.3f;
                Spread();
            }
        }
    }

    void Spread()
    {
        Vector3 tileSpreadDirection = tileCollider.center + spreadDirection;
    }

    void Burnt()
    {

    }
}
