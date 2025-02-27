﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectEternity.Core;
using ProjectEternity.Core.Item;
using ProjectEternity.GameScreens.AnimationScreen;

namespace ProjectEternity.GameScreens.TripleThunderScreen
{
    public class ProjectileBox : AttackBox
    {
        public enum BulletTrailTypes { StretchAll, StretchPartial, Particule }

        private class BulletTrail
        {
            private BulletTrailTypes BulletTrailType;
            private Texture2D TrailImage;
            private Particle3DSample.ParticleSystem TrailParticuleSystem;
            private int ParticleLifetime;
            private float ParticleScaleAtEnd;

            public void Draw(CustomSpriteBatch g, Vector2 Offset, Vector2 BulletPosition, Vector2 BulletSpeed, float Angle)
            {
                switch (BulletTrailType)
                {
                    case BulletTrailTypes.StretchAll:
                        Vector2 StartPos = BulletPosition - Offset - BulletSpeed;
                        Rectangle LineSize = new Rectangle((int)StartPos.X, (int)StartPos.Y, (int)BulletSpeed.Length() + TrailImage.Height, TrailImage.Height);

                        g.Draw(TrailImage, LineSize, null, Color.White, Angle, new Vector2(0, TrailImage.Height / 2), SpriteEffects.None, 0);
                        break;

                    case BulletTrailTypes.StretchPartial:
                        break;

                    case BulletTrailTypes.Particule:
                        break;
                }
            }
        }

        private Vector2 Position;
        private float Angle;
        private SimpleAnimation ProjectileAnimation;
        private BulletTrail Trail;
        private SimpleAnimation TrailAnimation;
        private ProjectileInfo ActiveProjectileInfo;

        public ProjectileBox(float Damage, Weapon.ExplosionOptions ExplosionAttributes, RobotAnimation Owner,
            Vector2 Position, Vector2 Size, float Angle, ProjectileInfo ActiveProjectileInfo)
            : base(Damage, ExplosionAttributes, Owner, false)
        {
            this.ActiveProjectileInfo = ActiveProjectileInfo;
            this.Position = Position;
            this.Angle = Angle;
            this.AffectedByGravity = ActiveProjectileInfo.AffectedByGravity;

            this.Speed = new Vector2((float)Math.Cos(Angle) * ActiveProjectileInfo.ProjectileSpeed, (float)Math.Sin(Angle) * ActiveProjectileInfo.ProjectileSpeed);

            ProjectileAnimation = ActiveProjectileInfo.ProjectileAnimation.Copy();
            ProjectileAnimation.Position = Position;

            if (ActiveProjectileInfo.TrailAnimation != null)
            {
                TrailAnimation = ActiveProjectileInfo.TrailAnimation.Copy();
                TrailAnimation.Position = Position;
            }

            Owner.SetAttackContext(this, Owner, Angle, Position);

            float MinX = Position.X - Size.X / 2f;
            float MinY = Position.Y - Size.Y / 2f;
            float MaxX = MinX + Size.X;
            float MaxY = MinY + Size.Y;


            Polygon NewPolygon = new Polygon();
            NewPolygon.ArrayVertex = new Vector2[4];
            NewPolygon.ArrayVertex[0] = new Vector2(MinX, MinY);
            NewPolygon.ArrayVertex[1] = new Vector2(MaxX, MaxY);
            NewPolygon.ArrayVertex[2] = new Vector2(MaxX, MaxY);
            NewPolygon.ArrayVertex[3] = new Vector2(MinX, MinY);

            NewPolygon.ComputePerpendicularAxis();
            NewPolygon.ComputerCenter();

            ListCollisionPolygon = new List<Polygon>(1) { NewPolygon };
        }

        public override void DoUpdate(GameTime gameTime)
        {
            if (TimeAlive > 10)
            {
                IsAlive = false;
            }
            else
            {
                //Todo: slowdown with air friction
                //todo: change damage based on speed
                if (AffectedByGravity)
                {
                    Speed += FightingZone.GravityVector;
                }

                if (Speed != Vector2.Zero)
                {
                    Angle = (float)Math.Atan2(Speed.Y, Speed.X);
                }

                Owner.SetAttackContext(this, Owner, Angle, ListCollisionPolygon[0].Center);

                if (ProjectileAnimation != null)
                {
                    ProjectileAnimation.Angle = Angle;
                    ProjectileAnimation.Update(gameTime);
                }

                if (TrailAnimation != null)
                {
                    TrailAnimation.Angle = Angle;
                    TrailAnimation.Update(gameTime);
                }
            }
        }

        public override void SetAngle(float Angle)
        {
            if (ProjectileAnimation != null)
            {
                ProjectileAnimation.Angle = Angle;
            }
            if (TrailAnimation != null)
            {
                TrailAnimation.Angle += Angle;
            }
        }

        public override void Move(GameTime gameTime)
        {
            base.Move(gameTime);

            Position += Speed;

            if (ProjectileAnimation != null)
            {
                ProjectileAnimation.Position += Speed;
            }
            if (TrailAnimation != null)
            {
                TrailAnimation.Position += Speed;
            }

            DistanceTravelled += Speed.Length();
        }

        public override void DrawRegular(CustomSpriteBatch g)
        {
            base.DrawRegular(g);

            if (Trail != null)
            {
                Trail.Draw(g, Vector2.Zero, ListCollisionPolygon[0].Center, Speed, Angle);
            }
            else if (ActiveProjectileInfo.TrailType == 1)
            {
                g.DrawLine(GameScreen.sprPixel, Position, Position + Speed, Color.FromNonPremultiplied(255, 255, 255, 127), 1);
            }

            if (ProjectileAnimation != null)
            {
                ProjectileAnimation.Draw(g);
            }
        }

        public override void DrawAdditive(CustomSpriteBatch g)
        {
            if (TrailAnimation != null && ActiveProjectileInfo.TrailEffectType == 1)
            {
                TrailAnimation.Draw(g);
            }
        }

        public override void OnCollision(PolygonCollisionResult FinalCollisionResult, Polygon FinalCollisionPolygon, out Vector2 CollisionPoint)
        {
            IsAlive = false;
            CollisionPoint = ListCollisionPolygon[0].Center;
        }
    }
}
