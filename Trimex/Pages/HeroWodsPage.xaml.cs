using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class HeroWodsPage : ContentPage
{
    private readonly IHeroWodRepository _heroWodRepository;
    private readonly IWorkoutNoteRepository _workoutNoteRepository;
    private readonly IServiceProvider _serviceProvider;

    private IReadOnlyList<HeroWod> _allWods = [];
    private HeroWod? _selectedWod;
    private double _sheetDragY;
    private CancellationTokenSource? _longPressCts;

    // Filter state
    private bool _filterMen;
    private bool _filterWomen;
    private bool _filterUnder1H;
    private bool _filterNoTimeLimit;
    private bool _filterAmrap;
    private bool _filterEmom;
    private bool _filterTabata;
    private bool _filterForTime;
    private bool _filterCustom;

    public HeroWodsPage(
        IHeroWodRepository heroWodRepository,
        IWorkoutNoteRepository workoutNoteRepository,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _heroWodRepository = heroWodRepository;
        _workoutNoteRepository = workoutNoteRepository;
        _serviceProvider = serviceProvider;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = LoadWodsAsync();
    }

    private async Task LoadWodsAsync()
    {
        _allWods = await _heroWodRepository.GetAllAsync();
        ClearPendingDeleteSelection();
        ApplyFilters();
    }

    // ── Chip handlers ────────────────────────────────────────────────────────

    private void OnMenChipTapped(object? sender, TappedEventArgs e)
    {
        _filterMen = !_filterMen;
        SetChipActive(MenChipBorder, MenChipLabel, _filterMen);
        ApplyFilters();
    }

    private void OnWomenChipTapped(object? sender, TappedEventArgs e)
    {
        _filterWomen = !_filterWomen;
        SetChipActive(WomenChipBorder, WomenChipLabel, _filterWomen);
        ApplyFilters();
    }

    private void OnUnder1HChipTapped(object? sender, TappedEventArgs e)
    {
        _filterUnder1H = !_filterUnder1H;
        if (_filterUnder1H)
        {
            _filterNoTimeLimit = false;
            SetChipActive(NoTimeLimitChipBorder, NoTimeLimitChipLabel, false);
        }
        SetChipActive(Under1HChipBorder, Under1HChipLabel, _filterUnder1H);
        ApplyFilters();
    }

    private void OnNoTimeLimitChipTapped(object? sender, TappedEventArgs e)
    {
        _filterNoTimeLimit = !_filterNoTimeLimit;
        if (_filterNoTimeLimit)
        {
            _filterUnder1H = false;
            SetChipActive(Under1HChipBorder, Under1HChipLabel, false);
        }
        SetChipActive(NoTimeLimitChipBorder, NoTimeLimitChipLabel, _filterNoTimeLimit);
        ApplyFilters();
    }

    private void OnAmrapTypeChipTapped(object? sender, TappedEventArgs e)
    {
        _filterAmrap = !_filterAmrap;
        if (_filterAmrap) DeactivateCustomChip();
        SetChipActive(AmrapTypeChipBorder, AmrapTypeChipLabel, _filterAmrap);
        ApplyFilters();
    }

    private void OnEmomTypeChipTapped(object? sender, TappedEventArgs e)
    {
        _filterEmom = !_filterEmom;
        if (_filterEmom) DeactivateCustomChip();
        SetChipActive(EmomTypeChipBorder, EmomTypeChipLabel, _filterEmom);
        ApplyFilters();
    }

    private void OnTabataTypeChipTapped(object? sender, TappedEventArgs e)
    {
        _filterTabata = !_filterTabata;
        if (_filterTabata) DeactivateCustomChip();
        SetChipActive(TabataTypeChipBorder, TabataTypeChipLabel, _filterTabata);
        ApplyFilters();
    }

    private void OnForTimeTypeChipTapped(object? sender, TappedEventArgs e)
    {
        _filterForTime = !_filterForTime;
        if (_filterForTime) DeactivateCustomChip();
        SetChipActive(ForTimeTypeChipBorder, ForTimeTypeChipLabel, _filterForTime);
        ApplyFilters();
    }

    private void OnCustomTypeChipTapped(object? sender, TappedEventArgs e)
    {
        _filterCustom = !_filterCustom;

        if (_filterCustom)
        {
            // CUSTOM is exclusive: clear all type chips
            _filterAmrap = _filterEmom = _filterTabata = _filterForTime = false;
            SetChipActive(AmrapTypeChipBorder,   AmrapTypeChipLabel,   false);
            SetChipActive(EmomTypeChipBorder,    EmomTypeChipLabel,    false);
            SetChipActive(TabataTypeChipBorder,  TabataTypeChipLabel,  false);
            SetChipActive(ForTimeTypeChipBorder, ForTimeTypeChipLabel, false);
        }

        SetChipActive(CustomTypeChipBorder, CustomTypeChipLabel, _filterCustom);
        FabButton.IsVisible = _filterCustom;
        ApplyFilters();
    }

    private void DeactivateCustomChip()
    {
        _filterCustom = false;
        SetChipActive(CustomTypeChipBorder, CustomTypeChipLabel, false);
        FabButton.IsVisible = false;
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        ClearPendingDeleteSelection();
        ApplyFilters();
    }

    // ── Filtering ────────────────────────────────────────────────────────────

    private void ApplyFilters()
    {
        var results = _allWods.AsEnumerable();

        // Custom / hero split
        results = _filterCustom
            ? results.Where(w => w.IsCustom)
            : results.Where(w => !w.IsCustom);

        var searchText = SearchEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!string.IsNullOrEmpty(searchText))
            results = results.Where(w => w.Name.ToLowerInvariant().Contains(searchText));

        if (_filterMen || _filterWomen)
            results = results.Where(w =>
                (_filterMen   && w.GenderCategory == "Men") ||
                (_filterWomen && w.GenderCategory == "Women"));

        if (_filterUnder1H)
            results = results.Where(IsUnderOneHour);
        else if (_filterNoTimeLimit)
            results = results.Where(w => w.Type == WorkoutTypes.ForTime && w.TimeCap is null);

        if (!_filterCustom && (_filterAmrap || _filterEmom || _filterTabata || _filterForTime))
            results = results.Where(w =>
                (_filterAmrap   && w.Type == WorkoutTypes.Amrap)   ||
                (_filterEmom    && w.Type == WorkoutTypes.Emom)    ||
                (_filterTabata  && w.Type == WorkoutTypes.Tabata)  ||
                (_filterForTime && w.Type == WorkoutTypes.ForTime));

        var list = results.ToList();
        WodsCollectionView.ItemsSource = list;
        ResultsCountLabel.Text = $"showing {list.Count} WODs";
    }

    private static bool IsUnderOneHour(HeroWod wod) => wod.Type switch
    {
        WorkoutTypes.Amrap   => wod.Duration.HasValue && wod.Duration.Value < 3600,
        WorkoutTypes.ForTime => wod.TimeCap.HasValue  && wod.TimeCap.Value  < 3600,
        _                    => true
    };

    // ── Card tap & detail sheet ───────────────────────────────────────────────

    private async void OnWodCardClicked(object? sender, EventArgs e)
    {
        var wod = (sender as VisualElement)?.BindingContext as HeroWod;
        if (wod is null)
        {
            return;
        }

        if (wod.IsPendingDelete)
        {
            return;
        }

        ClearPendingDeleteSelection();

        _selectedWod = wod;
        await ShowDetailSheetAsync(wod);
    }

    private void OnWodCardPressed(object? sender, EventArgs e)
    {
        var wod = (sender as VisualElement)?.BindingContext as HeroWod;
        if (wod is null || !wod.IsCustom)
        {
            return;
        }

        CancelLongPressDetection();

        var cts = new CancellationTokenSource();
        _longPressCts = cts;
        _ = TriggerLongPressSelectionAsync(wod, cts.Token);
    }

    private void OnWodCardReleased(object? sender, EventArgs e)
    {
        CancelLongPressDetection();
    }

    private async Task TriggerLongPressSelectionAsync(HeroWod wod, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var item in _allWods)
                {
                    item.IsPendingDelete = item.UniqueId == wod.UniqueId;
                }

                CancelLongPressDetection();
                ApplyFilters();
            });
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void CancelLongPressDetection()
    {
        if (_longPressCts is null)
        {
            return;
        }

        _longPressCts.Cancel();
        _longPressCts.Dispose();
        _longPressCts = null;
    }

    private void ClearPendingDeleteSelection()
    {
        var changed = false;

        foreach (var item in _allWods)
        {
            if (!item.IsPendingDelete)
            {
                continue;
            }

            item.IsPendingDelete = false;
            changed = true;
        }

        if (changed)
        {
            ApplyFilters();
        }
    }

    private async void OnDeleteCustomWodClicked(object? sender, EventArgs e)
    {
        var wod = (sender as VisualElement)?.BindingContext as HeroWod;
        if (wod is null || !wod.IsCustom)
        {
            return;
        }

        var confirm = await DisplayAlert("Remove WOD", $"Delete '{wod.Name}'?", "Delete", "Cancel");
        if (!confirm)
        {
            return;
        }

        await _heroWodRepository.DeleteAsync(wod.UniqueId);

        _allWods = _allWods.Where(x => x.UniqueId != wod.UniqueId).ToList();
        _selectedWod = _selectedWod?.UniqueId == wod.UniqueId ? null : _selectedWod;
        ApplyFilters();
    }

    private async Task ShowDetailSheetAsync(HeroWod wod)
    {
        DetailTypeLabel.Text            = wod.TypeBadge;
        DetailTypeLabel.TextColor       = wod.TypeBadgeTextColor;
        DetailTypeBadge.BackgroundColor = wod.TypeBadgeBgColor;
        DetailNameLabel.Text            = wod.Name.ToUpperInvariant();
        DetailGenderLabel.Text = wod.IsCustom ? "Custom WOD" : wod.GenderCategory;
        TimeTargetLabel.Text   = wod.DurationDisplay;

        ExercisesLayout.Children.Clear();
        foreach (var line in wod.ExerciseLines)
        {
            ExercisesLayout.Children.Add(new Label
            {
                Text       = line,
                TextColor  = Color.FromArgb("#B8B8B8"),
                FontSize   = 14,
                FontFamily = "OpenSansRegular"
            });
        }

        var lastRecord = await _workoutNoteRepository.GetLatestAsync(wod.Type, wod.UniqueId);
        LastRecordLabel.Text = string.IsNullOrWhiteSpace(lastRecord?.Notes) ? "—" : lastRecord.Notes;

        OverlayDimmer.IsVisible  = true;
        OverlayDimmer.Opacity    = 0;
        DetailSheet.IsVisible    = true;
        DetailSheet.TranslationY = 700;

        await Task.WhenAll(
            OverlayDimmer.FadeToAsync(1, 250),
            DetailSheet.TranslateToAsync(0, 0, 300, Easing.CubicOut));
    }

    private async Task HideDetailSheetAsync()
    {
        await Task.WhenAll(
            OverlayDimmer.FadeToAsync(0, 200),
            DetailSheet.TranslateToAsync(0, 700, 250, Easing.CubicIn));

        OverlayDimmer.IsVisible = false;
        DetailSheet.IsVisible   = false;
        _selectedWod            = null;
        CancelLongPressDetection();
        ClearPendingDeleteSelection();
    }

    private async void OnDimmerTapped(object? sender, TappedEventArgs e)
    {
        ClearPendingDeleteSelection();
        await HideDetailSheetAsync();
    }

    private void OnDetailSheetPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _sheetDragY = 0;
                break;

            case GestureStatus.Running:
                // Android reports per-frame deltas; iOS/Windows report cumulative totals.
#if ANDROID
                _sheetDragY = Math.Max(0, _sheetDragY + e.TotalY);
#else
                _sheetDragY = Math.Max(0, e.TotalY);
#endif
                DetailSheet.TranslationY = _sheetDragY;
                OverlayDimmer.Opacity = Math.Max(0, 1.0 - _sheetDragY / 400.0);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (_sheetDragY > 150)
                    _ = HideDetailSheetAsync();
                else
                    _ = Task.WhenAll(
                        DetailSheet.TranslateToAsync(0, 0, 200, Easing.CubicOut),
                        OverlayDimmer.FadeToAsync(1, 200));
                break;
        }
    }

    // ── FAB ──────────────────────────────────────────────────────────────────

    private async void OnCreateCustomWodClicked(object? sender, EventArgs e)
    {
        CancelLongPressDetection();
        ClearPendingDeleteSelection();
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<CustomWodCreationPage>());
    }

    // ── Navigation to timer ───────────────────────────────────────────────────

    private async void OnGoToTimerClicked(object? sender, EventArgs e)
    {
        if (_selectedWod is null) return;

        var wod = _selectedWod;
        await HideDetailSheetAsync();
        await Navigation.PushAsync(new WorkoutTimerPage(BuildRequest(wod)));
    }

    private static WorkoutConfigurationRequest BuildRequest(HeroWod wod) => wod.Type switch
    {
        WorkoutTypes.Amrap => new WorkoutConfigurationRequest
        {
            Type            = WorkoutTypes.Amrap,
            TypeDisplayName = "AMRAP",
            DurationSeconds = wod.Duration ?? 1200,
            TimeCapSeconds  = wod.Duration ?? 1200,
            DurationLabel   = $"{(wod.Duration ?? 1200) / 60} minutes",
            Notes           = wod.Notes,
            HeroWodUniqueId = wod.UniqueId,
            HeroWodName     = wod.Name,
            WodDescription  = wod.WodDescription,
            CountsDown      = true,
            SupportsRounds  = true
        },
        WorkoutTypes.ForTime => new WorkoutConfigurationRequest
        {
            Type            = WorkoutTypes.ForTime,
            TypeDisplayName = "FOR TIME",
            DurationSeconds = wod.TimeCap ?? 0,
            TimeCapSeconds  = wod.TimeCap,
            DurationLabel   = wod.TimeCap.HasValue ? $"{wod.TimeCap.Value / 60} minutes cap" : "No cap",
            Notes           = wod.Notes,
            HeroWodUniqueId = wod.UniqueId,
            HeroWodName     = wod.Name,
            WodDescription  = wod.WodDescription,
            CountsDown      = false,
            SupportsRounds  = false
        },
        _ => new WorkoutConfigurationRequest
        {
            Type            = wod.Type,
            TypeDisplayName = WorkoutTypes.ToDisplayName(wod.Type),
            DurationSeconds = wod.Duration ?? wod.TimeCap ?? 0,
            TimeCapSeconds  = wod.TimeCap,
            DurationLabel   = wod.DurationDisplay,
            Notes           = wod.Notes,
            HeroWodUniqueId = wod.UniqueId,
            HeroWodName     = wod.Name,
            WodDescription  = wod.WodDescription,
            CountsDown      = false,
            SupportsRounds  = false
        }
    };

    // ── Chip helper ───────────────────────────────────────────────────────────

    private static void SetChipActive(Border chipBorder, Label chipLabel, bool active)
    {
        chipLabel.TextColor        = active ? Color.FromArgb("#00363D") : Color.FromArgb("#8A8A8A");
        chipBorder.BackgroundColor = active ? Color.FromArgb("#DAFF6E") : Color.FromArgb("#262626");
    }
}
