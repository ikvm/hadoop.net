using Org.Apache.Commons.Logging;
using Org.Apache.Hadoop.Conf;


namespace Org.Apache.Hadoop.FS
{
	public class TestFileContext
	{
		private static readonly Log Log = LogFactory.GetLog(typeof(TestFileContext));

		/// <exception cref="System.Exception"/>
		[Fact]
		public virtual void TestDefaultURIWithoutScheme()
		{
			Configuration conf = new Configuration();
			conf.Set(FileSystem.FsDefaultNameKey, "/");
			try
			{
				FileContext.GetFileContext(conf);
				NUnit.Framework.Assert.Fail(typeof(UnsupportedFileSystemException) + " not thrown!"
					);
			}
			catch (UnsupportedFileSystemException ufse)
			{
				Log.Info("Expected exception: ", ufse);
			}
		}
	}
}
