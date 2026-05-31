using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.UI;
using Macrocosm.Content.Machines.Consumers.Autocrafters;

namespace Macrocosm.Common.UI.Machines;

public class UIAutocrafterRecipeBrowser : UIElement
{
    private UIItemBrowser itemBrowser;

    private readonly List<Item> availableResults = new();
    private readonly Dictionary<int, List<Recipe>> recipesByResultType = new();
    private readonly AutocrafterTEBase autocrafter;

    public Action<Item, IReadOnlyList<Recipe>> OnResultClicked { get; set; }

    public UIAutocrafterRecipeBrowser(AutocrafterTEBase machine)
    {
        autocrafter = machine;
    }

    public override void OnInitialize()
    {
        Width.Set(0f, 1f);
        Height.Set(0f, 1f);
        SetPadding(0f);

        PopulateAvailableRecipes();

        itemBrowser = new UIItemBrowser(CreateNonEmptyFilters(availableResults), addMiscFallback: false)
        {
            Width = new(0f, 1f),
            Height = new(0f, 1f),
        };
        itemBrowser.OnEntrySelected += OnItemSelected;
        Append(itemBrowser);

        itemBrowser.SetEntries(availableResults);
    }

    private void PopulateAvailableRecipes()
    {
        availableResults.Clear();
        recipesByResultType.Clear();

        foreach (var recipe in Main.recipe)
        {
            if (recipe?.createItem is Item item && item.type > ItemID.None && autocrafter.RecipeAllowed(recipe))
            {
                if (!recipesByResultType.TryGetValue(item.type, out var recipes))
                {
                    recipes = new();
                    recipesByResultType[item.type] = recipes;
                    availableResults.Add(item.Clone());
                }

                recipes.Add(recipe);
            }
        }
    }

    private static List<IItemEntryFilter> CreateNonEmptyFilters(IReadOnlyList<Item> items)
    {
        List<IItemEntryFilter> baseFilters = UIItemBrowser.DefaultFilters();
        List<IItemEntryFilter> filters = baseFilters
            .Where(filter => items.Any(filter.FitsFilter))
            .ToList();

        var miscFallback = new ItemFilters.MiscFallback(baseFilters);
        if (items.Any(miscFallback.FitsFilter))
            filters.Add(miscFallback);

        return filters;
    }

    private void OnItemSelected(int index, Item item)
    {
        if (index >= 0 && index < availableResults.Count && recipesByResultType.TryGetValue(item.type, out var recipes))
            OnResultClicked?.Invoke(item, recipes);
    }
}
