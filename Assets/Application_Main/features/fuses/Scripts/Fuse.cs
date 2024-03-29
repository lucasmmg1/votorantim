using UnityEngine;

public class Fuse : MonoBehaviour
{
    #region Variables

    #region Protected Variables
    
    protected float zCoordinate;
    protected Vector3 offset;
    
    #endregion

    #region Public Variables

    public bool CanDrag {get; set;}
    public Rigidbody Rigidbody3d { get; private set; }
    public FuseAnchor Anchor { get; set; }

    #endregion

    #endregion

    #region Methods

    #region Protected Methods

    protected void Awake()
    {
        Rigidbody3d = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        if (FuseboxController.Instance.Grabbed != null) return;
        
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition();
        FuseboxController.Instance.OnGrabFuse(this);
        if (Anchor != null)
            Anchor.OnFuseRemoved(this);
    }
    private void OnMouseDrag()
    {
        if (!CanDrag) return;
        transform.position = GetMouseWorldPosition() + offset;
    }
    private void OnMouseUp()
    {
        if (FuseboxController.Instance.Grabbed != this) return;
        
        FuseboxController.Instance.OnReleaseFuse();
        if (FuseboxController.Instance.LockedAnchor != null)
            FuseboxController.Instance.LockedAnchor.OnFusePlaced(this);
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate));
    }

    #endregion

    #endregion
}