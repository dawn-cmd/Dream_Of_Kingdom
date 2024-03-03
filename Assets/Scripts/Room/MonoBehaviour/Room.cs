using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a room in the game.
/// </summary>
public class Room : MonoBehaviour
{
    /// <summary>
    /// The column index of the room.
    /// </summary>
    public int column;

    /// <summary>
    /// The line index of the room.
    /// </summary>
    public int line;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The data of the room.
    /// </summary>
    public RoomDataSO roomData;

    /// <summary>
    /// The state of the room.
    /// </summary>
    public RoomState roomState;
    public List<Vector2Int> linkTo = new();

    [Header("Broadcast")]
    public ObjectEventSO loadRoomEvent;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        // deal with the room click
        // Debug.Log("Room clicked: " + roomData.roomType);
        if (roomState == RoomState.Locked || roomState == RoomState.Visited) return;
        loadRoomEvent.RaiseEvent(this, this);
    }

    /// <summary>
    /// Sets up the room with the specified column, line, and room data.
    /// </summary>
    /// <param name="column">The column index of the room.</param>
    /// <param name="line">The line index of the room.</param>
    /// <param name="roomData">The data of the room.</param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Attainable => Color.white,
            RoomState.Locked => Color.gray,
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            _ => spriteRenderer.color
        };
    }
}
