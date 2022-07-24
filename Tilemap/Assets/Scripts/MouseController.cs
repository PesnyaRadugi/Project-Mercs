using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject cursor;


    void Start()
    {
        
    }

    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            GameObject overlayTile = hit.Value.collider.gameObject;
            cursor.transform.position = overlayTile.transform.position;

            if (Input.GetMouseButtonDown(1))
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile();
            }
        }
    }

    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D  = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length != 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First(); //тут вот хз, по идее это тупо для того, чтобы тайлы под другими тайлами не подсвечивались
        }

        return null;
    }
}
