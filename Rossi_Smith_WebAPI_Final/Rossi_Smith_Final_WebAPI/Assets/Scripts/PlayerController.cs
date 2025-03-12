using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    float speed = 5.0f;
    public bool isFrozen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        if (isFrozen) return;

        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, moveY);
    }
}
