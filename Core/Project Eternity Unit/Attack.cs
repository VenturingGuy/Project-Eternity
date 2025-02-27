﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ProjectEternity.Core.Item;
using ProjectEternity.Core.Units;
using Microsoft.Xna.Framework;

namespace ProjectEternity.Core.Attacks
{
    public enum WeaponStyle { M, R, Special };

    // The Primary property of an attack. PLA means it is a platoon attack, which can be used to support other units. ALL means that it is an All attack, which hits multiple targets instead of one.
    public enum WeaponPrimaryProperty : byte { None = 0, PLA = 1, ALL = 2, Break = 3, Multi = 4, Dash = 5, MAP = 6 };

    /* The secondary property of an attack. Examples of Secondary Properties are:
    * P: Post Movement. Means that the attack can be used after moving.
    * B: Beam Attack. Enemies that are weak to Beams will take more damage, but certain barriers will reduce their damage to 0.
    * S: Special Attack. Can reduce an enemy's stats for three turns. Which stat is reduced is listed under Special:, on the bottom right corner of the menu.*/
    [Flags]
    public enum WeaponSecondaryProperty : byte { None = 0, PostMovement = 0x1, SwordCut = 0x2, ShootDown = 0x4 };

    public enum WeaponMAPProperties : byte {  Spread = 0, Direction = 1, Targeted = 2 };

    public class AttackAnimations
    {
        public AnimationInfo[] ArrayAnimationPath;

        public AnimationInfo Start { get { return ArrayAnimationPath[0]; } set { ArrayAnimationPath[0] = value; } }

        public AnimationInfo EndHit { get { return ArrayAnimationPath[1]; } set { ArrayAnimationPath[1] = value; } }

        public AnimationInfo EndMiss { get { return ArrayAnimationPath[2]; } set { ArrayAnimationPath[2] = value; } }

        public AnimationInfo EndDestroyed { get { return ArrayAnimationPath[3]; } set { ArrayAnimationPath[3] = value; } }

        public AnimationInfo EndBlocked { get { return ArrayAnimationPath[4]; } set { ArrayAnimationPath[4] = value; } }

        public AnimationInfo EndParried { get { return ArrayAnimationPath[5]; } set { ArrayAnimationPath[5] = value; } }

        public AnimationInfo EndShootDown { get { return ArrayAnimationPath[6]; } set { ArrayAnimationPath[6] = value; } }

        public AnimationInfo EndNegated { get { return ArrayAnimationPath[7]; } set { ArrayAnimationPath[7] = value; } }

        public AttackAnimations()
        {
            ArrayAnimationPath = new AnimationInfo[8];
        }

        public AnimationInfo this[int Index]
        {
            get
            {
                return ArrayAnimationPath[Index];
            }
            set
            {
                ArrayAnimationPath[Index] = value;
            }
        }

        public int Count { get { return ArrayAnimationPath.Length; } }
    }

    public struct MAPAttackAttributes
    {
        public int Width;
        public int Height;
        public List<List<bool>> ListChoice; // When Directional, default direction is down.
        public WeaponMAPProperties Property;
        public bool FriendlyFire;
        public int Delay;

        public MAPAttackAttributes(BinaryReader BR)
        {
            Width = BR.ReadInt32();
            Height = BR.ReadInt32();
            ListChoice = new List<List<bool>>(Width * 2 + 1);
            for (int X = 0; X < Width * 2 + 1; X++)
            {
                ListChoice.Add(new List<bool>(Height * 2 + 1));
                for (int Y = 0; Y < Height * 2 + 1; Y++)
                    ListChoice[X].Add(BR.ReadBoolean());
            }

            Property = (WeaponMAPProperties)BR.ReadByte();
            FriendlyFire = BR.ReadBoolean();
            Delay = BR.ReadInt32();
        }

        public bool CanAttackTarget(Vector3 StartPosition, Vector3 TargetPosition, int StartingMV, int FinishingMV)
        {
            int StartX = (int)StartPosition.X;
            int StartY = (int)StartPosition.Y;
            int TargetX = (int)TargetPosition.X;
            int TargetY = (int)TargetPosition.Y;
            
            if (Property == WeaponMAPProperties.Spread)
            {
                for (int X = ListChoice.Count - 1; X >= 0; --X)
                {
                    for (int Y = ListChoice[X].Count - 1; Y >= 0; --Y)
                    {
                        //Not an active tile.
                        if (!ListChoice[X][Y])
                            continue;
                        
                        //Start a the cursor position.
                        float PosX = StartX - Width + X;
                        float PosY = StartY - Height + Y;

                        //If a Unit is in range.
                        if (PosX == TargetX && PosY == TargetY)
                        {
                            return true;
                        }
                    }
                }
            }

            #region Dirrection

            else if (Property == WeaponMAPProperties.Direction)
            {
                if (GetTargetsDirectionalUp(StartPosition).Contains(TargetPosition))
                    return true;

                if (GetTargetsDirectionalDown(StartPosition).Contains(TargetPosition))
                    return true;

                if (GetTargetsDirectionalLeft(StartPosition).Contains(TargetPosition))
                    return true;

                if (GetTargetsDirectionalRight(StartPosition).Contains(TargetPosition))
                    return true;
            }

            #endregion

            else if (Property == WeaponMAPProperties.Targeted)
            {
                int DistanceX = Math.Abs(StartX - TargetX);
                int DistanceY = Math.Abs(StartY - TargetY);
                //If a Unit is in range.
                if (DistanceX + Width >= StartingMV && DistanceX <= FinishingMV + Width &&
                    DistanceY + Height >= StartingMV && DistanceY <= FinishingMV + Height)
                {
                    for (int X = ListChoice.Count - 1; X >= 0; --X)
                    {
                        for (int Y = ListChoice[X].Count - 1; Y >= 0; --Y)
                        {
                            //Not an active tile.
                            if (!ListChoice[X][Y])
                                continue;
                            int DistanceCenterX = X - Width;
                            int DistanceCenterY = Y - Height;

                            if (DistanceX >= StartingMV + DistanceCenterX && DistanceX <= FinishingMV + DistanceCenterX &&
                                DistanceY >= StartingMV + DistanceCenterY && DistanceY <= FinishingMV + DistanceCenterY)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public List<Vector3> GetTargetsDirectionalUp(Vector3 Position)
        {
            List<Vector3> AttackChoice = new List<Vector3>();

            float PosX = Position.X - Width;
            float PosY = Position.Y - 1;

            for (int X = 0; X < ListChoice.Count; X++)
            {
                for (int Y = 0; Y < ListChoice[X].Count; Y++)
                {
                    if (ListChoice[X][Y])
                    {
                        AttackChoice.Add(new Vector3(PosX + X, PosY - Y, Position.Z));
                    }
                }
            }

            return AttackChoice;
        }

        public List<Vector3> GetTargetsDirectionalDown(Vector3 Position)
        {
            List<Vector3> AttackChoice = new List<Vector3>();

            float PosX = Position.X - Width;
            float PosY = Position.Y + 1;

            for (int X = 0; X < ListChoice.Count; X++)
            {
                for (int Y = 0; Y < ListChoice[X].Count; Y++)
                {
                    if (ListChoice[X][Y])
                    {
                        AttackChoice.Add(new Vector3(PosX + X, PosY + Y, Position.Z));
                    }
                }
            }

            return AttackChoice;
        }

        public List<Vector3> GetTargetsDirectionalLeft(Vector3 Position)
        {
            List<Vector3> AttackChoice = new List<Vector3>();

            float PosX = Position.X - 1;
            float PosY = Position.Y - Width;

            for (int X = 0; X < ListChoice.Count; X++)
            {
                for (int Y = 0; Y < ListChoice[X].Count; Y++)
                {
                    if (ListChoice[X][Y])
                    {
                        AttackChoice.Add(new Vector3(PosX - Y, PosY + X, Position.Z));
                    }
                }
            }

            return AttackChoice;
        }

        public List<Vector3> GetTargetsDirectionalRight(Vector3 Position)
        {
            List<Vector3> AttackChoice = new List<Vector3>();

            float PosX = Position.X + 1;
            float PosY = Position.Y - Width;

            for (int X = 0; X < ListChoice.Count; X++)
            {
                for (int Y = 0; Y < ListChoice[X].Count; Y++)
                {
                    if (ListChoice[X][Y])
                    {
                        AttackChoice.Add(new Vector3(PosX + Y, PosY + X, Position.Z));
                    }
                }
            }

            return AttackChoice;
        }
    }

    public class Attack : ShopItem
    {
        public enum MinorAttributes { PostMovement, Beam, Assault, Status, IgnoreSizePenalty, CombinationAttack };

        public WeaponStyle Style;// - Shows either Melee, Ranged or Special; determines which Pilot Stat to call on.
        public string PowerFormula;
        public int RangeMinimum;
        public int RangeMaximum;// - How close (or far) an enemy must be for this weapon to be used on them. Normally expressed in Min-Max (e.g. 2-4 means the weapon works on enemies between two and four cells away).
        public WeaponPrimaryProperty Pri;
        public WeaponSecondaryProperty Sec;
        public BaseAutomaticSkill[] ArrayAttackAttributes;
        public int Accuracy;// A flat bonus used in the accuracy formula. Hit can be improved via Pilot Abilities, Unit Abilities, and Parts.
        public int Critical;// A flat bonus used in the critical hit rate formula. Crit can be improved via Pilot Abilities, Unit Abilities, and Parts.

        public int Ammo;// Certain attacks use ammo. If they hit 0, they will be unable to use that gun until resupplied. Ammo can can be improved via Pilot Abilities, Unit Abilities, and Parts.
        private int _MaxAmmo;
        public int MaxAmmo { get { return _MaxAmmo; } set { _MaxAmmo = value; Ammo = Math.Min(Ammo, value); } }
        public int ENCost;// Certain attacks use EN. If EN is reduced to 0, the unit will be unable to use that attack. EN costs can be can be lowered via Pilot Abilities, Unit Abilities, and Parts.
        public int MoraleRequirement;// Most attacks require morale to use. Morale is gained by defeating the enemy, but is not lowered by using an attack.

        public string AttackType;/* - Shows which category along the "Ways to knock down your foes" spectrum the weapon lies.
                                * Solid Shot - Machine guns, bazookas and solid-shell cannons.
                                * Energy Shot - Beam rifles, beam cannons and variations on such.
                                * Solid Blade - Metal weapons, both sharp and blunt, used at close range.
                                * Energy Blade - Beam swords and variants thereof.
                                * Melee - Punches, kicks and all the myriad ways.
                                * Remote - Attack drones hitting from all angles. Also comes in Energy and Solid.
                                * Special - Multiple types, or something more.*/
        public string SpecialEffects;// - Any additional information about an attack not shown above. Status attacks use this area to show

        public bool CanAttack { get { return _CanAttack; } }
        private bool _CanAttack;//Tell if an ennemy is in range.

        public List<string> ListQuoteSet;
        public Dictionary<string, char> DicTerrainAttribute;

        public MAPAttackAttributes MAPAttributes;
        public readonly AttackAnimations Animations;
        public readonly bool IsExternal;

        public Attack()
        {
            Animations = new AttackAnimations();
            DicTerrainAttribute = new Dictionary<string, char>();
            DicTerrainAttribute.Add("Land", 'A');
            DicTerrainAttribute.Add("Air", 'A');
            DicTerrainAttribute.Add("Sea", 'A');
            DicTerrainAttribute.Add("Space", 'A');
        }

        public Attack(string Name, string Description, int Price, string PowerFormula, int RangeMin, int RangeMax, WeaponPrimaryProperty Pri, WeaponSecondaryProperty Sec,
            int Hit, int Crit, int AmmoMax, int ENCost, int MoraleRequirement, string Type, Dictionary<string, char> DicTerrainAttribute)
            : base(Name, Description, Price)
        {
            ArrayAttackAttributes = new BaseAutomaticSkill[0];
            this.PowerFormula = PowerFormula;
            this.RangeMinimum = RangeMin;
            this.RangeMaximum = RangeMax;
            this.Pri = Pri;
            this.Sec = Sec;
            this.Accuracy = Hit;
            this.Critical = Crit;
            this.MaxAmmo = AmmoMax;
            this.ENCost = ENCost;
            this.MoraleRequirement = MoraleRequirement;
            this.AttackType = Type;
            this.DicTerrainAttribute = DicTerrainAttribute;
        }

        public Attack(Attack Weapon)
            : this(Weapon.FullName, Weapon.Description, Weapon.Price, Weapon.PowerFormula, Weapon.RangeMinimum, Weapon.RangeMaximum, Weapon.Pri, Weapon.Sec, Weapon.Accuracy, Weapon.Critical, Weapon.MaxAmmo, Weapon.ENCost, Weapon.MoraleRequirement, Weapon.AttackType, Weapon.DicTerrainAttribute)
        {
        }

        public Attack(string AttackPath, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            Animations = new AttackAnimations();
            IsExternal = true;

            FileStream FS = new FileStream("Content/Attacks/" + AttackPath + ".pew", FileMode.Open, FileAccess.Read);
            BinaryReader BR = new BinaryReader(FS, Encoding.UTF8);
            BR.BaseStream.Seek(0, SeekOrigin.Begin);

            Init(BR, AttackPath, DicRequirement, DicEffect);

            FS.Close();
            BR.Close();
        }

        public Attack(BinaryReader BR, string AttackPath, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            Animations = new AttackAnimations();
            IsExternal = false;

            Init(BR, AttackPath, DicRequirement, DicEffect);
        }

        public void Init(BinaryReader BR, string AttackName, Dictionary<string, BaseSkillRequirement> DicRequirement, Dictionary<string, BaseEffect> DicEffect)
        {
            //Create the Part file.
            this.FullName = AttackName;
            this.ItemName = Path.GetFileNameWithoutExtension("Content/Attacks/" + AttackName + ".pew");
            this.Description = BR.ReadString();

            this.PowerFormula = BR.ReadString();
            this.ENCost = BR.ReadInt32();
            this.MaxAmmo = BR.ReadInt32();
            this.MoraleRequirement = BR.ReadInt32();
            this.RangeMinimum = BR.ReadInt32();
            this.RangeMaximum = BR.ReadInt32();
            this.Accuracy = BR.ReadInt32();
            this.Critical = BR.ReadInt32();

            this.Pri = (WeaponPrimaryProperty)BR.ReadByte();
            if (this.Pri == WeaponPrimaryProperty.MAP)
            {
                MAPAttributes = new MAPAttackAttributes(BR);
            }

            this.Sec = (WeaponSecondaryProperty)BR.ReadByte();

            int AttackType = BR.ReadInt32();

            if (AttackType == 0)
            {
                this.AttackType = "Blank";
                Style = WeaponStyle.M;
            }
            else if (AttackType == 1)
            {
                this.AttackType = "Melee";
                Style = WeaponStyle.M;
            }
            else if (AttackType == 2)
            {
                this.AttackType = "Solid blade";
                Style = WeaponStyle.M;
            }
            else if (AttackType == 3)
            {
                this.AttackType = "Energy blade";
                Style = WeaponStyle.M;
            }
            else if (AttackType == 4)
            {
                this.AttackType = "Solid shot";
                Style = WeaponStyle.R;
            }
            else if (AttackType == 5)
            {
                this.AttackType = "Energy shot";
                Style = WeaponStyle.R;
            }
            else if (AttackType == 6)
            {
                this.AttackType = "Remote";
                Style = WeaponStyle.R;
            }
            else if (AttackType == 7)
            {
                this.AttackType = "Special";
                Style = WeaponStyle.R;
            }

            int TerrainGradeAir = BR.ReadInt32();
            int TerrainGradeLand = BR.ReadInt32();
            int TerrainGradeSea = BR.ReadInt32();
            int TerrainGradeSpace = BR.ReadInt32();

            char[] Grades = new char[6] { '-', 'S', 'A', 'B', 'C', 'D' };
            DicTerrainAttribute = new Dictionary<string, char>(4);
            DicTerrainAttribute.Add("Air", Grades[TerrainGradeAir]);
            DicTerrainAttribute.Add("Land", Grades[TerrainGradeLand]);
            DicTerrainAttribute.Add("Sea", Grades[TerrainGradeSea]);
            DicTerrainAttribute.Add("Space", Grades[TerrainGradeSpace]);

            Int32 AttackAttributesCount = BR.ReadInt32();
            this.ArrayAttackAttributes = new BaseAutomaticSkill[AttackAttributesCount];

            for (int S = 0; S < AttackAttributesCount; ++S)
                ArrayAttackAttributes[S] = new BaseAutomaticSkill("Content/Attacks/Attributes/" + BR.ReadString() + ".peaa", DicRequirement, DicEffect);

            ListQuoteSet = new List<string>();
            int ListQuoteSetCount = BR.ReadInt32();
            for (int R = 0; R < ListQuoteSetCount; R++)
                ListQuoteSet.Add(BR.ReadString());
        }

        public void UpdateAttack(Unit CurrentUnit, Vector3 StartPosition, Vector3 TargetPosition, string TargetMovementType, bool CanMove)
        {
            if (CanAttackTarget(CurrentUnit, StartPosition, TargetPosition, TargetMovementType, CanMove))
            {
                _CanAttack = true;
            }
            else
            {
                _CanAttack = false;
            }
        }

        public void DisableAttack()
        {
            _CanAttack = false;
        }

        private bool CanAttackTarget(Unit CurrentUnit, Vector3 StartPosition, Vector3 TargetPosition, string TargetMovementType, bool CanMove)
        {
            //If the Mech have enough EN to use the weapon and the weapon have enough ammo to fire.
            if (CurrentUnit.EN < ENCost || (Ammo <= 0 && MaxAmmo > 0)
                || MoraleRequirement > CurrentUnit.PilotMorale)
                return false;

            if (DicTerrainAttribute[TargetMovementType] == '-')
                return false;

            //Define the minimum and maximum value of the attack range.
            int MinDistance = RangeMinimum;
            int MaxDistance = RangeMaximum;
            if (MaxDistance > 1)
                MaxDistance += CurrentUnit.Boosts.RangeModifier;

            //If it's a post-movement weapon or the mech can still move.
            if (CanMove || (!CanMove && ((Sec & WeaponSecondaryProperty.PostMovement) == WeaponSecondaryProperty.PostMovement ||
                                            (CurrentUnit.Boosts.PostMovementModifier.Attack && CurrentUnit.Boosts.PostMovementModifier.Attack))))
            {
                if (Pri == WeaponPrimaryProperty.MAP)
                {
                    return MAPAttributes.CanAttackTarget(StartPosition, TargetPosition, MinDistance, MaxDistance);
                }
                else
                {
                    float Distance = Math.Abs(StartPosition.X - TargetPosition.X) + Math.Abs(StartPosition.Y - TargetPosition.Y);
                    //If a Unit is in range.
                    if (Distance >= MinDistance && Distance <= MaxDistance)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int GetPower(Unit Owner)
        {
            if (Owner != null)
            {
                return (int)(Convert.ToInt32(FormulaParser.ActiveParser.Evaluate(PowerFormula)) * (1 + Owner.UnitStat.AttackUpgrades.Value * 0.05));
            }
            else
            {
                return Convert.ToInt32(FormulaParser.ActiveParser.Evaluate(PowerFormula));
            }
        }

        public override string ToString()
        {
            return ItemName;
        }

        #region Terrain attributes

        public int TerrainAttribute(string Terrain)
        {
            switch (DicTerrainAttribute[Terrain])
            {
                case 'S':
                    return 20;

                case 'A':
                    return 0;

                case 'B':
                    return -20;

                case 'C':
                    return -40;

                case 'D':
                    return -60;
            }
            return 0;
        }

        #endregion
    }
}
