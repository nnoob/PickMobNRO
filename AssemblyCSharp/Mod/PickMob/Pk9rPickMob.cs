using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyCSharp.Mod.PickMob
{
    public class Pk9rPickMob
    {
        private const int ID_ITEM_GEM = 77;
        private const int DEFAULT_HP_BUFF = 20;
        private const int DEFAULT_MP_BUFF = 20;
        private static readonly sbyte[] IdSkillsBase = {0, 2, 17, 4};
        private static readonly short[] IdItemBlockBase = 
            { 225, 353, 354, 355, 356, 357, 358, 359, 360, 362 };
            
        public static bool IsTanSat = false;
        public static bool IsNeSieuQuai = true;
        public static bool IsVuotDiaHinh = true;
        public static List<int> IdMobsTanSat = new();
        public static List<int> TypeMobsTanSat = new();
        public static List<sbyte> IdSkillsTanSat = new(IdSkillsBase);

        public static bool IsAutoPickItems;
        public static bool IsItemMe = true;
        public static bool IsLimitTimesPickItem = true;
        public static int TimesAutoPickItemMax = 7;
        public static List<short> IdItemPicks = new();
        public static List<short> IdItemBlocks = new(IdItemBlockBase);
        public static List<sbyte> TypeItemPicks = new();
        public static List<sbyte> TypeItemBlock = new();

        public static int HpBuff = 0;
        public static int MpBuff = 0;

        public static bool Chat(string text)
        {
            if (text == "add")
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
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                    }
                    else
                    {
                        IdItemPicks.Add(itemMap.template.id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                    }
                }
                else
                {
                    GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
                }
            }
            else if (text == "addt")
            {
                Mob mob = Char.myCharz().mobFocus;
                ItemMap itemMap = Char.myCharz().itemFocus;
                if (mob != null)
                {
                    if (TypeMobsTanSat.Contains(mob.templateId))
                    {
                        TypeMobsTanSat.Remove(mob.templateId);
                        GameScr.info1.addInfo($"Đã xoá loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                    }
                    else
                    {
                        TypeMobsTanSat.Add(mob.templateId);
                        GameScr.info1.addInfo($"Đã thêm loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                    }
                }
                else if (itemMap != null)
                {
                    if (TypeItemPicks.Contains(itemMap.template.type))
                    {
                        TypeItemPicks.Remove(itemMap.template.type);
                        GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                    }
                    else
                    {
                        TypeItemPicks.Add(itemMap.template.type);
                        GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                    }
                }
                else
                {
                    GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
                }
            }
            else if (text == "anhat")
            {
                IsAutoPickItems = !IsAutoPickItems;
                GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItems ? "Bật" : "Tắt"), 0);
            }
            else if (text == "itm")
            {
                IsItemMe = !IsItemMe;
                GameScr.info1.addInfo("Lọc không nhặt vật phẩm của người khác: " + (IsItemMe ? "Bật" : "Tắt"), 0);
            }
            else if (text == "sln")
            {
                IsLimitTimesPickItem = !IsLimitTimesPickItem;
                StringBuilder builder = new();
                builder.Append($"Giới hạn số lần nhặt là ");
                builder.Append(TimesAutoPickItemMax);
                builder.Append(IsLimitTimesPickItem ? ": Bật" : ": Tắt");
                GameScr.info1.addInfo(builder.ToString(), 0);
            }
            else if (IsGetInfoChat<int>(text, "sln"))
            {
                TimesAutoPickItemMax = GetInfoChat<int>(text, "sln");
                GameScr.info1.addInfo("Số lần nhặt giới hạn là: " + TimesAutoPickItemMax, 0);
            }
            else if (IsGetInfoChat<short>(text, "addi"))
            {
                short id = GetInfoChat<short>(text, "addi");
                if (IdItemPicks.Contains(id))
                {
                    IdItemPicks.Remove(id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
                else
                {
                    IdItemPicks.Add(id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
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
                        GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                    }
                    else
                    {
                        IdItemBlocks.Add(itemMap.template.id);
                        GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                    }
                }
                else
                {
                    GameScr.info1.addInfo("Cần trỏ vào vật phẩm cần chặn khi auto nhặt", 0);
                }
            }    
            else if (IsGetInfoChat<short>(text, "blocki"))
            {
                short id = GetInfoChat<short>(text, "blocki");
                if (IdItemBlocks.Contains(id))
                {
                    IdItemBlocks.Remove(id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
                else
                {
                    IdItemBlocks.Add(id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
            }
            else if (IsGetInfoChat<sbyte>(text, "addti"))
            {
                sbyte type = GetInfoChat<sbyte>(text, "addti");
                if (TypeItemPicks.Contains(type))
                {
                    TypeItemPicks.Remove(type);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item: " + type, 0);
                }
                else
                {
                    TypeItemPicks.Add(type);
                    GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item: " + type, 0);
                }    
            }
            else if (IsGetInfoChat<sbyte>(text, "blockti"))
            {
                sbyte type = GetInfoChat<sbyte>(text, "blockti");
                if (TypeItemBlock.Contains(type))
                {
                    TypeItemBlock.Remove(type);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách không tự động nhặt loại item: " + type, 0);
                }
                else
                {
                    TypeItemBlock.Add(type);
                    GameScr.info1.addInfo("Đã thêm vào danh sách không tự động nhặt loại item: " + type, 0);
                }
            }
            else if (text == "clri")
            {
                IdItemPicks.Clear();
                TypeItemPicks.Clear();
                TypeItemBlock.Clear();
                IdItemBlocks.Clear();
                IdItemBlocks.AddRange(IdItemBlockBase);
                GameScr.info1.addInfo("Danh sách lọc item đã được đặt lại mặc định", 0);
            }
            else if (text == "cnn")
            {
                IdItemPicks.Clear();
                TypeItemPicks.Clear();
                TypeItemBlock.Clear();
                IdItemBlocks.Clear();
                IdItemBlocks.AddRange(IdItemBlockBase);
                IdItemPicks.Add(ID_ITEM_GEM);
                GameScr.info1.addInfo("Đã cài đặt chỉ nhặt ngọc", 0);
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
            else if (IsGetInfoChat<int>(text, "addtm"))
            {
                int id = GetInfoChat<int>(text, "addtm");
                if (TypeMobsTanSat.Contains(id))
                {
                    TypeMobsTanSat.Remove(id);
                    GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                }
                else
                {
                    TypeMobsTanSat.Add(id);
                    GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                }
            }
            else if (text == "clrm")
            {
                IdMobsTanSat.Clear();
                TypeMobsTanSat.Clear();
                GameScr.info1.addInfo("Đã xoá danh sách đánh quái", 0);
            }
            else if (text == "skill")
            {
                SkillTemplate template = Char.myCharz().myskill.template;
                if (IdSkillsTanSat.Contains(template.id))
                {
                    IdSkillsTanSat.Remove(template.id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
                else
                {
                    IdSkillsTanSat.Add(template.id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
            }
            else if (IsGetInfoChat<int>(text, "skill"))
            {
                int index = GetInfoChat<int>(text, "skill") - 1;
                SkillTemplate template = Char.myCharz().nClass.skillTemplates[index];
                if (IdSkillsTanSat.Contains(template.id))
                {
                    IdSkillsTanSat.Remove(template.id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
                else
                {
                    IdSkillsTanSat.Add(template.id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
            }
            else if (IsGetInfoChat<sbyte>(text, "skillid"))
            {
                sbyte id = GetInfoChat<sbyte>(text, "skillid");
                if (IdSkillsTanSat.Contains(id))
                {
                    IdSkillsTanSat.Remove(id);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                }
                else
                {
                    IdSkillsTanSat.Add(id);
                    GameScr.info1.addInfo("Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                }
            }
            else if (text == "clrs")
            {
                IdSkillsTanSat.Clear();
                IdSkillsTanSat.AddRange(IdSkillsBase);
                GameScr.info1.addInfo("Đã đặt danh sách skill sử dụng tự động đánh quái về mặc định", 0);
            }
            else if (text == "abf")
            {
                if (HpBuff == 0 && MpBuff == 0)
                {
                    GameScr.info1.addInfo("Tự động sử dụng đậu thần: Tắt", 0);
                }
                else
                {
                    HpBuff = DEFAULT_HP_BUFF;
                    MpBuff = DEFAULT_MP_BUFF;
                    GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
                }    
            }
            else if (IsGetInfoChat<int>(text, "abf"))
            {
                HpBuff = GetInfoChat<int>(text, "abf");
                MpBuff = 0;
                GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%", 0);
            }
            else if (IsGetInfoChat<int>(text, "abf", 2))
            {
                int[] vs = GetInfoChat<int>(text, "abf", 2);
                HpBuff = vs[0];
                MpBuff = vs[1];
                GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
            }
            else if (text == "vdh")
            {
                IsVuotDiaHinh = !IsVuotDiaHinh;
                GameScr.info1.addInfo("Tự động đánh quái vượt địa hình: " + (IsVuotDiaHinh ? "Bật" : "Tắt"), 0);
            }
            else
            {
                return false;
            }
            return true;
        }

        public static bool HotKeys()
        {
            switch (GameCanvas.keyAsciiPress)
            {
                case 't':
                    Chat("ts");
                    break;
                case 'n':
                    Chat("anhat");
                    break;
                case 'a':
                    Chat("add");
                    break;
                case 'b':
                    Chat("abf");
                    break;
                default:
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
            if (mob.status != 1 && mob.status != 0)
            {
                mob.timeLastDie = mSystem.currentTimeMillis();
                mob.countDie++;
                if (mob.countDie > 10)
                    mob.countDie = 0;
            }
        }

        public static void UpdateCountDieMob(Mob mob)
        {
            if (mob.levelBoss != 0)
                mob.countDie = 0;
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

        private static bool IsGetInfoChat<T>(string text, string s, int n)
        {
            if (!text.StartsWith(s))
            {
                return false;
            }
            try
            {
                string[] vs = text.Substring(s.Length).Split(' ');
                for (int i = 0; i < n; i++)
                {
                    Convert.ChangeType(vs[i], typeof(T));
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static T[] GetInfoChat<T>(string text, string s, int n)
        {
            T[] ts = new T[n];
            string[] vs = text.Substring(s.Length).Split(' ');
            for (int i = 0; i < n; i++)
            {
                ts[i] = (T)Convert.ChangeType(vs[i], typeof(T));
            }
            return ts;
        }
        #endregion
    }
}
