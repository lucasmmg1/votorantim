using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChecarEquipamentosDeSeguranca : QuestView
{
    #region Variables

    #region Protected Variables

    [SerializeField] protected OutlineObjectEffectView[] vestuarios;
    [SerializeField] protected CanvasGroup canvasGC;
    [SerializeField] protected TextMeshProUGUI vestuariosGottenTMP;

    #endregion

    #endregion

    #region Methods

    protected void Start()
    {
        canvasGC.alpha = 0;
    }

    #region Public Methods
    
    public override IEnumerator Quest()
    {
        LeanTween.alphaCanvas(canvasGC, 1, 0.5f).setIgnoreTimeScale(true);
        
        foreach (var vestuario in vestuarios)
            vestuario.StartFeedback();
        
        do
        {
            Status = QuestStatus.InProgress;
            vestuariosGottenTMP.text = $"{vestuarios.Count(vestuario => !vestuario.gameObject.activeSelf)} / {vestuarios.Length}";
            yield return new WaitForEndOfFrame();
        } while (vestuarios.Any(vestuario => vestuario.gameObject.activeSelf));
        
        Status = QuestStatus.Completed;
        LeanTween.alphaCanvas(canvasGC, 0, 0.5f).setIgnoreTimeScale(true);
    }

    #endregion

    #endregion
}