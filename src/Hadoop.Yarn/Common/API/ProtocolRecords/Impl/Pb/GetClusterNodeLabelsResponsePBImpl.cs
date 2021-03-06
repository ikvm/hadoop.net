using System.Collections.Generic;
using Org.Apache.Hadoop.Yarn.Api.Protocolrecords;
using Org.Apache.Hadoop.Yarn.Proto;
using Sharpen;

namespace Org.Apache.Hadoop.Yarn.Api.Protocolrecords.Impl.PB
{
	public class GetClusterNodeLabelsResponsePBImpl : GetClusterNodeLabelsResponse
	{
		internal ICollection<string> labels;

		internal YarnServiceProtos.GetClusterNodeLabelsResponseProto proto = YarnServiceProtos.GetClusterNodeLabelsResponseProto
			.GetDefaultInstance();

		internal YarnServiceProtos.GetClusterNodeLabelsResponseProto.Builder builder = null;

		internal bool viaProto = false;

		public GetClusterNodeLabelsResponsePBImpl()
		{
			this.builder = YarnServiceProtos.GetClusterNodeLabelsResponseProto.NewBuilder();
		}

		public GetClusterNodeLabelsResponsePBImpl(YarnServiceProtos.GetClusterNodeLabelsResponseProto
			 proto)
		{
			this.proto = proto;
			viaProto = true;
		}

		private void MaybeInitBuilder()
		{
			if (viaProto || builder == null)
			{
				builder = YarnServiceProtos.GetClusterNodeLabelsResponseProto.NewBuilder(proto);
			}
			viaProto = false;
		}

		private void MergeLocalToBuilder()
		{
			if (this.labels != null && !this.labels.IsEmpty())
			{
				builder.ClearNodeLabels();
				builder.AddAllNodeLabels(this.labels);
			}
		}

		private void MergeLocalToProto()
		{
			if (viaProto)
			{
				MaybeInitBuilder();
			}
			MergeLocalToBuilder();
			proto = ((YarnServiceProtos.GetClusterNodeLabelsResponseProto)builder.Build());
			viaProto = true;
		}

		public virtual YarnServiceProtos.GetClusterNodeLabelsResponseProto GetProto()
		{
			MergeLocalToProto();
			proto = viaProto ? proto : ((YarnServiceProtos.GetClusterNodeLabelsResponseProto)
				builder.Build());
			viaProto = true;
			return proto;
		}

		private void InitNodeLabels()
		{
			if (this.labels != null)
			{
				return;
			}
			YarnServiceProtos.GetClusterNodeLabelsResponseProtoOrBuilder p = viaProto ? proto
				 : builder;
			this.labels = new HashSet<string>();
			Sharpen.Collections.AddAll(this.labels, p.GetNodeLabelsList());
		}

		public override void SetNodeLabels(ICollection<string> labels)
		{
			MaybeInitBuilder();
			if (labels == null || labels.IsEmpty())
			{
				builder.ClearNodeLabels();
			}
			this.labels = labels;
		}

		public override ICollection<string> GetNodeLabels()
		{
			InitNodeLabels();
			return this.labels;
		}

		public override int GetHashCode()
		{
			System.Diagnostics.Debug.Assert(false, "hashCode not designed");
			return 0;
		}

		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (other.GetType().IsAssignableFrom(this.GetType()))
			{
				return this.GetProto().Equals(this.GetType().Cast(other).GetProto());
			}
			return false;
		}
	}
}
