﻿namespace WebApplication1.Models
{
    public class AppSettings : Options
    {
        public string DirPath { get; set; }

        public override string OptionsName => "AppSettings";
    }
}
