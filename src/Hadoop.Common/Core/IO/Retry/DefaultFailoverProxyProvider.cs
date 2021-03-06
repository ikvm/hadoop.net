/*
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements.  See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership.  The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License.  You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System;
using Org.Apache.Hadoop.Ipc;


namespace Org.Apache.Hadoop.IO.Retry
{
	/// <summary>
	/// An implementation of
	/// <see cref="FailoverProxyProvider{T}"/>
	/// which does nothing in the
	/// event of failover, and always returns the same proxy object.
	/// </summary>
	public class DefaultFailoverProxyProvider<T> : FailoverProxyProvider<T>
	{
		private T proxy;

		private Type iface;

		public DefaultFailoverProxyProvider(Type iface, T proxy)
		{
			this.proxy = proxy;
			this.iface = iface;
		}

		public override Type GetInterface()
		{
			return iface;
		}

		public override FailoverProxyProvider.ProxyInfo<T> GetProxy()
		{
			return new FailoverProxyProvider.ProxyInfo<T>(proxy, null);
		}

		public override void PerformFailover(T currentProxy)
		{
		}

		// Nothing to do.
		/// <exception cref="System.IO.IOException"/>
		public virtual void Close()
		{
			RPC.StopProxy(proxy);
		}
	}
}
