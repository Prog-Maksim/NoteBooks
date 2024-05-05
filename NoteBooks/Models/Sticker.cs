using System;

namespace NoteBooks.Models;

public class Sticker
{
    public string Name { get; set; }
    public string Color { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime CurrentDateUpdate { get; set; }
    public bool StickerThumbtack { get; set; }
    public bool StickerFavorite { get; set; }
    public bool StickerIsOpen { get; set; }

    public Sticker(string name, string color, DateTime dateStart, DateTime currentDateUpdate, bool stickerThumbtack,
        bool stickerFavorite, bool stickerIsOpen) =>
        (Name, Color, DateStart, CurrentDateUpdate, StickerThumbtack, StickerFavorite, StickerIsOpen) = (name, color,
            dateStart, currentDateUpdate, stickerThumbtack, stickerFavorite, stickerIsOpen);
}