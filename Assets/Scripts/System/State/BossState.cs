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
    protected SpriteRenderer spriteRenderer;
    protected Material mat;
    protected int shdpropID;
    protected int prev;
    protected IEnumerator NextPattern;
    protected IEnumerator CurrentPattern;
    protected float hit_effect_glow_time;
    protected float hit_effect_cooltime;
    public float max_hp;
    public float hp;
    // Start is called before the first frame update
    protected void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mat = spriteRenderer.material;
        shdpropID = Shader.PropertyToID("Vector1_1a86493a026c45d681477ade9a8e96e3");
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

        mat.SetFloat(shdpropID, hit_effect_glow_time);

        if (hit_effect_glow_time < 0)
            hit_effect_glow_time = 0;
        else
            hit_effect_glow_time -= 0.05f;
        
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
            hit_effect_cooltime = 0.75f;
            hpGage.val = 255f;
            hit_effect_glow_time = 1;


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
