using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Shielded : Entity
{
    int FacingRight;
    float coolDown = 1f;
    float curretTimer = 0f;
    FParticleDefinition pd = new FParticleDefinition("bubble");

    public Shielded(Vector2 pos)
    {
        FacingRight = 1;
        sprite = new FSprite("shielded");
        AddChild(sprite);
        InGamePage.CurrentInGamePage.entityContainer.AddChild(this);
        Position = pos;
        SetPosition(pos);
        trackPosition = InGamePage.CurrentInGamePage.getPlayer().GetPosition();
        setEnergy(100);
        scoreValue = 100;
        hInput = -1f;
    }
    float hInput = 0;

    Vector2 trackPosition;
    float passedTime;
    public override void Update()
    {
        float forwardStep = 5f;
        float angle = 0;
        passedTime += 1;

        float dist = Vector2.Distance(InGamePage.CurrentInGamePage.getPlayer().GetPosition(), Position);
        if (dist < 500f)
        {
            Shoot();
        }

        if (hInput > 0)
        {
            sprite.scaleX = 1f;
            FacingRight = 1;
        }
        else if (hInput < 0)
        {
            sprite.scaleX = -1f;
            FacingRight = -1;
        }

        setRotationalAngle(getRotationalAngle() + angle);

        if (getRotationalAngle() > 45)
        {
            setRotationalAngle(45);
        }
        else if (getRotationalAngle() < -45)
        {
            setRotationalAngle(-45);
        }

        if (FacingRight > 0)
        {
            rotation = -getRotationalAngle();

        }
        else
        {
            rotation = getRotationalAngle();
        }

        if (Position.x > Futile.screen.width/2)
        {
            Position.x = -Futile.screen.width / 2 + sprite.width / 2;
        }

        if (Position.x < -Futile.screen.width / 2)
        {
            Position.x = Futile.screen.width/2 + sprite.width / 2;
        }


        velocity.x += Mathf.Cos(getRotationalAngle() * Mathf.Deg2Rad) * Time.deltaTime * forwardStep * hInput;
        velocity.y += Mathf.Sin(getRotationalAngle() * Mathf.Deg2Rad) * Time.deltaTime * forwardStep;

        velocity.x *= 0.9f;
        velocity.y *= 0.8f;

        Position += velocity;
        SetPosition(Position);

        if (Mathf.Abs(velocity.x) > 1)
        {
            pd = new FParticleDefinition("bubble");
            pd.startScale = 1.5f;
            pd.startColor = new Color(255, 255, 255, 0.8f);
            pd.endColor = new Color(255, 255, 255, 0.1f);
            pd.x = Position.x;
            pd.y = Position.y;
            Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f, 80.0f);
            pd.speedX = speed.x;
            pd.speedY = speed.y;
            pd.lifetime = RXRandom.Range(0.2f, 0.5f);
            InGamePage.CurrentInGamePage.projectilesParticles.AddParticle(pd);
        }
        curretTimer += 1f * Time.deltaTime;

    }

    public void Shoot()
    {
        if (curretTimer >= coolDown)
        {
            new DeepCharge(Position, getRotationalAngle(), FacingRight);
            curretTimer = 0;
            FSoundManager.PlaySound("drop");

        }
    }

}
