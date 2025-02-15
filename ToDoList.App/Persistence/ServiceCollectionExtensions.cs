﻿using Microsoft.Extensions.DependencyInjection;

namespace ToDoList.App.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceManagers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<AppStatePersistenceManager>();
        serviceCollection.AddSingleton<TaskGroupPagePersistenceManager>();
        serviceCollection.AddSingleton<TasksForTodayPersistenceManager>();

        return serviceCollection;
    }
}