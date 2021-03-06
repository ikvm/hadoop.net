using System;
using Org.Apache.Hadoop.Conf;
using Org.Apache.Hadoop.FS;
using Org.Apache.Hadoop.IO;
using Org.Apache.Hadoop.Mapreduce;
using Org.Apache.Hadoop.Security;
using Sharpen;

namespace Org.Apache.Hadoop.Mapreduce.Lib.Chain
{
	/// <summary>
	/// A simple wrapper class that delegates most of its functionality to the
	/// underlying context, but overrides the methods to do with record readers ,
	/// record writers and configuration.
	/// </summary>
	internal class ChainMapContextImpl<Keyin, Valuein, Keyout, Valueout> : MapContext
		<KEYIN, VALUEIN, KEYOUT, VALUEOUT>
	{
		private RecordReader<KEYIN, VALUEIN> reader;

		private RecordWriter<KEYOUT, VALUEOUT> output;

		private TaskInputOutputContext<KEYIN, VALUEIN, KEYOUT, VALUEOUT> @base;

		private Configuration conf;

		internal ChainMapContextImpl(TaskInputOutputContext<KEYIN, VALUEIN, KEYOUT, VALUEOUT
			> @base, RecordReader<KEYIN, VALUEIN> rr, RecordWriter<KEYOUT, VALUEOUT> rw, Configuration
			 conf)
		{
			this.reader = rr;
			this.output = rw;
			this.@base = @base;
			this.conf = conf;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual KEYIN GetCurrentKey()
		{
			return reader.GetCurrentKey();
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual VALUEIN GetCurrentValue()
		{
			return reader.GetCurrentValue();
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual bool NextKeyValue()
		{
			return reader.NextKeyValue();
		}

		public virtual InputSplit GetInputSplit()
		{
			if (@base is MapContext)
			{
				MapContext<KEYIN, VALUEIN, KEYOUT, VALUEOUT> mc = (MapContext<KEYIN, VALUEIN, KEYOUT
					, VALUEOUT>)@base;
				return mc.GetInputSplit();
			}
			else
			{
				return null;
			}
		}

		public virtual Counter GetCounter<_T0>(Enum<_T0> counterName)
			where _T0 : Enum<E>
		{
			return @base.GetCounter(counterName);
		}

		public virtual Counter GetCounter(string groupName, string counterName)
		{
			return @base.GetCounter(groupName, counterName);
		}

		public virtual OutputCommitter GetOutputCommitter()
		{
			return @base.GetOutputCommitter();
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual void Write(KEYOUT key, VALUEOUT value)
		{
			output.Write(key, value);
		}

		public virtual string GetStatus()
		{
			return @base.GetStatus();
		}

		public virtual TaskAttemptID GetTaskAttemptID()
		{
			return @base.GetTaskAttemptID();
		}

		public virtual void SetStatus(string msg)
		{
			@base.SetStatus(msg);
		}

		public virtual Path[] GetArchiveClassPaths()
		{
			return @base.GetArchiveClassPaths();
		}

		public virtual string[] GetArchiveTimestamps()
		{
			return @base.GetArchiveTimestamps();
		}

		/// <exception cref="System.IO.IOException"/>
		public virtual URI[] GetCacheArchives()
		{
			return @base.GetCacheArchives();
		}

		/// <exception cref="System.IO.IOException"/>
		public virtual URI[] GetCacheFiles()
		{
			return @base.GetCacheFiles();
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetCombinerClass()
		{
			return @base.GetCombinerClass();
		}

		public virtual Configuration GetConfiguration()
		{
			return conf;
		}

		public virtual Path[] GetFileClassPaths()
		{
			return @base.GetFileClassPaths();
		}

		public virtual string[] GetFileTimestamps()
		{
			return @base.GetFileTimestamps();
		}

		public virtual RawComparator<object> GetCombinerKeyGroupingComparator()
		{
			return @base.GetCombinerKeyGroupingComparator();
		}

		public virtual RawComparator<object> GetGroupingComparator()
		{
			return @base.GetGroupingComparator();
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetInputFormatClass()
		{
			return @base.GetInputFormatClass();
		}

		public virtual string GetJar()
		{
			return @base.GetJar();
		}

		public virtual JobID GetJobID()
		{
			return @base.GetJobID();
		}

		public virtual string GetJobName()
		{
			return @base.GetJobName();
		}

		public virtual bool GetJobSetupCleanupNeeded()
		{
			return @base.GetJobSetupCleanupNeeded();
		}

		public virtual bool GetTaskCleanupNeeded()
		{
			return @base.GetTaskCleanupNeeded();
		}

		/// <exception cref="System.IO.IOException"/>
		public virtual Path[] GetLocalCacheArchives()
		{
			return @base.GetLocalCacheArchives();
		}

		/// <exception cref="System.IO.IOException"/>
		public virtual Path[] GetLocalCacheFiles()
		{
			return @base.GetLocalCacheArchives();
		}

		public virtual Type GetMapOutputKeyClass()
		{
			return @base.GetMapOutputKeyClass();
		}

		public virtual Type GetMapOutputValueClass()
		{
			return @base.GetMapOutputValueClass();
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetMapperClass()
		{
			return @base.GetMapperClass();
		}

		public virtual int GetMaxMapAttempts()
		{
			return @base.GetMaxMapAttempts();
		}

		public virtual int GetMaxReduceAttempts()
		{
			return @base.GetMaxReduceAttempts();
		}

		public virtual int GetNumReduceTasks()
		{
			return @base.GetNumReduceTasks();
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetOutputFormatClass()
		{
			return @base.GetOutputFormatClass();
		}

		public virtual Type GetOutputKeyClass()
		{
			return @base.GetMapOutputKeyClass();
		}

		public virtual Type GetOutputValueClass()
		{
			return @base.GetOutputValueClass();
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetPartitionerClass()
		{
			return @base.GetPartitionerClass();
		}

		public virtual bool GetProfileEnabled()
		{
			return @base.GetProfileEnabled();
		}

		public virtual string GetProfileParams()
		{
			return @base.GetProfileParams();
		}

		public virtual Configuration.IntegerRanges GetProfileTaskRange(bool isMap)
		{
			return @base.GetProfileTaskRange(isMap);
		}

		/// <exception cref="System.TypeLoadException"/>
		public virtual Type GetReducerClass()
		{
			return @base.GetReducerClass();
		}

		public virtual RawComparator<object> GetSortComparator()
		{
			return @base.GetSortComparator();
		}

		public virtual bool GetSymlink()
		{
			return @base.GetSymlink();
		}

		public virtual string GetUser()
		{
			return @base.GetUser();
		}

		/// <exception cref="System.IO.IOException"/>
		public virtual Path GetWorkingDirectory()
		{
			return @base.GetWorkingDirectory();
		}

		public virtual void Progress()
		{
			@base.Progress();
		}

		public virtual Credentials GetCredentials()
		{
			return @base.GetCredentials();
		}

		public virtual float GetProgress()
		{
			return @base.GetProgress();
		}
	}
}
