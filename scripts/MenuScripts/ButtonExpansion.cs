using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class ButtonExpansion : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    private bool isToBeExpanded = false;
    [SerializeField] private float xScale=1f;
    private void OnDisable()
    {
        isToBeExpanded = false;
    }
    private void Update()
    {
        if (!isToBeExpanded)
        {
            if (gameObject.transform.localScale.x > xScale)
            {
                gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0);
            }
            return;
        }

        if(gameObject.transform.localScale.x < xScale+ xScale*0.2f)
        {
            gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0);
        }
    }
    public void OnPointerEnter (PointerEventData eventData)
    {
        isToBeExpanded = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isToBeExpanded = false;
    }
}
