
namespace CSNovelCrawler.Class
{


	public delegate void TaskDelegate(DelegateParameter para);
    public delegate void SysDelegate();
	/// <summary>
	/// 委派
	/// </summary>
	public class DelegateContainer
	{
		public TaskDelegate Refresh { get; set; }
        public SysDelegate Exit { get; set; }
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
