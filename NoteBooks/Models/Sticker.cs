using System;

namespace StickyNotes.Models;

public class Sticker
{
    public string Name { get; set; }
    public int Color { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime CurrentDateUpdate { get; set; }
    public bool StickerThumbtack { get; set; }
    public bool StickerFavorite { get; set; }
    public string StickerCurrentPath { get; set; }
    public StickyData StickyData { get; set; }

    public Sticker(string name, int color, DateTime dateStart, DateTime currentDateUpdate, bool stickerThumbtack,
        bool stickerFavorite, string stickerCurrentPath, StickyData stickyData) =>
        (Name, Color, DateStart, CurrentDateUpdate, StickerThumbtack, StickerFavorite, StickerCurrentPath, StickyData) = (name, color,
            dateStart, currentDateUpdate, stickerThumbtack, stickerFavorite, stickerCurrentPath, stickyData);
}