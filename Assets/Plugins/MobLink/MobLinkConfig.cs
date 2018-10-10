using System;
using System.Collections;
using UnityEngine;

namespace com.moblink.unity3d
{
	[Serializable]
	public class MobLinkConfig
	{
		public string appKey;
		public string appSecret;

		public MobLinkConfig()
		{
			this.appKey = "276c6bc1f0dd1";
			this.appSecret = "fc0470e431c0fc25a36b9c9d7801f633";
		}
	}		

}
