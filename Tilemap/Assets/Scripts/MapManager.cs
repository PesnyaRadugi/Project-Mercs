using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } } //Ну це тупо синглтон, доступ до якого надається через властивість, інкапсуляція хулі


    public Dictionary<Vector2Int, PathNode> map; //Це словник з тайлами, словник це колекція, що приймає 2 параметри: Ключ і Значення, ключ це тип даних, а значення тип прийнятих значень


    [SerializeField] private PathNode pathNodePrefab;  //Це два поля, що задаються в інспекторі (про це сигналізує [SerialzeField]), приймають у себе геймобжект сховища клітин і префаб цих клітин
    [SerializeField] private GameObject pathNodesContainer;


    private void Awake() 
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Singleton of tilemap already exists, replacing it with new...");
            Destroy(this.gameObject); //Жестко надристал полный синглтон говна
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>(); 
        map = new Dictionary<Vector2Int, PathNode>();

        BoundsInt bounds = tileMap.cellBounds; 


        //Пробігаємось по координатах ігрового поля
        for (int z = bounds.max.z; z >= -bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var nodeLoaction = new Vector3Int(x, y, z); //Привласнюємо координати клітинам
                    var nodeKey = new Vector2Int(x, y); //Задаємо ключ із двох координат, щоб потім передати його до словника
                    if (tileMap.HasTile(nodeLoaction) && !map.ContainsKey(nodeKey))
                    {
                        var pathNode = Instantiate(pathNodePrefab, pathNodesContainer.transform); //Закидаємо в змінну префаб клітини та інстанціюємо його в об'єкті TilesContainer
                        var cellWorldPosition = tileMap.GetCellCenterWorld(nodeLoaction);
                        pathNode.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                        pathNode.gridLocation = nodeLoaction;
                        map.Add(nodeKey, pathNode); //Закидаємо все в словник, рядки зверху ^ задають позицію клітин
                    }
                }
            }           
        }
    }

    void Update()
    {
        
    }
}
