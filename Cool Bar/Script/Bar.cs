using Godot;
using System;
using System.Collections.Generic;

public partial class Bar : Node2D
{
    Dictionary<string, bool> IsActivated;
    Dictionary<string, Sprite2D> TickDictionary;

    ProgressBar ProgBar;
    Button AttackButton;

    int BarHealth;
    int TempHealth;
    int DamageDealt;

    Label EnemiesDefeated;
    int EnemyCounter;

    Timer Cooldown;
    Timer RoundEnd;
    Timer DeathTimer;
    Timer DamageTimer;

    Random rnd = new Random();

    Texture2D TickDefault;
    Texture2D TickSuccess;
    Texture2D TickFail;
    Texture2D TickCritical;

    PackedScene Tick;

    PackedScene Goblin;
    PackedScene Skeleton;
    PackedScene Mushroom;
    PackedScene FlyingEye;

    PackedScene DamageNumber;

    AudioStreamWav SuccessSound;
    AudioStreamWav CritSound;

    Enemy CurrentEnemy;
    int Gold;
    Label GoldLabel;

    int Button2Cost;
    Label Button2Label;
    int Button3Cost;
    Label Button3Label;
    int Button4Cost;
    Label Button4Label;

    Label TickNumberLabel;
    Label SuccessChanceLabel;
    Label TickResistanceLabel;
    Label TickDamageLabel;

    AnimationPlayer AnimPlayer;

    PackedScene GameOverOverlay;

    int TickNumber;
    int Turn;

    int TickDamage;
    int TickResistance;
    int SuccessChance;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SuccessSound = (AudioStreamWav)ResourceLoader.Load("res://Sound/SuccessSound.wav");
        CritSound = (AudioStreamWav)ResourceLoader.Load("res://Sound/CritSound.wav");

        Tick = (PackedScene)ResourceLoader.Load("res://Scenes/Tick.tscn");

        Goblin = (PackedScene)ResourceLoader.Load("res://Scenes/goblin.tscn");
        Skeleton = (PackedScene)ResourceLoader.Load("res://Scenes/skeleton.tscn");
        Mushroom = (PackedScene)ResourceLoader.Load("res://Scenes/mushroom.tscn");
        FlyingEye = (PackedScene)ResourceLoader.Load("res://Scenes/flying_eye.tscn");

        DamageNumber = (PackedScene)ResourceLoader.Load("res://Scenes/DamageNumber.tscn");

        GameOverOverlay = (PackedScene)ResourceLoader.Load("res://Scenes/GameOverOverlay.tscn");

        TickDefault = (Texture2D)ResourceLoader.Load("res://Sprites/Tick/Tick.png");
        TickSuccess = (Texture2D)ResourceLoader.Load("res://Sprites/Tick/TickSuccess.png");
        TickCritical = (Texture2D)ResourceLoader.Load("res://Sprites/Tick/TickCritical.png");
        TickFail = (Texture2D)ResourceLoader.Load("res://Sprites/Tick/TickFail.png");


        ProgBar = GetTree().Root.GetNode("Node2D").GetNode<ProgressBar>("ProgressBar");
        AttackButton = GetTree().Root.GetNode("Node2D").GetNode<Button>("AttackButton");
        EnemiesDefeated = GetTree().Root.GetNode("Node2D").GetNode<Label>("DefeatedEnemies");
        EnemyCounter = 0;

        Turn = 0;
        TickNumber = 20;

        Gold = 0;
        GoldLabel = GetTree().Root.GetNode("Node2D").GetNode<Label>("CurrentGold");
        Button2Label = GetTree().Root.GetNode("Node2D").GetNode("BuyButton2").GetNode<Label>("Label");
        Button3Label = GetTree().Root.GetNode("Node2D").GetNode("BuyButton3").GetNode<Label>("Label");
        Button4Label = GetTree().Root.GetNode("Node2D").GetNode("BuyButton4").GetNode<Label>("Label");

        TickNumberLabel = GetNode<Label>("TickNumberLabel");
        SuccessChanceLabel = GetNode<Label>("SuccessChanceLabel");
        TickResistanceLabel = GetNode<Label>("TickResistanceLabel");
        TickDamageLabel = GetNode<Label>("TickDamageLabel");

        Button2Cost = 20;
        Button3Cost = 40;
        Button4Cost = 100;

        SuccessChance = 50;
        TickDamage = 1;
        TickResistance = 0;

        Cooldown = GetNode<Timer>("Timer");
        RoundEnd = GetNode<Timer>("RoundEnd");
        DeathTimer = GetNode<Timer>("DeathTimer");
        DamageTimer = GetNode<Timer>("DamageTimer");

        TickNumberLabel.Text = "Number of Ticks: " + TickNumber.ToString();
        TickResistanceLabel.Text = "Resistance: " + TickResistance.ToString();
        SuccessChanceLabel.Text = "Success Chance: " + (100 - SuccessChance).ToString() + "%";
        TickDamageLabel.Text = "Damage: " + TickDamage.ToString();

        Cooldown.Paused = true;

        UpdateTickNumber();
        CreateEnemy();
    }

    public void UpdateTickNumber()
    {
        TickDictionary = new Dictionary<string, Sprite2D>();
        IsActivated = new Dictionary<string, bool>();

        for (int i = 1; i <= TickNumber; i++)
        {
            string tickName = "Tick" + i.ToString();
            TickDictionary.Add(tickName, (Sprite2D)GetNode(tickName));

            IsActivated.Add(tickName, false);
        }
    }

    public void RemoveTick(int removeNumber)
    {
        int i = 0;
        while (i < removeNumber & TickNumber > 0) 
        { 
            if(rnd.Next(100) < TickResistance)
            {
                GD.Print("Hit Parried");
            }

            else
            {
                string tickName = "Tick" + TickNumber.ToString();
                AnimPlayer = TickDictionary[tickName].GetNode<AnimationPlayer>("AnimationPlayer");
                AnimPlayer.Play("Death");

                TickNumber -= 1;
                i++;
            }
            
        }

        if(TickNumber <= 0)
        {
            GD.Print("Game over");
            ColorRect gameOverOverlay = (ColorRect)GameOverOverlay.Instantiate();
            AnimationPlayer animationPlayer = (AnimationPlayer)gameOverOverlay.GetNode("AnimationPlayer");
            GetTree().Root.GetNode("Node2D").AddChild(gameOverOverlay);
            animationPlayer.Play("FadeIn");

        }

        TickNumberLabel.Text = "Number of Ticks: " + TickNumber.ToString();
    }

    public void AddTick(int addNumber)
    {
        for (int i = 0; i < addNumber; i++)
        {
            if(TickNumber >= 100)
            {
                GD.Print("Maximum number of ticks");
            }

            else
            {
                string tickName = "Tick" + (TickNumber + 1).ToString();

                Sprite2D tick = (Sprite2D)Tick.Instantiate();
                tick.Position = new Vector2(TickNumber * 6, 0);
                tick.Name = tickName;

                UpdateTickNumber();

                TickNumber += 1;
                AddChild(tick);

                TickDictionary.Add(tickName, (Sprite2D)GetNode(tickName));

                AnimPlayer = TickDictionary[tick.Name].GetNode<AnimationPlayer>("AnimationPlayer");
                AnimPlayer.Play("Arrive");
            }

        }

        TickNumberLabel.Text = "Number of Ticks: " + TickNumber.ToString();
    }

    public void _on_buy_button_1_pressed()
    {
        if (TickNumber >= 100)
        {
            TickNumber = 100;
        }

        else if(Gold >= 5)
        {
            AddTick(1);
            Gold -= 5;
            GoldLabel.Text = "Current Gold: " + Gold.ToString();

        }

    }

    public void _on_buy_button_2_pressed()
    {
        if (TickResistance >= 70)
        {
            TickResistance = 70;
            GetTree().Root.GetNode("Node2D").GetNode<Button>("BuyButton2").Disabled = true;
        }

        else if (Gold >= Button2Cost)
        {
            TickResistance += 2;
            Gold -= Button2Cost;
            GoldLabel.Text = "Current Gold: " + Gold.ToString();
            Button2Cost = (int)Mathf.Floor(Button2Cost * 1.1);
            Button2Label.Text = "Cost: " + Button2Cost.ToString() + " gold";
            TickResistanceLabel.Text = "Resistance: " + TickResistance.ToString() +"%";
        }
    }

    public void _on_buy_button_3_pressed()
    {
        if (Gold >= Button3Cost)
        {
            SuccessChance--;
            Gold -= Button3Cost;
            GoldLabel.Text = "Current Gold: " + Gold.ToString();
            Button3Cost = (int)Mathf.Floor(Button3Cost * 1.2);
            Button3Label.Text = "Cost: " + Button3Cost.ToString() + " gold";
            SuccessChanceLabel.Text = "Success Chance: " + (100 - SuccessChance).ToString() + "%";
        }
    }

    public void _on_buy_button_4_pressed()
    {
        if (Gold >= Button4Cost)
        {
            TickDamage++;
            Gold -= Button4Cost;
            GoldLabel.Text = "Current Gold: " + Gold.ToString();
            Button4Cost = (int)Mathf.Floor(Button4Cost * 2);
            Button4Label.Text = "Cost: " + Button4Cost.ToString() + " gold";
            TickDamageLabel.Text = "Damage: " + TickDamage.ToString();
        }
    }

    public void _on_timer_timeout()
    {
        if (Turn == TickNumber)
        {
            Cooldown.Paused = true;

            foreach (KeyValuePair<string, bool> tick in IsActivated)
            {
                IsActivated[tick.Key] = false;
            }
            Turn = 0;
            GD.Print("End of turn achieved");

            InflictDamage();
        }

        else
        {
            int tickNum = rnd.Next(TickNumber) + 1;
            string tickName = "Tick" + tickNum.ToString();
            while (IsActivated[tickName] != false)
            {
                tickNum = rnd.Next(TickNumber) + 1;
                tickName = "Tick" + tickNum.ToString();
            }

            Sprite2D tick = TickDictionary[tickName];

            AnimPlayer = tick.GetNode<AnimationPlayer>("AnimationPlayer");
            AudioStreamPlayer audioPlayer = tick.GetNode<AudioStreamPlayer>("AudioStreamPlayer");

            int num = rnd.Next(100);

            if (num >= SuccessChance + 49)
            {
                tick.Texture = TickCritical;
                AnimPlayer.Play("Critical");

                TempHealth -= 5 * TickDamage;
                audioPlayer.VolumeDb = -8;
                audioPlayer.Stream = CritSound;
                audioPlayer.Play();
            }

            else if (num >= SuccessChance)
            {
                tick.Texture = TickSuccess;
                AnimPlayer.Play("Success");
                TempHealth -= TickDamage;
                audioPlayer.VolumeDb = -24;
                audioPlayer.Stream = SuccessSound;
                audioPlayer.Play();
            }

            else
            {
                tick.Texture = TickFail;
                AnimPlayer.Play("Fail");
            }

            IsActivated[tickName] = true;
            Turn += 1;
            Cooldown.WaitTime = 0.05;
        }
    }

    public void _on_death_timer_timeout()
    {
        CreateEnemy();
        DeathTimer.Stop();
    }

    public void _on_button_pressed()
    {
        UpdateTickNumber();

        foreach (KeyValuePair<string, Sprite2D> tick in TickDictionary)
        {
            tick.Value.Texture = TickDefault;
        }

        Cooldown.WaitTime = 0.1;
        Cooldown.Paused = false;
        Cooldown.Start();

        AttackButton.Disabled = true;
    }

    public override void _Process(double delta)
    {
        ProgBar.Value = Math.Clamp(ProgBar.Value - 1, BarHealth, ProgBar.MaxValue);
    }


    public void InflictDamage()
    {
        GpuParticles2D particle;

        RoundEnd.WaitTime = 0.5;
        RoundEnd.Paused = false;
        RoundEnd.Start();

        CurrentEnemy.Play("TakeHit");
        particle = CurrentEnemy.GetNode<GpuParticles2D>("GPUParticles2D");
        particle.Emitting = true;
        CurrentEnemy.Player.Play("TakeHit");
        DamageDealt = BarHealth - TempHealth;
        BarHealth = TempHealth;

        Label dmgNum = (Label)DamageNumber.Instantiate();
        dmgNum.Text = "-" + DamageDealt.ToString();
        CurrentEnemy.AddChild(dmgNum);
    }

    public void _on_round_end_timeout()
    {
        if (BarHealth <= 0)
        {
            DefeatEnemy();
        }

        else
        {
            CurrentEnemy.Play("Attack");
            CurrentEnemy.AudioStreamPlayer.Play();
            DamageTimer.WaitTime = 0.7;
            DamageTimer.Start();
        }
        RoundEnd.Paused = true;
        
    }
    public void _on_damage_timer_timeout()
    {
        DamageTimer.Stop();
        RemoveTick(rnd.Next(EnemyCounter/4) + 1 + (int)Mathf.Floor(Math.Pow((EnemyCounter / 8), 2)));
        UpdateTickNumber();

        AttackButton.Disabled = false;

        GD.Print(BarHealth);
        GD.Print(TempHealth);
        GD.Print(ProgBar.MaxValue);
        GD.Print(ProgBar.Value);
    }
    public void DefeatEnemy()
    {
        AddTick(3);
        UpdateTickNumber();

        CurrentEnemy.Play("Death");
        CurrentEnemy.Player.Play("DeathFade");
        DeathTimer.WaitTime = 1;
        DeathTimer.Start();

        Gold += 10 + rnd.Next(5 + EnemyCounter * 4) + EnemyCounter * 2;
        GoldLabel.Text = "Current Gold: " + Gold.ToString();
        UpdateCounter();

    }

    public void CreateEnemy()
    {
        int randomEnemyNumber = rnd.Next(4);
        int enemyHealth = rnd.Next(11 + EnemyCounter) + 10 + (int)Mathf.Floor(Math.Pow((EnemyCounter / 2), 2));
        Enemy enemy;

        if (randomEnemyNumber == 0)
        {
            enemy = (Enemy)Goblin.Instantiate();
        }

        else if (randomEnemyNumber == 1)
        {
            enemy = (Enemy)Skeleton.Instantiate();
        }

        else if (randomEnemyNumber == 2)
        {
            enemy = (Enemy)Mushroom.Instantiate();
        }

        else
        {
            enemy = (Enemy)FlyingEye.Instantiate();
        }
        enemy.Position = new Vector2(200, -10);
        CurrentEnemy = enemy;
        
        BarHealth = enemyHealth;
        TempHealth = BarHealth;
        ProgBar.MaxValue = BarHealth;
        ProgBar.Value = BarHealth;

        enemy.Scale = new Vector2(1, 1) + new Vector2(1,1) * BarHealth / 50;

        ProgBar.AddChild(enemy);

        GD.Print(BarHealth);
        GD.Print(TempHealth);
        GD.Print(ProgBar.MaxValue);
        GD.Print(ProgBar.Value);
        AttackButton.Disabled = false;
    }

    public void UpdateCounter()
    {
        EnemyCounter++;
        EnemiesDefeated.Text = "Enemies defeated: " + EnemyCounter.ToString();
    }
}
