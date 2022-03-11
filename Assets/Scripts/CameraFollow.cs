
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public MovePlayer player;
         
    public float smoothSpeed = 0.125f;
    public Vector3 defaultOffset;
    public Vector3 combatOffset;
    public Vector3 offset;
    public float zoomSpeed = 3f;
    private void Start()
    {
        offset = defaultOffset;
    }
    private void LateUpdate()
    {
        Vector3 newOffset = defaultOffset;
        if(player.currentOpponent != null)
        {
            newOffset = combatOffset;
        }
        offset = Vector3.Lerp(offset, newOffset, Time.deltaTime * zoomSpeed);

        transform.position = target.position + offset;
    }


}
