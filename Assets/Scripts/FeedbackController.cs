using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public class FeedbackController : MonoBehaviour
{
    [field: Header("Toggles")]
    [field: Space]
    [SerializeField] private Toggle allToggle;
    [SerializeField] private Toggle runEffectToggle;
    [SerializeField] private Toggle runEffectOnTurnBoostToggle;
    [SerializeField] private Toggle runEffectOnGroundOnlyToggle;
    [SerializeField] private Toggle doubleJumpEffectToggle;
    [SerializeField] private Toggle wallSlideEffectToggle;
    [SerializeField] private Toggle dashEffectToggle;
    [SerializeField] private Toggle deformPlayerEffectToggle;

    [field: Header("Pre-selected")]
    [field: Space]
    [SerializeField] private bool runEffect = true;
    [SerializeField] private bool runEffectOnTurnBoost = false;
    [SerializeField] private bool runEffectOnGroundOnly = true;
    [SerializeField] private bool doubleJumpEffect = true;
    [SerializeField] private bool wallSlideEffect = true;
    [SerializeField] private bool dashEffect = true;
    [SerializeField] private bool deformPlayerEffect = true;

    public bool EmitRunEffect
    {
        get => runEffectToggle.isOn;
        private set {}
    }

    public bool EmitRunEffectOnTurnBoost
    {
        get => runEffectOnTurnBoostToggle.isOn;
        private set {}
    }
    public bool EmitRunEffectOnGroundOnly
    {
        get => runEffectOnGroundOnlyToggle.isOn;
        private set {}
    }
    public bool EmitDoubleJumpEffect
    {
        get => doubleJumpEffectToggle.isOn;
        private set {}
    }
    public bool EmitWallSlideEffect
    {
        get => wallSlideEffectToggle.isOn;
        private set {}
    }
    public bool EmitDashEffect
    {
        get => dashEffectToggle.isOn;
        private set { }
    }

    public bool DeformPlayerEffect
    {
        get => deformPlayerEffectToggle.isOn;
        private set { }
    }

    public static FeedbackController Instance;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        runEffectToggle.SetIsOnWithoutNotify(runEffect);
        runEffectOnTurnBoostToggle.SetIsOnWithoutNotify(runEffectOnTurnBoost);
        runEffectOnGroundOnlyToggle.SetIsOnWithoutNotify(runEffectOnGroundOnly);
        doubleJumpEffectToggle.SetIsOnWithoutNotify(doubleJumpEffect);
        wallSlideEffectToggle.SetIsOnWithoutNotify(wallSlideEffect);
        dashEffectToggle.SetIsOnWithoutNotify(dashEffect);
        deformPlayerEffectToggle.SetIsOnWithoutNotify(deformPlayerEffect);
        UpdateAllToggle();
    }

    public void UpdateAllToggle()
    {
        allToggle.SetIsOnWithoutNotify(runEffectToggle.isOn &&
                                       runEffectOnTurnBoostToggle.isOn &&
                                       doubleJumpEffectToggle.isOn &&
                                       wallSlideEffectToggle.isOn &&
                                       dashEffectToggle.isOn &&
                                       runEffectOnGroundOnlyToggle.isOn &&
                                       deformPlayerEffectToggle.isOn);
    }

    public void OnEmitAll()
    {
        runEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        runEffectOnTurnBoostToggle.SetIsOnWithoutNotify(allToggle.isOn);
        runEffectOnGroundOnlyToggle.SetIsOnWithoutNotify(allToggle.isOn);
        doubleJumpEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        wallSlideEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        dashEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        deformPlayerEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
    }
}