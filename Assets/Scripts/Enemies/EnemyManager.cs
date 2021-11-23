using Zenject;
using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyManager : ITickable, IFixedTickable
{
    readonly Settings _settings;
    readonly Enemy.Factory _enemyFactory;
    readonly SignalBus _signalBus;
    readonly EnemyRegistry _enemyReg;
    readonly LevelBounds _bounds;

    Transform enemiesParent;
    bool gameStarted;

    public EnemyManager(
        Enemy.Factory enemyFactory, SignalBus signalBus,
        EnemyRegistry enemyReg, LevelBounds bounds, Settings settings)
    {
        _enemyFactory = enemyFactory;
        _signalBus = signalBus;
        _enemyReg = enemyReg;
        _bounds = bounds;
        _settings = settings;
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {
        if(_enemyReg.EnemyCount == 0 && gameStarted)
        {
            GenerateWave();
        }
    }

    public void Stop()
    {
        gameStarted = false;
        while(_enemyReg.EnemyCount > 0)
        {
            _enemyReg.Enemies[_enemyReg.EnemyCount - 1].Despawn();
        }
    }

    public void Start()
    {
        gameStarted = true;
        GenerateWave();
    }

    private void GenerateWave()
    {
        int enemyCount = _settings.enemyColumns * _settings.enemyRows;
        for (int i = 0; i < enemyCount; i++)
        {
            _enemyFactory.Create();
        }

        List<Enemy> randList = new List<Enemy>(_enemyReg.Enemies);
        enemiesParent = _enemyReg.Enemies[0].transform.parent;
        enemiesParent.position = Vector3.zero;

        for (int posIndex = 0; posIndex < enemyCount; posIndex++)
        {
            float XOffset = (posIndex / _settings.enemyRows) * _settings.xOffset;   // Divide by rows
            float YOffset = (posIndex % _settings.enemyRows) * _settings.yOffset;   // Modulo gives remainder

            Vector3 startPos = new Vector3(-(float)(_settings.enemyColumns - 1) / 2 * _settings.xOffset, 0, _bounds.Bottom + _settings.topOffset);

            int randIndex = UnityEngine.Random.Range(0, randList.Count);
            randList[randIndex].gameObject.transform.position = startPos + new Vector3(XOffset, 0, YOffset);
            randList.RemoveAt(randIndex);
        }

        _signalBus.Fire<WaveCreatedSignal>();
    }

    [Serializable]
    public class Settings
    {
        public int enemyColumns;
        public int enemyRows;
        public float xOffset;
        public float yOffset;
        public float topOffset;
    }
}
