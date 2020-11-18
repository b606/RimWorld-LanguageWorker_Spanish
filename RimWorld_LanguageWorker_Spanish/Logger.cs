// <code-header>
//   <author>b606</author>
//   <summary>
//		File logger class outside the game path (in the system /tmp folder).
//	 </summary>
// </code-header>

using System;
using System.IO;
using System.Text;

namespace RimWorld_LanguageWorker_Spanish
{
	public class Logger : IDisposable
	{
		string filename;
		StreamWriter sw;

		public Logger(string name)
		{
			filename = Path.Combine(Path.GetTempPath(), name);
			sw = new StreamWriter(filename, false, Encoding.UTF8);
			sw.AutoFlush = true;
		}

		public void Message(string str)
		{
			sw.WriteLine(str);
			sw.Flush();
		}

		public void Flush()
		{
			sw.Flush();
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// Dispose managed state (managed objects).
				}

				// Free unmanaged resources (unmanaged objects) and override a finalizer below.
				sw.Flush();
				sw.Close();

				disposedValue = true;
			}
		}

		~Logger()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}