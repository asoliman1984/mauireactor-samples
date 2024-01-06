﻿using MauiReactor;
namespace TrackizerApp.Pages.Components;

partial class RoundedEntry : Component
{
    MauiControls.Entry? _entry;

    [Prop]
    string? _labelText;

    [Prop]
    string? _text;

    [Prop]
    Action<string>? _onTextChanged;

    public override VisualNode Render()
        => VStack(spacing: 4,
            Theme.BodySmall(_labelText)
                .TextColor(Theme.Grey50),
            Grid(
                Border()
                    .Stroke(Theme.Grey70)
                    .StrokeCornerRadius(16),

                Entry(entry => _entry = entry)
                    .Text(_text ?? string.Empty)
                    .OnAfterTextChanged(_onTextChanged)
                    .HeightRequest(48)
                    .Margin(5, 0)
                    .Keyboard(Keyboard.Email)
                    .TextColor(Theme.White)
                
                )
            )
            .BackgroundColor(Colors.Transparent)
            .OnTapped(() => _entry?.Focus());
}

class RoundedPasswordEntryState
{
    public string Password { get; set; } = string.Empty;
}

partial class RoundedPasswordEntry : Component<RoundedPasswordEntryState>
{
    MauiControls.Entry? _entry;

    [Prop]
    string? _labelText;

    [Prop]
    string? _text;

    [Prop]
    Action<string>? _onTextChanged;

    public override VisualNode Render()
        => VStack(spacing: 4,
            Theme.BodySmall(_labelText)
                .TextColor(Theme.Grey50),
            Grid(
                Border()
                    .Stroke(Theme.Grey70)
                    .StrokeCornerRadius(16),

                Entry(entry => _entry = entry)
                    .Text(_text ?? string.Empty)
                    .OnTextChanged(OnPasswordChanged)
                    .HeightRequest(48)
                    .TextColor(Theme.White)
                    .Margin(5,0)
                    .IsPassword(true)
                ),

            Grid("5", "* * * *",
                Border().GridColumn(0).StrokeCornerRadius(9, 0, 9, 0).BackgroundColor(() => State.Password.Length > 8 ? Theme.Accents100 : State.Password.Length > 0 ? Theme.Primary100 : Theme.Grey70),
                Border().GridColumn(1).BackgroundColor(() => State.Password.Length > 8 ? Theme.Accents100 : State.Password.Length > 4 ? Theme.Primary100 : Theme.Grey70),
                Border().GridColumn(2).BackgroundColor(() => State.Password.Length > 8 ? Theme.Accents100 : Theme.Grey70),
                Border().GridColumn(3).StrokeCornerRadius(0, 9, 0, 9).BackgroundColor(() => State.Password.Length > 8 ? Theme.Accents100 : Theme.Grey70)
                )
                .ColumnSpacing(3)
                .Margin(0,24,0,0)
            ,

            Theme.BodySmall("Use 8 or more characters with a mix of letters, numbers & symbols.")
                .TextColor(Theme.Grey50)
                .Margin(0,16,0,0)
                

            )
            .BackgroundColor(Colors.Transparent)
            .OnTapped(() => _entry?.Focus());

    void OnPasswordChanged(object? sender, MauiControls.TextChangedEventArgs e)
    {
        SetState(s => s.Password = e.NewTextValue, false);
        if (e.NewTextValue.Length > 8)
        {
            _onTextChanged?.Invoke(e.NewTextValue);
        }
    }
}