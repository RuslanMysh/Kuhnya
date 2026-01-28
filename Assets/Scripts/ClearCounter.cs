using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private KitchenObject kitchenObject;
  
    public override void Interact(Player player)
    {
        
        
    }



}
