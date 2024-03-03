using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Map Layout")]
    public MapLayoutSO mapLayout;
    public void UpdateMapLayoutData(object val)
    {
        var roomVector = (Vector2Int)val;
        var currentRoom = mapLayout.mapRoomDataList.Find(room => room.column == roomVector.x && room.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(room => room.column == roomVector.x);
        foreach (var room in sameColumnRooms)
        {
            if (room.line == roomVector.y) continue;
            room.roomState = RoomState.Locked;
        }
        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(room => room.column == link.x && room.line == link.y);
            linkedRoom.roomState = RoomState.Attainable; 
        }
    }
}
