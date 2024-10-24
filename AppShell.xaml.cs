using CS341Project.AppEntry;
using CS341Project.EventMap;
using CS341Project.Scheduler;
using CS341Project.Seeding;
using CS341Project.ThumbGen;

namespace CS341Project;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("title_page", typeof(TitlePage));
        Routing.RegisterRoute("home_page", typeof(HomePage));
        Routing.RegisterRoute("login_page", typeof(LogInPage));
        Routing.RegisterRoute("sign_up_page", typeof(SignUpPage));
        Routing.RegisterRoute("event_list_page", typeof(EventListPage));
        Routing.RegisterRoute("event_map_page", typeof(EventMapPage));
        Routing.RegisterRoute("scheduler_event_page", typeof(SchedulerEventPage));
        Routing.RegisterRoute("event_pop_up", typeof(EventPopup));
        Routing.RegisterRoute("seeding_list_page", typeof(SeedingListPage));
        Routing.RegisterRoute("seed_generator_page", typeof(SeedGeneratorPage));
        Routing.RegisterRoute("double_elim_page", typeof(DoubleElimPage));
        Routing.RegisterRoute("select_competitors_page", typeof(SelectCompetitorsPage));
        Routing.RegisterRoute("thumb_gen_manual_page", typeof(ThumbGenManualPage));
        Routing.RegisterRoute("thumbnail_gen_page", typeof(ThumbGenPage));
    }
}