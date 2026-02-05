using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;

    private const string OPENCLOSE = "OpenClose";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObjects;
    }

    private void ContainerCounter_OnPlayerGrabbedObjects(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPENCLOSE);
    }
}
