using Content.Shared.ADT.SpeechBarks;
using Content.Shared.Speech;
using Robust.Shared.Prototypes;

namespace Content.Server.VoiceMask;

[RegisterComponent]
public sealed partial class VoiceMaskerComponent : Component
{
    [DataField]
    public string LastSetName = "Unknown";

    [DataField]
    public ProtoId<SpeechVerbPrototype>? LastSpeechVerb;

    [DataField]
    public string? LastSetVoice; // Corvax-TTS

    [DataField]
    public ProtoId<BarkPrototype> LastSetBark = "Human1"; // ADT Barks

    [DataField]
    public float LastSetPitch = 1f; // ADT Barks

    [DataField]
    public EntProtoId Action = "ActionChangeVoiceMask";

    [DataField]
    public EntityUid? ActionEntity;
}
