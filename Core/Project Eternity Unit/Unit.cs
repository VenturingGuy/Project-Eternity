﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using FMOD;
using Roslyn;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectEternity.Core.Item;
using ProjectEternity.Core.Parts;
using ProjectEternity.Core.Attacks;
using ProjectEternity.Core.Characters;
using ProjectEternity.Core.Skill;

namespace ProjectEternity.Core.Units
{
    public class UnitAnimations
    {
        public AnimationInfo[] ArrayAnimationPath;

        public AnimationInfo Default { get { return ArrayAnimationPath[0]; } }

        public AnimationInfo Hit { get { return ArrayAnimationPath[1]; } }

        public AnimationInfo HitToDefault { get { return ArrayAnimationPath[2]; } }

        public AnimationInfo MoveFoward { get { return ArrayAnimationPath[3]; } }

        public AnimationInfo MoveFowardToDefault { get { return ArrayAnimationPath[4]; } }

        public AnimationInfo MoveBackward { get { return ArrayAnimationPath[5]; } }

        public AnimationInfo MoveBackwardToDefault { get { return ArrayAnimationPath[6]; } }

        public AnimationInfo Destroyed { get { return ArrayAnimationPath[7]; } }

        public AnimationInfo Shield { get { return ArrayAnimationPath[8]; } }

        public AnimationInfo ShieldToDefault { get { return ArrayAnimationPath[9]; } }

        public AnimationInfo ParryStart { get { return ArrayAnimationPath[10]; } }

        public AnimationInfo ParryEnd { get { return ArrayAnimationPath[11]; } }

        public AnimationInfo ShootDownFiring { get { return ArrayAnimationPath[12]; } }

        public AnimationInfo ShootDownShot { get { return ArrayAnimationPath[13]; } }

        public UnitAnimations()
        {
            ArrayAnimationPath = new AnimationInfo[14];
        }

        public UnitAnimations(BinaryReader BR)
        {
            int AnimtionCount = BR.ReadInt32();
            ArrayAnimationPath = new AnimationInfo[AnimtionCount];

            for (int A = 0; A < AnimtionCount; ++A)
                ArrayAnimationPath[A] = new AnimationInfo(BR.ReadString());
        }

        public string this[int i]
        {
            set
            {
                ArrayAnimationPath[i] = new AnimationInfo(value);
            }
        }
    }

    public class StatsBoosts
    {
        /// <summary>
        /// Used to add actions choices after another action.
        /// </summary>
        public struct PostActionValue
        {
            public bool Attack;
            public bool Transform;
            public bool Spirit;
            public bool Move;
        }
        
        public PostActionValue PostMovementModifier;
        public PostActionValue PostAttackModifier;
        public bool AttackFirstModifier;
        public bool AutoDodgeModifier;
        public bool IgnoreDefenseBonusModifier;
        public bool NullifyDamageModifier;
        public bool RepairModifier;
        public bool ResupplyModifier;
        public bool ShieldModifier;
        public bool SwordCutModifier;
        public bool ShootDownModifier;
        public int SupportAttackModifier;
        public int SupportAttackModifierMax;
        public int SupportDefendModifier;
        public int SupportDefendModifierMax;
        public List<string> ParryModifier;
        public List<string> NullifyAttackModifier;

        public int MovementModifier;
        public int RangeModifier;
        public int ExtraActionsPerTurn;

        public int FinalDamageModifier;
        public float FinalDamageMultiplier;
        public int FinalDamageTakenFixedModifier;
        public int BaseDamageModifier;
        public float BaseDamageMultiplier;
        public float BaseDamageTakenReductionMultiplier;

        public int CriticalHitRateModifier;
        public bool CriticalAlwaysSucceed;
        public bool CriticalAlwaysFail;

        public int   CriticalFinalDamageModifier;
        public float CriticalFinalDamageMultiplier;
        public int   CriticalBaseDamageModifier;
        public float CriticalBaseDamageMultiplier;
        public int   CriticalDamageTakenFixedModifier;
        public float CriticalDamageTakenReductionMultiplier;

        public float EXPMultiplier;
        public float PPMultiplier;
        public float MoneyMultiplier;
        public int HPMinModifier;
        public int MoraleModifier;
        public int ENCostModifier;
        public int AmmoMaxModifier;

        public int HPMaxModifier;
        public int ENMaxModifier;
        public int ArmorModifier;
        public int MobilityModifier;
        public int MVMaxModifier;
        public float HPMaxMultiplier;
        public float ENMaxMultiplier;
        public float ArmorMultiplier;
        public float MobilityMultiplier;
        public float MVMaxMultiplier;
        public Dictionary<string, int> DicTerrainLetterAttributeModifier;

        public int AccuracyModifier;
        public int AccuracyFixedModifier;//Give a fixed accuracy value.
        public float AccuracyMultiplier;
        public int EvasionModifier;
        public int EvasionFixedModifier;//Give a fixed evasion value.

        public StatsBoosts()
        {
            PostMovementModifier = new PostActionValue();
            PostAttackModifier = new PostActionValue();
            ParryModifier = new List<string>();
            NullifyAttackModifier = new List<string>();
            DicTerrainLetterAttributeModifier = new Dictionary<string, int>();
            Reset();
        }

        public void Reset()
        {
            PostMovementModifier = new PostActionValue();
            PostAttackModifier = new PostActionValue();
            AttackFirstModifier = false;
            AutoDodgeModifier = false;
            IgnoreDefenseBonusModifier = false;
            NullifyDamageModifier = false;
            RepairModifier = false;
            ResupplyModifier = false;
            ShieldModifier = false;
            SwordCutModifier = false;
            ShootDownModifier = false;
            ParryModifier.Clear();
            NullifyAttackModifier.Clear();

            MovementModifier = 0;
            RangeModifier = 0;
            ExtraActionsPerTurn = 0;
            FinalDamageModifier = 0;
            FinalDamageMultiplier = 1;
            FinalDamageTakenFixedModifier = 0;
            BaseDamageModifier = 0;
            BaseDamageMultiplier = 1;
            BaseDamageTakenReductionMultiplier = 1;
            CriticalHitRateModifier = 0;
            CriticalAlwaysSucceed = false;
            CriticalAlwaysFail = false;

            CriticalFinalDamageModifier = 0;
            CriticalFinalDamageMultiplier = 1;
            CriticalBaseDamageModifier = 0;
            CriticalBaseDamageMultiplier = 0;
            CriticalDamageTakenFixedModifier = 0;
            CriticalDamageTakenReductionMultiplier = 1;

            EXPMultiplier = 1;
            PPMultiplier = 1;
            MoneyMultiplier = 1;
            HPMinModifier = 0;
            MoraleModifier = 0;
            ENCostModifier = 0;
            AmmoMaxModifier = 0;

            HPMaxModifier = 0;
            ENMaxModifier = 0;
            ArmorModifier = 0;
            MobilityModifier = 0;
            MVMaxModifier = 0;

            HPMaxMultiplier = 1;
            ENMaxMultiplier = 1;
            ArmorMultiplier = 1;
            MobilityMultiplier = 1;
            MVMaxMultiplier = 1;

            Dictionary<string, int> DicTerrainLetterAttributeModifierCopy = new Dictionary<string, int>(DicTerrainLetterAttributeModifier.Count);

            foreach (var Terrain in DicTerrainLetterAttributeModifier)
            {
                DicTerrainLetterAttributeModifierCopy.Add(Terrain.Key, 0);
            }

            DicTerrainLetterAttributeModifier = DicTerrainLetterAttributeModifierCopy;

            AccuracyModifier = 0;
            AccuracyFixedModifier = 0;//Give a fixed accuracy value.
            AccuracyMultiplier = 1;
            EvasionModifier = 0;
            EvasionFixedModifier = 0;//Give a fixed evasion value.

            SupportAttackModifier = 0;
            SupportAttackModifierMax = 0;
            SupportDefendModifier = 0;
            SupportDefendModifierMax = 0;
        }
    }
    
    public abstract partial class Unit : ShopItem
    {
        public static char[] Grades = new char[6] { '-', 'S', 'A', 'B', 'C', 'D' };

        /// <summary>
        /// List of Unit path refered by their Name.
        /// </summary>

        public enum AnimationTypes { EnemyNeutral, AttackPhase1, AttackPhase2, AttackPhase3, AttackMiss, EnemyHit, EnemyDodge1, EnemyDodge2, EnemyRecover, EnemyDamage };

        public enum BattleDefenseChoices { Attack = 0, Defend = 1, Evade = 2, DoNothing = 3 };

        #region Variables

        public FMODSound BattleTheme { get { return string.IsNullOrEmpty(Pilot.BattleThemeName) ? null : Character.DicBattleTheme[Pilot.BattleThemeName]; } }

        public string BattleThemeName { get { return Pilot.BattleThemeName; } }

        public abstract string UnitTypeName { get; }//Used to reconstruct Units from files.

        protected UnitStats _UnitStat;
        public UnitStats UnitStat { get { return _UnitStat; } }

        public TagSystem TeamTags;//Used to make units unavailable
        public string UnitTags;//Used to categorize units
        public string TeamEventID;

        public string SpriteMapPath;
        public Texture2D SpriteMap;
        public string SpriteUnitPath;
        public Texture2D SpriteUnit;
        
        protected int _HP;
        protected int _EN;
        public int HP { get { return _HP; } }
        public int EN { get { return _EN; } }
        public string Size { get { return _UnitStat.Size; } }

        #region Stats getters

        public int MaxHP { get { return _UnitStat.MaxHP; } set { _UnitStat.MaxHP = value; } }

        public int MaxEN { get { return _UnitStat.MaxEN; } set { _UnitStat.MaxEN = value; } }

        public int RegenEN { get { return _UnitStat.RegenEN; } set { _UnitStat.RegenEN = value; } }

        public int Armor { get { return _UnitStat.Armor; } set { _UnitStat.Armor = value; } }

        public int Mobility { get { return _UnitStat.Mobility; } set { _UnitStat.Mobility = value; } }

        public int MaxMovement { get { return _UnitStat.MaxMovement; } set { _UnitStat.MaxMovement = value; } }

        #endregion

        #region Pilot getters and setters

        public Character Pilot
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0];
                else
                    return null;
            }
        }

        public string PilotName
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].Name;
                else
                    return "";
            }
        }

        public int PilotLevel
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].Level;
                else
                    return 0;
            }
        }

        public int PilotEXP
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].EXP;
                else
                    return 0;
            }
            set
            {
                if (ArrayCharacterActive.Length > 0)
                    ArrayCharacterActive[0].EXP = value;
            }
        }

        public int PilotNextEXP
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].NextEXP;
                else
                    return 0;
            }
        }

        public int PilotMorale
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].Will;
                else
                    return 0;
            }
            set
            {
                if (ArrayCharacterActive.Length > 0)
                    ArrayCharacterActive[0].Will = value;
            }
        }

        public int PilotSP
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].SP;
                else
                    return 0;
            }
            set
            {
                if (ArrayCharacterActive.Length > 0)
                    ArrayCharacterActive[0].SP = value;
            }
        }

        public int PilotMaxSP
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].MaxSP;
                else
                    return 0;
            }
        }

        public int PilotKills
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].Kills;
                else
                    return 0;
            }
            set
            {
                if (ArrayCharacterActive.Length > 0)
                    ArrayCharacterActive[0].Kills = value;
            }
        }

        public int PilotPilotPoints
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].PilotPoints;
                else
                    return 0;
            }
            set
            {
                if (ArrayCharacterActive.Length > 0)
                    ArrayCharacterActive[0].PilotPoints = value;
            }
        }

        public int PilotMEL
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].MEL;
                else
                    return 0;
            }
        }

        public int PilotRNG
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].RNG;
                else
                    return 0;
            }
        }

        public int PilotDEF
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].DEF;
                else
                    return 0;
            }
        }

        public int PilotSKL
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].SKL;
                else
                    return 0;
            }
        }

        public int PilotEVA
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].EVA;
                else
                    return 0;
            }
        }

        public int PilotHIT
        {
            get
            {
                if (ArrayCharacterActive.Length > 0)
                    return ArrayCharacterActive[0].HIT;
                else
                    return 0;
            }
        }

        #endregion

        public bool CanAttack { get { foreach (Attack ActiveAttack in ListAttack) { if (ActiveAttack.CanAttack) return true; }; return false; } }//If the character is in range to attack.
        public BattleDefenseChoices BattleDefenseChoice;

        public int MaxCharacter = 5;
        public Character[] ArrayCharacterActive;
        public UnitPart[] ArrayParts;
        public BaseAutomaticSkill[] ArrayUnitAbility { get { return _UnitStat.ArrayUnitAbility; } }

        public UnitAnimations Animations { get { return _UnitStat.Animations; } }

        public Dictionary<string, int> DicTerrainValue { get { return _UnitStat.DicTerrainValue; } }

        public List<string> ListTerrainChoices { get { return _UnitStat.ListTerrainChoices; } }

        public List<string> ListIgnoreSkill;//List used to ignore other skills.
        public List<string> ListCharacterIDWhitelist { get { return _UnitStat.ListCharacterIDWhitelist; } }

        public List<Attack> ListAttack { get { return _UnitStat.ListAttack; } }

        public int PLAAttack { get { return _UnitStat.PLAAttack; } }

        public int AttackIndex;
        public Attack CurrentAttack { get { return AttackIndex >= 0 && AttackIndex < _UnitStat.ListAttack.Count ? _UnitStat.ListAttack[AttackIndex] : null; } }

        public string AttackAccuracy;
        public string MAPAttackAccuracyA;
        public string MAPAttackAccuracyB;

        public StatsBoosts Boosts { get { return _UnitStat.Boosts; } }

        #endregion

        protected Unit()
            : this(null)
        {
        }

        protected Unit(string Name)
            : base(Name)
        {
            ListIgnoreSkill = new List<string>();
            AttackIndex = -1;
            AttackAccuracy = "";
            _UnitStat = new UnitStats(new bool[1, 1] { { true } });
            ArrayCharacterActive = new Character[0];
            ArrayParts = new UnitPart[0];
            TeamTags = new TagSystem();
            TeamEventID = "";
        }

        public void ShareStats(Unit UnitToShareFrom, UnitStats.UnitLinkTypes UnitLinkType)
        {
            _UnitStat.ShareStats(UnitToShareFrom._UnitStat, UnitLinkType);
        }

        public static Dictionary<string, Unit> LoadAllUnits()
        {
            Dictionary<string, Unit> DicUnitType = new Dictionary<string, Unit>();

            foreach (KeyValuePair<string, Unit> ActiveUnit in LoadFromAssemblyFiles(Directory.GetFiles("Units", "*.dll", SearchOption.AllDirectories)))
            {
                DicUnitType.Add(ActiveUnit.Key, ActiveUnit.Value);
            }

            List<Assembly> ListAssembly = RoslynWrapper.GetCompiledAssembliesFromFolder("Units", "*.csx", SearchOption.AllDirectories);
            foreach (Assembly ActiveAssembly in ListAssembly)
            {
                foreach (KeyValuePair<string, Unit> ActiveUnit in LoadFromAssembly(ActiveAssembly, typeof(Unit)))
                {
                    DicUnitType.Add(ActiveUnit.Key, ActiveUnit.Value);
                }
            }

            return DicUnitType;
        }

        public static Dictionary<string, Unit> LoadFromAssembly(Assembly ActiveAssembly, params object[] Args)
        {
            Dictionary<string, Unit> DicUnitType = new Dictionary<string, Unit>();
            List<Unit> ListSkillEffect = ReflectionHelper.GetObjectsFromBaseTypes<Unit>(typeof(Unit), ActiveAssembly.GetTypes(), Args);

            foreach (Unit Instance in ListSkillEffect)
            {
                DicUnitType.Add(Instance.UnitTypeName, Instance);
            }

            return DicUnitType;
        }

        public static Dictionary<string, Unit> LoadFromAssemblyFiles(string[] ArrayFilePath, params object[] Args)
        {
            Dictionary<string, Unit> DicUnitType = new Dictionary<string, Unit>();

            for (int F = 0; F < ArrayFilePath.Length; F++)
            {
                Assembly ActiveAssembly = Assembly.LoadFile(Path.GetFullPath(ArrayFilePath[F]));
                foreach(KeyValuePair<string, Unit> ActiveUnit in LoadFromAssembly(ActiveAssembly, Args))
                {
                    DicUnitType.Add(ActiveUnit.Key, ActiveUnit.Value);
                }
            }

            return DicUnitType;
        }

        public static Unit FromFullName(string Name, Microsoft.Xna.Framework.Content.ContentManager Content,
            Dictionary<string, Unit> DicUnitType, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            string[] UnitInfo = Name.Split(new[] { "/" }, StringSplitOptions.None);
            return FromType(UnitInfo[0], Name.Remove(0, UnitInfo[0].Length + 1), Content, DicUnitType, DicRequirement, DicEffect);
        }

        public static Unit FromType(string UnitType, string Name, Microsoft.Xna.Framework.Content.ContentManager Content, 
            Dictionary<string, Unit> DicUnitType, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            return DicUnitType[UnitType].FromFile(Name, Content, DicRequirement, DicEffect);
        }

        public abstract Unit FromFile(string Name, Microsoft.Xna.Framework.Content.ContentManager Content, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect);

        public void QuickSave(BinaryWriter BW)
        {
            BW.Write(UnitTypeName);
            BW.Write(_UnitStat.Name);
            BW.Write(HP);
            BW.Write(EN);

            BW.Write(ArrayCharacterActive.Length);
            for (int C = 0; C < ArrayCharacterActive.Length; ++C)
            {
                ArrayCharacterActive[C].QuickSave(BW);
            }

            DoQuickSave(BW);
        }

        protected abstract void DoQuickSave(BinaryWriter BW);

        public void QuickLoad(BinaryReader BR, Microsoft.Xna.Framework.Content.ContentManager Content, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            _HP = BR.ReadInt32();
            _EN = BR.ReadInt32();

            int ArrayCharacterActiveLength = BR.ReadInt32();
            ArrayCharacterActive = new Character[ArrayCharacterActiveLength];
            for (int C = 0; C < ArrayCharacterActive.Length; ++C)
            {
                ArrayCharacterActive[C] = Character.QuickLoadFromFile(BR, Content, DicRequirement, DicEffect);
            }

            DoQuickLoad(BR, Content);
        }

        protected abstract void DoQuickLoad(BinaryReader BR, Microsoft.Xna.Framework.Content.ContentManager Content);

        public static Unit QuickLoadFromFile(BinaryReader BR, Microsoft.Xna.Framework.Content.ContentManager Content, Dictionary<string, Unit> DicUnitType, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            string UnitType = BR.ReadString();
            string UnitName = BR.ReadString();

            Unit NewUnit = Unit.FromType(UnitType, UnitName, Content, DicUnitType, DicRequirement, DicEffect);
            NewUnit.QuickLoad(BR, Content, DicRequirement, DicEffect);

            return NewUnit;
        }

        public void Init()
        {
            //Initialise the Unit stats.
            _HP = MaxHP;
            _EN = MaxEN;
            foreach (Character ActiveCharacter in ArrayCharacterActive)
            {
                ActiveCharacter.Init();
            }

            _UnitStat.Init();
            DoInit();
        }

        public abstract void DoInit();

        public void UpdateAllAttacks(Vector3 StartPosition, Vector3 TargetPosition, string TargetMovementType, bool CanMove)
        {
            for (int A = 0; A < ListAttack.Count; A++)
            {
                if (!ListAttack[A].CanAttack)
                {
                    ListAttack[A].UpdateAttack(this, StartPosition, TargetPosition, TargetMovementType, CanMove);
                }
            }
        }

        public void UpdateNonMAPAttacks(Vector3 StartPosition, Vector3 TargetPosition, string TargetMovementType, bool CanMove)
        {
            for (int A = 0; A < ListAttack.Count; A++)
            {
                if (ListAttack[A].Pri != WeaponPrimaryProperty.MAP)
                {
                    ListAttack[A].UpdateAttack(this, StartPosition, TargetPosition, TargetMovementType, CanMove);
                }
                else
                {
                    ListAttack[A].DisableAttack();
                }
            }
        }

        public void DisableAllAttacks()
        {
            for (int A = 0; A < ListAttack.Count; A++)
            {
                ListAttack[A].DisableAttack();
            }
        }

        public void ReloadSkills(Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect, Dictionary<string, ManualSkillTarget> DicTarget)
        {
            for (int P = 0; P < ArrayParts.Length; ++P)
            {
                if (ArrayParts[P] != null && ArrayParts[P].PartType == PartTypes.Consumable)
                {
                    UnitConsumablePart ActivePart = (UnitConsumablePart)ArrayParts[P];
                    ActivePart.Spirit.ReloadSkills(DicRequirement, DicEffect, DicTarget);
                }
            }

            for (int C = 0; C < ArrayCharacterActive.Length; ++C)
            {
                ArrayCharacterActive[C].ReloadSkills(DicRequirement, DicEffect, DicTarget);
            }
        }

        public abstract void ReinitializeMembers(Unit InitializedUnitBase);

        public void KillUnit()
        {
            _HP = 0;
        }

        public void DamageUnit(int Damage)
        {
            int NewHP = ComputeRemainingHPAfterDamage(Damage);
            if (_HP <= 0)
            {
                _HP = NewHP;
            }
            else
            {
                _HP = NewHP;
            }
        }

        public int ComputeRemainingHPAfterDamage(int Damage)
        {
            return Math.Max(Boosts.HPMinModifier, HP - Damage);
        }

        public void HealUnit(int HealValue)
        {
            _HP = Math.Min(_HP + HealValue, MaxHP);
        }

        public void ConsumeEN(int Amount)
        {
            _EN = Math.Max(_EN - Amount, 0);
        }

        public void RefillEN(int Amount)
        {
            _EN = Math.Min(_EN + Amount, MaxEN);
        }

        public void ResetBoosts()
        {
            Boosts.Reset();
            ListIgnoreSkill.Clear();
            if (ArrayCharacterActive == null)
                return;

            for (int C = ArrayCharacterActive.Length - 1; C >= 0; --C)
            {
                if (ArrayCharacterActive[C] == null)
                {
                    continue;
                }
                ArrayCharacterActive[C].BonusMEL = 0;
                ArrayCharacterActive[C].BonusRNG = 0;
                ArrayCharacterActive[C].BonusDEF = 0;
                ArrayCharacterActive[C].BonusSKL = 0;
                ArrayCharacterActive[C].BonusEVA = 0;
                ArrayCharacterActive[C].BonusHIT = 0;
                ArrayCharacterActive[C].BonusMaxSP = 0;
                ArrayCharacterActive[C].MaxWill = 150;
                ArrayCharacterActive[C].WillBonus = 0;
            }
        }

        public void ActivePassiveBuffs()
        {
            for (int P = 0; P < ArrayParts.Length; ++P)
            {
                if (ArrayParts[P] != null)
                {
                    ArrayParts[P].ActivatePassiveBuffs();
                }
            }
        }

        public int SizeValue
        {
            get
            {
                switch (Size)
                {
                    case "SS":
                        return -20;

                    case "S":
                        return -10;

                    case "M":
                        return 0;

                    case "L":
                        return 10;

                    case "2L":
                        return 20;

                    case "3L":
                        return 30;
                }
                return 0;
            }
        }

        public void ExecuteSkillsEffects()
        {
            ResetBoosts();
            for (int C = ArrayCharacterActive.Length - 1; C >= 0; --C)
            {
                ArrayCharacterActive[C].Effects.ExecuteAllEffects(ListIgnoreSkill);
            }
        }

        public void UpdateSkillsLifetime(string LifetimeType)
        {
            for (int C = ArrayCharacterActive.Length - 1; C >= 0; --C)
            {
                ArrayCharacterActive[C].Effects.UpdateAllEffectsLifetime(LifetimeType);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveSquad">Can be null if not using a Squad</param>
        public virtual void OnPlayerPhaseStart(Squad ActiveSquad)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveSquad">Can be null if not using a Squad</param>
        public virtual void OnTurnEnd(Squad ActiveSquad)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveSquad">Can be null if not using a Squad</param>
        /// <param name="ListActionMenuChoice">Action Panel Holder</param>
        public virtual List<ActionPanel> OnMenuMovement(Squad ActiveSquad, ActionPanelHolder ListActionMenuChoice)
        { return new List<ActionPanel>(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveSquad">Can be null if not using a Squad</param>
        /// <param name="ListActionMenuChoice">Action Panel Holder</param>
        public virtual List<ActionPanel> OnMenuSelect(Squad ActiveSquad, ActionPanelHolder ListActionMenuChoice)
        { return new List<ActionPanel>(); }
        
        public static Unit LoadUnitWithProgress(BinaryReader BR, Microsoft.Xna.Framework.Content.ContentManager Content, Dictionary<string, Unit> DicUnitType, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            string UnitFullName = BR.ReadString();
            string UnitTypeName = BR.ReadString();
            string TeamEventID = BR.ReadString();
            Unit NewUnit = Unit.FromType(UnitTypeName, UnitFullName, Content, DicUnitType, DicRequirement, DicEffect);
            NewUnit.TeamEventID = TeamEventID;

            int ArrayCharacterActiveLength = BR.ReadInt32();
            NewUnit.ArrayCharacterActive = new Character[ArrayCharacterActiveLength];
            for (int C = 0; C < ArrayCharacterActiveLength; C++)
            {
                NewUnit.ArrayCharacterActive[C] = Character.LoadCharacterWithProgression(BR, Content, DicRequirement, DicEffect);
            }

            return NewUnit;
        }

        public void Save(BinaryWriter BW)
        {
            BW.Write(_UnitStat.Name);
            BW.Write(UnitTypeName);
            BW.Write(TeamEventID);

            BW.Write(ArrayCharacterActive.Length);
            for (int C = 0; C < ArrayCharacterActive.Length; C++)
            {
                ArrayCharacterActive[C].SaveProgression(BW);
            }
        }

        public abstract GameScreens.GameScreen GetCustomizeScreen();

        #region TerrainAttributes
        
        public char TerrainLetterAttribute(string Terrain)
        {
            return Grades[Math.Min(Grades.Length - 1, _UnitStat.TerrainAttributeValue(Terrain))];
        }

        public int TerrainMobilityAttribute(string Terrain)
        {
            switch (TerrainLetterAttribute(Terrain))
            {
                case 'S':
                    return 20;

                case 'A':
                    return 10;

                case 'B':
                    return 0;

                case 'C':
                    return -15;

                case 'D':
                    return -30;
            }
            return 0;
        }

        public int TerrainArmorAttribute(string Terrain)
        {
            switch (TerrainLetterAttribute(Terrain))
            {
                case 'S':
                    return 20;

                case 'A':
                    return 10;

                case 'B':
                    return 0;

                case 'C':
                    return -10;

                case 'D':
                    return -20;
            }
            return 0;
        }

        #endregion
    }
}
