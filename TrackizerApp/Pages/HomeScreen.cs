﻿using MauiReactor;
using MauiReactor.Parameters;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;
using TrackizerApp.Pages.Dialogs;
using TrackizerApp.Pages.Views;

namespace TrackizerApp.Pages;



public enum HomeScreenView
{
    [Display(Name = "")]
    Home,

    [Display(Name = "Spending & Budgets")]
    Budgets,

    [Display(Name = "Calendar")]
    Calendar,

    [Display(Name = "Credit Cards")]
    CreditCards
}

class HomeScreenState
{
    public HomeScreenView View { get; set; }

}

partial class HomeScreen : Component<HomeScreenState>
{
    [Param]
    IParameter<User> _loggedUser;

    public override VisualNode Render()
        => new BaseScreenLayout
        {
            Grid(
                !_loggedUser.Value.IsLoggedIn ?
                Grid()
                :
                RenderPageBody(),

                Theme.H3(State.View.GetDisplayName())
                    .VStart()
                    .HCenter()
                    .TextColor(Theme.Grey30)
                    .Margin(23,32),

                Image("settings_dark.png")
                    .HeightRequest(24)
                    .VStart()
                    .HEnd()
                    .Margin(23,32)
                    .OnTapped(ShowSettings)
                )
        }
        .OnAppearing(OnAppearing)
        .StatusBarColor(State.View == HomeScreenView.Home || State.View == HomeScreenView.Calendar ? Theme.Grey70 : Theme.Grey80)
        ;

    void ShowSettings()
    {

    }

    async void OnAppearing()
    {
        if (!_loggedUser.Value.IsLoggedIn &&
            Navigation?.ModalStack.Count == 0)
        {
            await Navigation.PushModalAsync<WelcomeScreen>();
        }
    }

    Grid RenderPageBody()
        => Grid(

            new SlidingView
            {
                RenderView(HomeScreenView.Home),
                RenderView(HomeScreenView.Budgets),
                RenderView(HomeScreenView.Calendar),
                RenderView(HomeScreenView.CreditCards),
            }
            .SelectedIndex((int)State.View)
            .OnSelectedViewIndex(view => SetState(s => s.View = (HomeScreenView)view)),

            new NavigationBar()
                .View(State.View)
                .OnViewChanged(view => SetState(s => s.View = view))
                .OnNewSubscription(OnNewSubscription)
            );

    void OnNewSubscription()
    {
        Navigation?.PushModalAsync<NewSubscription>();
    }

    VisualNode RenderView(HomeScreenView view)
        => view switch
        {
            HomeScreenView.Home => new HomeView()
                .IsVisible(State.View == view)
                .OnShowBudgetView(() => SetState(s => s.View = HomeScreenView.Budgets)),
            HomeScreenView.Budgets => new BudgetsView()
                .IsVisible(State.View == view),
            HomeScreenView.Calendar => new CalendarView(),
            HomeScreenView.CreditCards => new CreditCardsView(),
            _ => throw new NotSupportedException(),
        };
}
