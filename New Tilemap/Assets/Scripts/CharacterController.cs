using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] internal Vector2Int pos;
    private float speed = 2f;

    private void Start() 
    {
        PositionCharacterOnNode(0, 0);
    }

    private void Update()
    {
        if (MapManager.Instance.path.Count > 0)
        {
            FollowPath();
        }

        // if (currentPathNode == null)
        // {
        //     if (Input.GetMouseButtonDown(1))
        //     {
        //         var pos = MapManager.Instance.floor.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //         PositionCharacterOnNode(pos.x, pos.y);
        //     }
        // }
        //Ctrl + / по выделенному тексту
    }

    private void FollowPath()
    {
        var path = MapManager.Instance.path;
        var step = Time.deltaTime * speed;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[0].x + 0.5f, path[0].y + 0.5f), step);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.position == new Vector3(path[0].x + 0.5f, path[0].y + 0.5f))
        {
            PositionCharacterOnNode(path[0].x, path[0].y);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnNode(int x, int y) //Закрепляет позицию персонажа на клетке, на всякий случа и чтобы PathFindin знал откуда строить путь
    {
        transform.position = new Vector3(x + 0.5f, y + 0.5f);
        pos = new Vector2Int(x, y);
    }
}
