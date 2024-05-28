using FlexAirFit.Application.Services;
using FlexAirFit.Core.Models;

namespace FlexAirFit.Web;

public class Context(AdminService adminService,
                      BonusService bonusService,
                      ClientProductService clientProductService,
                      ClientService clientService,
                      MembershipService membershipService,
                      ProductService productService,
                      ScheduleService scheduleService,
                      TrainerService trainerService,
                      UserService userService,
                      WorkoutService workoutService)
{
    public AdminService AdminService { get; } = adminService;
    public BonusService BonusService { get; } = bonusService;
    public ClientProductService ClientProductService { get; } = clientProductService;
    public ClientService ClientService { get; } = clientService;
    public MembershipService MembershipService { get; } = membershipService;
    public ProductService ProductService { get; } = productService;
    public ScheduleService ScheduleService { get; } = scheduleService;
    public TrainerService TrainerService { get; } = trainerService;
    public UserService UserService { get; } = userService;
    public WorkoutService WorkoutService { get; } = workoutService;

    public object? UserObject { get; set; }
    public User? CurrentUser { get; set; }
}