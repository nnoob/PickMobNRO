using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.PickMob
{
    public class PickMobController
    {
        private const int TIME_REPICKITEM = 500;
        private const int TIME_DELAY_TANSAT = 500;
        private const int ID_ICON_ITEM_TDLT = 4387;
        private static readonly sbyte[] IdSkillsCannotAttack = 
            { 10, 11, 14, 23, 7 };

        public static bool IsPickingItem;

        private static bool IsWait;
        private static long TimeStartWait;
        private static long TimeWait;

        public static List<ItemMap> ItemPicks = new();
        private static int IndexItemPick = 0;

        public static void Update()
        {
            if (IsWaiting())
                return;

            if (Char.myCharz().statusMe == 14 || Char.myCharz().cHP <= 0)
                return;

            if (Pk9rPickMob.IsAutoPickItem)
            {
                if (IsPickingItem)
                {
                    if (IndexItemPick >= ItemPicks.Count)
                    {
                        IsPickingItem = false;
                        return;
                    }

                    ItemMap itemMap = ItemPicks[IndexItemPick];
                    if (IsPickItem(itemMap))
                    {
                        switch (GetTpyeDistanceItem(itemMap))
                        {
                            case TpyeDistanceItem.PickItemTDLT:
                                Char.myCharz().cx = itemMap.xEnd;
                                Char.myCharz().cy = itemMap.yEnd;
                                Service.gI().charMove();
                                break;
                            case TpyeDistanceItem.PickItemTanSat:
                                Char.myCharz().currentMovePoint = new MovePoint(itemMap.xEnd, itemMap.yEnd);
                                Char.myCharz().mobFocus = null;
                                Wait(TIME_REPICKITEM);
                                return;
                        }
                        Service.gI().pickItem(itemMap.itemMapID);
                        Wait(TIME_REPICKITEM);
                    }
                    IndexItemPick++;
                    return;
                }

                ItemPicks.Clear();
                IndexItemPick = 0;
                for (int i = 0; i < GameScr.vItemMap.size(); i++)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                    if (IsPickItem(itemMap))
                    {
                        ItemPicks.Add(itemMap);
                    }
                }
                if (ItemPicks.Count > 0)
                {
                    IsPickingItem = true;
                    return;
                }
            }

            if (Pk9rPickMob.IsTanSat)
            {
                Char myChar = Char.myCharz();
                myChar.clearFocus(0);

                if (myChar.mobFocus != null && !IsMobTanSat(myChar.mobFocus))
                    myChar.mobFocus = null;

                if (myChar.mobFocus == null)
                    myChar.mobFocus = GetMobTanSat();

                if (myChar.mobFocus != null)
                {
                    Skill skill = GetSkillAttack();
                    if (skill != null)
                    {
                        Mob mobFocus = Char.myCharz().mobFocus;
                        mobFocus.x = mobFocus.xFirst;
                        mobFocus.y = mobFocus.yFirst;
                        GameScr.gI().doSelectSkill(skill, true);
                        GameScr.gI().MyDoDoubleClickToObj(mobFocus);
                    }
                }
                else
                {
                    Mob mob = GetMobRevive();
                    Char.myCharz().currentMovePoint = new MovePoint(mob.xFirst, mob.yFirst);
                }
                Wait(TIME_DELAY_TANSAT);
            }
        }

        #region Get data pick item
        public static bool IsPickItem(ItemMap itemMap)
        {
            if (!FilterItemPick(itemMap))
                return false;

            if (Pk9rPickMob.IsItemMe && !IsMyItem(itemMap))
                return false;

            if (itemMap.countAutoPick > Pk9rPickMob.TimesAutoPickItemMax)
                return false;

            if (GetTpyeDistanceItem(itemMap) == TpyeDistanceItem.CannotPickItem)
                return false;

            return true;
        }

        private static TpyeDistanceItem GetTpyeDistanceItem(ItemMap itemMap)
        {
            Char myChar = Char.myCharz();
            if (Res.abs(myChar.cx - itemMap.xEnd) < 60 && Res.abs(myChar.cy - itemMap.yEnd) < 60)
                return TpyeDistanceItem.PickItemNormal;

            if (ItemTime.isExistItem(ID_ICON_ITEM_TDLT))
                return TpyeDistanceItem.PickItemTDLT;

            if (Pk9rPickMob.IsTanSat)
                return TpyeDistanceItem.PickItemTanSat;

            return TpyeDistanceItem.CannotPickItem;
        }

        private static bool FilterItemPick(ItemMap itemMap)
        {
            if (Pk9rPickMob.IdItemPicks.Count != 0 && !Pk9rPickMob.IdItemPicks.Contains(itemMap.template.id))
                return false;

            if (Pk9rPickMob.IdItemBlocks.Count != 0 && Pk9rPickMob.IdItemBlocks.Contains(itemMap.template.id))
                return false;

            return true;
        }

        private static bool IsMyItem(ItemMap itemMap)
        {
            return itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1;
        }

        enum TpyeDistanceItem
        {
            CannotPickItem,
            PickItemNormal,
            PickItemTDLT,
            PickItemTanSat
        }
        #endregion

        #region Get data tan sat
        private static Mob GetMobTanSat()
        {
            Mob mobDmin = null;
            int d;
            int dmin = int.MaxValue;
            Char myChar = Char.myCharz();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                d = (mob.xFirst - myChar.cx) * (mob.xFirst - myChar.cx) + (mob.yFirst - myChar.cy) * (mob.yFirst - myChar.cy);
                if (IsMobTanSat(mob) && d < dmin)
                {
                    mobDmin = mob;
                    dmin = d;
                }
            }
            return mobDmin;
        }

        private static Mob GetMobRevive()
        {
            Mob mobTmin = null;
            long tmin = mSystem.currentTimeMillis();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (IsMobRevive(mob) && mob.timeLastDie < tmin)
                {
                    mobTmin = mob;
                    tmin = mob.timeLastDie;
                }
            }
            return mobTmin;
        }

        public static bool IsMobTanSat(Mob mob)
        {
            if (!FilterMobTanSat(mob))
                return false;

            if (mob.status == 0 || mob.status == 1 || mob.hp <= 0 || mob.isMobMe)
                return false;

            if (mob.levelBoss != 0 && Pk9rPickMob.IsNeSieuQuai && !ItemTime.isExistItem(ID_ICON_ITEM_TDLT))
                return false;

            return true;
        }

        private static bool IsMobRevive(Mob mob)
        {
            if (!FilterMobTanSat(mob) || mob.isMobMe)
                return false;

            if ((mob.countDie == 10 || mob.levelBoss != 0) && Pk9rPickMob.IsNeSieuQuai && !ItemTime.isExistItem(ID_ICON_ITEM_TDLT))
                return false;

            return true;
        }

        private static bool FilterMobTanSat(Mob mob)
        {
            return Pk9rPickMob.IdMobsTanSat.Count == 0 || Pk9rPickMob.IdMobsTanSat.Contains(mob.mobId);
        }

        private static Skill GetSkillAttack()
        {
            Skill skill = null;
            for (int i = 0; i < GameScr.keySkill.Length; i++)
                if (IsSkillBetter(GameScr.keySkill[i], skill))
                    skill = GameScr.keySkill[i];
            return skill;
        }

        public static bool IsSkillBetter(Skill SkillBetter, Skill skill)
        {
            if (SkillBetter == null || !CanUseSkill(SkillBetter))
                return false;

            if (skill != null && skill.coolDown >= SkillBetter.coolDown)
                return false;

            return true;
        }

        private static bool CanUseSkill(Skill skill)
        {
            if (skill.paintCanNotUseSkill || Char.myCharz().skillInfoPaint() != null)
                return false;

            if (IdSkillsCannotAttack.Contains(skill.template.id))
                return false;

            if (Char.myCharz().cMP < GetManaUseSkill(skill))
                return false;

            return true;
        }

        private static int GetManaUseSkill(Skill skill)
        {
            if (skill.template.manaUseType == 2)
                return 1;
            else if (skill.template.manaUseType == 1)
                return (skill.manaUse * Char.myCharz().cMPFull / 100);
            else
                return skill.manaUse;
        }
        #endregion

        #region Control update
        private static void Wait(int time)
        {
            IsWait = true;
            TimeStartWait = mSystem.currentTimeMillis();
            TimeWait = time;
        }

        private static bool IsWaiting()
        {
            if (IsWait && (mSystem.currentTimeMillis() - TimeStartWait >= TimeWait))
                IsWait = false;
            return IsWait;
        }
        #endregion
    }
}
