using System;
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
    [SerializeField] private Toggle trampolineBounceEffectToggle;
    [SerializeField] private Toggle hitEffectToggle;
    [SerializeField] private Toggle dieEffectToggle;
    [SerializeField] private Toggle vibrationsEffectToggle;
    [SerializeField] private Toggle cameraShakeEffectToggle;

    [field: Header("Pre-selected")]
    [field: Space]
    [SerializeField] private bool runEffect = true;
    [SerializeField] private bool runEffectOnTurnBoost = false;
    [SerializeField] private bool runEffectOnGroundOnly = true;
    [SerializeField] private bool doubleJumpEffect = true;
    [SerializeField] private bool wallSlideEffect = true;
    [SerializeField] private bool dashEffect = true;
    [SerializeField] private bool deformPlayerEffect = true;
    [SerializeField] private bool trampolineBounceEffect = true;
    [SerializeField] private bool hitEffect = true;
    [SerializeField] private bool dieEffect = true;
    [SerializeField] private bool vibrationsEffect = true;
    [SerializeField] private bool cameraShakeEffect = true;

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

    public bool TrampolineBounceEffect
    {
        get => trampolineBounceEffectToggle.isOn;
        private set { }
    }

    public bool HitEffect
    {
        get => hitEffectToggle.isOn;
        private set { }
    }

    public bool DieEffect
    {
        get => dieEffectToggle.isOn;
        private set { }
    }

    public bool VibrationsEffect
    {
        get => vibrationsEffectToggle.isOn;
        private set { }
    }

    public bool CameraShakeEffect
    {
        get => cameraShakeEffectToggle.isOn;
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
        trampolineBounceEffectToggle.SetIsOnWithoutNotify(trampolineBounceEffect);
        hitEffectToggle.SetIsOnWithoutNotify(hitEffect);
        dieEffectToggle.SetIsOnWithoutNotify(dieEffect);
        vibrationsEffectToggle.SetIsOnWithoutNotify(vibrationsEffect);
        cameraShakeEffectToggle.SetIsOnWithoutNotify(cameraShakeEffect);
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
                                       deformPlayerEffectToggle.isOn &&
                                       trampolineBounceEffectToggle.isOn &&
                                       hitEffectToggle.isOn &&
                                       dieEffectToggle.isOn &&
                                       vibrationsEffectToggle.isOn &&
                                       cameraShakeEffectToggle.isOn);
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
        trampolineBounceEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        hitEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        dieEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        vibrationsEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
        cameraShakeEffectToggle.SetIsOnWithoutNotify(allToggle.isOn);
    }
}