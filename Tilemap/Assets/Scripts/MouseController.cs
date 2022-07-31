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
    [SerializeField] private List<PathNode> path;
    [SerializeField] private float speed = 1f;

    void Start()
    {
        pathFinder = new PathFinder();
        path = new List<PathNode>();
    }

    private void LateUpdate() //Has sex with u'r mum
    {
        RaycastHit2D? hit = GetFocusedOnNode();

        if (hit.HasValue)
        {
            PathNode pathNode = hit.Value.collider.gameObject.GetComponent<PathNode>();
            cursor.transform.position = pathNode.transform.position;

            if (Input.GetMouseButtonDown(1))
            {

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnNode(pathNode);
                    character.currentNode = pathNode;
                }
                else
                {
                    path = pathFinder.FindPath(character.currentNode, pathNode);
                }
                pathNode.GetComponent<PathNode>().ShowNode();
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
            PositionCharacterOnNode(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnNode(PathNode pathNode) //Places character on selected tile
    {
        character.transform.position = new Vector3(pathNode.transform.position.x, pathNode.transform.position.y, pathNode.transform.position.z);
        character.currentNode = pathNode;
    }

    private RaycastHit2D? GetFocusedOnNode() //Tile selection method
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
