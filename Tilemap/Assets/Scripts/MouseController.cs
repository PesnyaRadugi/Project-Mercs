using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MouseController : MonoBehaviour
{

    void Start()
    {
        
    }

    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
    
        if (focusedTileHit.HasValue)
        {
            GameObject overlayTile = focusedTileHit.Value.collider.gameObject;
            transform.position = overlayTile.transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile();
            }
        }
    }

    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
    
        RaycastHit2D[]  hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        } 

        return null;
    }

}
