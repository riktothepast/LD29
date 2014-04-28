using UnityEngine;
using System.Collections.Generic;
using System;

public class InGamePage : Page{
    FLabel hudStuff;
    FLabel hudShadow;
    FLabel gameOver;
    public static InGamePage CurrentInGamePage { get; private set; }
    public List<Entity> enemies;
    public List<Entity> projectiles, Enemyprojectiles;
    public FContainer entityContainer, enemyContainer, projectileContainer, particleContainer;
    public FParticleSystem projectilesParticles;
    int minimalEnemies = 5;

    FContainer gameArea;
    Player jugador;
    public InGamePage() {
        FSprite bg = new FSprite("Atlases/bg");
        bg.width = Futile.screen.width;
        bg.height = Futile.screen.height;
        AddChild(bg);
        hudStuff = new FLabel("Minecraftia", "Energy:");
        hudShadow = new FLabel("Minecraftia", "Energy:");
        hudStuff.scale = 0.7f;
        hudShadow.scale = 0.7f;
        hudShadow.color = Color.black;
        hudStuff.SetPosition(new Vector2(0  , (Futile.screen.height/2)*0.9f));
        hudShadow.SetPosition(new Vector2( 1f, ((Futile.screen.height / 2) * 0.9f - 1)));


        gameArea = new FContainer();
        enemies = new List<Entity>();
        projectiles = new List<Entity>();
        Enemyprojectiles = new List<Entity>();
        entityContainer = new FContainer();
        projectileContainer = new FContainer();
        particleContainer = new FContainer();
        gameArea.AddChild(particleContainer);
        particleContainer.AddChild(projectilesParticles = new FParticleSystem(300));

        gameArea.AddChild(entityContainer);
        gameArea.AddChild(projectileContainer);
        AddChild(gameArea);
        InGamePage.CurrentInGamePage = this;
        ListenForUpdate(Update);
        jugador = new Player();
        GenerateFoe();

        AddChild(hudShadow);
        AddChild(hudStuff);
        
    }


    public void Update(){

        hudStuff.text = "Energy: " + jugador.getEnergy() + "\n Score: " + jugador.getScore();
        hudShadow.text = hudStuff.text;
        if (jugador.getEnergy() <= 0)
        {
            hudStuff.text = "Game Over, \n press return to continue...";
            hudShadow.text = hudStuff.text;
            gameArea.alpha--;
            projectileContainer.alpha--;
            entityContainer.alpha--;
            particleContainer.alpha--;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Game.instance.GoToPage(PageType.MainMenuPage);
            }

            return;
        }

        for (int x = enemies.Count - 1; x >= 0; x--)
        {
            enemies[x].Update();
            if (enemies[x].getEnergy() < 0)
            {
                entityContainer.RemoveChild(enemies[x]);
                enemies.RemoveAt(x);
            }
        }

        for (int x = projectiles.Count-1; x >= 0; x-- )
        {
            projectiles[x].Update();
            for (int w = enemies.Count - 1; w >= 0; w--)
            {
                if (projectiles[x].sprite.localRect.CloneAndOffset(projectiles[x].x,projectiles[x].y).CheckIntersect(enemies[w].sprite.localRect.CloneAndOffset(enemies[w].x,enemies[w].y))) {
                    enemies[w].setEnergy(enemies[w].getEnergy()-projectiles[x].getEnergy());
                    jugador.AddScore(enemies[w].scoreValue);
                    projectiles[x].ttl = -2f;
                    ScreenShake(2, 3);
                    FSoundManager.PlaySound("explosion");
                }
            }
            if (projectiles[x].ttl < 0)
            {
                projectiles[x].DestroyEffect();
                projectileContainer.RemoveChild(projectiles[x]);
                projectiles.RemoveAt(x);
            }
        }

        for (int x = Enemyprojectiles.Count - 1; x >= 0; x--)
        {
            Enemyprojectiles[x].Update();
            if (Enemyprojectiles[x].sprite.localRect.CloneAndOffset(Enemyprojectiles[x].x, Enemyprojectiles[x].y).CheckIntersect(jugador.sprite.localRect.CloneAndOffset(jugador.Position.x, jugador.Position.y)))
            {
                jugador.setEnergy(jugador.getEnergy()-Enemyprojectiles[x].getEnergy());
                ScreenShake(3,10);
                Enemyprojectiles[x].ttl = -1;
                FSoundManager.PlaySound("explosion");
            }
            if (Enemyprojectiles[x].ttl < 0)
            {
                Enemyprojectiles[x].DestroyEffect();
                projectileContainer.RemoveChild(Enemyprojectiles[x]);
                Enemyprojectiles.RemoveAt(x);
            }
        }
        
        jugador.Update();

        if (enemies.Count < minimalEnemies) 
        {
            GenerateFoe();
        }

        if(jugador.getScore()>0){
        if ((jugador.getScore() % 300) == 0) {
            Shielded shiel = new Shielded(new Vector2(jugador.GetPosition().x + UnityEngine.Random.Range(-Futile.screen.width * 2, Futile.screen.width * 2), Futile.screen.halfHeight - 20));
            enemies.Add(shiel);
            jugador.AddScore(jugador.getScore()+10);
        }

        if ((jugador.getScore() % 500) == 0)
        {
           minimalEnemies ++ ;
        }
            }
    }

    public Player getPlayer() {
        return jugador;
    }

    void GenerateFoe() {
        Follower foll = new Follower(new Vector2(jugador.GetPosition().x + UnityEngine.Random.Range(-Futile.screen.width*2, Futile.screen.width*2), UnityEngine.Random.Range(-Futile.screen.height / 2, Futile.screen.height / 2)));
        enemies.Add(foll);
    }

    public void ScreenShake(int time, int amplitude) {
        ShakeUtil.Go(this, time, amplitude);
    }
}