namespace SEOS.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Sandbox.Common.ObjectBuilders;
    using Sandbox.Game.Entities;
    using Sandbox.Game.EntityComponents;
    using Sandbox.ModAPI;
    using VRage;
    using VRage.Game;
    using VRage.Game.Entity;
    using VRage.ModAPI;
    using VRage.ObjectBuilders;
    using VRageMath;

    internal class Spawn
    {

        private static readonly SerializableBlockOrientation EntityOrientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

        private static readonly MyObjectBuilder_CubeGrid CubeGridBuilder = new MyObjectBuilder_CubeGrid()
        {
            EntityId = 0,
            GridSizeEnum = MyCubeSize.Large,
            IsStatic = true,
            Skeleton = new List<BoneInfo>(),
            LinearVelocity = Vector3.Zero,
            AngularVelocity = Vector3.Zero,
            ConveyorLines = new List<MyObjectBuilder_ConveyorLine>(),
            BlockGroups = new List<MyObjectBuilder_BlockGroup>(),
            Handbrake = false,
            XMirroxPlane = null,
            YMirroxPlane = null,
            ZMirroxPlane = null,
            PersistentFlags = MyPersistentEntityFlags2.InScene,
            Name = "ArtificialCubeGrid",
            DisplayName = "FieldEffect",
            CreatePhysics = false,
            DestructibleBlocks = true,
            PositionAndOrientation = new MyPositionAndOrientation(Vector3D.Zero, Vector3D.Forward, Vector3D.Up),

            CubeBlocks = new List<MyObjectBuilder_CubeBlock>()
                {
                    new MyObjectBuilder_CubeBlock()
                    {
                        EntityId = 0,
                        BlockOrientation = EntityOrientation,
                        SubtypeName = string.Empty,
                        Name = string.Empty,
                        Min = Vector3I.Zero,
                        Owner = 0,
                        ShareMode = MyOwnershipShareModeEnum.None,
                        DeformationRatio = 0,
                    }
                }
        };

        private static readonly MyObjectBuilder_Meteor TestBuilder = new MyObjectBuilder_Meteor()
        {
            EntityId = 0,
            LinearVelocity = Vector3.Zero,
            AngularVelocity = Vector3.Zero,
            PersistentFlags = MyPersistentEntityFlags2.InScene,
            Name = "GravityMissile",
            PositionAndOrientation = new MyPositionAndOrientation(Vector3D.Zero, Vector3D.Forward, Vector3D.Up)
        };

        public static MyEntity EmptyEntity(string displayName, string model, MyEntity parent, bool parented = false)
        {
            try
            {
                var myParent = parented ? parent : null;
                var ent = new MyEntity { NeedsWorldMatrix = true };
                ent.Init(new StringBuilder(displayName), model, myParent, null);
                ent.Name = $"{parent.EntityId}";
                MyAPIGateway.Entities.AddEntity(ent);
                return ent;
            }
            catch (Exception ex) { Session.SessionLog.Line($"Exception in EmptyEntity: {ex}"); return null; }
        }

        public static MyEntity SpawnBlock(string subtypeId, string name, bool isVisible = false, bool hasPhysics = false, bool isStatic = false, bool toSave = false, bool destructible = false, long ownerId = 0)
        {
            try
            {
                CubeGridBuilder.Name = name;
                CubeGridBuilder.CubeBlocks[0].SubtypeName = subtypeId;
                CubeGridBuilder.CreatePhysics = hasPhysics;
                CubeGridBuilder.IsStatic = isStatic;
                CubeGridBuilder.DestructibleBlocks = destructible;
                var ent = (MyEntity)MyAPIGateway.Entities.CreateFromObjectBuilder(CubeGridBuilder);

                ent.Flags &= ~EntityFlags.Save;
                ent.Render.Visible = isVisible;
                MyAPIGateway.Entities.AddEntity(ent);

                return ent;
            }
            catch (Exception ex)
            {
                Session.SessionLog.Line("Exception in Spawn");
                Session.SessionLog.Line($"{ex}");
                return null;
            }
        }

    }

    public class SubGridInfo
    {
        public readonly MyCubeGrid Grid;
        public readonly bool MainGrid;
        public readonly bool MechSub;
        public float Integrity;
        public SubGridInfo(MyCubeGrid grid, bool mainGrid, bool mechSub)
        {
            Grid = grid;
            MainGrid = mainGrid;
            MechSub = mechSub;
        }
    }

    public class BlockSets
    {
        public readonly HashSet<MyResourceSourceComponent> Sources = new HashSet<MyResourceSourceComponent>();
        public readonly HashSet<MyShipController> ShipControllers = new HashSet<MyShipController>();
        public readonly HashSet<BatteryInfo> Batteries = new HashSet<BatteryInfo>();
    }

    public struct BatteryInfo
    {
        public readonly MyResourceSourceComponent Source;
        public readonly MyResourceSinkComponent Sink;
        public readonly MyCubeBlock CubeBlock;
        public BatteryInfo(MyResourceSourceComponent source)
        {
            Source = source;
            Sink = Source.Entity.Components.Get<MyResourceSinkComponent>();
            CubeBlock = source.Entity as MyCubeBlock;
        }
    }

    public struct SubGridComputedInfo
    {
        public readonly MyCubeGrid Grid;
        public readonly float Integrity;
        public SubGridComputedInfo(MyCubeGrid grid, float integrity)
        {
            Grid = grid;
            Integrity = integrity;
        }
    }

    public struct FactionMemberRank
    {
        public string FactionName { get; set; }
        public ulong MemberId { get; set; }
        public string MemberName { get; set; }
        public FactionRank MilitaryRank { get; set; }
        public DateTime PromotionDate { get; set; }

        public FactionMemberRank(string factionName, ulong memberId, string memberName, FactionRank militaryRank, DateTime promotionDate)
        {
            FactionName = factionName;
            MemberId = memberId;
            MemberName = memberName;
            MilitaryRank = militaryRank;
            PromotionDate = promotionDate;
        }
    }

    public enum FactionRank
    {
        Private,
        Corporal,
        Sergeant,
        StaffSergeant,
        SergeantFirstClass,
        MasterSergeant,
        FirstSergeant,
        SergeantMajor,
        CommandSergeantMajor,
        SecondLieutenant,
        FirstLieutenant,
        Captain,
        Major,
        LieutenantColonel,
        Colonel,
        BrigadierGeneral,
        MajorGeneral,
        LieutenantGeneral,
        General
    }


}


