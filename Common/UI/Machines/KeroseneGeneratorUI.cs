using Macrocosm.Common.Storage;
using Macrocosm.Common.Systems.Power;
using Macrocosm.Common.UI.Themes;
using Macrocosm.Content.Liquids;
using Macrocosm.Content.Machines.Generators.Fuel;
using Microsoft.Xna.Framework;
using ModLiquidLib.ModLoader;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;


namespace Macrocosm.Common.UI.Machines;

public class KeroseneGeneratorUI : MachineUI
{
    public KeroseneGeneratorTE KeroseneGenerator => MachineTE as KeroseneGeneratorTE;

    private UIPanel backgroundPanel;
    private UIPanel fuelPanel;

    private UIText rpmText;
    private UIPanelProgressBar rpmProgressBar;
    private UITextPanel<string> powerStatusText;

    private List<UILiquidTankPiston> pistons = new();

    private float enginePhase = 0f;

    private static readonly float[] V8CrankPhaseOffsets =
    {
        0f / 4f, 3f / 4f, 1f / 4f, 2f / 4f,
        3f / 4f, 2f / 4f, 0f / 4f, 1f / 4f,
    };

    public KeroseneGeneratorUI()
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        Width.Set(495f, 0f);
        Height.Set(330f, 0f);

        title.Top.Set(-36, 0);

        //Recalculate();

        backgroundPanel = new()
        {
            Width = new(0, 1),
            Height = new(0, 1),
            BorderColor = UITheme.Current.PanelStyle.BorderColor,
            BackgroundColor = UITheme.Current.PanelStyle.BackgroundColor
        };
        Append(backgroundPanel);

        powerStatusText = new("", textScale: 1f, large: false)
        {
            HAlign = 1f,
            VAlign = 0.04f,
            Width = new(0, 0.6f - 0.01f),
            BorderColor = UITheme.Current.PanelStyle.BorderColor,
            BackgroundColor = UITheme.Current.PanelStyle.BackgroundColor
        };
        backgroundPanel.Append(powerStatusText);

        rpmProgressBar = new()
        {
            HAlign = 0f,
            VAlign = 0.04f,
            Width = new(0, 0.4f - 0.01f),
            Height = new(0, 0.22f),
            FillColor = new Color(0, 255, 0),
            FillColorEnd = new Color(255, 0, 0),
            IsVertical = false,
            BorderColor = UITheme.Current.PanelStyle.BorderColor,
            BackgroundColor = UITheme.Current.PanelStyle.BackgroundColor
        };
        backgroundPanel.Append(rpmProgressBar);

        rpmText = new("", textScale: 1f, large: false)
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
            //Width = new(0, 0.4f - 0.01f),
        };
        rpmProgressBar.Append(rpmText);

        fuelPanel = new()
        {
            HAlign = 0.5f,
            Top = new(0, 0.27f),
            Height = new(0, 0.72f),
            Width = new(0, 1f),
            BorderColor = UITheme.Current.PanelStyle.BorderColor,
            BackgroundColor = UITheme.Current.PanelStyle.BackgroundColor
        };
        fuelPanel.SetPadding(0f);
        backgroundPanel.Append(fuelPanel);

        const float slotSize = 48f;
        const float slotLeft = 28f;
        const float slotRailWidth = 92f;
        const float pistonWidth = 62f;
        const float pistonHeight = 60f;
        const float pistonColumnGap = 18f;
        const float pistonRowGap = 18f;

        float pistonGridWidth = 4f * pistonWidth + 3f * pistonColumnGap;
        float pistonGridHeight = 2f * pistonHeight + pistonRowGap;

        UIElement pistonGrid = new()
        {
            Left = new(slotRailWidth, 0f),
            Width = new(-slotRailWidth, 1f),
            Height = new(pistonGridHeight, 0f),
            VAlign = 0.5f
        };
        pistonGrid.SetPadding(0f);
        fuelPanel.Append(pistonGrid);

        float pistonStartX = (495f - slotRailWidth - pistonGridWidth) / 2f;

        pistons = new();
        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                var piston = new UILiquidTankPiston(LiquidLoader.LiquidType<RocketFuel>())
                {
                    Left = new(pistonStartX + col * (pistonWidth + pistonColumnGap), 0),
                    Top = new(row * (pistonHeight + pistonRowGap), 0),
                    Width = new(62, 0f),
                    Height = new(60, 0f),
                    BorderColor = UITheme.Current.PanelStyle.BorderColor,
                    BackgroundColor = UITheme.Current.PanelStyle.BackgroundColor
                };
                pistons.Add(piston);
                pistonGrid.Append(piston);
            }
        }

        if (KeroseneGenerator.Inventory is not null)
        {
            int slotCount = KeroseneGenerator.Inventory.Size;

            for (int i = 0; i < slotCount; i++)
            {
                var inputSlot = KeroseneGenerator.Inventory.ProvideItemSlot(i, ItemSlot.Context.ChestItem);

                inputSlot.Left = new(slotLeft + i * slotSize, 0f);
                inputSlot.Top = new(0f, 0f);
                inputSlot.VAlign = 0.5f;
                inputSlot.SetPadding(0f);
                fuelPanel.Append(inputSlot);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Inventory.ActiveInventory = KeroseneGenerator.Inventory;

        string power = $"{KeroseneGenerator.GeneratedPower:F2}";
        powerStatusText.SetText(Language.GetText("Mods.Macrocosm.Machines.Common.GeneratedPower").Format(power));

        float rpmProgress = KeroseneGenerator.RPMProgress;
        rpmProgressBar.Progress = rpmProgress;

        float rpm = KeroseneGenerator.RPM;
        rpmText.SetText($"{(int)rpm} RPM");

        bool running = rpmProgress > 0.01f;
        if (running)
            enginePhase = (enginePhase + MathHelper.Lerp(0.008f, 0.033f, rpmProgress)) % 1f;

        for (int i = 0; i < pistons.Count; i++)
        {
            pistons[i].Running = running;
            pistons[i].Phase = (enginePhase + V8CrankPhaseOffsets[i]) % 1f;
        }
    }

}
