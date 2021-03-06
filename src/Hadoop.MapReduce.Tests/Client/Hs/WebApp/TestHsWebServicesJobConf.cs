using System.Collections.Generic;
using System.IO;
using Com.Google.Inject;
using Com.Google.Inject.Servlet;
using Com.Sun.Jersey.Api.Client;
using Com.Sun.Jersey.Guice.Spi.Container.Servlet;
using Com.Sun.Jersey.Test.Framework;
using Javax.WS.RS.Core;
using Javax.Xml.Parsers;
using NUnit.Framework;
using Org.Apache.Hadoop.Conf;
using Org.Apache.Hadoop.FS;
using Org.Apache.Hadoop.Mapreduce;
using Org.Apache.Hadoop.Mapreduce.V2.Api.Records;
using Org.Apache.Hadoop.Mapreduce.V2.App;
using Org.Apache.Hadoop.Mapreduce.V2.HS;
using Org.Apache.Hadoop.Mapreduce.V2.Util;
using Org.Apache.Hadoop.Yarn.Webapp;
using Org.Codehaus.Jettison.Json;
using Org.W3c.Dom;
using Org.Xml.Sax;
using Sharpen;

namespace Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp
{
	/// <summary>Test the history server Rest API for getting the job conf.</summary>
	/// <remarks>
	/// Test the history server Rest API for getting the job conf. This
	/// requires created a temporary configuration file.
	/// /ws/v1/history/mapreduce/jobs/{jobid}/conf
	/// </remarks>
	public class TestHsWebServicesJobConf : JerseyTest
	{
		private static Configuration conf = new Configuration();

		private static HistoryContext appContext;

		private static HsWebApp webApp;

		private static FilePath testConfDir = new FilePath("target", typeof(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
			).Name + "confDir");

		private sealed class _ServletModule_88 : ServletModule
		{
			public _ServletModule_88()
			{
			}

			protected override void ConfigureServlets()
			{
				Path confPath = new Path(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.testConfDir.ToString(), MRJobConfig.JobConfFile);
				Configuration config = new Configuration();
				FileSystem localFs;
				try
				{
					localFs = FileSystem.GetLocal(config);
					confPath = localFs.MakeQualified(confPath);
					OutputStream @out = localFs.Create(confPath);
					try
					{
						Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf.conf.WriteXml(@out
							);
					}
					finally
					{
						@out.Close();
					}
					if (!localFs.Exists(confPath))
					{
						NUnit.Framework.Assert.Fail("error creating config file: " + confPath);
					}
				}
				catch (IOException e)
				{
					NUnit.Framework.Assert.Fail("error creating config file: " + e.Message);
				}
				Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf.appContext = new 
					MockHistoryContext(0, 2, 1, confPath);
				Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf.webApp = Org.Mockito.Mockito
					.Mock<HsWebApp>();
				Org.Mockito.Mockito.When(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.webApp.Name()).ThenReturn("hsmockwebapp");
				this.Bind<JAXBContextResolver>();
				this.Bind<HsWebServices>();
				this.Bind<GenericExceptionHandler>();
				this.Bind<WebApp>().ToInstance(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.webApp);
				this.Bind<AppContext>().ToInstance(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.appContext);
				this.Bind<HistoryContext>().ToInstance(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.appContext);
				this.Bind<Configuration>().ToInstance(Org.Apache.Hadoop.Mapreduce.V2.HS.Webapp.TestHsWebServicesJobConf
					.conf);
				this.Serve("/*").With(typeof(GuiceContainer));
			}
		}

		private Injector injector = Guice.CreateInjector(new _ServletModule_88());

		public class GuiceServletConfig : GuiceServletContextListener
		{
			protected override Injector GetInjector()
			{
				return this._enclosing.injector;
			}

			internal GuiceServletConfig(TestHsWebServicesJobConf _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly TestHsWebServicesJobConf _enclosing;
		}

		/// <exception cref="System.Exception"/>
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			testConfDir.Mkdir();
		}

		[AfterClass]
		public static void Stop()
		{
			FileUtil.FullyDelete(testConfDir);
		}

		public TestHsWebServicesJobConf()
			: base(new WebAppDescriptor.Builder("org.apache.hadoop.mapreduce.v2.hs.webapp").ContextListenerClass
				(typeof(TestHsWebServicesJobConf.GuiceServletConfig)).FilterClass(typeof(GuiceFilter
				)).ContextPath("jersey-guice-filter").ServletPath("/").Build())
		{
		}

		/// <exception cref="Org.Codehaus.Jettison.Json.JSONException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void TestJobConf()
		{
			WebResource r = Resource();
			IDictionary<JobId, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job> jobsMap = appContext
				.GetAllJobs();
			foreach (JobId id in jobsMap.Keys)
			{
				string jobId = MRApps.ToString(id);
				ClientResponse response = r.Path("ws").Path("v1").Path("history").Path("mapreduce"
					).Path("jobs").Path(jobId).Path("conf").Accept(MediaType.ApplicationJson).Get<ClientResponse
					>();
				NUnit.Framework.Assert.AreEqual(MediaType.ApplicationJsonType, response.GetType()
					);
				JSONObject json = response.GetEntity<JSONObject>();
				NUnit.Framework.Assert.AreEqual("incorrect number of elements", 1, json.Length());
				JSONObject info = json.GetJSONObject("conf");
				VerifyHsJobConf(info, jobsMap[id]);
			}
		}

		/// <exception cref="Org.Codehaus.Jettison.Json.JSONException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void TestJobConfSlash()
		{
			WebResource r = Resource();
			IDictionary<JobId, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job> jobsMap = appContext
				.GetAllJobs();
			foreach (JobId id in jobsMap.Keys)
			{
				string jobId = MRApps.ToString(id);
				ClientResponse response = r.Path("ws").Path("v1").Path("history").Path("mapreduce"
					).Path("jobs").Path(jobId).Path("conf/").Accept(MediaType.ApplicationJson).Get<ClientResponse
					>();
				NUnit.Framework.Assert.AreEqual(MediaType.ApplicationJsonType, response.GetType()
					);
				JSONObject json = response.GetEntity<JSONObject>();
				NUnit.Framework.Assert.AreEqual("incorrect number of elements", 1, json.Length());
				JSONObject info = json.GetJSONObject("conf");
				VerifyHsJobConf(info, jobsMap[id]);
			}
		}

		/// <exception cref="Org.Codehaus.Jettison.Json.JSONException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void TestJobConfDefault()
		{
			WebResource r = Resource();
			IDictionary<JobId, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job> jobsMap = appContext
				.GetAllJobs();
			foreach (JobId id in jobsMap.Keys)
			{
				string jobId = MRApps.ToString(id);
				ClientResponse response = r.Path("ws").Path("v1").Path("history").Path("mapreduce"
					).Path("jobs").Path(jobId).Path("conf").Get<ClientResponse>();
				NUnit.Framework.Assert.AreEqual(MediaType.ApplicationJsonType, response.GetType()
					);
				JSONObject json = response.GetEntity<JSONObject>();
				NUnit.Framework.Assert.AreEqual("incorrect number of elements", 1, json.Length());
				JSONObject info = json.GetJSONObject("conf");
				VerifyHsJobConf(info, jobsMap[id]);
			}
		}

		/// <exception cref="Org.Codehaus.Jettison.Json.JSONException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void TestJobConfXML()
		{
			WebResource r = Resource();
			IDictionary<JobId, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job> jobsMap = appContext
				.GetAllJobs();
			foreach (JobId id in jobsMap.Keys)
			{
				string jobId = MRApps.ToString(id);
				ClientResponse response = r.Path("ws").Path("v1").Path("history").Path("mapreduce"
					).Path("jobs").Path(jobId).Path("conf").Accept(MediaType.ApplicationXml).Get<ClientResponse
					>();
				NUnit.Framework.Assert.AreEqual(MediaType.ApplicationXmlType, response.GetType());
				string xml = response.GetEntity<string>();
				DocumentBuilderFactory dbf = DocumentBuilderFactory.NewInstance();
				DocumentBuilder db = dbf.NewDocumentBuilder();
				InputSource @is = new InputSource();
				@is.SetCharacterStream(new StringReader(xml));
				Document dom = db.Parse(@is);
				NodeList info = dom.GetElementsByTagName("conf");
				VerifyHsJobConfXML(info, jobsMap[id]);
			}
		}

		/// <exception cref="Org.Codehaus.Jettison.Json.JSONException"/>
		public virtual void VerifyHsJobConf(JSONObject info, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job
			 job)
		{
			NUnit.Framework.Assert.AreEqual("incorrect number of elements", 2, info.Length());
			WebServicesTestUtils.CheckStringMatch("path", job.GetConfFile().ToString(), info.
				GetString("path"));
			// just do simple verification of fields - not data is correct
			// in the fields
			JSONArray properties = info.GetJSONArray("property");
			for (int i = 0; i < properties.Length(); i++)
			{
				JSONObject prop = properties.GetJSONObject(i);
				string name = prop.GetString("name");
				string value = prop.GetString("value");
				NUnit.Framework.Assert.IsTrue("name not set", (name != null && !name.IsEmpty()));
				NUnit.Framework.Assert.IsTrue("value not set", (value != null && !value.IsEmpty()
					));
			}
		}

		public virtual void VerifyHsJobConfXML(NodeList nodes, Org.Apache.Hadoop.Mapreduce.V2.App.Job.Job
			 job)
		{
			NUnit.Framework.Assert.AreEqual("incorrect number of elements", 1, nodes.GetLength
				());
			for (int i = 0; i < nodes.GetLength(); i++)
			{
				Element element = (Element)nodes.Item(i);
				WebServicesTestUtils.CheckStringMatch("path", job.GetConfFile().ToString(), WebServicesTestUtils
					.GetXmlString(element, "path"));
				// just do simple verification of fields - not data is correct
				// in the fields
				NodeList properties = element.GetElementsByTagName("property");
				for (int j = 0; j < properties.GetLength(); j++)
				{
					Element property = (Element)properties.Item(j);
					NUnit.Framework.Assert.IsNotNull("should have counters in the web service info", 
						property);
					string name = WebServicesTestUtils.GetXmlString(property, "name");
					string value = WebServicesTestUtils.GetXmlString(property, "value");
					NUnit.Framework.Assert.IsTrue("name not set", (name != null && !name.IsEmpty()));
					NUnit.Framework.Assert.IsTrue("name not set", (value != null && !value.IsEmpty())
						);
				}
			}
		}
	}
}
