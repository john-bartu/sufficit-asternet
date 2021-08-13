using System;
using System.Threading;
using AsterNET.FastAGI;

namespace AsterNET.Util
{
	/// <summary>
	/// A TaskThread sits in a loop, asking the pool for a job, and servicing it.
	/// </summary>
	internal class ThreadTask : ThreadClass
	{
		private ThreadPool threadPool;

		#region ThreadPool
		public ThreadPool ThreadPool
		{
			get { return threadPool; }
		}
		#endregion

		#region ThreadTask(ThreadPool enclosingInstance, string name)
		public ThreadTask(ThreadPool enclosingInstance, string name)
			: base(name)
		{
			this.threadPool = enclosingInstance;
		}
		#endregion

		#region Run()
		/// <summary>
		/// Get a job from the pool, run it, repeat. If the obtained job is null, we exit the loop and the thread.
		/// </summary>
		public override void Run(object obj)
		{
			CancellationToken cancellationToken = (CancellationToken)obj;

			while (!cancellationToken.IsCancellationRequested)
			{
				AGIConnectionHandler job = ThreadPool.obtainJob();
				if (job == null)
					break;
				job.Run(cancellationToken);
			}
		}
		#endregion
	}
}
