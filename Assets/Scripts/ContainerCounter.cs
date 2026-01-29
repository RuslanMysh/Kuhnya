using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;



    public override void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);

        }
        else
        {
            //отдаём объект игроку в руки
            kitchenObject.SetKitchenObjectParent(player);
        }

    }

  
}
