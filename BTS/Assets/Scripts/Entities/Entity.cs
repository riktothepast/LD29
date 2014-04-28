using UnityEngine;
using System.Collections;

public abstract class Entity : FContainer
{
    public Rect collisionBox { set; get; }
    public FSprite sprite { set; get; }
    float rotationalAngle;
    int energy;
    public Vector2 velocity;
    public Vector2 Position;
    public float ttl;
    public int scoreValue;

	public virtual void Start () {
	
	}
	
	public virtual void Update () {
	
	}

    public virtual void Destroy() { 
        // explosion particle stuff here. remove from memory.

    }

    public float getRotationalAngle() {
        return rotationalAngle;
    }

    public void setRotationalAngle(float val) {
        rotationalAngle = val;
    }

    public int getEnergy() {
        return energy;
    }

    public void setEnergy(int val) {
        energy = val;
    }
    FParticleDefinition pd1 = new FParticleDefinition("bubble");

    public void DestroyEffect(){
        pd1 = new FParticleDefinition("bubble");
        pd1.startScale = 2f;
        pd1.startColor = new Color(47, 44, 44, 0.8f);
        pd1.endColor = new Color(255, 255, 255, 0.1f);
        int part = RXRandom.Range(120, 150);
        for (int x = 0; x < part; x++)
        {
            pd1.x = Position.x;
            pd1.y = Position.y;
            Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f, 40.0f);
            pd1.speedX = speed.x;
            pd1.speedY = speed.y;
            pd1.lifetime = RXRandom.Range(0.8f, 1f);
            InGamePage.CurrentInGamePage.projectilesParticles.AddParticle(pd1);
        }
    }
}
