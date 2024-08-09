using System.Numerics;
using Content.Client.UserInterface.Controls;
using Content.Shared.Whitelist;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Content.Shared.ADT.Phantom;
using Robust.Shared.Utility;
using Content.Shared.Preferences;
using Content.Shared.Actions;

namespace Content.Client.ADT.Phantom.UI;

[GenerateTypedNameReferences]
public sealed partial class PhantomFreedomMenu : RadialMenu
{
    [Dependency] private readonly EntityManager _entManager = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    private readonly SpriteSystem _spriteSystem;
    public List<(NetEntity, HumanoidCharacterProfile, string)> Vessels = new();

    public event Action<string>? OnSelectFreedom;

    public PhantomFreedomMenu()
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);

        _spriteSystem = _entManager.System<SpriteSystem>();
    }

    public void Populate(RequestPhantomFreedomMenuEvent args)
    {
        var parent = FindControl<RadialContainer>("Main");

        foreach (var protoId in args.Prototypes)
        {
            var proto = _proto.Index(protoId);


            var button = new PhantomFreedomMenuButton
            {
                StyleClasses = { "RadialMenuButton" },
                SetSize = new Vector2(64f, 64f),
                ToolTip = Loc.GetString(proto.Name ?? String.Empty),
                ID = proto.ID,
            };

            if (proto.TryGetComponent(out InstantActionComponent? action) && action.Icon != null)
            {
                var tex = new TextureRect
                {
                    HorizontalAlignment = HAlignment.Left,
                    VerticalAlignment = VAlignment.Top,
                    Texture = _spriteSystem.Frame0(action.Icon ?? SpriteSpecifier.Invalid),
                    TextureScale = new Vector2(1.5f, 1.5f),
                    SetSize = new Vector2(48f, 48f),
                };
                button.AddChild(tex);
            }
            parent.AddChild(button);

        }
        foreach (var child in Children)
        {
            if (child is not RadialContainer container)
                continue;
            AddFreedomClickAction(container);
        }
    }
    private void AddFreedomClickAction(RadialContainer container)
    {
        foreach (var child in container.Children)
        {
            if (child is not PhantomFreedomMenuButton castChild)
                continue;

            castChild.OnButtonUp += _ =>
            {
                OnSelectFreedom?.Invoke(castChild.ID ?? String.Empty);
            };
        }
    }
}


public sealed class PhantomFreedomMenuButton : RadialMenuTextureButton
{
    public string? ID;
}