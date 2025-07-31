using UnityEngine;

public class Door : Interactable
{
    private Animator animator;
    private bool isOpen = false;

    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private bool isOpenByDefault;

    public override string UseText => isOpen ? "Close" : "Open" + (isLocked ? " (Locked)" : "");

    void Start()
    {
        animator = GetComponent<Animator>();
        if (isOpenByDefault)
            Open();
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
    }

    public void Open(Transform player)
    {
        if (Vector3.Distance(transform.position + transform.forward, player.position) > Vector3.Distance(transform.position - transform.forward, player.position))
        {
            if (animator != null)
                animator.SetTrigger("OpenIn");
            isOpen = true;
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("OpenOut");
            isOpen = true;
        }
    }

    public void Open()
    {
        if (animator != null)
            animator.SetTrigger("OpenIn");
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
}
