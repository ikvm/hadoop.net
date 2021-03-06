using System.Collections.Generic;
using NUnit.Framework;
using Org.Apache.Hadoop.Conf;


namespace Org.Apache.Hadoop.Security
{
	public class TestHttpCrossOriginFilterInitializer
	{
		[Fact]
		public virtual void TestGetFilterParameters()
		{
			// Initialize configuration object
			Configuration conf = new Configuration();
			conf.Set(HttpCrossOriginFilterInitializer.Prefix + "rootparam", "rootvalue");
			conf.Set(HttpCrossOriginFilterInitializer.Prefix + "nested.param", "nestedvalue");
			conf.Set("outofscopeparam", "outofscopevalue");
			// call function under test
			IDictionary<string, string> filterParameters = HttpCrossOriginFilterInitializer.GetFilterParameters
				(conf, HttpCrossOriginFilterInitializer.Prefix);
			// retrieve values
			string rootvalue = filterParameters["rootparam"];
			string nestedvalue = filterParameters["nested.param"];
			string outofscopeparam = filterParameters["outofscopeparam"];
			// verify expected values are in place
			Assert.Equal("Could not find filter parameter", "rootvalue", rootvalue
				);
			Assert.Equal("Could not find filter parameter", "nestedvalue", 
				nestedvalue);
			NUnit.Framework.Assert.IsNull("Found unexpected value in filter parameters", outofscopeparam
				);
		}
	}
}
