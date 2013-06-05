
namespace CSNovelCrawler.Class
{


	public delegate void TaskDelegate(DelegateParameter para);

	/// <summary>
	/// 委派
	/// </summary>
	public class DelegateContainer
	{
		public TaskDelegate Refresh { get; set; }
	}

	public class DelegateParameter
	{
		public TaskInfo SourceTask { get; set; }
	}

    public class ParaRefresh : DelegateParameter
    {
        public ParaRefresh(TaskInfo task) { SourceTask = task; }
    }


}
