using System;
using System.Collections.Generic;
using System.IO;
using Org.Apache.Hadoop.Conf;
using Org.Apache.Hadoop.IO;
using Org.Apache.Hadoop.IO.Serializer;
using Org.Apache.Hadoop.Mapreduce;
using Org.Apache.Hadoop.Util;
using Sharpen;

namespace Org.Apache.Hadoop.Mapreduce.Lib.Join
{
	/// <summary>This InputSplit contains a set of child InputSplits.</summary>
	/// <remarks>
	/// This InputSplit contains a set of child InputSplits. Any InputSplit inserted
	/// into this collection must have a public default constructor.
	/// </remarks>
	public class CompositeInputSplit : InputSplit, Writable
	{
		private int fill = 0;

		private long totsize = 0L;

		private InputSplit[] splits;

		private Configuration conf = new Configuration();

		public CompositeInputSplit()
		{
		}

		public CompositeInputSplit(int capacity)
		{
			splits = new InputSplit[capacity];
		}

		/// <summary>Add an InputSplit to this collection.</summary>
		/// <exception cref="System.IO.IOException">
		/// If capacity was not specified during construction
		/// or if capacity has been reached.
		/// </exception>
		/// <exception cref="System.Exception"/>
		public virtual void Add(InputSplit s)
		{
			if (null == splits)
			{
				throw new IOException("Uninitialized InputSplit");
			}
			if (fill == splits.Length)
			{
				throw new IOException("Too many splits");
			}
			splits[fill++] = s;
			totsize += s.GetLength();
		}

		/// <summary>Get ith child InputSplit.</summary>
		public virtual InputSplit Get(int i)
		{
			return splits[i];
		}

		/// <summary>Return the aggregate length of all child InputSplits currently added.</summary>
		/// <exception cref="System.IO.IOException"/>
		public override long GetLength()
		{
			return totsize;
		}

		/// <summary>Get the length of ith child InputSplit.</summary>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual long GetLength(int i)
		{
			return splits[i].GetLength();
		}

		/// <summary>Collect a set of hosts from all child InputSplits.</summary>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public override string[] GetLocations()
		{
			HashSet<string> hosts = new HashSet<string>();
			foreach (InputSplit s in splits)
			{
				string[] hints = s.GetLocations();
				if (hints != null && hints.Length > 0)
				{
					foreach (string host in hints)
					{
						hosts.AddItem(host);
					}
				}
			}
			return Sharpen.Collections.ToArray(hosts, new string[hosts.Count]);
		}

		/// <summary>getLocations from ith InputSplit.</summary>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		public virtual string[] GetLocation(int i)
		{
			return splits[i].GetLocations();
		}

		/// <summary>Write splits in the following format.</summary>
		/// <remarks>
		/// Write splits in the following format.
		/// <c>&lt;count&gt;&lt;class1&gt;&lt;class2&gt;...&lt;classn&gt;&lt;split1&gt;&lt;split2&gt;...&lt;splitn&gt;
		/// 	</c>
		/// </remarks>
		/// <exception cref="System.IO.IOException"/>
		public virtual void Write(DataOutput @out)
		{
			WritableUtils.WriteVInt(@out, splits.Length);
			foreach (InputSplit s in splits)
			{
				Text.WriteString(@out, s.GetType().FullName);
			}
			foreach (InputSplit s_1 in splits)
			{
				SerializationFactory factory = new SerializationFactory(conf);
				Org.Apache.Hadoop.IO.Serializer.Serializer serializer = factory.GetSerializer(s_1
					.GetType());
				serializer.Open((DataOutputStream)@out);
				serializer.Serialize(s_1);
			}
		}

		/// <summary><inheritDoc/></summary>
		/// <exception cref="System.IO.IOException">
		/// If the child InputSplit cannot be read, typically
		/// for failing access checks.
		/// </exception>
		public virtual void ReadFields(DataInput @in)
		{
			// Generic array assignment
			int card = WritableUtils.ReadVInt(@in);
			if (splits == null || splits.Length != card)
			{
				splits = new InputSplit[card];
			}
			Type[] cls = new Type[card];
			try
			{
				for (int i = 0; i < card; ++i)
				{
					cls[i] = Sharpen.Runtime.GetType(Text.ReadString(@in)).AsSubclass<InputSplit>();
				}
				for (int i_1 = 0; i_1 < card; ++i_1)
				{
					splits[i_1] = ReflectionUtils.NewInstance(cls[i_1], null);
					SerializationFactory factory = new SerializationFactory(conf);
					Deserializer deserializer = factory.GetDeserializer(cls[i_1]);
					deserializer.Open((DataInputStream)@in);
					splits[i_1] = (InputSplit)deserializer.Deserialize(splits[i_1]);
				}
			}
			catch (TypeLoadException e)
			{
				throw new IOException("Failed split init", e);
			}
		}
	}
}
