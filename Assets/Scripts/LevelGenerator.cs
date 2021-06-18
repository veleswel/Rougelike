using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction { left, up, right, down }
public enum JointDirection { horizontal, vertical }

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _layoutRoom;

    [SerializeField]
    private GameObject _doorVertical, _doorHorizontal;

    [SerializeField]
    private RoomPrefabs _rooms;

    [SerializeField]
    private int _roomsToGenerate;

    [SerializeField]
    private Gradient _startToEndColoring;

    [SerializeField]
    private Transform _generatorPoint;

    [SerializeField]
    private float _xOffset, _yOffset;

    private Direction _selectedDirection;

    [SerializeField]
    private LayerMask _generatorRoomLayer;

    private List<GameObject> _generatedRoomLayouts = new List<GameObject>();

    private List<GameObject> _generatedOutlines = new List<GameObject>();

    private List<AdjacentRoomJoint> _roomJoints = new List<AdjacentRoomJoint>();

    [SerializeField]
    private RoomCenter _startRoomCenter, _endRoomCenter;

    [SerializeField]
    private RoomCenter[] _potentialRoomCenters;

    [SerializeField]
    private RoomClearedCondition _roomClearedCondition;
    [SerializeField]
    private RoomBecameActiveCondition _roomBecameActiveCondition;

    void Start()
    {
        while(_generatedRoomLayouts.Count < _roomsToGenerate)
        {
            GameObject room = Instantiate(_layoutRoom, _generatorPoint.position, _generatorPoint.rotation);
            room.GetComponent<SpriteRenderer>().color = _startToEndColoring.Evaluate((float)_generatedRoomLayouts.Count / _roomsToGenerate);
            _generatedRoomLayouts.Add(room);

            while(Physics2D.OverlapCircle(_generatorPoint.position, 1f, _generatorRoomLayer))
            {
                MoveGenerationPoint();
            }
        }

        foreach (GameObject roomLayout in _generatedRoomLayouts)
        {
            CreateRoomOutline(roomLayout);
        }

        for (int i = 0; i < _generatedOutlines.Count; i++)
        {
            RoomCenter centerPrefab;
            if (i == 0)
            {
                centerPrefab = _startRoomCenter;
            }
            else if (i == _generatedOutlines.Count - 1)
            {
                centerPrefab = _endRoomCenter;
            }
            else
            {
                int rand = Random.Range(0, _potentialRoomCenters.Length);
                centerPrefab = _potentialRoomCenters[rand];
            }

            CreateRoomCenterAndJoints(_generatedOutlines[i], centerPrefab);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void MoveGenerationPoint()
    {
        _selectedDirection = (Direction)Random.Range(0, 4);
        
        switch (_selectedDirection)
        {
            case Direction.up:
                _generatorPoint.position += new Vector3(0f, _yOffset, 0f);
                break;
            case Direction.right:
                _generatorPoint.position += new Vector3(_xOffset, 0f, 0f);
                break;
            case Direction.down:
                _generatorPoint.position += new Vector3(0f, -_yOffset, 0f);
                break;
            case Direction.left:
                _generatorPoint.position += new Vector3(-_xOffset, 0f, 0f);
                break;
        }
    }

    void CreateRoomOutline(GameObject roomLayout)
    {
        bool isRoomLeft = Physics2D.OverlapCircle(roomLayout.transform.position + new Vector3(-_xOffset, 0f, 0f), 1f, _generatorRoomLayer);
        bool isRoomRight = Physics2D.OverlapCircle(roomLayout.transform.position + new Vector3(_xOffset, 0f, 0f), 1f, _generatorRoomLayer);
        bool isRoomUp = Physics2D.OverlapCircle(roomLayout.transform.position + new Vector3(0f, _yOffset, 0f), 1f, _generatorRoomLayer);
        bool isRoomDown = Physics2D.OverlapCircle(roomLayout.transform.position + new Vector3(0f, -_yOffset, 0f), 1f, _generatorRoomLayer);

        int directions = 0;
        directions += isRoomLeft ? 1 : 0;
        directions += isRoomRight ? 1 : 0;
        directions += isRoomUp ? 1 : 0;
        directions += isRoomDown ? 1 : 0;

        GameObject chosenPrefab = null;

        switch (directions)
        {
            case 0:
                Debug.Log("No exits found for the room at position: " + roomLayout.transform.position.ToString());
                break;
            case 1:
                if (isRoomLeft)
                {
                    chosenPrefab = _rooms._left;
                }
                if (isRoomRight)
                {
                    chosenPrefab = _rooms._right;
                }
                if (isRoomUp)
                {
                    chosenPrefab = _rooms._up;
                }
                if (isRoomDown)
                {
                    chosenPrefab = _rooms._down;
                }
                break;
            case 2:
                if (isRoomLeft && isRoomRight)
                {
                    chosenPrefab = _rooms._leftRight;
                }

                if (isRoomLeft && isRoomUp)
                {
                    chosenPrefab = _rooms._leftUp;
                }

                if (isRoomLeft && isRoomDown)
                {
                    chosenPrefab = _rooms._leftDown;
                }

                if (isRoomRight && isRoomUp)
                {
                    chosenPrefab = _rooms._rightUp;
                }

                if (isRoomRight && isRoomDown)
                {
                    chosenPrefab = _rooms._rightDown;
                }

                if (isRoomUp && isRoomDown)
                {
                    chosenPrefab = _rooms._upDown;
                }
                break;
            case 3:
                if (isRoomLeft && isRoomRight && isRoomUp)
                {
                    chosenPrefab = _rooms._leftRightUp;
                }
                if (isRoomLeft && isRoomRight && isRoomDown)
                {
                    chosenPrefab = _rooms._leftRightDown;
                }
                if (isRoomLeft && isRoomUp && isRoomDown)
                {
                    chosenPrefab = _rooms._leftUpDown;
                }
                if (isRoomRight && isRoomUp && isRoomDown)
                {
                    chosenPrefab = _rooms._rightUpDown;
                }
                break;
            case 4:
                chosenPrefab = _rooms._leftRightUpDown;
                break;
        }

        if (chosenPrefab != null)
        {
            GameObject outline = Instantiate(chosenPrefab, roomLayout.transform.position, transform.rotation);
            roomLayout.GetComponent<RoomLayout>().Room = outline.GetComponent<Room>();
            _generatedOutlines.Add(outline);
        }
    }

    void CreateRoomCenterAndJoints(GameObject roomOutline, RoomCenter centerPrefab)
    {
        Room room = roomOutline.GetComponent<Room>();
        if (room == null)
        {
            Debug.LogError("Can't get Room component for generated room: " + roomOutline.name);
            return;
        }

        RoomCenter center = Instantiate(centerPrefab, room.transform.position, room.transform.rotation);
        center.SetRoom(room);
        center.transform.parent = room.transform;

        Collider2D left = Physics2D.OverlapCircle(roomOutline.transform.position + new Vector3(-_xOffset, 0f, 0f), 1f, _generatorRoomLayer);
        Collider2D right = Physics2D.OverlapCircle(roomOutline.transform.position + new Vector3(_xOffset, 0f, 0f), 1f, _generatorRoomLayer);
        Collider2D up = Physics2D.OverlapCircle(roomOutline.transform.position + new Vector3(0f, _yOffset, 0f), 1f, _generatorRoomLayer);
        Collider2D down = Physics2D.OverlapCircle(roomOutline.transform.position + new Vector3(0f, -_yOffset, 0f), 1f, _generatorRoomLayer);

        List<GameObject> doors = new List<GameObject>();

        if (left)
        {
            Room leftRoom = left.gameObject.GetComponent<RoomLayout>().Room;
            if (leftRoom)
            {
                room.SetAdjacentRoom(Direction.left, leftRoom);
                
                Vector3 jointPosition = new Vector3(room.transform.position.x - (room.transform.position.x - leftRoom.transform.position.x) / 2f, room.transform.position.y, room.transform.position.z);
                
                AdjacentRoomJoint thisLeft = new AdjacentRoomJoint(room, leftRoom, jointPosition, JointDirection.horizontal);
                if (!IsRoomJointAlreadyThere(thisLeft))
                {
                    GameObject door = Instantiate(_doorVertical, jointPosition, room.transform.rotation);
                    door.transform.parent = room.transform;
                    doors.Add(door);
                    _roomJoints.Add(thisLeft);
                }
            }
            else
            {
                Debug.LogError("Can't get Room component from RoomLayout: " + left.gameObject.ToString());
            }
        }

        if (right)
        {
            Room rightRoom = right.gameObject.GetComponent<RoomLayout>().Room;
            if (rightRoom)
            {
                room.SetAdjacentRoom(Direction.right, rightRoom);
                
                Vector3 jointPosition = new Vector3(room.transform.position.x + (rightRoom.transform.position.x - room.transform.position.x) / 2f, room.transform.position.y, room.transform.position.z);
                
                AdjacentRoomJoint thisRight = new AdjacentRoomJoint(room, rightRoom, jointPosition, JointDirection.horizontal);
                if (!IsRoomJointAlreadyThere(thisRight))
                {
                    GameObject door = Instantiate(_doorVertical, jointPosition, room.transform.rotation);
                    door.transform.parent = room.transform;
                    doors.Add(door);
                    _roomJoints.Add(thisRight);
                }
            }
            else
            {
                Debug.LogError("Can't get Room component from RoomLayout: " + right.gameObject.ToString());
            }
        }

        if (up)
        {
            Room upRoom = up.gameObject.GetComponent<RoomLayout>().Room;
            if (upRoom)
            {
                room.SetAdjacentRoom(Direction.up, upRoom);
                
                Vector3 jointPosition = new Vector3(room.transform.position.x, room.transform.position.y + (upRoom.transform.position.y - room.transform.position.y) / 2f, room.transform.position.z);
                
                AdjacentRoomJoint thisUp = new AdjacentRoomJoint(room, upRoom, jointPosition, JointDirection.vertical);
                if (!IsRoomJointAlreadyThere(thisUp))
                {
                    GameObject door = Instantiate(_doorHorizontal, jointPosition, room.transform.rotation);
                    door.transform.parent = room.transform;
                    doors.Add(door);
                    _roomJoints.Add(thisUp);
                }
            }
            else
            {
                Debug.LogError("Can't get Room component from RoomLayout: " + up.gameObject.ToString());
            }
        }

        if (down)
        {
            Room downRoom = down.gameObject.GetComponent<RoomLayout>().Room;
            if (downRoom)
            {
                room.SetAdjacentRoom(Direction.down, downRoom);
                
                Vector3 jointPosition = new Vector3(room.transform.position.x, room.transform.position.y - (room.transform.position.y - downRoom.transform.position.y) / 2f, room.transform.position.z);
                
                AdjacentRoomJoint thisDown = new AdjacentRoomJoint(room, downRoom, jointPosition, JointDirection.vertical);
                if (!IsRoomJointAlreadyThere(thisDown))
                {
                    GameObject door = Instantiate(_doorHorizontal, jointPosition, room.transform.rotation);
                    door.transform.parent = room.transform;
                    doors.Add(door);
                    _roomJoints.Add(thisDown);
                }
            }
            else
            {
                Debug.LogError("Can't get Room component from RoomLayout: " + down.gameObject.ToString());
            }
        }

        BaseCondition condition;
        if (center.Enemies.Count > 0)
        {
            condition = Instantiate(_roomClearedCondition, Vector3.zero, Quaternion.identity);
            (condition as RoomClearedCondition).SetRoom(room);
        }
        else
        {
            condition = Instantiate(_roomBecameActiveCondition, Vector3.zero, Quaternion.identity);
            (condition as RoomBecameActiveCondition).SetRoom(room);
        }

        condition.transform.parent = room.transform;

        foreach(GameObject doorObject in doors)
        {
            Door door = doorObject.GetComponent<Door>();
            door.AddCondition(condition);
            door.SetCombinationRule(EOpenDoorConditionCombinationRule.Or);
        }
    }

    bool IsRoomJointAlreadyThere(AdjacentRoomJoint joint)
    {
        foreach (AdjacentRoomJoint j in _roomJoints)
        {
            if (j == joint)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject  _left, _right, _up, _down;
    public GameObject _leftRight, _leftUp, _leftDown, _rightUp, _rightDown, _upDown;
    public GameObject _leftRightUp, _leftRightDown, _leftUpDown, _rightUpDown;
    public GameObject  _leftRightUpDown;
}

[System.Serializable]
public class AdjacentRoomJoint
{
    public AdjacentRoomJoint(Room room1, Room room2, Vector3 pos, JointDirection dir)
    {
        Room1 = room1;
        Room2 = room2;
        Position = pos;
        Direction = dir;
    }
    public Room Room1, Room2;
    public JointDirection Direction { get; private set; }
    public Vector3 Position { get; private set; }
    public static bool operator == (AdjacentRoomJoint lhs, AdjacentRoomJoint rhs) 
    {
        return (lhs.Room1 == rhs.Room1 && lhs.Room2 == rhs.Room2) || (lhs.Room1 == rhs.Room2 && lhs.Room2 == rhs.Room1);
    }
    public static bool operator != (AdjacentRoomJoint lhs, AdjacentRoomJoint rhs) 
    {
        return lhs.Room1 != rhs.Room1 && lhs.Room2 != rhs.Room2 && lhs.Room1 != rhs.Room2 && lhs.Room2 != rhs.Room1;
    }
    public override string ToString() 
    {
        return Room1.name + " - " + Room2.name;
    }
    public override bool Equals(object obj)
    {
        AdjacentRoomJoint joint = obj as AdjacentRoomJoint;
        if (joint != null)
        {
            return joint == this;
        }
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return (Room1, Room2).GetHashCode();
    }
}