using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    private CharacterInfo character;
    [SerializeField] private GameObject cursor;
    [SerializeField] private float speed = 1;


    void Start()
    {
        
    }

    void LateUpdate() //Has sex with u'r mum
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.transform.position = overlayTile.transform.position;

            if (Input.GetMouseButtonDown(1))
            {

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);   
                }
                overlayTile.GetComponent<OverlayTile>().ShowTile();
            }
        }
    }

    private void PositionCharacterOnTile(OverlayTile overlayTile) //Places character on selected tile
    {
        character.transform.position = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y, overlayTile.transform.position.z);
        character.currentTile = overlayTile;
    }

    private RaycastHit2D? GetFocusedOnTile() //Tile selection method
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D  = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length != 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }
}
