﻿using System;
using System.Collections.Generic;
using ProjectEternity.Core;
using ProjectEternity.Core.Units;
using ProjectEternity.Core.Attacks;
using ProjectEternity.Core.Effects;
using ProjectEternity.GameScreens.BattleMapScreen;

namespace ProjectEternity.GameScreens.DeathmatchMapScreen
{
    public partial class DeathmatchMap
    {
        /**
         * ATTACK = (((Attack Power * ((Pilot Will + Pilot Stat)/200) * Attack Side Terrain Performance) + Additive Base Damage Bonuses) * Base Damage Multiplier Bonuses
DEFENSE = ((Robot Armor Stat * ((Pilot Will + Pilot Def)/200) * Defense Side Terrain Performance) + Additive Base Defense Bonuses * Multiplying base defense bonuses) * Tile Bonus

FINAL DAMAGE = (((ATTACK - DEFENSE) * (ATTACKED AND DEFENDER SIZE COMPARISON)) + Additive Final Damage Bonuses) * Final Damage Multiplier Bonuses
    */
        public static int AttackFormula(Unit Attacker, int WeaponTerrain)
        {
            int PilotMorale = Attacker.PilotMorale;
            int PilotPower;

            if (Attacker.CurrentAttack.Style == WeaponStyle.M)
                PilotPower = Attacker.PilotMEL;
            else
                PilotPower = Attacker.PilotRNG;

            int AttackFormula = (int)(Attacker.CurrentAttack.GetPower(Attacker) * (PilotMorale + PilotPower) / 200f * (1 + WeaponTerrain / 100f));
            return AttackFormula;
        }

        public int AttackFormula(Unit Attacker, string AttackerTerrainType)
        {
            int WeaponTerrain = Attacker.CurrentAttack.TerrainAttribute(AttackerTerrainType);
            
            return AttackFormula(Attacker, WeaponTerrain);
        }

        public static int DefenseFormula(int Armor, int PilotMorale, int DefenderPilotDEF, int DefenderTerrain)
        {
            int DefenseFormula = (int)(Armor * ((PilotMorale + DefenderPilotDEF) / 200f) * (1 + DefenderTerrain / 100f));
            return DefenseFormula;
        }

        public int DefenseFormula(Unit Defender, string DefenderTerrainType, Terrain DefenderTerrain)
        {
            int Armor = Defender.Armor + GetTerrainBonus(DefenderTerrain, TerrainActivation.OnEveryTurns, TerrainBonus.Armor);
            int DefenderTerrainRate = Defender.TerrainArmorAttribute(DefenderTerrainType);
            int DefenderPilotDEF = Defender.PilotDEF;
            
            return DefenseFormula(Armor, Defender.PilotMorale, DefenderPilotDEF, DefenderTerrainRate);
        }

        public BattleResult DamageFormula(Unit Attacker, Squad AttackerSquad, float DamageModifier,
            Unit Defender, Squad DefenderSquad, Unit.BattleDefenseChoices DefenseChoice, bool CalculateCritical)
        {
            string AttackerTerrainType;
            string DefenderTerrainType;
            Terrain DefenderTerrain;

            if (AttackerSquad.IsFlying)
            {
                AttackerTerrainType = "Air";
            }
            else
            {
                AttackerTerrainType = GetTerrainType(AttackerSquad.X, AttackerSquad.Y, AttackerSquad.LayerIndex);
            }

            if (DefenderSquad.IsFlying)
            {
                DefenderTerrainType = "Air";
                DefenderTerrain = null;
            }
            else
            {
                DefenderTerrainType = GetTerrainType(DefenderSquad.X, DefenderSquad.Y, DefenderSquad.LayerIndex);
                DefenderTerrain = GetTerrain(DefenderSquad);
            }

            //Check if the Unit can counter the attack.
            bool NullifyAttack = CanNullifyAttack(Attacker.CurrentAttack, AttackerTerrainType, DefenderSquad.CurrentMovement, DefenderSquad, Defender.Boosts);

            int Attack = AttackFormula(Attacker, AttackerTerrainType);
            int Defense = DefenseFormula(Defender, DefenderTerrainType, DefenderTerrain);

            BattleResult Result = DamageFormula(Attacker, DamageModifier, Attack, Defender, DefenseChoice, NullifyAttack, Defense, CalculateCritical);

            return Result;
        }

        //(((Pilot Hit Stat/2 + 130) * Final Terrain Multiplier) + Weapon Hit Rate) + Base Hit Rate Effect
        public static int Accuracy(Unit Attacker, int FinalAttackerTerrainMultiplier)
        {
            return (int)((((Attacker.PilotHIT / 2 + 130) * ((100 + FinalAttackerTerrainMultiplier) / 100.0)) + Attacker.CurrentAttack.Accuracy + Attacker.Boosts.AccuracyModifier) * Attacker.Boosts.AccuracyMultiplier);
        }

        public int Accuracy(Unit Attacker, string AttackerTerrainType)
        {
            int AttackerTerrain = 0;
            int AttackerPilotTerrain = 0;
            char AttackerTerrainLetter = Attacker.TerrainLetterAttribute(AttackerTerrainType);
            char AttackerPilotTerrainLetter = Attacker.Pilot.TerrainGrade.GetTerrain(AttackerTerrainType);

            switch (AttackerTerrainLetter)
            {
                case 'S':
                    AttackerTerrain = 20;
                    break;

                case 'A':
                    AttackerTerrain = 10;
                    break;

                case 'B':
                    AttackerTerrain = 0;
                    break;

                case 'C':
                    AttackerTerrain = -10;
                    break;

                case 'D':
                    AttackerTerrain = -20;
                    break;
            }

            switch (AttackerPilotTerrainLetter)
            {
                case 'S':
                    AttackerPilotTerrain = 20;
                    break;

                case 'A':
                    AttackerPilotTerrain = 10;
                    break;

                case 'B':
                    AttackerPilotTerrain = 0;
                    break;

                case 'C':
                    AttackerPilotTerrain = -10;
                    break;

                case 'D':
                    AttackerPilotTerrain = -20;
                    break;
            }

            int FinalAttackerTerrainMultiplier = 0;

            switch (AttackerTerrain + AttackerPilotTerrain)
            {
                case -40:
                case -30:
                    FinalAttackerTerrainMultiplier = -60;
                    break;

                case -20:
                case -10:
                    FinalAttackerTerrainMultiplier = -40;
                    break;

                case 0:
                case 10:
                    FinalAttackerTerrainMultiplier = -20;
                    break;

                case 20:
                case 30:
                    FinalAttackerTerrainMultiplier = 0;
                    break;

                case 40:
                    FinalAttackerTerrainMultiplier = 20;
                    break;
            }

            return Accuracy(Attacker, FinalAttackerTerrainMultiplier);
        }

        //((Pilot Evasion/2)+Robot Mobility) * Final Terrain Multiplier) + Base Evasion Effect
        public static int Evasion(Unit Defender, int TerrainBonus, int FinalDefenderTerrainMultiplier)
        {
            int other = Defender.Boosts.EvasionModifier + TerrainBonus;

            return (int)((Defender.PilotEVA / 2 + Defender.Mobility) * ((100 + FinalDefenderTerrainMultiplier) / 100.0)) + other;
        }

        public int Evasion(Unit Defender, string DefenderTerrainType, Terrain DefenderTerrain)
        {
            int DefenderTerrainRate = 0;
            int DefenderPilotTerrain = 0;
            char AttackerTerrainLetter = Defender.TerrainLetterAttribute(DefenderTerrainType);
            char AttackerPilotTerrainLetter = Defender.Pilot.TerrainGrade.GetTerrain(DefenderTerrainType);

            switch (AttackerTerrainLetter)
            {
                case 'S':
                    DefenderTerrainRate = 20;
                    break;

                case 'A':
                    DefenderTerrainRate = 10;
                    break;

                case 'B':
                    DefenderTerrainRate = 0;
                    break;

                case 'C':
                    DefenderTerrainRate = -10;
                    break;

                case 'D':
                    DefenderTerrainRate = -20;
                    break;
            }

            switch (AttackerPilotTerrainLetter)
            {
                case 'S':
                    DefenderPilotTerrain = 20;
                    break;

                case 'A':
                    DefenderPilotTerrain = 10;
                    break;

                case 'B':
                    DefenderPilotTerrain = 0;
                    break;

                case 'C':
                    DefenderPilotTerrain = -10;
                    break;

                case 'D':
                    DefenderPilotTerrain = -20;
                    break;
            }

            int FinalDefenderTerrainMultiplier = 0;

            switch (DefenderTerrainRate + DefenderPilotTerrain)
            {
                case -40:
                case -30:
                    FinalDefenderTerrainMultiplier = -60;
                    break;

                case -20:
                case -10:
                    FinalDefenderTerrainMultiplier = -40;
                    break;

                case 0:
                case 10:
                    FinalDefenderTerrainMultiplier = -20;
                    break;

                case 20:
                case 30:
                    FinalDefenderTerrainMultiplier = 0;
                    break;

                case 40:
                    FinalDefenderTerrainMultiplier = 20;
                    break;
            }

            return Evasion(Defender, GetTerrainBonus(DefenderTerrain, TerrainActivation.OnEveryTurns, TerrainBonus.Evasion), FinalDefenderTerrainMultiplier);
        }

        //(((Attacker Hit Rate + Defender Evasion) * Size Difference Multiplier) + Additive final hit rate effect) * Multiplying final hit rate effect
        public int CalculateHitRate(Unit Attacker, string AttackerTerrainType, Unit Defender, string DefenderTerrainType, Terrain DefenderTerrain, Unit.BattleDefenseChoices DefenseChoice)
        {
            int SizeCompare = Attacker.SizeValue - Defender.SizeValue;

            float BaseHitRate;
            //If the Attacker have an accuracy modifier, use it.
            if (Attacker.Boosts.AccuracyFixedModifier > 0)
                BaseHitRate = Attacker.Boosts.AccuracyFixedModifier;
            //If the Defender have an accuracy modifier, use it.
            else if (Defender.Boosts.EvasionFixedModifier > 0)
                BaseHitRate = 100 - Defender.Boosts.EvasionFixedModifier;
            //Force the defender to dodge the attack.
            else if (Defender.Boosts.AutoDodgeModifier)
                BaseHitRate = 0;
            else//No particular modifier, use basic hit rate formula.
            {
                BaseHitRate = (Accuracy(Attacker, AttackerTerrainType) - Evasion(Defender, DefenderTerrainType, DefenderTerrain)) * (1 + -SizeCompare / 100f);
                if (DefenseChoice == Unit.BattleDefenseChoices.Evade)
                    BaseHitRate *= 0.5f;
            }
            return (int)Math.Max(0, Math.Min(100, BaseHitRate));
        }

        public int CalculateHitRate(Unit Attacker, Squad AttackerSquad, Unit Defender, Squad DefenderSquad, Unit.BattleDefenseChoices DefenseChoice)
        {
            string AttackerTerrainType;
            string DefenderTerrainType;
            Terrain DefenderTerrain;

            if (AttackerSquad.IsFlying)
            {
                AttackerTerrainType = "Air";
            }
            else
            {
                AttackerTerrainType = GetTerrainType(AttackerSquad.X, AttackerSquad.Y, AttackerSquad.LayerIndex);
            }

            if (DefenderSquad.IsFlying)
            {
                DefenderTerrainType = "Air";
                DefenderTerrain = null;
            }
            else
            {
                DefenderTerrainType = GetTerrainType(DefenderSquad.X, DefenderSquad.Y, DefenderSquad.LayerIndex);
                DefenderTerrain = GetTerrain(DefenderSquad);
            }

            return CalculateHitRate(Attacker, AttackerTerrainType, Defender, DefenderTerrainType, DefenderTerrain, DefenseChoice);
        }

        private static int GetTerrainBonus(Terrain ActiveTerrain, TerrainActivation TerrainActivationType, TerrainBonus TerrainBonusType)
        {
            if (ActiveTerrain == null)
            {
                return 0;
            }

            int Output = 0;

            for (int i = 0; i < ActiveTerrain.ListActivation.Length; i++)
            {
                if (ActiveTerrain.ListActivation[i] == TerrainActivationType && ActiveTerrain.ListBonus[i] == TerrainBonusType)
                    Output += ActiveTerrain.ListBonusValue[i];
            }

            return Output;
        }

        private BattleResult GetBattleResult(Unit Attacker, Squad AttackerSquad, float DamageModifier, Unit Defender, Squad DefenderSquad, bool ActivateSkills, bool CalculateCritical)
        {
            ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.BeforeAttackRequirementName, DefenderSquad, Defender);
            ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.BeforeGettingAttackedRequirementName, AttackerSquad, Attacker);
            
            BattleResult Result;

            int BaseHitRate;

            BaseHitRate = CalculateHitRate(Attacker, AttackerSquad, Defender, DefenderSquad, Defender.BattleDefenseChoice);

            bool AttackHit = RandomHelper.RandomActivationCheck(BaseHitRate);
            
            if (AttackHit)
            {
                if (ActivateSkills)
                {
                    ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.BeforeHitRequirementName, DefenderSquad, Defender);
                    ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.BeforeGettingHitRequirementName, AttackerSquad, Attacker);
                }

                Result = DamageFormula(Attacker, AttackerSquad, DamageModifier, Defender, DefenderSquad, Defender.BattleDefenseChoice, CalculateCritical);
            }
            else
            {
                if (ActivateSkills)
                {
                    ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.BeforeMissRequirementName, DefenderSquad, Defender);
                    ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.BeforeGettingMissedRequirementName, AttackerSquad, Attacker);
                }

                Result = new BattleResult();
                Result.AttackDamage = 0;
                Result.AttackMissed = true;
            }

            Result.Accuracy = BaseHitRate;
            Result.Target = Defender;
            //Remove EN from the weapon cost.
            if (Attacker.CurrentAttack.ENCost > 0)
            {
                Result.AttackAttackerFinalEN = Math.Max(0, Attacker.EN - (Attacker.CurrentAttack.ENCost + Attacker.Boosts.ENCostModifier));
            }
            else
            {
                Result.AttackAttackerFinalEN = Attacker.EN;
            }

            GlobalBattleContext.Result = Result;

            if (ActivateSkills)
            {
                Attacker.UpdateSkillsLifetime(SkillEffect.LifetimeTypeOnAttack);
                Defender.UpdateSkillsLifetime(SkillEffect.LifetimeTypeOnEnemyAttack);

                if (AttackHit)
                {
                    Attacker.UpdateSkillsLifetime(SkillEffect.LifetimeTypeOnHit);
                    Defender.UpdateSkillsLifetime(SkillEffect.LifetimeTypeOnEnemyHit);

                    ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.AfterHitRequirementName, DefenderSquad, Defender);
                    ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.AfterGettingHitRequirementName, AttackerSquad, Attacker);
                }
                else
                {
                    ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.AfterMissRequirementName, DefenderSquad, Defender);
                    ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.AfterGettingMissedRequirementName, AttackerSquad, Attacker);
                }

                ActivateAutomaticSkills(AttackerSquad, Attacker, DeathmatchSkillRequirement.AfterAttackRequirementName, DefenderSquad, Defender);
                ActivateAutomaticSkills(DefenderSquad, Defender, DeathmatchSkillRequirement.AfterGettingAttackedRequirementName, AttackerSquad, Attacker);
            }

            return Result;
        }

        public void FinalizeBattle(Squad Attacker, SupportSquadHolder ActiveSquadSupport, int AttackerPlayerIndex, Squad TargetSquad, SupportSquadHolder TargetSquadSupport, int DefenderPlayerIndex, SquadBattleResult ResultAttack, SquadBattleResult ResultDefend)
        {
            Squad Target = TargetSquad;
            if (TargetSquadSupport.ActiveSquadSupport != null)
            {
                Target = TargetSquadSupport.ActiveSquadSupport;
                //Remove 1 Support Defend.
                --TargetSquadSupport.ActiveSquadSupport.CurrentLeader.Boosts.SupportDefendModifier;
            }

            List<Unit> ListDeadDefender = new List<Unit>();

            FinalizeBattle(Attacker, AttackerPlayerIndex, Target, DefenderPlayerIndex, ResultAttack, ListDeadDefender);

            //Counter attack
            if (TargetSquad.CurrentLeader.BattleDefenseChoice == Unit.BattleDefenseChoices.Attack && TargetSquad.CurrentLeader.HP > 0)
            {
                FinalizeBattle(TargetSquad, DefenderPlayerIndex, Attacker, AttackerPlayerIndex, ResultDefend, new List<Unit>());
            }

            //Support Attack
            if (ActiveSquadSupport.ActiveSquadSupport != null && Attacker.CurrentLeader.HP > 0 && TargetSquad.CurrentLeader.HP > 0)
            {
                //Remove 1 Support Defend.
                --ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.Boosts.SupportAttackModifier;

                FinalizeBattle(ActiveSquadSupport.ActiveSquadSupport.CurrentLeader, ActiveSquadSupport.ActiveSquadSupport, AttackerPlayerIndex,
                    TargetSquad.CurrentLeader, TargetSquad, DefenderPlayerIndex, ResultAttack.ResultSupportAttack, ListDeadDefender);
            }

            Attacker.UpdateSquad();
            if (ActiveSquadSupport != null && ActiveSquadSupport.ActiveSquadSupport != null)
                ActiveSquadSupport.ActiveSquadSupport.UpdateSquad();
            TargetSquad.UpdateSquad();
            if (TargetSquadSupport != null && TargetSquadSupport.ActiveSquadSupport != null)
                TargetSquadSupport.ActiveSquadSupport.UpdateSquad();

            #region Explosions

            //Explosion of death cutscene
            if (Attacker.CurrentLeader == null)
                PushScreen(new BattleMapScreen.ExplosionCutscene(CenterCamera, this, Attacker));
            if (TargetSquad.CurrentLeader == null)
                PushScreen(new BattleMapScreen.ExplosionCutscene(CenterCamera, this, TargetSquad));

            #endregion

            UpdateMapEvent(EventTypeOnBattle, 1);
        }

        private void FinalizeBattle(Squad Attacker, int AttackerPlayerIndex,
                                   Squad Defender, int DefenderPlayerIndex,
                                   SquadBattleResult Result, List<Unit> ListDeadDefender)
        {
            for (int U = 0; U < Attacker.UnitsAliveInSquad; U++)
            {
                FinalizeBattle(Attacker[U], Attacker, AttackerPlayerIndex, Result.ArrayResult[U].Target, Defender, DefenderPlayerIndex, Result.ArrayResult[U], ListDeadDefender);
            }

            if (!Attacker.ListAttackedTeam.Contains(ListPlayer[DefenderPlayerIndex].Team))
                Attacker.ListAttackedTeam.Add(ListPlayer[DefenderPlayerIndex].Team);

            if (!Defender.ListAttackedTeam.Contains(ListPlayer[AttackerPlayerIndex].Team))
                Defender.ListAttackedTeam.Add(ListPlayer[AttackerPlayerIndex].Team);
        }

        private void FinalizeBattle(Unit Attacker, Squad AttackerSquad, int AttackerPlayerIndex,
                                   Unit Defender, Squad DefenderSquad, int DefenderPlayerIndex,
                                   BattleResult Result, List<Unit> ListDeadDefender)
        {
            if (Attacker.CurrentAttack != null && !ListDeadDefender.Contains(Result.Target))
            {
                FinalizeAttack(Attacker, Result);

                //Will Gains
                if (Result.Target.HP <= 0)
                {
                    ListDeadDefender.Add(Result.Target);

                    FinalizeDeath(AttackerSquad, AttackerPlayerIndex, DefenderSquad, DefenderPlayerIndex, Result.Target);

                    for (int C = 0; C < Attacker.ArrayCharacterActive.Length; C++)
                    {
                        Attacker.ArrayCharacterActive[C].Will += Attacker.ArrayCharacterActive[C].Personality.WillGainDestroyedEnemy;
                    }

                    for (int U = 0; U < AttackerSquad.UnitsAliveInSquad; U++)
                    {
                        if (Attacker == AttackerSquad[U])
                            continue;

                        for (int C = 1; C < AttackerSquad[U].ArrayCharacterActive.Length; C++)
                        {
                            AttackerSquad[U].ArrayCharacterActive[C].Will += 2;
                        }
                    }
                }
                else if (Result.AttackMissed)
                {
                    for (int C = 0; C < Attacker.ArrayCharacterActive.Length; C++)
                    {
                        Attacker.ArrayCharacterActive[C].Will += Attacker.ArrayCharacterActive[C].Personality.WillGainMissedEnemy;
                    }

                    for (int C = 0; C < Result.Target.ArrayCharacterActive.Length; C++)
                    {
                        Result.Target.ArrayCharacterActive[C].Will += Result.Target.ArrayCharacterActive[C].Personality.WillGainEvaded;
                    }
                }
                else if (!Result.AttackMissed)
                {
                    for (int C = 0; C < Attacker.ArrayCharacterActive.Length; C++)
                    {
                        Attacker.ArrayCharacterActive[C].Will += Attacker.ArrayCharacterActive[C].Personality.WillGainHitEnemy;
                    }

                    for (int C = 0; C < Result.Target.ArrayCharacterActive.Length; C++)
                    {
                        Result.Target.ArrayCharacterActive[C].Will += Result.Target.ArrayCharacterActive[C].Personality.WillGainGotHit;
                    }
                }
            }
        }

        private void FinalizeDeath(Squad Attacker, int AttackerPlayerIndex,
                                   Squad Defender, int DefenderPlayerIndex,
                                   Unit DeadDefender)
        {
            //Unit killed.
            //Every allies gain morale.
            for (int P = 0; P < ListPlayer.Count; P++)
            {
                if (ListPlayer[P].Team == ListPlayer[AttackerPlayerIndex].Team)
                {
                    for (int U = 0; U < ListPlayer[P].ListSquad.Count; U++)
                    {
                        if (ListPlayer[P].ListSquad[U].CurrentLeader == null || ListPlayer[P].ListSquad[U] == Attacker)
                            continue;

                        for (int C = 0; C < ListPlayer[P].ListSquad[U].CurrentLeader.ArrayCharacterActive.Length; C++)
                        {
                            ListPlayer[P].ListSquad[U].CurrentLeader.ArrayCharacterActive[C].Will += 1;
                        }
                    }
                }
                else if (ListPlayer[P].Team == ListPlayer[DefenderPlayerIndex].Team)
                {
                    for (int U = 0; U < ListPlayer[P].ListSquad.Count; U++)
                    {
                        if (ListPlayer[P].ListSquad[U].CurrentLeader == null)
                            continue;

                        for (int C = 0; C < ListPlayer[P].ListSquad[U].CurrentLeader.ArrayCharacterActive.Length; C++)
                        {
                            ListPlayer[P].ListSquad[U].CurrentLeader.ArrayCharacterActive[C].Will += ListPlayer[P].ListSquad[U].CurrentLeader.ArrayCharacterActive[C].Personality.WillGainAlliedUnitDestroyed;
                        }
                    }
                }
            }
        }

        private void FinalizeAttack(Unit UnitAttacker, BattleResult Result)
        {
            Result.Target.DamageUnit(Result.AttackDamage);

            //Remove Leader Ammo if needed.
            if (UnitAttacker.CurrentAttack.MaxAmmo > 0)
                --UnitAttacker.CurrentAttack.Ammo;

            UnitAttacker.ConsumeEN(UnitAttacker.EN - Result.AttackAttackerFinalEN);

            UnitAttacker.PilotEXP += (int)(Result.Target.Pilot.EXPValue * UnitAttacker.Boosts.EXPMultiplier);

            if (UnitAttacker.PilotEXP >= UnitAttacker.PilotNextEXP)
                UnitAttacker.Pilot.LevelUp();

            int Money = 0;
            Constants.Money += (int)(Money * UnitAttacker.Boosts.MoneyMultiplier);
            int PilotPoint = 0;
            UnitAttacker.PilotPilotPoints += (int)(PilotPoint * UnitAttacker.Boosts.PPMultiplier);
            
            ActivateAutomaticSkills(null, UnitAttacker, string.Empty, null, UnitAttacker);
            ActivateAutomaticSkills(null, Result.Target, string.Empty, null, Result.Target);
        }

        public SquadBattleResult CalculateFinalHP(Squad Attacker, Squad SupportAttacker, int AttackerPlayerIndex, FormationChoices AttackerFormationChoice,
            Squad Defender, Squad SupportDefender, int DefenderPlayerIndex, bool ActivateSkills, bool CalculateCritical)
        {
            SquadBattleResult SquadResult = new SquadBattleResult(new BattleResult[Attacker.UnitsAliveInSquad]);

            Squad TargetSquad = Defender;
            if (SupportDefender != null)
            {
                TargetSquad = SupportDefender;
            }
            
            GlobalBattleContext.Result.Target = null;
            GlobalBattleContext.SupportAttack = null;
            GlobalBattleContext.SupportDefend = null;

            if (SupportAttacker != null)
            {
                GlobalBattleContext.SupportAttack = SupportAttacker.CurrentLeader;
            }
            if (SupportDefender != null)
            {
                GlobalBattleContext.SupportDefend = SupportDefender.CurrentLeader;
            }

            int TotalLeaderDamage = 0;

            if (ActivateSkills)
            {
                if (Attacker.CurrentLeader.CurrentAttack != null)
                {
                    TotalLeaderDamage = GetBattleResult(Attacker.CurrentLeader, Attacker, 1, TargetSquad.CurrentLeader, TargetSquad, false, CalculateCritical).AttackDamage;
                }

                ActivateAutomaticSkills(Attacker, Attacker.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad, TargetSquad.CurrentLeader);
                ActivateAutomaticSkills(TargetSquad, TargetSquad.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, Attacker, Attacker.CurrentLeader);

                if (AttackerFormationChoice == FormationChoices.Spread)
                {
                    for (int i = 1; i < Attacker.UnitsAliveInSquad && i < TargetSquad.UnitsAliveInSquad; i++)
                    {
                        if (Attacker[i].CurrentAttack != null)
                        {
                            ActivateAutomaticSkills(Attacker, Attacker[i], DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad, TargetSquad[i]);
                            ActivateAutomaticSkills(TargetSquad, TargetSquad[i], DeathmatchSkillRequirement.BattleStartRequirementName, Attacker, Attacker[i]);
                        }
                    }
                }
                else if (AttackerFormationChoice == FormationChoices.Focused)
                {
                    int DefenderHP = TargetSquad.CurrentLeader.HP;

                    for (int i = 1; i < Attacker.UnitsAliveInSquad; i++)
                    {
                        if (Attacker[i].CurrentAttack != null && DefenderHP >= 0)
                        {
                            TotalLeaderDamage += GetBattleResult(Attacker[i], Attacker, WingmanDamageModifier,
                                                                        TargetSquad.CurrentLeader, TargetSquad, false, CalculateCritical).AttackDamage;

                            DefenderHP = TargetSquad.CurrentLeader.ComputeRemainingHPAfterDamage(TotalLeaderDamage);

                            ActivateAutomaticSkills(Attacker, Attacker[i], DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad, TargetSquad.CurrentLeader);
                            ActivateAutomaticSkills(TargetSquad, TargetSquad.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, Attacker, Attacker[i]);
                        }
                    }
                }
                else if (AttackerFormationChoice == FormationChoices.ALL)
                {
                    for (int i = 1; i < TargetSquad.UnitsAliveInSquad; i++)
                    {
                        ActivateAutomaticSkills(Attacker, Attacker.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad, TargetSquad[i]);
                        ActivateAutomaticSkills(TargetSquad, TargetSquad[i], DeathmatchSkillRequirement.BattleStartRequirementName, Attacker, Attacker.CurrentLeader);
                    }
                }

                if (SupportAttacker != null && TargetSquad.CurrentLeader.ComputeRemainingHPAfterDamage(TotalLeaderDamage) > 0)
                {
                    ActivateAutomaticSkills(SupportAttacker, SupportAttacker.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad, TargetSquad.CurrentLeader);
                    ActivateAutomaticSkills(TargetSquad, TargetSquad.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, SupportAttacker, SupportAttacker.CurrentLeader);
                }

                if (SupportDefender != null)
                {
                    ActivateAutomaticSkills(SupportDefender, SupportDefender.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, Attacker);
                    ActivateAutomaticSkills(SupportDefender, SupportDefender.CurrentLeader, DeathmatchSkillRequirement.SupportDefendRequirementName, Attacker);
                }
            }

            if (Attacker.CurrentLeader.CurrentAttack != null)
            {
                SquadResult.ArrayResult[0] = GetBattleResult(Attacker.CurrentLeader, Attacker, 1, TargetSquad.CurrentLeader, TargetSquad, ActivateSkills, CalculateCritical);
            }

            TotalLeaderDamage = SquadResult.ArrayResult[0].AttackDamage;

            if (AttackerFormationChoice == FormationChoices.Spread)
            {
                for (int i = 1; i < Attacker.UnitsAliveInSquad && i < TargetSquad.UnitsAliveInSquad; i++)
                {
                    if (Attacker[i].CurrentAttack != null)
                    {
                        SquadResult.ArrayResult[i] = GetBattleResult(Attacker[i], Attacker, WingmanDamageModifier,
                                                                    TargetSquad[i], TargetSquad, ActivateSkills, CalculateCritical);
                    }
                }
            }
            else if (AttackerFormationChoice == FormationChoices.Focused)
            {
                int DefenderHP = TargetSquad.CurrentLeader.HP;

                for (int i = 1; i < Attacker.UnitsAliveInSquad; i++)
                {
                    if (Attacker[i].CurrentAttack != null && DefenderHP >= 0)
                    {
                        SquadResult.ArrayResult[i] = GetBattleResult(Attacker[i], Attacker, WingmanDamageModifier,
                                                                    TargetSquad.CurrentLeader, TargetSquad, ActivateSkills, CalculateCritical);

                        TotalLeaderDamage += SquadResult.ArrayResult[i].AttackDamage;

                        DefenderHP = TargetSquad.CurrentLeader.ComputeRemainingHPAfterDamage(SquadResult.ArrayResult[i].AttackDamage);
                    }
                }
            }
            else if (AttackerFormationChoice == FormationChoices.ALL)
            {
                for (int i = 1; i < TargetSquad.UnitsAliveInSquad; i++)
                {
                    SquadResult.ArrayResult[i] = GetBattleResult(Attacker.CurrentLeader, Attacker, 1,
                                                                TargetSquad[i], TargetSquad, ActivateSkills, CalculateCritical);
                }
            }

            if (SupportAttacker != null && TargetSquad.CurrentLeader.ComputeRemainingHPAfterDamage(TotalLeaderDamage) > 0)
            {
                if (ActivateSkills)
                {
                    ActivateAutomaticSkills(SupportAttacker, SupportAttacker.CurrentLeader, DeathmatchSkillRequirement.BattleStartRequirementName, TargetSquad);
                    ActivateAutomaticSkills(SupportAttacker, SupportAttacker.CurrentLeader, DeathmatchSkillRequirement.SupportAttackRequirementName, TargetSquad);
                }
                
                SquadResult.ResultSupportAttack = GetBattleResult(SupportAttacker.CurrentLeader, SupportAttacker, 0.75f, TargetSquad.CurrentLeader, TargetSquad, ActivateSkills, CalculateCritical);
            }

            if (ActivateSkills)
            {
                UpdateMapEvent(EventTypeOnBattle, 0);
            }

            return SquadResult;
        }
    }
}
