﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectEternity.Core;
using ProjectEternity.Core.Item;
using ProjectEternity.Core.Magic;
using ProjectEternity.GameScreens.AnimationScreen;

namespace ProjectEternity.GameScreens.TripleThunderScreen
{
    public class CollisionPolygon
    {
        public bool IsDead;
        public Polygon ActivePolygon;

        public CollisionPolygon(Polygon ActivePolygon)
        {
            IsDead = false;
            this.ActivePolygon = ActivePolygon;
        }
    }

    public class RobotAnimation : ComplexAnimation
    {
        public uint ID;
        public readonly string Name;
        public int MaxHP;
        public int MaxEN;
        public int HP;
        public int EN;
        public int Team;
        public int Kill;
        public int Death;
        public bool HasKnockback;
        public bool IsDynamic;
        public bool IsUpdated;//Used to disable robot inside vehicles
        public float RespawnTimer;
        public Rectangle Camera;
        public RobotInput InputManager;
        public RobotInputManager InputManagerHelper;

        public Vector2 Speed;
        public Vector2 TotalMovementThisFrame;
        public Vector2 NormalizedGroundVector;
        public Vector2 NormalizedPerpendicularGroundVector;
        public List<Polygon> ListCollidingGroundPolygon;
        public List<Polygon> ListIgnoredGroundPolygon;

        public int CurrentLane;//Current max Y position.
        public List<CollisionPolygon> ListCollisionPolygon;

        public bool LockAnimation;
        protected Weapon CurrentStanceAnimations;
        protected Weapon StandingAnimations { get { return ListStanceAnimation[0]; } }
        protected Weapon CrouchAnimations { get { return ListStanceAnimation.Count > 1 ? ListStanceAnimation[1] : null; } }
        protected Weapon RollAnimations { get { return ListStanceAnimation.Count > 2 ? ListStanceAnimation[2] : null; } }
        protected Weapon ProneAnimations { get { return ListStanceAnimation.Count > 3 ? ListStanceAnimation[3] : null; } }
        public WeaponHolder Weapons;
        protected List<Weapon> ListStanceAnimation;
        public MovementInputs CurrentMovementInput;
        public string ActiveAttackStance;
        public string ActiveMovementStance;
        public int ViewDistance = 20;
        public float Accel;
        public float MaxWalkSpeed;
        public float JumpSpeed;
        public Vector2 LastPositionOnGround;
        public bool IsInAir { get; private set; }
        public bool IsOnGround { get { return !IsInAir; } }

        public readonly EquipmentLoadout Equipment;
        public readonly UnitSounds Sounds;

        public static Random Random = new Random();
        protected Layer CurrentLayer;
        private bool CollisionBetweenRobot = false;
        public Core.AI.AIContainer RobotAI;
        public EffectHolder Effects;
        public ISFXGenerator PlayerSFXGenerator { get; private set; }
        public float GravityMax { get { return Layer.GravityMax; } }
        public float Gravity { get { return Layer.Gravity; } }
        public Vector2 GravityVector { get { return CurrentLayer.GravityVector; } }
        public float Friction { get { return Layer.Friction; } }

        public List<MagicSpell> ListMagicSpell;
        public Dictionary<string, string> DicStoredVariable;// Used for extra stuff

        private RobotAnimation()
            : base()
        {
            IsUpdated = true;
            Camera = new Rectangle(0, 0, Constants.Width, Constants.Height);
            InputManager = new NullRobotInput();
            InputManagerHelper = new NullRobotInputManager();
            Weapons = new WeaponHolder(0);
            IsInAir = false;
            CurrentLane = 1350;
            ListCollisionPolygon = new List<CollisionPolygon>();
            ListCollidingGroundPolygon = new List<Polygon>();
            ListIgnoredGroundPolygon = new List<Polygon>();
            Effects = new EffectHolder();
            ListMagicSpell = new List<MagicSpell>();
            ListMagicSpell.Add(new MagicSpell(null));
            DicStoredVariable = new Dictionary<string, string>();
        }

        /// <summary>
        /// Used for tests
        /// </summary>
        /// <param name="CurrentLayer"></param>
        /// <param name="Position"></param>
        /// <param name="Team"></param>
        /// <param name="ListWeapon"></param>
        public RobotAnimation(Layer CurrentLayer, Vector2 Position, int Team, List<Weapon> ListWeapon)
            : this()
        {
            this.CurrentLayer = CurrentLayer;
            this.Position = Position;
            this.Team = Team;

            if (ListWeapon != null)
            {
                Weapons = new WeaponHolder(ListWeapon.Count);
                foreach (Weapon ActiveWeapon in ListWeapon)
                {
                    Weapons.AddWeaponToStash(ActiveWeapon);
                }
            }

            if (ListWeapon.Count > 0)
                ChangeWeapon(0);
        }

        protected RobotAnimation(string Name, Layer CurrentLayer, Vector2 Position, int Team, EquipmentLoadout Equipment, ISFXGenerator PlayerSFXGenerator)
            : this()
        {
            this.PlayerSFXGenerator = PlayerSFXGenerator.Copy();
            this.Name = Name;
            this.CurrentLayer = CurrentLayer;
            this.Position = Position;
            this.Team = Team;
            this.Equipment = Equipment;
        }

        public RobotAnimation(string Name, Layer CurrentLayer, Vector2 Position, int Team, PlayerEquipment Equipment, ISFXGenerator PlayerSFXGenerator, List<Weapon> ListExtraWeapon)
            : this()
        {
            this.PlayerSFXGenerator = PlayerSFXGenerator.Copy();
            this.Name = Name;
            this.CurrentLayer = CurrentLayer;
            this.Position = Position;
            this.Team = Team;
            this.Equipment = new EquipmentLoadout(Equipment, this);

            FileStream FS = new FileStream("Content/Units/Triple Thunder/" + Name + ".peu", FileMode.Open, FileAccess.Read);
            BinaryReader BR = new BinaryReader(FS, Encoding.UTF8);
            BR.BaseStream.Seek(0, SeekOrigin.Begin);

            MaxHP = BR.ReadInt32();
            MaxEN = BR.ReadInt32();
            Accel = BR.ReadSingle();
            MaxWalkSpeed = BR.ReadSingle();
            JumpSpeed = BR.ReadSingle();
            HasKnockback = BR.ReadBoolean();
            IsDynamic = BR.ReadBoolean();

            int ListExtraAnimationCount = BR.ReadInt32();
            ListStanceAnimation = new List<Weapon>(ListExtraAnimationCount);
            for (int W = 0; W < ListExtraAnimationCount; ++W)
            {
                string ExtraAnimationPath = BR.ReadString();
                if (!string.IsNullOrEmpty(ExtraAnimationPath))
                {
                    if (CurrentLayer == null)
                    {
                        ListStanceAnimation.Add(new Weapon(ExtraAnimationPath, null, null));
                    }
                    else
                    {
                        ListStanceAnimation.Add(new Weapon(ExtraAnimationPath, CurrentLayer.DicRequirement, CurrentLayer.DicEffect));
                    }
                }
            }

            CurrentStanceAnimations = StandingAnimations;

            int ListWeaponCount = BR.ReadInt32();

            Weapons = new WeaponHolder(ListWeaponCount);
            for (int W = 0; W < ListWeaponCount; ++W)
            {
                string WeaponName = BR.ReadString();
                if (CurrentLayer == null)
                {
                    Weapons.AddWeaponToStash(new Weapon(WeaponName, null, null));
                }
                else
                {
                    Weapons.AddWeaponToStash(new Weapon(WeaponName, CurrentLayer.DicRequirement, CurrentLayer.DicEffect));
                }
            }

            if (ListExtraWeapon != null)
            {
                foreach (Weapon ActiveWeapon in ListExtraWeapon)
                {
                    ActiveWeapon.IsExtra = true;
                    Weapons.AddWeaponToStash(ActiveWeapon);
                    Weapons.UseWeapon(ActiveWeapon);
                }
            }

            Sounds = new UnitSounds(BR);

            FS.Close();
            BR.Close();

            Load();

            if (!Weapons.HasActiveWeapons)
            {
                if (Weapons.HasWeapons)
                    ChangeWeapon(0);
                else
                    ChangeWeapon(-1);
            }

            if (CurrentLayer != null)
            {
                UpdateSkills(BaseSkillRequirement.OnCreatedRequirementName);
            }
        }

        public override void Load()
        {
            DicTimeline.Clear();
            foreach (KeyValuePair<string, Timeline> Timeline in LoadTimelines(typeof(CoreTimeline)))
            {
                if (Timeline.Value is AnimationOriginTimeline)
                    continue;

                DicTimeline.Add(Timeline.Key, Timeline.Value);
            }
            foreach (KeyValuePair<string, Timeline> Timeline in LoadTimelines(typeof(TripleThunderTimeline), this))
            {
                DicTimeline.Add(Timeline.Key, Timeline.Value);
            }

            base.Load();

            if (Content != null)
            {
                Weapons.Load(Content);

                for (int W = 0; W < ListStanceAnimation.Count; ++W)
                {
                    if (ListStanceAnimation[W].ActiveProjectileInfo != null && ListStanceAnimation[W].ActiveProjectileInfo.ProjectileAnimation.Path != string.Empty)
                    {
                        ListStanceAnimation[W].ActiveProjectileInfo.ProjectileAnimation.Load(Content, "Animations/Sprites/");
                        ListStanceAnimation[W].NozzleFlashAnimation.Load(Content, "Animations/Sprites/");
                    }
                    if (ListStanceAnimation[W].ActiveProjectileInfo != null && ListStanceAnimation[W].ActiveProjectileInfo.TrailAnimation != null
                        && ListStanceAnimation[W].ActiveProjectileInfo.TrailAnimation.Path != string.Empty)
                    {
                        ListStanceAnimation[W].ActiveProjectileInfo.TrailAnimation.Load(Content, "Animations/Sprites/");
                    }
                    if (ListStanceAnimation[W].ExplosionAttributes.ExplosionAnimation.Path != string.Empty)
                    {
                        ListStanceAnimation[W].ExplosionAttributes.ExplosionAnimation.Load(Content, "Animations/Sprites/");
                    }
                }
            }

            SetAnimation(StandingAnimations.NoneCombo.AnimationName);
            CurrentMovementInput = MovementInputs.None;
            ActiveMovementStance = "None";
            Update(new GameTime());
            SetIdle();
        }

        public void ChangeMap(Rectangle CameraBounds)
        {
            for (int W = 0; W < ListStanceAnimation.Count; ++W)
            {
                ListStanceAnimation[W] = new Weapon(ListStanceAnimation[W].Name, CurrentLayer.DicRequirement, CurrentLayer.DicEffect);
            }

            Weapons.ChangeMap(CurrentLayer.DicRequirement, CurrentLayer.DicEffect);
            InputManager.ResetCameraBounds(CameraBounds);
            Load();
        }

        public void UpdateControls(GameplayTypes GameplayType, Rectangle CameraBounds)
        {
            InputManager = InputManagerHelper.GetRobotInput(GameplayType, this, CameraBounds);
        }

        public void Move(MovementInputs MovementInput)
        {
            Equipment.Move(MovementInput);
            
            if (LockAnimation)
                return;

            if (IsOnGround)
            {
                if (CurrentStanceAnimations == CrouchAnimations && Sounds.CrouchMoveSound != UnitSounds.CrouchMoveSounds.None)
                {
                    PlayerSFXGenerator.PlayCrouchMoveSound(Sounds.CrouchMoveSound);
                }
                else if (CurrentStanceAnimations == ProneAnimations && Sounds.ProneMoveSound != UnitSounds.ProneMoveSounds.None)
                {
                    PlayerSFXGenerator.PlayProneMoveSound(Sounds.ProneMoveSound);
                }
                else if (Sounds.StepGrassSound != UnitSounds.StepGrassSounds.None)
                {
                    PlayerSFXGenerator.PlayStepGrassSound(Sounds.StepGrassSound);
                }
            }

            SetRobotAnimation(ActiveMovementStance);
        }

        public void Crouch()
        {
            if (LockAnimation)
                return;

            if (CurrentStanceAnimations != CrouchAnimations)
            {
                if (CurrentMovementInput == MovementInputs.Left || CurrentMovementInput == MovementInputs.Right)
                {
                    Roll();
                }
                else
                {
                    CurrentStanceAnimations = CrouchAnimations;
                    SetRobotAnimation(ActiveMovementStance);
                    if (Sounds.CrouchStartSound != UnitSounds.CrouchStartSounds.None)
                    {
                        PlayerSFXGenerator.PlayCrouchStartSound(Sounds.CrouchStartSound);
                    }
                }
            }
            else
            {
                CurrentStanceAnimations = CrouchAnimations;
                SetRobotAnimation(ActiveMovementStance);
            }
        }

        public void StartCrouch()
        {
            if (CurrentMovementInput == MovementInputs.Left || CurrentMovementInput == MovementInputs.Right)
            {
                Roll();
            }
            else
            {
                CurrentStanceAnimations = CrouchAnimations;
                SetRobotAnimation(ActiveMovementStance);
                if (Sounds.CrouchStartSound != UnitSounds.CrouchStartSounds.None)
                {
                    PlayerSFXGenerator.PlayCrouchStartSound(Sounds.CrouchStartSound);
                }
            }
        }

        public void Roll()
        {
            LockAnimation = true;
            CurrentStanceAnimations = RollAnimations;
            SetRobotAnimation(CurrentStanceAnimations.NoneCombo.AnimationName);
            CurrentStanceAnimations = CrouchAnimations;
            if (Sounds.RollSound != UnitSounds.RollSounds.None)
            {
                PlayerSFXGenerator.PlayRollSound(Sounds.RollSound);
            }
        }

        public void GoProne()
        {
            if (LockAnimation)
                return;

            if (CurrentMovementInput == MovementInputs.Left || CurrentMovementInput == MovementInputs.Right)
            {
            }
            else
            {
                CurrentStanceAnimations = ProneAnimations;
                SetRobotAnimation(CurrentStanceAnimations.NoneCombo.AnimationName);
                if (Sounds.ProneStartSound != UnitSounds.ProneStartSounds.None)
                {
                    PlayerSFXGenerator.PlayProneStartSound(Sounds.ProneStartSound);
                }
            }
        }

        public void SetIdle()
        {
            if (LockAnimation)
                return;

            if (CurrentStanceAnimations == CrouchAnimations && Sounds.CrouchEndSound != UnitSounds.CrouchEndSounds.None)
            {
                PlayerSFXGenerator.PlayCrouchEndSound(Sounds.CrouchEndSound);
            }
            else if (CurrentStanceAnimations == ProneAnimations && Sounds.ProneEndSound != UnitSounds.ProneEndSounds.None)
            {
                PlayerSFXGenerator.PlayProneEndSound(Sounds.ProneEndSound);
            }

            Equipment.OnIdle();

            CurrentStanceAnimations = StandingAnimations;
            SetRobotAnimation("None");
        }

        public void SetRobotAnimation(string Animation)
        {
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                if (ActiveWeapon.ComboByName(Animation).AnimationType == AnimationTypes.FullAnimation)
                {
                    ActiveWeapon.CurrentAnimation = SetAnimation(ActiveWeapon.ComboByName(Animation).AnimationName);
                }
                else
                {
                    SetAnimation(CurrentStanceAnimations.ComboByName(Animation).AnimationName);
                    if (ActiveWeapon.CurrentAnimation == null)
                    {
                        ActiveWeapon.CurrentAnimation = AddPartialAnimation(ActiveWeapon.ComboByName(Animation).AnimationName);
                        ActiveWeapon.CurrentAnimation.UpdateKeyFrame(ActiveWeapon.CurrentAnimation.ActiveKeyFrame);
                    }
                }
            }
        }

        public void Jump()
        {
            CurrentMovementInput = MovementInputs.Up;
            Equipment.OnJump();
        }

        public void StopJump()
        {
            Equipment.OnStopJump();
        }

        public void UseJetpack(GameTime gameTime)
        {
            Equipment.OnJetpackUse(gameTime);
            IsInAir = true;
        }

        public void Land()
        {
            if (IsInAir)
            {
                IsInAir = false;
                Equipment.OnLand();
                UpdateSkills(TripleThunderRobotRequirement.OnGroundCollisionName);
            }
        }

        public void Fall()
        {
            IsInAir = true;
            Equipment.OnFall();
        }

        public override void Update(GameTime gameTime)
        {
            ActiveMovementStance = "None";
            CurrentMovementInput = MovementInputs.None;

            base.Update(gameTime);
        }

        public virtual void Update(GameTime gameTime, Dictionary<uint, RobotAnimation> DicRobot)
        {
            if (IsOnGround)
            {
                LastPositionOnGround = Position;
            }

            ActiveMovementStance = "None";
            CurrentMovementInput = MovementInputs.None;

            InputManager.Update(gameTime);
            UpdateSkills(TripleThunderRobotRequirement.OnStepName);

            if (CollisionBetweenRobot)
            {
                UpdateRobotCollisionWithRobot(this, DicRobot);
            }

            if (!UpdateRobotCollisionWithWorld(this))
            {
                Move(Speed);
            }

            if (IsDynamic)
            {
                //Ground.X is always positive.
                if (NormalizedGroundVector.X < 0)
                    NormalizedGroundVector = -NormalizedGroundVector;

                if (Speed.X > Friction)
                    Speed.X -= Friction;
                else if (Speed.X < -Friction)
                    Speed.X += Friction;
                else
                    Speed.X = 0;

                Equipment.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected void UpdateRobotCollisionWithRobot(RobotAnimation ActiveRobot, Dictionary<uint, RobotAnimation> DicRobot)
        {
            PolygonCollisionResult FinalCollisionResult = new PolygonCollisionResult(Vector2.Zero, -1);

            foreach (KeyValuePair<uint, RobotAnimation> EnemyRobot in DicRobot)
            {
                if (ActiveRobot == EnemyRobot.Value)
                    continue;

                foreach (CollisionPolygon ActiveCollision in ActiveRobot.ListCollisionPolygon)
                {
                    foreach (CollisionPolygon EnemyCollision in EnemyRobot.Value.ListCollisionPolygon)
                    {
                        PolygonCollisionResult CollisionResult = Polygon.PolygonCollisionSAT(ActiveCollision.ActivePolygon, EnemyCollision.ActivePolygon, ActiveRobot.Speed);

                        if (FinalCollisionResult.Distance < 0 || (CollisionResult.Distance >= 0 && CollisionResult.Distance < FinalCollisionResult.Distance))
                            FinalCollisionResult = CollisionResult;
                    }
                }
            }
            Vector2 FinalMovement;
            if (FinalCollisionResult.Distance >= 0)
            {
                Vector2 MovementCorection = FinalCollisionResult.Axis * FinalCollisionResult.Distance;
                FinalMovement = ActiveRobot.Speed + MovementCorection;
            }
            else
            {
                FinalMovement = ActiveRobot.Speed;
            }
            ActiveRobot.Move(FinalMovement);
        }

        public bool UpdateRobotCollisionWithWorld(RobotAnimation ActiveRobot)
        {
            bool HasCollided = false;
            ActiveRobot.ListCollidingGroundPolygon.Clear();

            List<Tuple<PolygonCollisionResult, Polygon>> ListAllCollidingPolygon;
            List<Tuple<PolygonCollisionResult, Polygon>> ListFloorCollidingPolygon;
            List<Tuple<PolygonCollisionResult, Polygon>> ListCeilingCollidingPolygon;
            List<Tuple<PolygonCollisionResult, Polygon>> ListWallCollidingPolygon;
            CurrentLayer.GetCollidingWorldPolygon(ActiveRobot, out ListAllCollidingPolygon, out ListFloorCollidingPolygon, out ListCeilingCollidingPolygon, out ListWallCollidingPolygon);

            Equipment.OnAnyCollision(ListAllCollidingPolygon);

            Vector2 MovementCorection = Vector2.Zero;
            //Floor
            if (ListFloorCollidingPolygon.Count > 0)
            {
                Equipment.OnFloorCollision(ListFloorCollidingPolygon);

                HasCollided = true;
            }
            //Ceiling
            if (ListCeilingCollidingPolygon.Count > 0)
            {
                Equipment.OnCeilingCollision(ListCeilingCollidingPolygon);

                HasCollided = true;
            }
            //Wall
            if (ListWallCollidingPolygon.Count > 0)
            {
                Equipment.OnWallCollision(ListWallCollidingPolygon);

                HasCollided = true;
            }

            if (ListAllCollidingPolygon.Count == 0)
            {
                if (ActiveRobot.IsOnGround)
                {
                    ActiveRobot.Fall();
                }

                ActiveRobot.ListIgnoredGroundPolygon.Clear();
                ActiveRobot.NormalizedGroundVector = new Vector2(-GravityVector.Y, GravityVector.X);
            }
            else
            {
                HasCollided = true;
                Vector2 FinalMovement = ActiveRobot.Speed + MovementCorection;

                ActiveRobot.Move(FinalMovement);
            }

            return HasCollided;
        }

        public void UpdateSkills(string RequirementName)
        {
            CurrentLayer.SetRobotContext(this);

            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                ActiveWeapon.UpdateSkills(RequirementName);
            }
        }

        public double GetMapVariable(string VariableName)
        {
            return CurrentLayer.GetMapVariable(VariableName);
        }

        public void SetRobotContext(Weapon ActiveWeapon, float Angle, Vector2 Position)
        {
            CurrentLayer.SetRobotContext(this, ActiveWeapon, Angle, Position);
        }

        public void SetAttackContext(Projectile ActiveAttackBox, RobotAnimation Owner, float Angle, Vector2 Position)
        {
            CurrentLayer.SetAttackContext(ActiveAttackBox, Owner, Angle, Position);
        }

        public void Freefall(GameTime gameTime)
        {
            Equipment.OnJetpackRest(gameTime);

            if (IsInAir)
            {
                ActiveMovementStance = "Airborne";
                NormalizedPerpendicularGroundVector = GravityVector;
            }
        }

        public void UseCombo(GameTime gameTime, AttackInputs AttackInput)
        {
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                if (UseCombo(gameTime, AttackInput, ActiveWeapon, false))
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="AttackInput"></param>
        /// <param name="ActiveWeapon"></param>
        /// <returns>Returns true if the combo was used.</returns>
        public bool UseCombo(GameTime gameTime, AttackInputs AttackInput, Weapon ActiveWeapon, bool ForceCombo)
        {
            //Only get the next combo if it is not set to avoid overriding it.
            if (ActiveWeapon.NextCombo == null)
            {
                // Already using a combo, fetch the next combo.
                if (ActiveWeapon.ActiveCombo != null)
                {
                    ActiveWeapon.NextCombo = GetNextCombo(ActiveWeapon.ActiveCombo, AttackInput, CurrentMovementInput, gameTime, ForceCombo);
                    
                    if (ActiveWeapon.NextCombo != null && ActiveWeapon.NextCombo.InstantActivation)
                    {
                        UseNextCombo(ActiveWeapon.NextCombo.AnimationType == AnimationTypes.PartialAnimation, ActiveWeapon);
                    }
                }
                //First use of a combo, use it immediatly.
                else
                {
                    Combo ActiveWeaponCombo = ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance);

                    if (ActiveWeaponCombo != null)
                    {
                        ActiveWeapon.NextCombo = GetNextCombo(ActiveWeaponCombo, AttackInput, CurrentMovementInput, gameTime, ForceCombo);
                        if (ActiveWeapon.NextCombo != null)
                        {
                            ActiveWeapon.ActiveCombo = ActiveWeaponCombo;
                            UseNextCombo(ActiveWeapon.NextCombo.AnimationType == AnimationTypes.PartialAnimation, ActiveWeapon);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private Combo GetNextCombo(Combo ActiveCombo, AttackInputs CurrentAttackInput, MovementInputs CurrentMovementInput, GameTime gameTime, bool ForceCombo)
        {
            for (int C = 0; C < ActiveCombo.ListNextCombo.Count; C++)
            {
                bool InputComplete = ForceCombo;
                InputChoice ActiveInputChoice = ActiveCombo.ListNextCombo[C].ListInputChoice[ActiveCombo.ListNextCombo[C].CurrentInputIndex];

                bool AttackInputAccepted = false;
                if (ActiveInputChoice.AttackInput != AttackInputs.None
                    && (ActiveInputChoice.AttackInput == AttackInputs.AnyHold || ActiveInputChoice.AttackInput == AttackInputs.AnyPress || ActiveInputChoice.AttackInput == CurrentAttackInput))
                {
                    AttackInputAccepted = true;
                }

                bool MovementInputAccepted = false;
                if (ActiveInputChoice.MovementInput == MovementInputs.Any || ActiveInputChoice.MovementInput == CurrentMovementInput)
                {
                    MovementInputAccepted = true;
                }

                bool NextInputDelayAccepted = false;
                if (ActiveInputChoice.CurrentDelay >= ActiveInputChoice.NextInputDelay)
                    NextInputDelayAccepted = true;
                else
                    ActiveInputChoice.CurrentDelay += gameTime.ElapsedGameTime.Milliseconds;

                bool FrameLimitAccepted = false;
                int ComboKeyFrame = ActiveKeyFrame;
                if (ActiveCombo.AnimationType == AnimationTypes.PartialAnimation)
                {
                    for (int P = ListActivePartialAnimation.Count - 1; P >= 0; --P)
                    {
                        if (ListActivePartialAnimation[P].AnimationPath == ActiveCombo.AnimationName)
                            ComboKeyFrame = ListActivePartialAnimation[P].ActiveKeyFrame;
                    }
                }
                if (ComboKeyFrame >= ActiveCombo.ListStart[C] && ComboKeyFrame <= ActiveCombo.ListEnd[C])
                    FrameLimitAccepted = true;

                if (AttackInputAccepted && MovementInputAccepted && NextInputDelayAccepted && FrameLimitAccepted)
                {
                    ActiveCombo.ListNextCombo[C].CurrentInputIndex++;
                    if (ActiveCombo.ListNextCombo[C].CurrentInputIndex >= ActiveCombo.ListNextCombo[C].ListInputChoice.Count)
                    {
                        InputComplete = true;
                    }
                }

                if (InputComplete)
                {
                    ActiveCombo.ListNextCombo[C].Reset();
                    return ActiveCombo.ListNextCombo[C];
                }
            }

            return null;
        }

        private void UseNextCombo(bool IsPartialAnimation, Weapon ActiveWeapon)
        {
            if (!ActiveWeapon.CanBeUsed)
            {
                Reload();
                return;
            }

            bool CanUseNextCombo = false;

            if (ActiveWeapon.ActiveCombo != null)
            {
                if ((ActiveWeapon.ActiveCombo.AnimationType == AnimationTypes.PartialAnimation) == IsPartialAnimation)
                {
                    CanUseNextCombo = true;
                }
                else
                {
                    Combo ActiveWeaponCombo = ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance);

                    if (ActiveWeaponCombo != null)
                    {
                        if ((ActiveWeapon.ActiveCombo.AnimationType == AnimationTypes.PartialAnimation) == IsPartialAnimation)
                        {
                            CanUseNextCombo = true;
                            ActiveWeaponCombo.Reset();
                        }
                    }
                }

                if (CanUseNextCombo && ActiveWeapon.NextCombo != null)
                {
                    if (ActiveWeapon.NextCombo.AnimationType == AnimationTypes.PartialAnimation)
                    {
                        RemovePartialAnimation(ActiveWeapon.ActiveCombo.AnimationName);
                        ActiveWeapon.CurrentAnimation = AddPartialAnimation(ActiveWeapon.NextCombo.AnimationName);
                        ActiveWeapon.CurrentAnimation.UpdateKeyFrame(ActiveWeapon.CurrentAnimation.ActiveKeyFrame++);
                    }
                    else
                    {
                        LockAnimation = true;
                        ActiveWeapon.CurrentAnimation = SetAnimation(ActiveWeapon.NextCombo.AnimationName);
                        ActiveWeapon.CurrentAnimation.UpdateKeyFrame(ActiveWeapon.CurrentAnimation.ActiveKeyFrame);
                    }
                }
            }

            ActiveWeapon.ActiveCombo = ActiveWeapon.NextCombo;
            ActiveWeapon.NextCombo = null;
        }

        public void ChangeWeapon(int WeaponIndex)
        {
            if (WeaponIndex == -1)//Unequip weapon
            {
                foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
                {
                    RemovePartialAnimation(ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).AnimationName);
                }

                Weapons.RemoveAllActiveWeapons();
                Weapons.UseWeapon(CurrentStanceAnimations);
            }
            else
            {
                string WeaponName = Weapons.GetWeaponName(WeaponIndex);
                Weapons.RemoveAllActiveWeapons();
                Weapons.UseWeapon(WeaponName);
                Weapon WeaponToUse = Weapons.GetWeapon(WeaponName);

                WeaponToUse.CurrentAnimation = null;
                Combo ActiveWeaponCombo = WeaponToUse.ActiveWeaponCombo(ActiveMovementStance);
                ActiveWeaponCombo.Reset();

                UseNextCombo(true, WeaponToUse);

                if (WeaponToUse.CurrentAnimation == null)
                {
                    WeaponToUse.CurrentAnimation = AddPartialAnimation(WeaponToUse.ComboByName(ActiveMovementStance).AnimationName);
                    WeaponToUse.CurrentAnimation.UpdateKeyFrame(WeaponToUse.CurrentAnimation.ActiveKeyFrame);
                }
            }
        }

        public void ChangeWeapon(Weapon WeaponToUse)
        {
            if (WeaponToUse == null)//Unequip weapon
            {
                foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
                {
                    RemovePartialAnimation(ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).AnimationName);
                }

                Weapons.RemoveAllActiveWeapons();
                Weapons.UseWeapon(CurrentStanceAnimations);
            }
            else if (!Weapons.HasActiveWeapon(WeaponToUse))
            {
                foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
                {
                    RemovePartialAnimation(ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).AnimationName);
                }

                Weapons.RemoveAllActiveWeapons();
                Weapons.UseWeapon(WeaponToUse);

                WeaponToUse.CurrentAnimation = null;
                Combo ActiveWeaponCombo = WeaponToUse.ActiveWeaponCombo(ActiveMovementStance);
                ActiveWeaponCombo.Reset();
                UseNextCombo(true, WeaponToUse);
                if (WeaponToUse.CurrentAnimation == null)
                {
                    WeaponToUse.CurrentAnimation = AddPartialAnimation(WeaponToUse.ComboByName(ActiveMovementStance).AnimationName);
                    WeaponToUse.CurrentAnimation.UpdateKeyFrame(WeaponToUse.CurrentAnimation.ActiveKeyFrame);
                }
            }
        }

        public void HolsterAndReplaceWeapon(Weapon WeaponToUse)
        {
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                RemovePartialAnimation(ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).AnimationName);
            }

            Weapons.HolsterAllActiveWeapons();
            Weapons.UseWeapon(WeaponToUse);

            WeaponToUse.CurrentAnimation = null;
            Combo ActiveWeaponCombo = WeaponToUse.ActiveWeaponCombo(ActiveMovementStance);
            ActiveWeaponCombo.Reset();
            UseNextCombo(true, WeaponToUse);
            if (WeaponToUse.CurrentAnimation == null)
            {
                WeaponToUse.CurrentAnimation = AddPartialAnimation(WeaponToUse.ComboByName(ActiveMovementStance).AnimationName);
                WeaponToUse.CurrentAnimation.UpdateKeyFrame(WeaponToUse.CurrentAnimation.ActiveKeyFrame);
            }
        }

        public void UnholsterWeaponsIfNeeded()
        {
            if (Weapons.HasHolsteredWeapons)
            {
                if (Weapons.ActivePrimaryWeapons.Contains(Weapons.ActiveSecondaryWeapons[0]))
                {
                    foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
                    {
                        RemovePartialAnimation(ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).AnimationName);
                    }
                    Weapons.RemoveAllActiveWeapons();
                }

                List<Weapon> ListUnHolsteredWeapon = Weapons.UseHolsteredWeapons();

                foreach (Weapon WeaponToUse in ListUnHolsteredWeapon)
                {
                    WeaponToUse.CurrentAnimation = null;
                    Combo ActiveWeaponCombo = WeaponToUse.ActiveWeaponCombo(ActiveMovementStance);
                    ActiveWeaponCombo.Reset();
                    UseNextCombo(true, WeaponToUse);
                    if (WeaponToUse.CurrentAnimation == null)
                    {
                        WeaponToUse.CurrentAnimation = AddPartialAnimation(WeaponToUse.ComboByName(ActiveMovementStance).AnimationName);
                        WeaponToUse.CurrentAnimation.UpdateKeyFrame(WeaponToUse.CurrentAnimation.ActiveKeyFrame);
                    }
                }
            }
        }

        public void FallThroughFloor()
        {
            foreach (Polygon ActivePolygon in ListCollidingGroundPolygon)
            {
                if (!ListIgnoredGroundPolygon.Contains(ActivePolygon))
                    ListIgnoredGroundPolygon.Add(ActivePolygon);
            }
        }

        public void Move(Vector2 Movement)
        {
            Position += Movement;
            TotalMovementThisFrame += Movement;

            foreach (CollisionPolygon ActiveCollision in ListCollisionPolygon)
            {
                ActiveCollision.ActivePolygon.Offset(Movement.X, Movement.Y);
            }
        }

        public void Charge(bool UseSecondaryWeapon, int MaxCharge, int ChargeAmountPerFrame)
        {
            if (UseSecondaryWeapon)
            {
                Weapons.ChargeSecondaryWeapon(ChargeAmountPerFrame, MaxCharge);
            }
            else
            {
                Weapons.ChargePrimaryWeapon(ChargeAmountPerFrame, MaxCharge);
            }
        }
        
        public void Shoot(Vector2 GunNozzlePosition, bool UseSecondaryWeapon)
        {
            if (UseSecondaryWeapon)
            {
                int i = 0;
                foreach (Weapon ActiveWeapon in Weapons.ActiveSecondaryWeapons)
                {
                    Shoot(GunNozzlePosition, ActiveWeapon, i++);
                }

                Weapons.ResetSecondaryWeaponCharge();
            }
            else
            {
                int i = 0;
                foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
                {
                    Shoot(GunNozzlePosition, ActiveWeapon, i++);
                }

                Weapons.ResetPrimaryWeaponCharge();
            }
        }

        private void Shoot(Vector2 GunNozzlePosition, Weapon ActiveWeapon, int WeaponIndex)
        {
            bool CanShoot;

            if (ActiveWeapon.AmmoPerMagazine > 0)
            {
                if (ActiveWeapon.AmmoCurrent > 0)
                {
                    --ActiveWeapon.AmmoCurrent;
                    CanShoot = true;
                }
                else
                {
                    CanShoot = false;
                }
            }
            else
            {
                CanShoot = true;
            }

            VisibleTimeline WeaponSlotTimeline;
            PartialAnimation WeaponAnimation;

            if (CanShoot)
            {
                float OffsetX = GunNozzlePosition.X - AnimationOrigin.Position.X;
                float OffsetY = GunNozzlePosition.Y - AnimationOrigin.Position.Y;

                if (ActiveWeapon.CurrentAnimation != null && DicPartialAnimation.TryGetValue(ActiveWeapon.CurrentAnimation.AnimationPath, out WeaponAnimation))
                {
                    OffsetX = GunNozzlePosition.X - WeaponAnimation.AnimationOrigin.Position.X;
                    OffsetY = GunNozzlePosition.Y - WeaponAnimation.AnimationOrigin.Position.Y;
                }

                float WeaponOffsetX = 0;
                float WeaponOffsetY = 0;
                if (DicActiveAnimationObject.TryGetValue("Weapon Slot " + (WeaponIndex + 1), out WeaponSlotTimeline))
                {
                    WeaponOffsetX = WeaponSlotTimeline.Position.X - AnimationOrigin.Position.X;
                    WeaponOffsetY = WeaponSlotTimeline.Position.Y - AnimationOrigin.Position.Y;
                }

                float Angle = ActiveWeapon.WeaponAngle;

                if (ActiveSpriteEffects == SpriteEffects.FlipHorizontally)
                {
                    WeaponOffsetX = -WeaponOffsetX;
                }

                double LenghtDirX = Math.Cos(Angle) * OffsetX;
                double LenghtDirY = Math.Sin(Angle) * OffsetX;
                double LenghtDirX2 = Math.Cos(Angle + MathHelper.ToRadians(90)) * OffsetY;
                double LenghtDirY2 = Math.Sin(Angle + MathHelper.ToRadians(90)) * OffsetY;

                Vector2 RealGunNozzlePosition = Position + new Vector2(WeaponOffsetX, WeaponOffsetY)
                    + new Vector2((float)(LenghtDirX + LenghtDirX2), (float)(LenghtDirY + LenghtDirY2));

                SetRobotContext(ActiveWeapon, Angle, RealGunNozzlePosition);

                foreach (MagicSpell ActiveSpell in ListMagicSpell)
                {
                    ActiveSpell.ExecuteSpell();
                }

                if (ActiveWeapon.HasSkills)
                {
                    ActiveWeapon.UpdateSkills("Shoot");
                }
                else
                {
                    ActiveWeapon.Shoot(this, RealGunNozzlePosition, Angle, new List<BaseAutomaticSkill>());
                }

                CreateNozzleFlashAnimation(ActiveWeapon.NozzleFlashAnimation, RealGunNozzlePosition, Angle);
            }
        }

        public void CreateNozzleFlashAnimation(SimpleAnimation NozzleFlashAnimation, Vector2 Position, float Angle)
        {
            SimpleAnimation NewVisualEffect = NozzleFlashAnimation.Copy();
            NewVisualEffect.Position = Position;
            NewVisualEffect.Angle = Angle;
            CurrentLayer.ListVisualEffects.Add(NewVisualEffect);
        }

        public void CreateAttackBox(AttackBox NewAttackBox)
        {
            CurrentLayer.AddProjectile(NewAttackBox);
        }

        internal void CreateAttackBox(string WeaponName, Vector2 GunNozzlePosition, List<AttackBox> ListAttack)
        {
            CurrentLayer.AddProjectile(ID, WeaponName, GunNozzlePosition, ListAttack);
        }

        public void Reload()
        {
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                ActiveWeapon.IsReloading = true;
                ActiveWeapon.AmmoCurrent = ActiveWeapon.AmmoPerMagazine;
                if (ActiveWeapon.ReloadCombo != null)
                {
                    ActiveWeapon.NextCombo = ActiveWeapon.ReloadCombo;
                    UseNextCombo(true, ActiveWeapon);
                }
            }
        }

        public void CreateCollisionBox(List<CollisionPolygon> ListNewCollisionPolygon)
        {
            foreach (CollisionPolygon ActiveCollisionPolygon in ListNewCollisionPolygon)
            {
                Vector2 Distance = (ActiveCollisionPolygon.ActivePolygon.Center - AnimationOrigin.Position) * Scale;

                ActiveCollisionPolygon.ActivePolygon.Offset(Position.X - ActiveCollisionPolygon.ActivePolygon.Center.X + Distance.X,
                                                            Position.Y - ActiveCollisionPolygon.ActivePolygon.Center.Y + Distance.Y);

                ActiveCollisionPolygon.ActivePolygon.Scale(Scale);

                bool CollisionPolygonReplaced = false;
                for (int C = ListCollisionPolygon.Count - 1; C >= 0 && !CollisionPolygonReplaced; --C)
                {
                    if (ListCollisionPolygon[C].IsDead)
                    {
                        ListCollisionPolygon[C] = ActiveCollisionPolygon;
                        CollisionPolygonReplaced = true;
                    }
                }

                if (!CollisionPolygonReplaced)
                    ListCollisionPolygon.Add(ActiveCollisionPolygon);

                ActiveCollisionPolygon.IsDead = false;
            }
        }

        public void DeleteCollisionBox(List<CollisionPolygon> ListNewCollisionPolygon)
        {
            foreach (CollisionPolygon ActiveCollisionPolygon in ListNewCollisionPolygon)
            {
                ActiveCollisionPolygon.IsDead = true;
            }
        }

        public Rectangle GetCollisionSize()
        {
            Rectangle CollisionSize = new Rectangle(0, 0, 0, 0);
            foreach (var a in ListCollisionPolygon)
            {
            }

            return CollisionSize;
        }

        #region Animation class methods

        public override void OnMarkerTimelineSpawn(AnimationLayer ActiveLayer, MarkerTimeline ActiveMarker)
        {
            base.OnMarkerTimelineSpawn(ActiveLayer, ActiveMarker);

            ActiveMarker.UpdateAnimationObject(ActiveMarker.SpawnFrame);
        }

        protected override void OnLoopEnd()
        {
            LockAnimation = false;
            base.OnLoopEnd();
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                if (ActiveWeapon.ActiveCombo != null && ActiveWeapon.ActiveCombo.AnimationType == AnimationTypes.FullAnimation)
                {
                    UseNextCombo(false, ActiveWeapon);
                }
            }
        }

        protected override void OnPartialAnimationLoopEnd(PartialAnimation ActivePartialAnimation)
        {
            RemovePartialAnimation(ActivePartialAnimation);
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                if (ActiveWeapon.IsReloading)
                {
                    if (ActiveWeapon.ReloadCombo != null)
                    {
                        if (ActiveWeapon.CurrentAnimation.AnimationPath == ActiveWeapon.ReloadCombo.AnimationName)
                        {
                            ActiveWeapon.IsReloading = false;
                            ActiveWeapon.CurrentAnimation = AddPartialAnimation(ActiveWeapon.ComboByName(ActiveMovementStance).AnimationName);
                            ActiveWeapon.CurrentAnimation.UpdateKeyFrame(ActiveWeapon.CurrentAnimation.ActiveKeyFrame);
                        }
                        else
                        {
                            UseNextCombo(true, ActiveWeapon);
                        }
                    }
                    else
                    {
                        ActiveWeapon.IsReloading = false;
                    }
                }
                else if (ActiveWeapon.CurrentAnimation == ActivePartialAnimation)
                {
                    ActiveWeapon.CurrentAnimation = null;
                    UseNextCombo(true, ActiveWeapon);
                    if (ActiveWeapon.CurrentAnimation == null)
                    {
                        ActiveWeapon.CurrentAnimation = AddPartialAnimation(ActiveWeapon.ComboByName(ActiveMovementStance).AnimationName);
                        ActiveWeapon.CurrentAnimation.UpdateKeyFrame(ActiveWeapon.CurrentAnimation.ActiveKeyFrame);
                    }
                }
                else
                {

                }
            }
            foreach (Weapon ActiveWeapon in Weapons.ActiveSecondaryWeapons)
            {
                if (ActiveWeapon.CurrentAnimation == ActivePartialAnimation)
                {
                    ActiveWeapon.CurrentAnimation = null;
                    UseNextCombo(true, ActiveWeapon);
                }
                else
                {

                }
            }
        }

        #endregion

        public Weapon CreateWeapon(string WeaponName)
        {
            Weapon NewWeapon = new Weapon(WeaponName, CurrentLayer.DicRequirement, CurrentLayer.DicEffect);
            NewWeapon.Load(Content);
            return NewWeapon;
        }

        public void ChangeLayer(Layer TargetLayer)
        {
            if (CurrentLayer != null && TargetLayer != null && CurrentLayer != TargetLayer)
            {
                for (int V = 0; V < CurrentLayer.GroundLevelCollision.ArrayVertex.Length - 1; V++)
                {
                    if (Position.X >= CurrentLayer.GroundLevelCollision.ArrayVertex[V].X && Position.X < CurrentLayer.GroundLevelCollision.ArrayVertex[V + 1].X)
                    {
                        Vector2 VertexLength = CurrentLayer.GroundLevelCollision.ArrayVertex[V + 1] - CurrentLayer.GroundLevelCollision.ArrayVertex[V];
                        float RealPosX = Position.X - CurrentLayer.GroundLevelCollision.ArrayVertex[V].X;
                        float ScaleX = RealPosX / VertexLength.X;
                        float GroundY = ScaleX * VertexLength.Y + CurrentLayer.GroundLevelCollision.ArrayVertex[V].Y;
                        float DistanceFromGround = GroundY - Position.Y;

                        for (int V2 = 0; V2 < TargetLayer.GroundLevelCollision.ArrayVertex.Length - 1; V2++)
                        {
                            if (Position.X >= TargetLayer.GroundLevelCollision.ArrayVertex[V2].X && Position.X < TargetLayer.GroundLevelCollision.ArrayVertex[V2 + 1].X)
                            {
                                Vector2 VertexLength2 = TargetLayer.GroundLevelCollision.ArrayVertex[V2 + 1] - TargetLayer.GroundLevelCollision.ArrayVertex[V2];
                                float RealPosX2 = Position.X - TargetLayer.GroundLevelCollision.ArrayVertex[V2].X;
                                float ScaleX2 = RealPosX2 / VertexLength2.X;
                                float GroundY2 = ScaleX2 * VertexLength2.Y + TargetLayer.GroundLevelCollision.ArrayVertex[V2].Y;

                                float NewY = GroundY2 - DistanceFromGround;
                                Position.Y = NewY;

                                CurrentLayer = TargetLayer;
                                return;
                            }
                        }
                    }
                }
            }

            CurrentLayer = TargetLayer;
        }

        public void UpdateAllWeaponsAngle(Vector2 Target)
        {
            int W = 0;
            foreach (Weapon ActiveWeapon in Weapons.ActivePrimaryWeapons)
            {
                bool RotateTowardMouse = true;

                if (ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance) != null)
                {
                    RotateTowardMouse = ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).ComboRotationType != ComboRotationTypes.None;
                }
                if (ActiveWeapon.ActiveCombo != null)
                {
                    RotateTowardMouse = ActiveWeapon.ActiveCombo.ComboRotationType != ComboRotationTypes.None;
                }

                float Angle = 0;
                if (RotateTowardMouse)
                {
                    Angle = (float)Math.Atan2(Target.Y, Target.X);
                }

                ActiveWeapon.WeaponAngle = Angle;

                if (Angle > -MathHelper.PiOver2 && Angle < MathHelper.PiOver2)
                {
                    ActiveSpriteEffects = SpriteEffects.None;
                }
                else
                {
                    ActiveSpriteEffects = SpriteEffects.FlipHorizontally;
                    Angle = MathHelper.Pi - Angle;
                }

                UpdatePrimaryWeaponAngle(Angle, W++);
            }

            W = 0;
            foreach (Weapon ActiveWeapon in Weapons.ActiveSecondaryWeapons)
            {
                bool RotateTowardMouse = false;

                if (ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance) != null)
                {
                    RotateTowardMouse = ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance).ComboRotationType != ComboRotationTypes.None;
                }
                if (ActiveWeapon.ActiveCombo != null)
                {
                    RotateTowardMouse = ActiveWeapon.ActiveCombo.ComboRotationType != ComboRotationTypes.None;
                }

                float Angle = 0;
                if (RotateTowardMouse)
                {
                    Angle = (float)Math.Atan2(Target.Y, Target.X);
                }

                ActiveWeapon.WeaponAngle = Angle;

                if (Angle > -MathHelper.PiOver2 && Angle < MathHelper.PiOver2)
                {
                    ActiveSpriteEffects = SpriteEffects.None;
                }
                else
                {
                    ActiveSpriteEffects = SpriteEffects.FlipHorizontally;
                    Angle = MathHelper.Pi - Angle;
                }

                UpdateSecondaryWeaponAngle(Angle, W++);
            }
        }

        public void UpdatePrimaryWeaponAngle(float Angle, int WeaponIndex)
        {
            Weapon ActiveWeapon = Weapons.ActivePrimaryWeapons[WeaponIndex];

            VisibleTimeline WeaponSlotTimeline;
            DicActiveAnimationObject.TryGetValue("Weapon Slot " + (WeaponIndex + 1), out WeaponSlotTimeline);
            UpdateWeaponAngle(Angle, ActiveWeapon, WeaponSlotTimeline);
        }

        public void UpdateSecondaryWeaponAngle(float Angle, int WeaponIndex)
        {
            Weapon ActiveWeapon = Weapons.ActiveSecondaryWeapons[WeaponIndex];

            VisibleTimeline WeaponSlotTimeline;
            DicActiveAnimationObject.TryGetValue("Weapon Slot " + (WeaponIndex + 1), out WeaponSlotTimeline);
            UpdateWeaponAngle(Angle, ActiveWeapon, WeaponSlotTimeline);
        }

        public void UpdateWeaponAngle(float Angle, Weapon ActiveWeapon, VisibleTimeline WeaponSlotTimeline)
        {
            if (ActiveWeapon.CurrentAnimation == null)
                return;

            VisibleTimeline WeaponTimeline = ActiveWeapon.CurrentAnimation.AnimationOrigin;
            Combo ActiveWeaponCombo = ActiveWeapon.ActiveWeaponCombo(ActiveMovementStance);

            if (ActiveWeaponCombo != null && WeaponSlotTimeline != null)
            {
                float TranslationX = WeaponTimeline.Position.X;
                float TranslationY = WeaponTimeline.Position.Y;

                if (ActiveWeaponCombo.ComboRotationType == ComboRotationTypes.RotateAroundWeaponSlot)
                {
                    ActiveWeapon.CurrentAnimation.TransformationMatrix =
                        Matrix.CreateTranslation(-TranslationX, -TranslationY, 0)
                        * Matrix.CreateRotationZ(Angle)
                        * Matrix.CreateTranslation(WeaponSlotTimeline.Position.X,
                                                   WeaponSlotTimeline.Position.Y, 0);
                }
                else if (ActiveWeaponCombo.ComboRotationType == ComboRotationTypes.RotateAroundRobot)
                {
                    Vector2 WeaponOffset = WeaponSlotTimeline.Position - AnimationOrigin.Position;
                    float ExtraAngle = (float)Math.Atan2(WeaponOffset.Y, WeaponOffset.X);
                    float WeaponLength = WeaponOffset.Length();

                    double LenghtDirX = Math.Cos(Angle + ExtraAngle) * WeaponLength;
                    double LenghtDirY = Math.Sin(Angle + ExtraAngle) * WeaponLength;

                    Vector2 RealGunNozzlePosition = AnimationOrigin.Position
                        + new Vector2((float)(LenghtDirX), (float)(LenghtDirY));

                    ActiveWeapon.CurrentAnimation.TransformationMatrix =
                        Matrix.CreateTranslation(-TranslationX, -TranslationY, 0)
                        * Matrix.CreateRotationZ(Angle)
                        * Matrix.CreateTranslation(RealGunNozzlePosition.X,
                                                   RealGunNozzlePosition.Y, 0);
                }
                else
                {
                    ActiveWeapon.CurrentAnimation.TransformationMatrix =
                        Matrix.CreateTranslation(-TranslationX, -TranslationY, 0)
                        * Matrix.CreateRotationZ(Angle)
                        * Matrix.CreateTranslation(WeaponSlotTimeline.Position.X,
                                                   WeaponSlotTimeline.Position.Y, 0);
                }
            }
            else
            {
                ActiveWeapon.CurrentAnimation.TransformationMatrix =
                    Matrix.CreateScale(0f);
            }
        }
    }
}
