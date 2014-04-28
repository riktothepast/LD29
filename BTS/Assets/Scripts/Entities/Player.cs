using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Player : Entity {
    int FacingRight;
    float coolDown = 2f;
    float curretTimer = 0f;
    int score = 0;
    FParticleDefinition pd = new FParticleDefinition("bubble");

    public Player() {
        FacingRight = 1;
        sprite = new FSprite("CCS");
        AddChild(sprite);
        InGamePage.CurrentInGamePage.entityContainer.AddChild(this);
        setEnergy(100);
    }

    public override void Update()
    {
        float forwardStep = 20f;
        float hInput = Input.GetAxis("Horizontal");
        float angle = Input.GetAxis("Vertical");

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

        if (hInput != 0) {
            FSoundManager.PlaySound("thrust",0.1f);
        }
        setRotationalAngle(getRotationalAngle() + angle);

        if (getRotationalAngle() > 45) {
            setRotationalAngle(45);
        }else if(getRotationalAngle() < -45){
            setRotationalAngle(-45);
        }

        if (FacingRight > 0)
        {
            rotation = -getRotationalAngle();
        }
        else {
            rotation = getRotationalAngle();
        }


        velocity.x += Mathf.Cos(getRotationalAngle()*Mathf.Deg2Rad) * Time.deltaTime * forwardStep * hInput;
        velocity.y += Mathf.Sin(getRotationalAngle()*Mathf.Deg2Rad) * Time.deltaTime * forwardStep;

        velocity.x *= 0.8f;
        velocity.y *= 0.8f;

        Position += velocity;
        SetPosition(Position);

        if(Position.y< -Futile.screen.halfHeight){
            Position.y = -Futile.screen.halfHeight + sprite.height;
            InGamePage.CurrentInGamePage.ScreenShake(2, 5);
            setRotationalAngle(getRotationalAngle() * -1);
            setEnergy(getEnergy()-5);
        }

        if (Position.y > Futile.screen.halfHeight)
        {
            Position.y = Futile.screen.halfHeight - sprite.height;
            InGamePage.CurrentInGamePage.ScreenShake(2, 5);
            setRotationalAngle(getRotationalAngle() *-1);
            setEnergy(getEnergy() - 5);
        }

        if (Position.x > Futile.screen.width / 2)
        {
            Position.x = -Futile.screen.width / 2 + sprite.width / 2;
        }

        if (Position.x < -Futile.screen.width / 2)
        {
            Position.x = Futile.screen.width / 2 + sprite.width / 2;
        }

        if (Input.GetKey(KeyCode.X))
        {
            Shoot();
        }

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
            curretTimer += 5f*Time.deltaTime;
    }

    public void Shoot() {
        if (curretTimer >= coolDown)
        {
            new Bullet(Position, getRotationalAngle(), FacingRight);
            curretTimer = 0;
            FSoundManager.PlaySound("shoot");
        }
    }

    public void AddScore(int val) {
        score += val;
    }

    public int getScore() {
        return this.score;
    }
}
