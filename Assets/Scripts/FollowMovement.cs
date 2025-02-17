using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    public GameObject Target;
    [SerializeField] private float speed; 

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
    }
}
