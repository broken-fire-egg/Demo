using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public Queue<IEnumerator> CoroutineQueue;
    public int PatternCount;
    public Transform[] MapVertex;
    public HitEffectHPGage hpGage;
    protected Animator animator;
    protected Rigidbody2D rigid;
    protected int prev;
    protected IEnumerator NextPattern;
    protected IEnumerator CurrentPattern;
    protected float hit_effect_cooltime;
    protected float max_hp;
    protected float hp;
    // Start is called before the first frame update
    protected void Awake()
    {
        animator = GetComponent<Animator>();
        TryGetComponent(out rigid);
    }
    protected void Start()
    {
        CoroutineQueue = new Queue<IEnumerator>();
        RandomizePattern();
    }
    protected void Update()
    {
        hit_effect_cooltime -= Time.deltaTime;
    }
    protected void RandomizePattern(int _prev = -1)
    {
        if (_prev == -1)
        {
            prev = Random.Range(0, PatternCount);
            CoroutineQueue.Enqueue(GetPattern(prev));
        }
        else
        {
            do
            {
                prev = Random.Range(0, PatternCount);
                CoroutineQueue.Enqueue(GetPattern(prev));
            } while (_prev == prev) ;
        }
    }
    // Update is called once per frame
    protected virtual IEnumerator GetPattern(int num)
    {
        return null;
    }
    public virtual float GetHPGage()
    {
        return hp / max_hp * 950f;

    }
    protected IEnumerator CheckQueue()
    {
        while(true)
        {
            if (CoroutineQueue.Count <= 0)
            {
                RandomizePattern(prev);
                yield return null;
            }
            else
            {
                CurrentPattern = CoroutineQueue.Dequeue();
                yield return CurrentPattern;
            }
        }
    }

    public virtual void SendMessageToBoss(string msg)
    {
        Debug.Log(msg);
    }
    public void HitEffect()
    {
        if(hit_effect_cooltime <= 0f)
        {
            hit_effect_cooltime = 1.5f;
            hpGage.val = 255f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<HeroBullet>(out var heroBullet))
        {
            hp -= heroBullet.damage;
            HitEffect();
        }
    }
}
