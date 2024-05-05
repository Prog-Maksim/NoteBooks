using System.Text.Json.Serialization;

namespace NoteBooks.Models;

public class StickerSetings
{
    public bool CloseMenuLeaveFocus { get; set; }
    public bool CloseAnimation { get; set; }
    public double DelayAnimationTopMenu { get; set; }
    public double DelayAnimationBottonMenu { get; set; }

    [JsonConstructor]
    public StickerSetings(bool closeMenuLeaveFocus, bool closeAnimation, double delayAnimationTopMenu,
        double delayAnimationBottonMenu) =>
        (CloseMenuLeaveFocus, CloseAnimation, DelayAnimationTopMenu, DelayAnimationBottonMenu) = (closeMenuLeaveFocus,
            closeAnimation, delayAnimationTopMenu, delayAnimationBottonMenu);
}