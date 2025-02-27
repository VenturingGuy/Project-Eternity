﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectEternity.Core;
using ProjectEternity.Core.Units;

namespace ProjectEternity.GameScreens.SorcererStreetScreen
{
    public class SorcererStreetUnit : UnitMapComponent
    {
        public Texture2D SpriteMap;

        public override int Width => 32;

        public override int Height => 32;

        public override bool IsActive => throw new System.NotImplementedException();

        public override void Draw2DOnMap(CustomSpriteBatch g, Vector3 Position, int SizeX, int SizeY, Color UnitColor)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawExtraOnMap(CustomSpriteBatch g, Vector3 PositionY, Color UnitColor)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawOverlayOnMap(CustomSpriteBatch g, Vector3 Position)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawTimeOfDayOverlayOnMap(CustomSpriteBatch g, Vector3 Position, int TimeOfDay)
        {
            throw new System.NotImplementedException();
        }
    }
}
