using UnityEngine;
using UnityEngine.Events;

public class Door : Interactable
{
    private Animator animator;
    private bool isOpen = false;

    [SerializeField]
    private UnityEvent OnOpen;
    [SerializeField] 
    private UnityEvent OnClose;

    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private bool isOpenByDefault;

    private bool isLockedByDefault;

    public override string UseText => isOpen ? "Close" : "Open" + (isLocked ? " (Locked)" : "");

    void Start()
    {
        isLockedByDefault = isLocked;
        animator = GetComponent<Animator>();
        if (isOpenByDefault)
            Open();
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetDoor;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetDoor;
    }

    public override void Interact(Transform player)
    {
        if (isLocked)
            return;

        base.Interact(player);

        if (isOpen)
        {
            Close();
        }
        else
        {
            Open(player);
        }
    }

    public void Close()
    {
        if(animator != null)
            animator.SetTrigger("Close");
        isOpen = false;
        OnClose?.Invoke();
    }

    public void Open(Transform player)
    {
        if (Vector3.Distance(transform.position + transform.forward, player.position) > Vector3.Distance(transform.position - transform.forward, player.position))
        {
            if (animator != null)
                animator.SetTrigger("OpenIn");
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("OpenOut");
        }

        OnOpen?.Invoke();
        isOpen = true;
    }

    public void Open()
    {
        if (animator != null)
            animator.SetTrigger("OpenIn");

        OnOpen?.Invoke();
        isOpen = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void Lock()
    {
        isLocked = true;
    }

    private void ResetDoor()
    {
        isLocked = isLockedByDefault;

        if (isOpenByDefault && !isOpen)
            Open();
        else if (!isOpenByDefault && isOpen)
            Close();
    }
}
