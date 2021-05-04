using System;
using System.Collections.Generic;

namespace AssemblyCSharp.Mod.PickMob
{
    public class Pk9rPickMob
    {
        private const int ID_ITEM_GEM = 77;

        public static bool IsTanSat = false;
        public static bool IsNeSieuQuai = true;
        public static List<int> IdMobsTanSat = new();

        public static bool IsAutoPickItem;
        public static bool IsItemMe = true;
        public static List<int> IdItemPicks = new();
        public static List<int> IdItemBlocks = new();

        public static bool Chat(string text)
        {
            if (text == "anhat")
            {
                IsAutoPickItem = !IsAutoPickItem;
                GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItem ? "Bật" : "Tắt"), 0);
            }
            else if (text == "itm")
            {
                IsItemMe = !IsItemMe;
                GameScr.info1.addInfo("Lọc không nhặt vật phẩm của người khác: " + (IsItemMe ? "Bật" : "Tắt"), 0);
            }
            else if (IsGetInfoChat<int>(text, "addt"))
            {
                int id = GetInfoChat<int>(text, "addt");
                if (IdItemPicks.Contains(id))
                {
                    IdItemPicks.Remove(id);
                    GameScr.info1.addInfo("Đã xoá item: " + id, 0);
                }
                else
                {
                    IdItemPicks.Add(id);
                    GameScr.info1.addInfo("Đã thêm item: " + id, 0);
                }
            }
            else if (text == "blocki")
            {
                ItemMap itemMap = Char.myCharz().itemFocus;
                if (itemMap != null)
                {
                    if (IdItemBlocks.Contains(itemMap.template.id))
                    {
                        IdItemBlocks.Remove(itemMap.template.id);
                        GameScr.info1.addInfo("Đã bỏ chặn item: " + itemMap.template.id, 0);
                    }
                    else
                    {
                        IdItemBlocks.Add(itemMap.template.id);
                        GameScr.info1.addInfo("Đã chặn item: " + itemMap.template.id, 0);
                    }
                }
                else
                {
                    GameScr.info1.addInfo("Cần trỏ vào vật phẩm cần chặn khi auto nhặt", 0);
                }
            }    
            else if (IsGetInfoChat<int>(text, "blocki"))
            {
                int id = GetInfoChat<int>(text, "blocki");
                if (IdItemBlocks.Contains(id))
                {
                    IdItemBlocks.Remove(id);
                    GameScr.info1.addInfo("Đã bỏ chặn item: " + id, 0);
                }
                else
                {
                    IdItemBlocks.Add(id);
                    GameScr.info1.addInfo("Đã chặn item: " + id, 0);
                }
            }
            else if (text == "cnn")
            {
                IdItemPicks.Clear();
                IdItemBlocks.Clear();
                IdItemPicks.Add(ID_ITEM_GEM);
                GameScr.info1.addInfo("Đã cài đặt chỉ nhặt ngọc", 0);
            }    
            else if (text == "clri")
            {
                IdItemPicks.Clear();
                IdItemBlocks.Clear();
                GameScr.info1.addInfo("Đã xoá tất cả danh sách item", 0);
            }    
            else if (text =="ts")
            {
                IsTanSat = !IsTanSat;
                GameScr.info1.addInfo("Tự động đánh quái: " + (IsTanSat ? "Bật" : "Tắt"), 0);
            }
            else if (text == "nsq")
            {
                IsNeSieuQuai = !IsNeSieuQuai;
                GameScr.info1.addInfo("Tàn sát né siêu quái: " + (IsNeSieuQuai ? "Bật" : "Tắt"), 0);
            }
            else if (IsGetInfoChat<int>(text, "addm"))
            {
                int id = GetInfoChat<int>(text, "addm");
                if (IdMobsTanSat.Contains(id))
                {
                    IdMobsTanSat.Remove(id);
                    GameScr.info1.addInfo("Đã xoá mob: " + id, 0);
                }
                else
                {
                    IdMobsTanSat.Add(id);
                    GameScr.info1.addInfo("Đã thêm mob: " + id, 0);
                }
            }
            else if (text == "clrm")
            {
                IdMobsTanSat.Clear();
                GameScr.info1.addInfo("Đã xoá tất cả mob tàn sát", 0);
            }
            else if (text == "add")
            {
                Mob mob = Char.myCharz().mobFocus;
                ItemMap itemMap = Char.myCharz().itemFocus;
                if (mob != null)
                {
                    if (IdMobsTanSat.Contains(mob.mobId))
                    {
                        IdMobsTanSat.Remove(mob.mobId);
                        GameScr.info1.addInfo("Đã xoá mob: " + mob.mobId, 0);
                    }
                    else
                    {
                        IdMobsTanSat.Add(mob.mobId);
                        GameScr.info1.addInfo("Đã thêm mob: " + mob.mobId, 0);
                    }
                }
                else if (itemMap != null)
                {
                    if (IdItemPicks.Contains(itemMap.template.id))
                    {
                        IdItemPicks.Remove(itemMap.template.id);
                        GameScr.info1.addInfo("Đã xoá item: " + itemMap.template.id, 0);
                    }
                    else
                    {
                        IdItemPicks.Add(itemMap.template.id);
                        GameScr.info1.addInfo("Đã thêm item: " + itemMap.template.id, 0);
                    }
                }
                else
                {
                    GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public static void Update()
        {
            PickMobController.Update();
        }

        public static void MobStartDie(object obj)
        {
            Mob mob = (Mob)obj;
            mob.timeLastDie = mSystem.currentTimeMillis();
            mob.countDie++;
            if (mob.countDie > 10)
                mob.countDie = 0;
        }

        public static void ResetCountDieMob(Mob mob)
        {
            if (mob.levelBoss != 0)
            {
                mob.countDie = 0;
            }    
        }

        #region Không cần liên kết với game
        private static bool IsGetInfoChat<T>(string text, string s)
        {
            if (!text.StartsWith(s))
            {
                return false;
            }
            try
            {
                Convert.ChangeType(text.Substring(s.Length), typeof(T));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static T GetInfoChat<T>(string text, string s)
        {
            return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
        }
        #endregion
    }
}
