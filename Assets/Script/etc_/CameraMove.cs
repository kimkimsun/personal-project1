using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(player1.position, player2.position, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}