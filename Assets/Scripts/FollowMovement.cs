using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    public GameObject Target;
    private float Dis;
    [SerializeField] private float speed; 
    [SerializeField] private float distance;
    [SerializeField] private float heightTolerance = 1f;

    [SerializeField] private float patrolDistance = 3f;

    void Update()
    {
        
        if (Target.transform.position.y - transform.position.y <= heightTolerance)
        {
            FollowPlayer();
        }
        
        else
        {
            Patrol();
        }
    }

    void FollowPlayer()
    {
        Dis = Vector2.Distance(transform.position, Target.transform.position);

        if (Dis >= distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
        }

        else
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, 1 * Time.deltaTime);
        }
    }

    void Patrol()
    {

    }
}
