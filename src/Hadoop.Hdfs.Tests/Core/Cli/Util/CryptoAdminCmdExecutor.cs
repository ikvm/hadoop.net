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
using Org.Apache.Hadoop.Hdfs.Tools;
using Org.Apache.Hadoop.Util;
using Sharpen;

namespace Org.Apache.Hadoop.Cli.Util
{
	public class CryptoAdminCmdExecutor : CommandExecutor
	{
		protected internal string namenode = null;

		protected internal CryptoAdmin admin = null;

		public CryptoAdminCmdExecutor(string namenode, CryptoAdmin admin)
		{
			this.namenode = namenode;
			this.admin = admin;
		}

		/// <exception cref="System.Exception"/>
		protected override void Execute(string cmd)
		{
			string[] args = GetCommandAsArgs(cmd, "NAMENODE", this.namenode);
			ToolRunner.Run(admin, args);
		}
	}
}
