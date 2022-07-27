using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour
{
    //Spawning character & cursor
    [SerializeField] private GameObject characterPrefab;
    private CharacterInfo character;
    [SerializeField] private GameObject cursor;

    //Pathfinding Fields
    private PathFinder pathFinder;
    [SerializeField] private List<OverlayTile> path;
    [SerializeField] private float speed = 1f;

    void Start()
    {
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
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
                    character.currentTile = overlayTile;
                }
                else
                {
                    path = pathFinder.FindPath(character.currentTile, overlayTile);
                }
                overlayTile.GetComponent<OverlayTile>().ShowTile();
            }
        }

        if (path.Count > 0)
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        var step = Time.deltaTime * speed;

        var zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
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
