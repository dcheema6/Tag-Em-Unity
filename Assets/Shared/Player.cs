using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    protected Shooter weapon;
    public string DisplayText;

    public int Score;

    protected virtual void Awake()
    {
        GameManager.Instance.Player = this;
    }

    protected virtual void Start()
    {
        Score = 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool isMyTurn()
    {
        return weapon.canFire;
    }

    public void SetTurn(bool turn)
    {
        if (!turn && weapon.canFire)
            Score++;

        weapon.canFire = turn;
    }

    public virtual void TakeHit()
    {
        GameManager.Instance.TurnChanged(this);
        SetTurn(true);
    }

    public virtual void AIHelper(Transform p)
    {
        return;
    }
}
