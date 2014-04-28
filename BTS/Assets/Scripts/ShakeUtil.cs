using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/*
Usage :

ShakeUtil.Go(myFNode,myDurationInSeconds,myAmplitudeInPixels);

*/
public class ShakeUtil {
    static Dictionary<FNode, ShakeUtil> _pendings = new Dictionary<FNode, ShakeUtil>();
	
	public Vector2 oPosition;
	public float duration,amplitude;
	public FNode node=null;
	
	public float curDuration,curAmplitude;
	
	public ShakeUtil () {
	}
	public void go(FNode node_,float duration_,float amplitude_) {
		oPosition=node_.GetPosition();
		curDuration=duration=duration_;
		curAmplitude=amplitude=amplitude_;
        if (_pendings.ContainsKey(node_))
            _pendings.Remove(node_);
		_pendings.Add(node_,this);
		if (node==null) {
			Futile.instance.SignalUpdate+=HandleUpdate;
		}
		node=node_;
	}
	protected void HandleUpdate() {
		curDuration-=Time.deltaTime;
		if (curDuration<0) {
			Stop();
		} else {
			curAmplitude=amplitude*curDuration/duration;
			node.x=oPosition.x+RXRandom.Range(-curAmplitude,curAmplitude);
			node.y=oPosition.y+RXRandom.Range(-curAmplitude,curAmplitude);
		}
	}
	protected void Stop() {
		if (node!=null) {
			Futile.instance.SignalUpdate-=HandleUpdate;
			node.SetPosition(oPosition);
			_pendings.Remove(node);
			node=null;
		}
	}
	static public void Go(FNode node_,float duration_,float amplitude_) {
		(new ShakeUtil()).go (node_,duration_,amplitude_);
	}
	static public void Cancel(FNode node) {
		ShakeUtil obj=null;
		_pendings.TryGetValue(node, out obj);
		if (obj!=null) {
			obj.Stop();
		}
	}
}