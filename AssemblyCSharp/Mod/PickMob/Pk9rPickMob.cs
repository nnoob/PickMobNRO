using System;
using System.Collections.Generic;

namespace AssemblyCSharp.Mod.PickMob
{
    public class Pk9rPickMob
    {
        public static bool IsTanSat = false;
        public static bool IsNeSieuQuai = true;
        public static List<int> IdMobsTanSat = new();

        public static bool IsAutoPickItem;
        public static bool IsItemMe = true;
        public static List<int> IdItemPicks = new();
        public static List<int> IdItemBlock = new();

        public static bool Chat(string text)
        {
            if (text == "anhat")
            {
                IsAutoPickItem = !IsAutoPickItem;
                GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItem ? "Bật" : "Tắt"), 0);
            }
            else if (text == "itemme")
            {
                IsItemMe = !IsItemMe;
                GameScr.info1.addInfo("Chỉ nhặt vật phẩm của mình: " + (IsItemMe ? "Bật" : "Tắt"), 0);
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
            else if (text == "clrm")
            {
                IdMobsTanSat.Clear();
                GameScr.info1.addInfo("Đã xoá tất cả mob tàn sát", 0);
            }
            else if (text == "add")
            {
                Mob mob = Char.myCharz().mobFocus;
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
                
                ItemMap itemMap = Char.myCharz().itemFocus;
                if (itemMap != null)
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
