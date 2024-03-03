using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Config")]
    public MapConfigSO mapConfig;
    [Header("Map Layout")]
    public MapLayoutSO mapLayout;
    [Header("Prefabs")]
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenWidth;
    private float screenHeight;
    private float columnWidth;
    private Vector3 generatePoint;
    public float border;
    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();
    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / mapConfig.roomBlueprints.Count;
        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }
    
    // private void Start()
    // {
    //     CreateMap();
    // }

    private void OnEnable() {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }
    public void CreateMap()
    {
        List<Room> previousColumnRooms = new();
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max + 1);
            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3(
                -screenWidth / 2 + border + columnWidth * column,
                startHeight,
                0
            );
            var newPosition = generatePoint;

            List<Room> currentColumnRooms = new();

            var roomGapY = screenHeight / (amount + 1);
            for (int i = 0; i < amount; i++)
            {
                newPosition.y = startHeight - roomGapY * i;
                // Make map arrangement more random
                if (column != 0)
                {
                    newPosition.x = generatePoint.x + UnityEngine.Random.Range(-columnWidth / 4, columnWidth / 4);
                }
                // Fix the boss room position
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 1.5f;
                }
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);
                room.roomState = column == 0 ? RoomState.Attainable : RoomState.Locked; 
                room.SetupRoom(column, i, GetRoomData(newType));
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            // if this line is not the first line, connect it with the previous line
            if (previousColumnRooms.Count > 0)
            {
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }
            previousColumnRooms = currentColumnRooms;
        }
        SaveMap();
    }
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new();
        foreach (var room in column1)
        {
            var target = ConnectToRandomRoom(room, column2, true);
            connectedColumn2Rooms.Add(target);
        }
        foreach (var room in column2)
        {
            if (connectedColumn2Rooms.Contains(room)) continue;
            ConnectToRandomRoom(room, column1, false);
        }
    }
    private Room ConnectToRandomRoom(Room room, List<Room> column2, bool isFromLeftToRight)
    {
        Room targetRoom;
        targetRoom = column2[UnityEngine.Random.Range(0, column2.Count)];
        if (isFromLeftToRight)
        {
            room.linkTo.Add(new Vector2Int(targetRoom.column, targetRoom.line));
        }
        else
        {
            targetRoom.linkTo.Add(new Vector2Int(room.column, room.line));
        }
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        return targetRoom;
    }
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }
    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption = options[UnityEngine.Random.Range(0, options.Length)];
        return (RoomType)Enum.Parse(typeof(RoomType), randomOption);
    }
    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var roomData = new MapRoomData
            {
                posX = room.transform.position.x,
                posY = room.transform.position.y,
                column = room.column,
                line = room.line,
                roomData = room.roomData,
                roomState = room.roomState,
                linkTo = room.linkTo
            };
            mapLayout.mapRoomDataList.Add(roomData);
        }

        mapLayout.linePositionList = new();
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var linePosition = new LinePosition
            {
                startPos = new SerializeVector3(line.GetPosition(0)),
                endPos = new SerializeVector3(line.GetPosition(1))
            };
            mapLayout.linePositionList.Add(linePosition);
        }
    }
    private void LoadMap()
    {
        foreach (var roomData in mapLayout.mapRoomDataList)
        {
            var room = Instantiate(roomPrefab, new Vector3(roomData.posX, roomData.posY, 0), Quaternion.identity, transform);
            room.roomState = roomData.roomState;
            room.linkTo = roomData.linkTo;
            room.SetupRoom(roomData.column, roomData.line, roomData.roomData);
            rooms.Add(room);
        }
        foreach (var linePosition in mapLayout.linePositionList)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, linePosition.startPos.ToVector3());
            line.SetPosition(1, linePosition.endPos.ToVector3());
            lines.Add(line);
        }
    }
}
