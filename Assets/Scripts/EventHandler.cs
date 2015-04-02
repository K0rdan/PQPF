using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MyExtensions;

public class EventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler
{
    public static GameObject catBeingDragged;
    private GameObject tooltip;
    private Vector3 startPosition;
    private Vector3 screenPoint;
    private Vector3 offset;

    public string catName;
    public int catHP;

    void Update()
    {
        // TEST Bulle
        if(bulle != null)
        {
            if (timerBulle == 0)
                timerBulle = Time.time;
            else
            {
                if((Time.time - timerBulle) >= 5)
                {
                    GameObject.Destroy(bulle);
                    bulle = null;
                }
            }
        }
        //
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        GameObject.Destroy(tooltip);                                    // Suppression du Tooltip
        //
        catBeingDragged = gameObject;
        startPosition = this.transform.position;
        //
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float marginVertical = 0, marginHorizontal = 0;
        // Verticalité de la Tooltip
        if (gameObject.transform.localPosition.y < 0)
        {
            // Horizontalité de la Tooltip
            if (gameObject.transform.localPosition.x < 0)
            {
                marginVertical = -30;
                marginHorizontal = 15;
                tooltip = GameObject.Instantiate(Resources.Load("Prefabs/GUI/TooltipSO")) as GameObject;
            }
            else
            {
                marginVertical = -30;
                marginHorizontal = 15;
				tooltip = GameObject.Instantiate(Resources.Load("Prefabs/GUI/TooltipSE")) as GameObject;
            }
        }
        else
        {
            // Horizontalité de la Tooltip
            if (gameObject.transform.localPosition.x < 0)
            {
                marginVertical = 15;
                marginHorizontal = -30;
				tooltip = GameObject.Instantiate(Resources.Load("Prefabs/GUI/TooltipNO")) as GameObject;
            }
            else
            {
                marginVertical = 15;
                marginHorizontal = -30;
				tooltip = GameObject.Instantiate(Resources.Load("Prefabs/GUI/TooltipNE")) as GameObject;
            }
        }

        tooltip.transform.SetParent(GameObject.Find("Panel_PopCats").transform, false);

        var rect = tooltip.GetComponent<RectTransform>();
        rect.SetDefaultScale();

        // Set internal display
        //tooltip.transform.Find("Panel/Tooltip_Name").gameObject.GetComponent<Text>().text = this.catName;
        //tooltip.transform.Find("Panel/Tooltip_HP/Tooltip_HP_Value").gameObject.GetComponent<Text>().text = this.catHP.ToString();
		Text t = tooltip.GetComponentInChildren<Text>();
		t.text = this.catName;
			//
			
        // Set position
        float h = gameObject.GetComponent<RectTransform>().GetHeight();
        float w = gameObject.GetComponent<RectTransform>().GetWidth();
        float posX = 0, posY = 0;

        // Verticalité de la Tooltip
        if (gameObject.transform.localPosition.y < 0)
            posY = gameObject.transform.localPosition.y + (h + tooltip.GetComponent<RectTransform>().GetHeight()) / 2 + marginVertical;
        else
            posY = gameObject.transform.localPosition.y - (h + tooltip.GetComponent<RectTransform>().GetHeight()) / 2 - marginVertical;

        // Horizontalité de la Tooltip
        if (gameObject.transform.localPosition.x < 0)
            posX = gameObject.transform.localPosition.x + (w + tooltip.GetComponent<RectTransform>().GetWidth())/2 + marginHorizontal;
        else
            posX = gameObject.transform.localPosition.x - (w + tooltip.GetComponent<RectTransform>().GetWidth())/2 - marginHorizontal;

        tooltip.transform.localPosition = new Vector3(posX, posY, gameObject.transform.localPosition.z);

		/*Text t = tooltip.GetComponentInChildren<Text> ();
		t.
		tooltip.GetComponentInChildren<Text>().text = "Test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test ";
		*/
		//Canvas.ForceUpdateCanvases(); // Force la mise à jour des canvas pour update le composant RectTransform (qui a été modifié par le text)
		//tooltip.GetComponentInChildren<Canvas>().
	}
	
	public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Destroy(tooltip);
    }

    // TEST BULLE
    private GameObject bulle;
    private float timerBulle;
    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (bulle == null)
        {
            timerBulle = 0;
            float marginVertical = 15, marginHorizontal = -30;
            bulle = GameObject.Instantiate(Resources.Load("Prefabs/BulleNE")) as GameObject;
            bulle.transform.SetParent(GameObject.Find("Panel_PopCats").transform, false);
            bulle.GetComponentInChildren<Text>().text = "Test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test ";

            Canvas.ForceUpdateCanvases(); // Force la mise à jour des canvas pour update le composant RectTransform (qui a été modifié par le text)

            var rect = bulle.GetComponent<RectTransform>();
            rect.SetDefaultScale();

            float posX, posY;
            float h = gameObject.GetComponent<RectTransform>().GetHeight();
            float w = gameObject.GetComponent<RectTransform>().GetWidth();
            Debug.Log(bulle.GetComponent<RectTransform>().GetHeight());
            posY = gameObject.transform.localPosition.y - (h + bulle.GetComponent<RectTransform>().GetHeight()) / 2 - marginVertical;
            posX = gameObject.transform.localPosition.x - (w + bulle.GetComponent<RectTransform>().GetWidth()) / 2 - marginHorizontal;
            bulle.transform.localPosition = new Vector3(posX, posY, gameObject.transform.localPosition.z);
        }
    }*/
    //
}
