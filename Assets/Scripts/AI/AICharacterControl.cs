using UnityEngine;


[RequireComponent(typeof(AICharacter))]
public class AICharacterControl : Player
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public AICharacter character { get; private set; }
    public Transform target;
    [SerializeField] uint maxRandomDistance;

    private float stoppingDistance;

    void Start()
    {
        base.Start();

        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<AICharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        stoppingDistance = agent.stoppingDistance;
    }

    void Update()
    {
        if (!agent.enabled)
            return;

        if (isMyTurn() && target != null)
        {
            agent.SetDestination(target.position);
            agent.stoppingDistance = stoppingDistance;
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            setRandomDestination();
            agent.stoppingDistance = 1;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false, true);
        else
            character.Move(target.position - transform.position, false, false, false);
            weapon.Fire();
    }

    void setRandomDestination()
    {
        Vector2 temp = Random.insideUnitCircle * maxRandomDistance;
        agent.SetDestination(new Vector3(temp.x + transform.position.x, transform.position.y, temp.y + transform.position.z));
    }

    public override void AIHelper(Transform t)
    {
        target = t;
    }
}
