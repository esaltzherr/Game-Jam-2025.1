using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    public GameObject Target;
    private float Dis;
    [SerializeField] private float speed; 
    [SerializeField] private float distance;

    void Update()
    {
        Dis = Vector2.Distance(transform.position, Target.transform.position);
        
        if(Dis >= distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, 1 * Time.deltaTime);
        }
    }
}
