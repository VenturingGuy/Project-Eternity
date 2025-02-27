﻿using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using ProjectEternity.Core;
using ProjectEternity.Core.Item;
using ProjectEternity.GameScreens.AnimationScreen;
using Microsoft.Xna.Framework;

namespace ProjectEternity.GameScreens.TripleThunderScreen
{
    public class DamageBoxTimeline : TripleThunderTimeline
    {
        private const string TimelineType = "Damage Box";

        private RobotAnimation Owner;
        private List<Polygon> ListCollisionPolygon;
        private List<Polygon> ListOriginalPolygon;
        public Rectangle SourceRectangle;
        private int _Damage;

        public DamageBoxTimeline()
            : base(TimelineType, "New Damage Box")
        {
            ListCollisionPolygon = new List<Polygon>();
            ListOriginalPolygon = new List<Polygon>();
        }

        /// <summary>
        /// Called by reflection
        /// </summary>
        public DamageBoxTimeline(RobotAnimation Owner)
            : this()
        {
            this.Owner = Owner;
            Origin = new Point(Width / 2, Height / 2);
        }

        public DamageBoxTimeline(BinaryReader BR, RobotAnimation Owner)
            : base(BR, TimelineType)
        {
            this.Owner = Owner;

            _Damage = BR.ReadInt32();

            int ListCollisionPolygonCount = BR.ReadInt32();
            ListCollisionPolygon = new List<Polygon>(ListCollisionPolygonCount);
            ListOriginalPolygon = new List<Polygon>();
            for (int P = ListCollisionPolygonCount - 1; P >= 0; --P)
            {
                Polygon NewPolygon = new Polygon(BR, 1, 1);
                NewPolygon.ComputePerpendicularAxis();
                NewPolygon.ComputerCenter();
                ListCollisionPolygon.Add(NewPolygon);
                ListOriginalPolygon.Add(new Polygon(NewPolygon));
            }

            ComputeSourceRectangle();

            Origin = new Point(Width / 2, Height / 2);

            int DicAnimationSpriteKeyFrameCount = BR.ReadInt32();
            for (int E = 0; E < DicAnimationSpriteKeyFrameCount; E++)
            {
                int Key = BR.ReadInt32();

                VisibleAnimationObjectKeyFrame NewAnimatedBitmapKeyFrame = new VisibleAnimationObjectKeyFrame(BR);

                DicAnimationKeyFrame.Add(Key, NewAnimatedBitmapKeyFrame);
            }
        }

        protected override VisibleTimeline DoLoadCopy(BinaryReader BR, Microsoft.Xna.Framework.Content.ContentManager Content, AnimationClass.AnimationLayer ActiveLayer)
        {
            return new DamageBoxTimeline(BR, Owner);
        }

        protected override void DoSave(BinaryWriter BW)
        {
            BW.Write(_Damage);

            BW.Write(ListCollisionPolygon.Count);
            foreach (Polygon ActiveCollisionPolygon in ListCollisionPolygon)
            {
                ActiveCollisionPolygon.Save(BW);
            }

            BW.Write(DicAnimationKeyFrame.Count);
            foreach (KeyValuePair<int, VisibleAnimationObjectKeyFrame> KeyFrame in DicAnimationKeyFrame)
            {
                BW.Write(KeyFrame.Key);
                KeyFrame.Value.Save(BW);
            }
        }

        public override Timeline Copy(AnimationClass.AnimationLayer ActiveLayer)
        {
            DamageBoxTimeline NewSetMarkerEvent = new DamageBoxTimeline(Owner);

            NewSetMarkerEvent.UpdateFrom(this, ActiveLayer);

            return NewSetMarkerEvent;
        }

        public override List<VisibleTimeline> CreateNewEditorItem(AnimationClass ActiveAnimation, AnimationClass.AnimationLayer ActiveLayer, int KeyFrame, System.Drawing.Point MousePosition)
        {
            List<VisibleTimeline> ReturnValue = new List<VisibleTimeline>();

            Vector2[] LocalPoints = new Vector2[4]
            {
                new Vector2(MousePosition.X - 40, MousePosition.Y - 40),
                new Vector2(MousePosition.X - 40, MousePosition.Y + 40),
                new Vector2(MousePosition.X + 40, MousePosition.Y + 40),
                new Vector2(MousePosition.X + 40, MousePosition.Y - 40),
            };

            List<Polygon> ListPolygon = new List<Polygon>();
            DamageBoxTimeline NewMeleeTimeline = new DamageBoxTimeline();
            PolygonCutterHelper NewSpawner = new PolygonCutterHelper(ActiveLayer.renderTarget, ListPolygon, true);

            Polygon NewPolygon = new Polygon(LocalPoints, GameScreen.GraphicsDevice.Viewport.Width, GameScreen.GraphicsDevice.Viewport.Height);
            NewSpawner.PolygonCutterViewer.ListPolygon.Add(NewPolygon);

            if (NewSpawner.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewMeleeTimeline.SpawnFrame = KeyFrame;
                NewMeleeTimeline.DeathFrame = KeyFrame + 10;
                NewMeleeTimeline.IsUsed = true;//Disable the spawner as we spawn the Marker manually.

                Vector2[] NewArrayVertex = new Vector2[NewSpawner.PolygonCutterViewer.ListPolygon[0].ArrayVertex.Length];
                Array.Copy(NewSpawner.PolygonCutterViewer.ListPolygon[0].ArrayVertex, NewArrayVertex, NewSpawner.PolygonCutterViewer.ListPolygon[0].ArrayVertex.Length);

                VisibleAnimationObjectKeyFrame NewPolygonCutterKeyFrame = new VisibleAnimationObjectKeyFrame(NewMeleeTimeline.Position,
                                                                                true, -1);

                Polygon NewKeyFramePolygon = new Polygon(NewArrayVertex, ActiveLayer.renderTarget.Width, ActiveLayer.renderTarget.Height);

                NewMeleeTimeline.Add(KeyFrame, NewPolygonCutterKeyFrame);
                NewMeleeTimeline.ListCollisionPolygon.Add(NewKeyFramePolygon);
                NewMeleeTimeline.ListOriginalPolygon.Add(new Polygon(NewKeyFramePolygon));
                NewKeyFramePolygon.UpdateWorldPosition(Vector2.Zero, 0f);

                NewMeleeTimeline.ComputeSourceRectangle();
                ReturnValue.Add(NewMeleeTimeline);
            }

            return ReturnValue;
        }

        public override void UpdateAnimationObject(int KeyFrame)
        {
            //An Event is being executed.
            if (NextEvent != null)
            {
                UpdateAnimationSprite(KeyFrame);
            }

            VisibleAnimationObjectKeyFrame ActiveKeyFrame;
            VisibleAnimationObjectKeyFrame ActiveAnimationSpriteKeyFrame;

            if (DicAnimationKeyFrame.TryGetValue(KeyFrame, out ActiveAnimationSpriteKeyFrame))
            {
                ActiveKeyFrame = ActiveAnimationSpriteKeyFrame;
                //If that animation has already been used, skip it.
                if (ActiveKeyFrame.IsUsed)
                    return;

                int NextKeyFrame = ActiveKeyFrame.NextKeyFrame;
                OnNewKeyFrameAnimationSprite(ActiveKeyFrame);
                for (int C = ListCollisionPolygon.Count - 1; C >= 0; --C)
                {
                    ListCollisionPolygon[C].Offset(Position.X, Position.Y);
                }

                if (Owner != null)
                {
                    MeleeBox NewAttackBox = new MeleeBox(_Damage, Owner, (DeathFrame - SpawnFrame) / FramesPerSeconds, true);

                    for (int C = ListCollisionPolygon.Count - 1; C >= 0; --C)
                    {
                        ListCollisionPolygon[C].Offset(Owner.Position.X - Owner.AnimationOrigin.Position.X, Owner.Position.Y - Owner.AnimationOrigin.Position.Y);
                    }

                    NewAttackBox.ListCollisionPolygon = ListCollisionPolygon;
                    //TODO: delete the attack box is the animation is cancelled
                    Owner.CreateAttackBox(NewAttackBox);
                }

                if (DicAnimationKeyFrame.TryGetValue(NextKeyFrame, out ActiveAnimationSpriteKeyFrame))
                {
                    ActiveKeyFrame = ActiveAnimationSpriteKeyFrame;
                    if (ActiveKeyFrame.IsProgressive)
                    {
                        OnProgressiveNextKeyFrameAnimationSprite(ActiveKeyFrame, KeyFrame, NextKeyFrame);
                    }
                    else
                    {
                        NextEvent = null;
                    }
                }
            }
        }

        public override void SpawnItem(AnimationClass ActiveAnimation, AnimationClass.AnimationLayer ActiveLayer, int KeyFrame)
        {
            for (int C = ListCollisionPolygon.Count - 1; C >= 0; --C)
            {
                for (int V = 0; V < ListCollisionPolygon[C].VertexCount; V++)
                {
                    ListCollisionPolygon[C].ArrayVertex[V] = ListOriginalPolygon[C].ArrayVertex[V];
                }

                ListCollisionPolygon[C].ComputerCenter();
            }

            base.SpawnItem(ActiveAnimation, ActiveLayer, KeyFrame);
        }

        public override void GetMinMax(out int MinX, out int MinY, out int MaxX, out int MaxY)
        {
            MinX = (int)Position.X + SourceRectangle.X;
            MinY = (int)Position.Y + SourceRectangle.Y;
            MaxX = MinX + SourceRectangle.Width;
            MaxY = MinY + SourceRectangle.Height;
        }

        public override void OnUpdatePosition(Vector2 Translation)
        {
            for (int C = ListCollisionPolygon.Count - 1; C >= 0; --C)
            {
                ListCollisionPolygon[C].Offset(Translation.X, Translation.Y);
            }
        }

        private void ComputeSourceRectangle()
        {
            float MinX = float.MaxValue;
            float MaxX = float.MinValue;
            float MinY = float.MaxValue;
            float MaxY = float.MinValue;

            foreach (Polygon ActivePolygon in ListOriginalPolygon)
            {
                for (int V = 0; V < ActivePolygon.VertexCount; V++)
                {
                    int VertexX = (int)ActivePolygon.ArrayVertex[V].X;
                    int VertexY = (int)ActivePolygon.ArrayVertex[V].Y;

                    if (VertexX < MinX)
                        MinX = VertexX;
                    if (VertexX > MaxX)
                        MaxX = VertexX;

                    if (VertexY < MinY)
                        MinY = VertexY;
                    if (VertexY > MaxY)
                        MaxY = VertexY;
                }
            }

            SourceRectangle = new Rectangle((int)MinX, (int)MinY, (int)(MaxX - MinX), (int)(MaxY - MinY));
        }

        public override void BeginDraw(CustomSpriteBatch g)
        {
        }

        public override void Draw(CustomSpriteBatch g, bool IsInEditMode)
        {
            if (IsInEditMode)
            {
                foreach (Polygon ActivePolygon in ListCollisionPolygon)
                {
                    for (int I = 0; I < ActivePolygon.ArrayIndex.Length; I += 3)
                    {
                        Vector2 Vertex1 = ActivePolygon.ArrayVertex[ActivePolygon.ArrayIndex[I]];
                        Vector2 Vertex2 = ActivePolygon.ArrayVertex[ActivePolygon.ArrayIndex[I + 1]];
                        Vector2 Vertex3 = ActivePolygon.ArrayVertex[ActivePolygon.ArrayIndex[I + 2]];

                        g.DrawLine(GameScreen.sprPixel, Vertex1, Vertex2, Color.Black, 1);
                        g.DrawLine(GameScreen.sprPixel, Vertex2, Vertex3, Color.Black, 1);
                        g.DrawLine(GameScreen.sprPixel, Vertex3, Vertex1, Color.Black, 1);
                    }
                    for (int V = 0; V < ActivePolygon.ArrayVertex.Length; V++)
                    {
                        g.Draw(GameScreen.sprPixel, new Rectangle((int)ActivePolygon.ArrayVertex[V].X - 2,
                                                        (int)ActivePolygon.ArrayVertex[V].Y - 2, 5, 5), Color.Black);
                    }
                }
            }
        }

        #region Properties

        [CategoryAttribute("Damage Box Attributes"),
        DescriptionAttribute(".")]
        public int Damage { get { return _Damage; }set { _Damage = value; } }

        [CategoryAttribute("Damage Box Attributes"),
        DescriptionAttribute(".")]
        public override int Width { get { return SourceRectangle.Width; } }

        [CategoryAttribute("Damage Box Attributes"),
        DescriptionAttribute(".")]
        public override int Height { get { return SourceRectangle.Height; } }

        #endregion
    }
}
