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
        animator.SetTrigger("Close");
        isOpen = false;
    }

    public void Open(Transform player)
    {
        if (Vector3.Distance(transform.position + transform.forward, player.position) > Vector3.Distance(transform.position - transform.forward, player.position))
        {
            animator.SetTrigger("OpenIn");
            isOpen = true;
        }
        else
        {
            animator.SetTrigger("OpenOut");
            isOpen = true;
        }
    }

    public void Open()
    {
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
