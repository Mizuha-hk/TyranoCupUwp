﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.Models
{
    public sealed class ScheduleModel
    {
        private static ScheduleModel _scheduleModel;

        private ScheduleModel() { }

        public static ScheduleModel GetInstance()
        {
            if (_scheduleModel == null)
            {
                _scheduleModel = new ScheduleModel();
            }
            return _scheduleModel;
        }
        public string Subject { get; set; }
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;
        public string Location { get; set; }
        public TimeSpan Reminder { get; set; }
    }
}
