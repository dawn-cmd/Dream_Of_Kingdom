using UnityEngine;

public class DragArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int pointsCount;
    public float arcModifier;
    private Vector3 mousePos;
    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(new(
            Input.mousePosition.x, 
            Input.mousePosition.y, 
            10
        ));
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mousePos);
    }
}
