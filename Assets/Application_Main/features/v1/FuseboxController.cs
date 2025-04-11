using System.Collections;
using System.Linq;
using UnityEngine;

public class FuseboxController : MonoBehaviour
{
    public static FuseboxController Instance;
    [SerializeField] private FuseAnchor[] anchors;
    [SerializeField] private Light[] sceneLights;
    [SerializeField] private GameObject defaultMasterFeedback, correctMasterFeedback;
    
    [SerializeField] private ExecuteActionOnRaycastTargetSeen3DView fuseLocator, feedbackLocator;
    
    private FuseAnchor lockedAnchor;
    private Fuse grabbed;
    
    public FuseAnchor LockedAnchor => lockedAnchor;
    public Fuse Grabbed => grabbed;
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnFuseFeedbackNotificationReceived(GameObject sender, object args)
    {
        StartCoroutine(OnFuseFeedbackNotificationReceivedCoroutine(sender, args));
    }
    public IEnumerator OnFuseFeedbackNotificationReceivedCoroutine(GameObject sender, object args)
    {
        OnFuseWasSeen(args as GameObject);
        yield return StartCoroutine(IsStillSeeingFuse());
        OnFuseWasLost(args as GameObject);
    }
    public IEnumerator IsStillSeeingFuse()
    {
        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (false);
        // todo: resolver
        
        yield return null;
    }
    public void OnFuseWasSeen(GameObject fuseGO)
    {
        var fuse = fuseGO.GetComponent<Fuse>();
        if (fuse == grabbed || fuse.Anchor != null) return;
        fuse.GetComponent<OutlineObjectEffectView>().StartFeedback();
    }
    public void OnFuseWasLost(GameObject fuseGO)
    {
        var fuse = fuseGO.GetComponent<Fuse>();
        if (fuse == grabbed || fuse.Anchor != null) return;
        fuse.GetComponent<OutlineObjectEffectView>().StopFeedback();
    }
    public void OnGrabFuse(Fuse fuse)
    {
        grabbed = fuse;
        grabbed.CanDrag = grabbed.Rigidbody3d.isKinematic = true;
        grabbed.Rigidbody3d.constraints = ~RigidbodyConstraints.FreezeAll;
    }
    public void OnReleaseFuse()
    {
        grabbed.CanDrag = grabbed.Rigidbody3d.isKinematic = false;
        grabbed = null;
    }

    public void EndDemonstration()
    {
        if (anchors.Any(anchor => anchor.placed != anchor.correctFuse)) return;
        foreach (var light in sceneLights)
            light.gameObject.SetActive(true);
        defaultMasterFeedback.SetActive(false);
        correctMasterFeedback.SetActive(true);
    }

    public void OnFuseboxFeedbackNotificationReceived(GameObject sender, object args)
    {
        StartCoroutine(OnFuseboxFeedbackNotificationReceivedCoroutine(sender, args));
    }
    protected IEnumerator OnFuseboxFeedbackNotificationReceivedCoroutine(GameObject sender, object args)
    {
        OnAnchorWasSeen(args as GameObject);
        yield return StartCoroutine(IsStillSeeingAnchor());
        OnAnchorWasLost(args as GameObject);
    }
    public IEnumerator IsStillSeeingAnchor()
    {
        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (false);
        // todo: resolver
        
        yield return null;
    }
    public void OnAnchorWasSeen(GameObject anchorGO)
    {
        var anchor = anchorGO.GetComponent<FuseAnchor>();
        
        foreach (var a in anchors)
            a.positionFeedback.SetActive(false);
        
        anchor.transform.GetChild(0).gameObject.SetActive(true);
        lockedAnchor = anchor;
    }
    public void OnAnchorWasLost(GameObject anchorGO)
    {
        var anchor = anchorGO.GetComponent<FuseAnchor>();
        
        anchor.transform.GetChild(0).gameObject.SetActive(false);
        lockedAnchor = null;
    }
}