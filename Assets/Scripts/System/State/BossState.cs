using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public Queue<IEnumerator> CoroutineQueue;
    public int PatternCount;
    public Transform[] MapVertex;
    protected Animator animator;
    protected Rigidbody2D rigid;
    protected int prev;
    protected IEnumerator NextPattern;
    protected IEnumerator CurrentPattern;
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

}
