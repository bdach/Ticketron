﻿using System;
using Windows.UI.Text;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToDoList.DB.Models;
using ToDoList.DB.Repositories;

namespace ToDoList.App.Views.DailyLog
{
    public sealed partial class DailyLogPage : Page
    {
        private readonly ITaskLogRepository _taskLogRepository;

        public DailyLogPage()
        {
            this.InitializeComponent();

            _taskLogRepository = App.Current.Services.GetRequiredService<ITaskLogRepository>();
        }

        private async System.Threading.Tasks.Task FetchLogAsync(DateTime date)
        {
            var dailyLog = await _taskLogRepository.GetDailyLogAsync(date);
            LogTreeView.ItemsSource = dailyLog.TaskProgress;
        }

        private async void LogDateChanged(object? _, DatePickerValueChangedEventArgs e)
            => await FetchLogAsync(e.NewDate.Date);

        private async void OnLoaded(object _, RoutedEventArgs __)
            => await FetchLogAsync(LogDatePicker.Date.Date);

        public static string FormatNotes(string notes)
            => string.IsNullOrWhiteSpace(notes) ? "Time logged without a note" : notes;

        public static FontStyle StyleForNotes(string notes)
            => string.IsNullOrWhiteSpace(notes) ? FontStyle.Italic : FontStyle.Normal;

        public static string FormatDuration(TimeSpan duration)
            => duration.Humanize(minUnit: TimeUnit.Minute);
    }

    internal class DailyLogItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TaskTemplate { get; set; }
        public DataTemplate? LogEntryTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(object item) => item switch
        {
            DailyTaskProgress => TaskTemplate,
            TaskLogEntry => LogEntryTemplate,
            _ => throw new ArgumentException($"Unsupported item type {item.GetType()}", nameof(item))
        };
    }
}
