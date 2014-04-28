using UnityEngine;
using System.Collections;

public class Bullet : Entity
{
    FParticleDefinition pd = new FParticleDefinition("bubble");
    int direction;
    public Bullet(Vector2 Position, float angle, int fr)
    {
        sprite = new FSprite("torpedo");
        sprite.color = Color.yellow;
        AddChild(sprite);
        InGamePage.CurrentInGamePage.projectileContainer.AddChild(this);
        InGamePage.CurrentInGamePage.projectiles.Add(this);
        this.Position = Position;
        SetPosition(Position);
        setRotationalAngle(angle);
        Position += velocity;
        SetPosition(Position);
        direction = fr;
        ttl = Random.Range(20,24);
        setEnergy(10);
    }
    float forwardStep = 30f;
    float currentStep = 0f;
    public override void Update()
    {
        if (currentStep < forwardStep) {
            currentStep += forwardStep* 0.5f * Time.deltaTime;
        }
        if (velocity.x > 0)
        {
            rotation = -getRotationalAngle();
            sprite.scaleX = 1f;

        }
        else
        {
            rotation = getRotationalAngle();
            sprite.scaleX = -1f;
        }

        rotation = getRotationalAngle();

        velocity.x += Mathf.Cos(getRotationalAngle() * Mathf.Deg2Rad) * Time.deltaTime * currentStep * direction;
        velocity.y += Mathf.Sin(getRotationalAngle() * Mathf.Deg2Rad) * Time.deltaTime * currentStep;

        Position += velocity;
        SetPosition(Position);
        ttl -= currentStep*Time.deltaTime;

        pd = new FParticleDefinition("bubble");
        pd.startScale = 1.5f;
        
        pd.startColor = new Color(255, 255, 255, 0.8f);
        pd.endColor = new Color(255, 255, 255, 0.1f);
        int part = RXRandom.Range(1, 10);
        for (int x = 0; x < part; x++)
        {
            pd.x = Position.x;
            pd.y = Position.y;
            Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f, 80.0f);
            pd.speedX = speed.x;
            pd.speedY = speed.y;
            pd.lifetime = RXRandom.Range(0.2f, 0.5f);
            InGamePage.CurrentInGamePage.projectilesParticles.AddParticle(pd);
        }
    }

}