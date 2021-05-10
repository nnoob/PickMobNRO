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
        private static readonly sbyte[] IdSkillsBase = { 0, 2, 17, 4 };
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

            bool isUseTDLT = ItemTime.isExistItem(ID_ICON_ITEM_TDLT);
            if (Pk9rPickMob.IsAutoPickItem && (!Pk9rPickMob.IsTanSat || !isUseTDLT))
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
                                Service.gI().pickItem(itemMap.itemMapID);
                                itemMap.countAutoPick++;
                                IndexItemPick++;
                                Wait(TIME_REPICKITEM);
                                return;
                            case TpyeDistanceItem.PickItemTanSat:
                                Char.myCharz().currentMovePoint = new MovePoint(itemMap.xEnd, itemMap.yEnd);
                                Char.myCharz().mobFocus = null;
                                Wait(TIME_REPICKITEM);
                                return;
                            case TpyeDistanceItem.PickItemNormal:
                                Service.gI().charMove();
                                Service.gI().pickItem(itemMap.itemMapID);
                                itemMap.countAutoPick++;
                                IndexItemPick++;
                                Wait(TIME_REPICKITEM);
                                return;
                        }
                    }
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
                if (Char.myCharz().isCharge)
                {
                    Wait(TIME_DELAY_TANSAT);
                    return;
                }

                Char myChar = Char.myCharz();
                myChar.clearFocus(0);

                if (myChar.mobFocus != null && !IsMobTanSat(myChar.mobFocus))
                    myChar.mobFocus = null;

                if (myChar.mobFocus == null)
                {
                    myChar.mobFocus = GetMobTanSat();
                    if (isUseTDLT && myChar.mobFocus != null)
                    {
                        myChar.cx = myChar.mobFocus.xFirst - 24;
                        myChar.cy = myChar.mobFocus.yFirst;
                        Service.gI().charMove();
                    }    
                }

                if (myChar.mobFocus != null)
                {
                    if (myChar.skillInfoPaint() == null)
                    {
                        Skill skill = GetSkillAttack();
                        if (skill != null && !skill.paintCanNotUseSkill)
                        {
                            Mob mobFocus = myChar.mobFocus;
                            mobFocus.x = mobFocus.xFirst;
                            mobFocus.y = mobFocus.yFirst;
                            GameScr.gI().doSelectSkill(skill, true);
                            GameScr.gI().MyDoDoubleClickToObj(mobFocus);
                        }
                    }
                }
                else if (!isUseTDLT)
                {
                    Mob mob = GetMobNext();
                    if (mob != null)
                    {
                        myChar.currentMovePoint = new MovePoint(mob.xFirst, mob.yFirst);
                    }
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

        private static Mob GetMobNext()
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

            if (Pk9rPickMob.IsNeSieuQuai && !ItemTime.isExistItem(ID_ICON_ITEM_TDLT) && mob.getTemplate().hp >= 3000)
            {
                if (mob.levelBoss != 0)
                {
                    Mob mobNextSieuQuai = null;
                    for (int i = 0; i < GameScr.vMob.size(); i++)
                    {
                        mobNextSieuQuai = (Mob)GameScr.vMob.elementAt(i);
                        if (mobNextSieuQuai.countDie == 10 && (mobNextSieuQuai.status == 0 || mobNextSieuQuai.status == 1))
                        {
                            break;
                        }
                    }
                    if (mobNextSieuQuai.countDie == 10 && (mobNextSieuQuai.status == 0 || mobNextSieuQuai.status == 1))
                    {
                        mob.timeLastDie = mobNextSieuQuai.timeLastDie;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (mob.countDie == 10 && (mob.status == 0 || mob.status == 1))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool FilterMobTanSat(Mob mob)
        {
            if (Pk9rPickMob.IdMobsTanSat.Count != 0 && !Pk9rPickMob.IdMobsTanSat.Contains(mob.mobId))
                return false;

            if (Pk9rPickMob.TypeMobsTanSat.Count != 0 && !Pk9rPickMob.TypeMobsTanSat.Contains(mob.templateId))
                return false;

            return true;
        }

        private static Skill GetSkillAttack()
        {
            Skill skill = null;
            Skill nextSkill;
            SkillTemplate skillTemplate = new();
            foreach (var id in Pk9rPickMob.IdSkillsTanSat)
            {
                skillTemplate.id = id;
                nextSkill = Char.myCharz().getSkill(skillTemplate);
                if (IsSkillBetter(nextSkill, skill))
                {
                    skill = nextSkill;
                }
            }
            return skill;
        }

        public static bool IsSkillBetter(Skill SkillBetter, Skill skill)
        {
            if (SkillBetter == null || Char.myCharz().skillInfoPaint() != null)
                return false;

            if (!CanUseSkill(SkillBetter))
                return false;

            bool isPrioritize = (SkillBetter.template.id == 17 && skill.template.id == 2) ||
                (SkillBetter.template.id == 9 && skill.template.id == 0);
            if (skill != null && skill.coolDown >= SkillBetter.coolDown && !isPrioritize)
                return false;

            return true;
        }

        private static bool CanUseSkill(Skill skill)
        {
            if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown)
                skill.paintCanNotUseSkill = false;

            if (skill.paintCanNotUseSkill && !IdSkillsBase.Contains(skill.template.id))
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
