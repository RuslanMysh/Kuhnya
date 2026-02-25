using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

  
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //не тумбе ничего не лежит
            if (player.HasKitchenObject())
            {
                //у игрока есть в руках объект кладём на тумбу
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //у игрока есть объект
            if (player.HasKitchenObject())
            {
                Debug.LogError("у игрока есть объект");
                //у игрока уже есть объект в руках и это тарелка
                if (player.GetKitchenObject().TryGetPlate(out PlateKithcenObject plateKithcenObject))
                {
                    Debug.LogError("у игрока уже есть объект в руках и это тарелка");
                    //игрок держит тарелку
                    if (plateKithcenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        Debug.LogError("игрок держит тарелку");
                        GetKitchenObject().DestroySelf();
                    }
                    
                }
                else
                {
                    //игрок держит объект, но не тарелку
                    if (GetKitchenObject().TryGetPlate(out plateKithcenObject))
                    {
                        //на тумбе есть тарелка
                        if (plateKithcenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            
            else
            {
                //у игрока ничего нет в руках
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }



}
