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
using Com.Sun.Javadoc;
using Jdiff;


namespace Org.Apache.Hadoop.Classification.Tools
{
	/// <summary>
	/// A <a href="http://java.sun.com/javase/6/docs/jdk/api/javadoc/doclet/">Doclet</a>
	/// for excluding elements that are annotated with
	/// <see cref="Org.Apache.Hadoop.Classification.InterfaceAudience.Private"/>
	/// or
	/// <see cref="Org.Apache.Hadoop.Classification.InterfaceAudience.LimitedPrivate"/>
	/// .
	/// It delegates to the JDiff Doclet, and takes the same options.
	/// </summary>
	public class ExcludePrivateAnnotationsJDiffDoclet
	{
		public static Com.Sun.Javadoc.LanguageVersion LanguageVersion()
		{
			return Com.Sun.Javadoc.LanguageVersion.Java15;
		}

		public static bool Start(RootDoc root)
		{
			System.Console.Out.WriteLine(typeof(ExcludePrivateAnnotationsJDiffDoclet).Name);
			return JDiff.Start(RootDocProcessor.Process(root));
		}

		public static int OptionLength(string option)
		{
			int length = StabilityOptions.OptionLength(option);
			if (length != null)
			{
				return length;
			}
			return JDiff.OptionLength(option);
		}

		public static bool ValidOptions(string[][] options, DocErrorReporter reporter)
		{
			StabilityOptions.ValidOptions(options, reporter);
			string[][] filteredOptions = StabilityOptions.FilterOptions(options);
			return JDiff.ValidOptions(filteredOptions, reporter);
		}
	}
}
