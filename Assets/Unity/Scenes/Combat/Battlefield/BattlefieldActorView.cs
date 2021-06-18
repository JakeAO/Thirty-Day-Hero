using Core.Actors;
using Unity.Scenes.Shared.Entities;
using Unity.Scenes.Shared.Status;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldActorView : MonoBehaviour
{
    [SerializeField] private RectTransform _actorViewRoot;

    [SerializeField] private Image _actorHighlight;

    [SerializeField] private CanvasGroup _hpStaCanvas;
    [SerializeField] private IHealthView _healthView;
    [SerializeField] private IStaminaView _staminaView;

    public uint ActorId => _actorData?.Id ?? 0u;

    private IActorView _actorView;
    private ICharacterActor _actorData;

    public void SetActorView(IActorView view)
    {
        _actorView = view;
        _actorData = view.Model;
    }
}