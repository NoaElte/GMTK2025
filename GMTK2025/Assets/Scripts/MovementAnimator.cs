using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovementAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;
    [SerializeField] 
    private Transform pointB;
    [SerializeField]
    private AnimationCurve speedCurve;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private UnityEvent onMoveStart;
    [SerializeField]
    private UnityEvent onMoveEnd;

    [SerializeField]
    private bool bypassReset;

    private bool isMoving = false;
    private Transform currentNextPos;
    private bool reset = false;

    private void Awake()
    {
        currentNextPos = pointB;
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetPosition;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetPosition;
    }

    public void Move()
    {
        if (isMoving)
            return;

        StartCoroutine(MoveOverTime());
    }

    private IEnumerator MoveOverTime()
    {
        isMoving = true;
        reset = false;

        onMoveStart?.Invoke();

        Vector3 startPos = transform.position;
        Vector3 endPos = currentNextPos.position;

        currentNextPos = currentNextPos == pointA ? pointB : pointA;

        float totalDistance = Vector3.Distance(startPos, endPos);
        float traveled = 0;

        while (traveled < totalDistance)
        {
            float step = speed * Time.deltaTime;
            traveled += step;

            float t = Mathf.Clamp01(traveled / totalDistance);
            float curveValue = speedCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, endPos, curveValue);

            if (reset)
            {
                Debug.Log("reset");
                Reset();
                break;
            }

            yield return null;
        }

        if (!reset)
        {
            transform.position = endPos;

            onMoveEnd?.Invoke();
        }

        isMoving = false;
    }

    public void ResetPosition()
    {
        if (bypassReset) 
            return;

        if (isMoving)
        {
            reset = true;
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        transform.position = pointA.position;

        currentNextPos = pointB;
    }
}
