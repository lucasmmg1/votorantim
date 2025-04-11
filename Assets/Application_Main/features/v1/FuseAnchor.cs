using UnityEngine;

public class FuseAnchor : MonoBehaviour
{
    public GameObject positionFeedback;
    public GameObject defaultLightFeedback, correctLightFeedback;
    public GameObject correctFuse;

    public GameObject placed;
    
    public void OnFusePlaced(Fuse fuse)
    {
        fuse.Anchor = this;
        
        fuse.transform.position = positionFeedback.transform.position;
        fuse.transform.rotation = Quaternion.identity;
        fuse.Rigidbody3d.constraints = RigidbodyConstraints.FreezeAll;
        positionFeedback.transform.parent.gameObject.SetActive(false);
        placed = fuse.gameObject;
        if (fuse.gameObject == correctFuse)
        {
            defaultLightFeedback.SetActive(false);
            correctLightFeedback.SetActive(true);
        }
        
        FuseboxController.Instance.EndDemonstration();
    }

    public void OnFuseRemoved(Fuse fuse)
    {
        positionFeedback.transform.parent.gameObject.SetActive(true);
        defaultLightFeedback.SetActive(true);
        correctLightFeedback.SetActive(false);
        placed = null;
        fuse.Anchor = null;
    }
}