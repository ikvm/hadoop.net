/*
*  Licensed to the Apache Software Foundation (ASF) under one or more
*  contributor license agreements.  See the NOTICE file distributed with
*  this work for additional information regarding copyright ownership.
*  The ASF licenses this file to You under the Apache License, Version 2.0
*  (the "License"); you may not use this file except in compliance with
*  the License.  You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*
*/
using System;
using Hadoop.Common.Core.Conf;
using Org.Apache.Hadoop.Conf;
using Org.Apache.Hadoop.IO.Compress;


namespace Org.Apache.Hadoop.IO.Compress.Bzip2
{
	/// <summary>This is a dummy compressor for BZip2.</summary>
	public class BZip2DummyCompressor : Compressor
	{
		/// <exception cref="System.IO.IOException"/>
		public virtual int Compress(byte[] b, int off, int len)
		{
			throw new NotSupportedException();
		}

		public virtual void End()
		{
			throw new NotSupportedException();
		}

		public virtual void Finish()
		{
			throw new NotSupportedException();
		}

		public virtual bool Finished()
		{
			throw new NotSupportedException();
		}

		public virtual long GetBytesRead()
		{
			throw new NotSupportedException();
		}

		public virtual long GetBytesWritten()
		{
			throw new NotSupportedException();
		}

		public virtual bool NeedsInput()
		{
			throw new NotSupportedException();
		}

		public virtual void Reset()
		{
		}

		// do nothing
		public virtual void SetDictionary(byte[] b, int off, int len)
		{
			throw new NotSupportedException();
		}

		public virtual void SetInput(byte[] b, int off, int len)
		{
			throw new NotSupportedException();
		}

		public virtual void Reinit(Configuration conf)
		{
		}
		// do nothing
	}
}
