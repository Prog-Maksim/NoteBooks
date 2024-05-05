using System.Collections.Generic;

namespace NoteBooks.Models;

public class StickersList
{
    public List<Sticker>? Stickers { get; set; }

    public StickersList(List<Sticker>? stickers = null) => (Stickers) = (stickers);
}