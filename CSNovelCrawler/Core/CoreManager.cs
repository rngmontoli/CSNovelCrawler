﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler.Core
{
    public class CoreManager
    {

        /// <summary>
        /// 起始路径
        /// </summary>
        public static string StartupPath { get; set; }
        /// <summary>
        /// 任務管理器
        /// </summary>
        public static TaskManager TaskManager { get; private set; }
        /// <summary>
        /// 插件管理器
        /// </summary>
        public static PluginManager PluginManager { get; private set; }
        /// <summary>
        /// 配置管理器
        /// </summary>
        public static ConfigManager ConfigManager { get; private set; }

        /// <summary>
        /// 初始化AcDown核心
        /// </summary>
        public static void Initialize()
        {
            StartupPath = System.Environment.CurrentDirectory;
            ConfigManager = new ConfigManager();
            ConfigManager.LoadSettings();
            PluginManager = new PluginManager();
            TaskManager = new TaskManager();
            TaskManager.LoadAllTasks();
            

        }
    }
}
