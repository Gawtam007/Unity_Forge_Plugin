//
// Copyright (c) Autodesk, Inc. All rights reserved.
// 
// This computer source code and related instructions and comments are the
// unpublished confidential and proprietary information of Autodesk, Inc.
// and are protected under Federal copyright and state trade secret law.
// They may not be disclosed to, copied or used by any third party without
// the prior written consent of Autodesk, Inc.
//
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
#if UNITY_EDITOR
using UnityEditor;
#endif
using SimpleJSON;


namespace Autodesk.Forge.ARKit {

	public class ForgeProperties : MonoBehaviour, ISerializationCallbackReceiver {

		#region Properties
		public string PropertiesString ="" ;
		//[NonSerialized]
		public JSONNode Properties { get; /*internal*/ set; }
		//private metadetail metadetail;

		#endregion

		#region Unity APIs
		protected virtual void Awake () {
			//metadetail = FindObjectOfType<metadetail>();
		}

		protected virtual void Update () {
		}

		public void OnBeforeSerialize()
		{
			if (Properties != null)
			{
				PropertiesString = Properties.ToString();
				//metadetail.meta(Properties);
			}
		}

		public void OnAfterDeserialize () {
			if ( !string.IsNullOrEmpty (PropertiesString) )
				Properties =JSON.Parse (PropertiesString) ;
		}

		#endregion

	}

}
