using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public Queue<IEnumerator> CoroutineQueue;
    public int PatternCount;

    protected int prev;
    protected IEnumerator NextPattern;
    protected IEnumerator CurrentPattern;
    // Start is called before the first frame update
    protected void Start()
    {
        CoroutineQueue = new Queue<IEnumerator>();
        RandomizePattern();
        StartCoroutine(CheckQueue());
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
    IEnumerator CheckQueue()
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

}
