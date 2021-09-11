using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [Header("Patience Bar")]
    [SerializeField] private GameObject patienceBarUIContainer;

    [Header("Orders")]
    [SerializeField] private GameObject orderUIContainer;
    [SerializeField] private Image orderIndicatorImage;

    public void UpdateUI(MealType desiredMeal, bool isReadyToOrder)
    {
        // TODO: After deciding order, show Ready image and start timer
        var path = "ScriptableObjects/DesiredMealUI/";
        if (!isReadyToOrder)
        {
            path += "_Thinking";
        }
        else
        {
            if (desiredMeal == MealType.None)
            {
                path += "_Ready";
            }
            else
            {
                path += desiredMeal.ToString();
            }
        }
        var mealUIData = Resources.Load<DesiredMealUI_SO>(path);
        orderIndicatorImage.sprite = mealUIData.mealSprite;
        orderUIContainer.SetActive(true);
    }

    public void HideUI()
    {
        patienceBarUIContainer.SetActive(false);
        orderUIContainer.SetActive(false);
    }
}
