using Macrocosm.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;
using System.IO;
using Terraria.ModLoader.IO;

namespace Macrocosm.Common.Systems.Power;

public abstract class ConsumerTE : MachineTE
{
    public float InputPower { get; set; }
    public float MinPower { get; set; } = 0f;
    public float MaxPower { get; set; }
    public virtual float PowerDemand => IsEnabledByPlayer ? MaxPower : 0f;
    public bool IsReceivingPower => InputPower > MinPower;
    public override bool IsRunning => IsReceivingPower;
    public float RatedPowerProgress => MathHelper.Clamp(MaxPower > 0f ? InputPower / MaxPower : 0f, 0f, 1f);
    public float DemandPowerProgress => MathHelper.Clamp(PowerDemand > 0f ? InputPower / PowerDemand : 0f, 0f, 1f);
    public float PowerProgress => RatedPowerProgress;

    public override void UpdatePowerState()
    {
        if (IsOnFrame && !IsRunning)
            TurnOff(automatic: true);
        else if (!IsOnFrame && IsRunning && IsEnabledByPlayer)
            TurnOn(automatic: true);
    }

    public override void OnPowerDisconnected() => InputPower = 0f;
    public override Color DisplayColor => IsRunning ? Color.Orange : Color.Orange.WithLuminance(0.5f);
    public override string GetPowerInfo()
    {
        string status = PowerDemand > 0f
            ? Language.GetText($"Mods.Macrocosm.Machines.Common.PowerInfo.Consumer").Format($"{InputPower:F2}", $"{PowerDemand:F2}", $"{MaxPower:F2}")
            : Language.GetText($"Mods.Macrocosm.Machines.Common.PowerInfo.ConsumerIdle").Format($"{MaxPower:F2}");

        return $"{status}";
    }

    public override void DrawMachinePowerInfo(SpriteBatch spriteBatch, Vector2 basePosition, Color lightColor)
    {
        string power = Language.GetText("Mods.Macrocosm.Machines.Common.PowerInfo.Common").Format($"{InputPower:F2}", $"{PowerDemand:F2}");

        Vector2 textSize = FontAssets.MouseText.Value.MeasureString(power);
        Vector2 position = new Vector2(basePosition.X + (MachineTile.Width * 16f / 2f) - (textSize.X / 2f) + 8f, basePosition.Y - 22f) - Main.screenPosition;
        Color color = DisplayColor;

        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.DeathText.Value, power, position - new Vector2(power.Length, 0), color, 0f, Vector2.Zero, Vector2.One * 0.4f, spread: 1.5f);
    }

    protected virtual void ConsumerNetSend(BinaryWriter writer) { }
    protected virtual void ConsumerNetReceive(BinaryReader reader) { }
    protected virtual void ConsumerSaveData(TagCompound tag) { }
    protected virtual void ConsumerLoadData(TagCompound tag) { }

    public sealed override void MachineNetSend(BinaryWriter writer)
    {
        writer.Write(InputPower);
        writer.Write(MinPower);
        writer.Write(MaxPower);
        ConsumerNetSend(writer);
    }

    public sealed override void MachineNetReceive(BinaryReader reader)
    {
        InputPower = reader.ReadSingle();
        MinPower = reader.ReadSingle();
        MaxPower = reader.ReadSingle();
        ConsumerNetReceive(reader);
    }

    public sealed override void MachineSaveData(TagCompound tag)
    {
        if (InputPower != 0f) tag[nameof(InputPower)] = InputPower;
        if (MinPower != 0f) tag[nameof(MinPower)] = MinPower;
        if (MaxPower != 0f) tag[nameof(MaxPower)] = MaxPower;
        ConsumerSaveData(tag);
    }

    public sealed override void MachineLoadData(TagCompound tag)
    {
        if (tag.ContainsKey(nameof(InputPower))) InputPower = tag.GetFloat(nameof(InputPower));
        if (tag.ContainsKey(nameof(MinPower))) MinPower = tag.GetFloat(nameof(MinPower));
        if (tag.ContainsKey(nameof(MaxPower))) MaxPower = tag.GetFloat(nameof(MaxPower));
        ConsumerLoadData(tag);
    }
}
