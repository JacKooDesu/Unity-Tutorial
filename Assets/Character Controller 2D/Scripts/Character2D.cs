using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2D : MonoBehaviour
{
    [Header("參數")]
    public float gravity;   // 引力
    [Range(0, 1f)] public float deAccScale = .2f;   // 剎車強度

    [Header("按鍵綁定")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;


    [Header("移動設定")]
    public float acceleration = 5f; // 加速度
    public float maxAcc = 10f;      // 最高速

    [Header("跳躍設定")]
    public LayerMask groundLayer;   // 地面 layer
    public LayerMask wallLayer;     // 牆壁layer
    public float jumpForce = .1f;    // 跳躍力道
    public int jumpMaxCount = 2;    // 跳躍次數
    [Range(0f, 1f)] public float jumpTimeMax = .1f;
    [SerializeField] int jumpCount; // 已跳躍次數
    float jumpTime = 0f;

    [System.Serializable]
    public class CastSetting
    {
        public float length;    // 長度
        public Vector2 offset;  // 偏移腳色中心
        [SerializeField, Range(-180f, 180f)]
        float angle = -90f;

        public Vector2 Direction
        {
            get
            {
                var rad = angle * Mathf.Deg2Rad;
                return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            }
        }

        public Color gizmoColor = Color.cyan;
    }

    [Header("射線設定")]
    public CastSetting[] groundCastSettings;
    public CastSetting[] wallCastSettings;

    // 內部變數
    Rigidbody2D rig;
    Collider2D col;
    [SerializeField] bool isOnGorund = false;
    [SerializeField] bool isOnWall = false;
    Vector2 currentVelocity;

    private void OnEnable()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey))
            Jump();

        if (Input.GetKeyUp(jumpKey))
            jumpTime = 0f;

        if (Input.GetKeyDown(rightKey))
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (Input.GetKeyDown(leftKey))
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void FixedUpdate()
    {
        if (jumpCount <= jumpMaxCount && jumpTime < jumpTimeMax && Input.GetKey(jumpKey))
        {
            currentVelocity.y += jumpForce;
            jumpTime += Time.fixedDeltaTime;
        }

        if (Input.GetKey(leftKey) && Input.GetKey(rightKey))
            currentVelocity.x += 0;
        else if (Input.GetKey(leftKey))
        {
            currentVelocity.x -= acceleration;
        }
        else if (Input.GetKey(rightKey))
        {
            currentVelocity.x += acceleration;
        }

        // 數值補正
        currentVelocity.x = Mathf.Lerp(currentVelocity.x, 0, deAccScale);
        currentVelocity.y -= isOnGorund ? 0 : gravity;

        currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxAcc, +maxAcc);
        // currentVelocity.y = Mathf.Clamp(currentVelocity.y, isOnGorund ? 0 : -maxAcc, +maxAcc);
        currentVelocity.y = Mathf.Clamp(currentVelocity.y, isOnGorund ? 0 : float.MinValue, +maxAcc);

        // rig.MovePosition(
        //     rig.position +
        //     Vector2.right * Mathf.Clamp(currentAcc, -maxAcc, +maxAcc) +
        //     Vector2.up * up);
        rig.MovePosition(currentVelocity * Time.fixedDeltaTime + rig.position);
    }

    #region CollisionTest

    private void OnCollisionEnter2D(Collision2D other)
    {
        var go = other.gameObject;
        var layer = 1 << go.layer;
        if (layer == groundLayer && !isOnGorund && CheckGround())
        {
            jumpCount = 0;
            isOnGorund = true;
            jumpTime=0;
                        if (Input.GetKey(jumpKey)) Jump();
        }
    }

    // private void OnCollisionStay2D(Collision2D other)
    // {
    //     var go = other.gameObject;
    //     var layer = 1 << go.layer;
    //     if (layer == groundLayer && !isOnGorund && CheckGround())
    //     {
    //         jumpCount = 0;
    //         isOnGorund = true;
    //         jumpTime=0;
    //                     // if (Input.GetKey(jumpKey)) Jump();
    //     }

    //     isOnWall = CheckWall();
    // }

    private void OnCollisionExit2D(Collision2D other)
    {
        var go = other.gameObject;
        var layer = 1 << go.layer;
        if (layer == groundLayer && isOnGorund && !CheckGround())
        {
            isOnGorund = false;
        }
    }

    #endregion

    #region Movement
    void Jump()
    {
        if (jumpCount < jumpMaxCount)
            currentVelocity.y = 0;
        jumpCount++;
        isOnGorund = false;
    }
    #endregion

    bool CheckGround()
    {
        foreach (var gcs in groundCastSettings)
        {
            var origin = (Vector2)transform.position + gcs.offset;

            var hit = Physics2D.Raycast(origin, gcs.Direction, gcs.length, groundLayer);

            if (hit.transform != null) return true;
        }

        return false;
    }

    bool CheckWall()
    {
        foreach (var wcs in wallCastSettings)
        {
            var origin = (Vector2)transform.position + wcs.offset;

            // 需要修正，v2 rotate 加入
            // var hit = Physics2D.Raycast(origin, wcs.Direction, wcs.length, wallLayer);
            var hit = Physics2D.Raycast(origin, (Vector2)transform.right, wcs.length, wallLayer);

            if (hit.transform != null) return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCastSettings == null)
            return;

        foreach (var gcs in groundCastSettings)
        {
            Gizmos.color = gcs.gizmoColor;
            var origin = (Vector2)transform.position + gcs.offset;
            Gizmos.DrawLine(origin, gcs.Direction * gcs.length + origin);
        }

        foreach (var wcs in wallCastSettings)
        {
            Gizmos.color = wcs.gizmoColor;
            var origin = (Vector2)transform.position + wcs.offset;
            // 需要修正，v2 rotate 加入
            // Gizmos.DrawLine(origin, wcs.Direction * wcs.length + origin);
            Gizmos.DrawLine(origin, (Vector2)transform.right * wcs.length + origin);
        }
    }
}
