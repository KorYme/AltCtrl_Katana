using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using System;
using UnityEngine.Serialization;

public class JoyconManager : MonoBehaviour
{
    // Settings accessible via Unity
    public bool EnableIMU = true;
    public bool EnableLocalize = true;

    // Different operating systems either do or don't like the trailing zero
    private const ushort vendor_id = 0x57e;
    private const ushort vendor_id_ = 0x057e;
    private const ushort product_l = 0x2006;
    private const ushort product_r = 0x2007;

    public List<Joycon> j; // Array of all connected Joy-Cons

    [SerializeField, Range(0f, 1f)] private float _bowPercentage;
    [SerializeField, Range(0f, 5f)] private float _bowTimeDifference;
    public event Action OnPlayersBow;

    private PlayersBowState _playersState = PlayersBowState.NotPossible;
    private float _playersBowTime = -1f;

    enum PlayersBowState
    {
        NotPossible,
        ReadyToBow,
        ReadyToStand,
    }

    void Awake()
    {
        if (InstanceManager.JoyconManager != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceManager.JoyconManager = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        j = new List<Joycon>();
        bool isLeft = false;
        HIDapi.hid_init();

        IntPtr ptr = HIDapi.hid_enumerate(vendor_id, 0x0);
        IntPtr top_ptr = ptr;

        if (ptr == IntPtr.Zero)
        {
            ptr = HIDapi.hid_enumerate(vendor_id_, 0x0);
            if (ptr == IntPtr.Zero)
            {
                HIDapi.hid_free_enumeration(ptr);
                Debug.Log("No Joy-Cons found!");
            }
        }
        hid_device_info enumerate;
        while (ptr != IntPtr.Zero)
        {
            enumerate = (hid_device_info)Marshal.PtrToStructure(ptr, typeof(hid_device_info));

            Debug.Log(enumerate.product_id);
            if (enumerate.product_id == product_l || enumerate.product_id == product_r)
            {
                if (enumerate.product_id == product_l)
                {
                    isLeft = true;
                    Debug.Log("Left Joy-Con connected.");
                }
                else if (enumerate.product_id == product_r)
                {
                    isLeft = false;
                    Debug.Log("Right Joy-Con connected.");
                }
                else
                {
                    Debug.Log("Non Joy-Con input device skipped.");
                }
                IntPtr handle = HIDapi.hid_open_path(enumerate.path);
                HIDapi.hid_set_nonblocking(handle, 1);
                j.Add(new Joycon(handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft));
            }
            ptr = enumerate.next;
        }
        HIDapi.hid_free_enumeration(top_ptr);
    }

    void Start()
    {
        for (int i = 0; i < j.Count; ++i)
        {
            Joycon jc = j[i];
            byte LEDs = 0x0;
            LEDs |= (byte)(0x1 << i);
            jc.Attach(leds_: LEDs);
            jc.Begin();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnPlayersBow?.Invoke();
        }

        for (int i = 0; i < j.Count; ++i)
        {
            j[i].Update();
        }
        if (j.Count < 2)
        {
            _playersState = PlayersBowState.NotPossible;
            return;
        }
        Quaternion p1 = j[0].GetVector();
        Quaternion p2 = j[1].GetVector();
        switch (_playersState)
        {
            case PlayersBowState.NotPossible:
            case PlayersBowState.ReadyToStand:
                if (Math.Abs(p1.z) < _bowPercentage && Math.Abs(p1.w) < _bowPercentage && Math.Abs(p2.z) < _bowPercentage && Math.Abs(p2.w) < _bowPercentage)
                {
                    if (_playersState == PlayersBowState.ReadyToStand)
                    {
                        OnPlayersBow?.Invoke();
                    }
                    _playersBowTime = _bowTimeDifference;
                    _playersState = PlayersBowState.ReadyToBow;
                }
                break;
            case PlayersBowState.ReadyToBow:
                _playersBowTime = _bowTimeDifference;
                if ((Math.Abs(p1.z) >= _bowPercentage || Math.Abs(p1.w) >= _bowPercentage) && (Math.Abs(p2.w) >= _bowPercentage || Math.Abs(p2.z) >= _bowPercentage))
                {
                    _playersState = PlayersBowState.ReadyToStand;
                }
                break;
        }

        if (_playersBowTime > 0)
        {
            _playersBowTime -= Time.deltaTime;
            if (_playersBowTime <= 0)
            {
                _playersState = PlayersBowState.NotPossible;
            }
        }
    }

    public void ResetPlayersState()
    {
        _playersState = PlayersBowState.NotPossible;
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < j.Count; ++i)
        {
            j[i].Detach();
        }
    }

    //  private void OnGUI()
    //  {
    //   GUI.Label(new Rect(10, 10, 1920, 20), _playersState.ToString());
    //   for (int i = 0; i < j.Count; i++)
    //   {
    // GUI.Label(new Rect(10, 250 + 20 * i, 1920, 20), j[i].GetVector().ToString());
    //   }
    //  }
}
