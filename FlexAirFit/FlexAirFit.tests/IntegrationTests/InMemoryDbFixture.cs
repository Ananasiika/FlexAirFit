using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Database.Context;
using FlexAirFit.Database.Converters;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.DbFixtures;

public class InMemoryDbFixture() : IDisposable
{
    public readonly FlexAirFitDbContext Context = new InMemoryDbContextFactory().GetDbContext();

    public async Task<User?> GetUserById(Guid userId)
    {
        return UserConverter.DbToCoreModel(await Context.Users.FindAsync(userId));
    }

    public async Task<Admin?> GetAdminById(Guid idAdmin)
    {
        return AdminConverter.DbToCoreModel(await Context.Admins.FindAsync(idAdmin));
    }

    public async Task<Bonus?> GetBonusById(Guid idBonus)
    {
        return BonusConverter.DbToCoreModel(await Context.Bonuses.FindAsync(idBonus));
    }

    public async Task<Client?> GetClientById(Guid idClient)
    {
        return ClientConverter.DbToCoreModel(await Context.Clients.FindAsync(idClient));
    }

    public async Task<Trainer?> GetTrainerById(Guid idTrainer)
    {
        return TrainerConverter.DbToCoreModel(await Context.Trainers.FindAsync(idTrainer));
    }

    public async Task<Workout?> GetWorkoutById(Guid idWorkout)
    {
        return WorkoutConverter.DbToCoreModel(await Context.Workouts.FindAsync(idWorkout));
    }
    
    public async Task<Schedule?> GetScheduleById(Guid idSchedule)
    {
        return ScheduleConverter.DbToCoreModel(await Context.Schedules.FindAsync(idSchedule));
    }
    
    public async Task<Product?> GetProductById(Guid idProduct)
    {
        return ProductConverter.DbToCoreModel(await Context.Products.FindAsync(idProduct));
    }
    
    public async Task<Membership?> GetMembershipById(Guid idMembership)
    {
        return MembershipConverter.DbToCoreModel(await Context.Memberships.FindAsync(idMembership));
    }

    public async Task InsertUsers(List<User> users)
    {
        foreach (var user in users)
        {
            await Context.Users.AddAsync(UserConverter.CoreToDbModel(user)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertClients(List<Client> clients)
    {
        foreach (var client in clients)
        {
            await Context.Clients.AddAsync(ClientConverter.CoreToDbModel(client)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertWorkouts(List<Workout> workouts)
    {
        foreach (var workout in workouts)
        {
            await Context.Workouts.AddAsync(WorkoutConverter.CoreToDbModel(workout)!);
        }
    }

    public async Task InsertTrainers(List<Trainer> trainers)
    {
        foreach (var trainer in trainers)
        {
            await Context.Trainers.AddAsync(TrainerConverter.CoreToDbModel(trainer)!);
        }
    }
    
    public async Task InsertAdmins(List<Admin> admins)
    {
        foreach (var admin in admins)
        {
            await Context.Admins.AddAsync(AdminConverter.CoreToDbModel(admin)!);
        }
    }
    
    public static List<User> CreateMockUsers()
    {
        return
        [
            new(Guid.NewGuid(), UserRole.Admin, "u1@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y"),
            new(Guid.NewGuid(), UserRole.Admin, "u2@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y"),
            new(Guid.NewGuid(), UserRole.Client, "u3@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y"),
            new(Guid.NewGuid(), UserRole.Trainer, "u4@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y")
        ];
    }
    
    public static List<Client> CreateMockClients()
    {
        var clients = new List<Client>
        {
            new Client(Guid.NewGuid(), "John Doe", "Male", new DateOnly(1990, 1, 1),
                Guid.NewGuid(), new DateOnly(2023, 12, 31), 2, null),
        
            new Client(Guid.NewGuid(), "Jane Smith", "Female", new DateOnly(1995, 5, 10),
                Guid.NewGuid(), new DateOnly(2022, 8, 31), null, null),
        
            new Client(Guid.NewGuid(), "Mike Johnson", "Male", new DateOnly(1985, 7, 20),
                Guid.NewGuid(), new DateOnly(2022, 12, 31), 0, null)
        };

        return clients;
    }
    

    public static List<Workout> CreateMockWorkouts()
    {
        var workouts = new List<Workout>
        {
            new Workout(Guid.NewGuid(), "Workout 1", "Description 1", Guid.NewGuid(), TimeSpan.FromMinutes(60), 1),
            new Workout(Guid.NewGuid(), "Workout 2", "Description 2", Guid.NewGuid(), TimeSpan.FromMinutes(45), 2),
            new Workout(Guid.NewGuid(), "Workout 3", "Description 3", Guid.NewGuid(), TimeSpan.FromMinutes(90), 3)
        };

        return workouts;
    }


    public static List<Trainer> CreateMockTrainers()
    {
        return new List<Trainer>
        {
            new Trainer(Guid.NewGuid(), "Trainer 1", "Male", "Fitness", 5, 4),
            new Trainer(Guid.NewGuid(), "Trainer 2", "Female", "Yoga", 3, 5),
            new Trainer(Guid.NewGuid(), "Trainer 3", "Male", "CrossFit", 8, 3)
        };
    }

    public void Dispose() => Context.Dispose();
}