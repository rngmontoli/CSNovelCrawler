using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace CSNovelCrawler
{

	/// <summary>
	/// Acdown委托
	/// </summary>
	/// <param name="para"></param>
	public delegate void AcTaskDelegate(DelegateParameter para);

	/// <summary>
	/// 委托包装类
	/// </summary>
	public class DelegateContainer
	{
		public DelegateContainer() { }
		public DelegateContainer(AcTaskDelegate newPartDele,
							 AcTaskDelegate refreshDele,
							 AcTaskDelegate tipTextDele,
							 AcTaskDelegate newTaskDele)
		{
			NewPart += newPartDele;
			Refresh += refreshDele;
			TipText += tipTextDele;
			NewTask += newTaskDele;
		}
		public AcTaskDelegate NewPart { get; set; }
		public AcTaskDelegate Refresh { get; set; }
		public AcTaskDelegate TipText { get; set; }
		public AcTaskDelegate NewTask { get; set; }
	}


	public class DelegateParameter
	{
		public TaskInfo SourceTask { get; set; }
	}

	public class ParaNewPart : DelegateParameter
	{
		public ParaNewPart(TaskInfo task, Int32 partNum) { SourceTask = task; PartNumber = partNum; }
		public Int32 PartNumber { get; set; }
	}
	public class ParaRefresh : DelegateParameter
	{
		public ParaRefresh(TaskInfo task) { SourceTask = task; }
	}
	public class ParaTipText : DelegateParameter
	{
		public ParaTipText(TaskInfo task, string tip) { SourceTask = task; TipText = tip; }
		public string TipText { get; set; }
	}

	public class ParaNewTask : DelegateParameter
	{
		public ParaNewTask(IPlugin plugin, string url, TaskInfo sourceTask)
		{
			Plugin = plugin;
			Url = url;
			SourceTask = sourceTask;
		}
		public IPlugin Plugin { get; set; }
		public string Url { get; set; }
	}

}
