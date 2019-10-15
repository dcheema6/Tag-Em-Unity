using UnityEngine;


[RequireComponent(typeof(TPCharacter))]
public class TPCharacterControl : Player
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
    }
    [SerializeField] MouseInput MouseControl;

    private Crosshair m_Crosshair;
    private Crosshair Crosshair
    {
        get
        {
            if (m_Crosshair == null)
                m_Crosshair = GetComponentInChildren<Crosshair>();
            return m_Crosshair;
        }
    }

    private InputController playerInput;
    private Vector2 mouseInput;

    private TPCharacter m_Character;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private bool m_Jump;

    private bool isTurn;

    void Awake()
    {
        base.Awake();
        playerInput = GameManager.Instance.InputController;
    }

    void Start()
    {
        base.Start();

        if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        m_Character = GetComponent<TPCharacter>();
    }

    void Update()
    {
        if (GameManager.Instance.InputController.Fire1)
            weapon.Fire();

        if (!m_Jump)
            m_Jump = playerInput.Jump;
        
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);
        
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
    }

    private void FixedUpdate()
    {
        float v = playerInput.Vertical;
        float h = playerInput.Horizontal;

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_Cam.forward + h*m_Cam.right;
        }
        else
            m_Move = v*Vector3.forward + h*Vector3.right;
#if !MOBILE_INPUT
        if (playerInput.LShift)
            m_Move *= 0.5f;
#endif
        m_Character.Move(m_Move, playerInput.Crouch, m_Jump);
        m_Jump = false;
    }
}
