using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{


    public GameObject FPSController;
    [Header("Empty GameObject")]
    public GameObject AnchorPoint;
    Transform Cam;

    void Awake()
    {
        Cam = FPSController.GetComponentInChildren<Camera>().transform;
    }

    //Camera movement
    Vector2 m_StartPos;

    public void OnDrag(PointerEventData eventData)
    {

        var offset = eventData.position - m_StartPos;

        Cam.rotation = Quaternion.Euler((AnchorPoint.transform.rotation.eulerAngles + new Vector3(-offset.y, offset.x, 0) * 0.2f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_StartPos = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        AnchorPoint.transform.rotation = Cam.rotation;
    }

}
