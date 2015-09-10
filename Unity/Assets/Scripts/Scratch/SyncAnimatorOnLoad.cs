using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncAnimatorOnLoad : NetworkBehaviour
{
	public Animator animator;

	public override bool OnSerialize (NetworkWriter writer, bool initialState)
	{
		var serialized = base.OnSerialize (writer, initialState);
		var time = animator.playbackTime;
		writer.Write (time);
		return serialized;
	}

	public override void OnDeserialize (NetworkReader reader, bool initialState)
	{
		base.OnDeserialize (reader, initialState);
		float time = 0f;
		try {
			time = reader.ReadSingle ();
		} catch (System.Exception e) {
			Debug.LogError ("Custom deserialization exception.");
			Debug.LogError (e);
		}
		animator.playbackTime = time;
	}
}
