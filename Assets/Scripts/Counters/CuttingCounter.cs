using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CurringRecipeSO[] cutKitchenObjectSOArray;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public static event EventHandler OnAnyCut;
    public static CuttingCounter Instance { get; private set; }
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Debug.LogError("Больше одного CuttingCounter");
    //    }
    //}
    private int cuttingProgress;
    /// <summary>
    /// взятие предметов игроком с cuttingCounter
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //на тумбе ничего не лежит
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                }
                // у игрока есть в руках объект
            }
        }
        else
        {
            //на тумбе уже лежит объект
            if (player.HasKitchenObject())
            {
                //у игрока уже есть объект в руках
            }
            else
            {
                //у игрока ничего нет в руках
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
    /// <summary>
    /// нарезка предметов игроком
    /// </summary>
    /// <param name="player"></param>
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CurringRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO cutkitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();


                KitchenObject.SpawnKitchenObject(cutkitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CurringRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO.output;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CurringRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    /// <summary>
    /// принимает объект для нарезки и достаёт из рецепта нарезки
    /// </summary>
    /// <param name="inputKitchenObjectSO"> объект для нарезки</param>
    /// <returns>нарезанная версия</returns>
    private CurringRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CurringRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
