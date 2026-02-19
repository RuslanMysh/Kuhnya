using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{
    public EventHandler OnRecipeSpawned;
    public EventHandler OnRecipeCompleted;
    public EventHandler OnRecipeSuccess;
    public EventHandler OnRecipeFailed;
    
    public static DeliveryManager Instance { private set; get; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("” нас два Delivery Manager");
        }
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer  -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }
    public void DeliverRecipe(PlateKithcenObject plateKithcenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; ++i)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSoList.Count == plateKithcenObject.GetKitchenObjectSOList().Count)
            {
                bool platesContentsMatchesRecipe = true;
                //кол-во ингредиентов совпадает
                foreach (KitchenObjectSO kitchenObjectSO in waitingRecipeSO.kitchenObjectSoList)
                {
                    bool ingredientFound = false;
                    //пробежимс€ по всем ингредиентам
                    foreach (KitchenObjectSO platekitchenObjectSO in plateKithcenObject.GetKitchenObjectSOList())
                    {
                        //пробегаемс€ по всем ингредиентам в тарелке
                        if (platekitchenObjectSO == kitchenObjectSO)
                        {
                            //ингредиенты совпали
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        //ингр рецепта не был найден на тарелке
                        platesContentsMatchesRecipe = false;
                    }
                }
                if (platesContentsMatchesRecipe)
                {
                    Debug.Log("»грок доставил нужный рецепт!");
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        //—овпадений не найдено
        //игрок доставил не тот рецепт
        Debug.Log("игрок доставил не тот рецепт");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
